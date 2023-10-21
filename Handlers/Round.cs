using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using HarmonyLib;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using MEC;
using PlayerRoles;
using RussianRoulette.Localization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Message = Exiled.API.Features.Broadcast;

namespace RussianRoulette.Handlers
{
    class Round
    {
        public static Round Instance = new Round();

        private Message _broadcastHandler = new Message();
        private List<Ragdoll> _ragdolls = new List<Ragdoll>();
        private Handlers.Spawn _spawn;
        private List<Player> _playerOrder;
        private int _currentPlayer = 0;
        private int _bulletInBarrel = -1;
        private int _currentBullet = -1;

        public int CurrentBullet { get { return _currentBullet; } set { _currentBullet = value; } }
        public int BulletInBarrel { get { return _bulletInBarrel; } set { _bulletInBarrel = value; } }
        public int CurrentPlayer { get { return _currentPlayer; } set { _currentPlayer = value; } }
        public List<Player> PlayerOrder { get { return _playerOrder; } }

        public void StartRound()
        {
            Log.Info("New Round Started");
            _spawn = new Handlers.Spawn();
            _playerOrder = _spawn.SpawnPlayers();

            Abilities.Instance.GiveAbilitiesToPlayers(_playerOrder);

            ReloadWeapon();

            GiveWeaponToRandom();
        }

        private void GiveWeaponToRandom()
        {
            int index = Random.Range(0, _playerOrder.Count);
            GiveWeaponToPlayer(index);
            _currentPlayer = index;
        }

        private void GiveWeaponToPlayer(int pIndex)
        {
            Log.Info("Added Revolver.");
            _playerOrder[pIndex].Inventory.ServerAddItem(ItemType.GunRevolver, 3);
        }

        private void RemoveWeaponFromPlayer(int pIndex)
        {
            Log.Info("Removed Revolver.");
            _playerOrder[pIndex].Inventory.ServerRemoveItem(3, null);
        }

        public void TriggerPulled(DryfiringWeaponEventArgs ev)
        {
            RemoveWeaponFromPlayer(_currentPlayer);

            bool hasBullet = _currentBullet == _bulletInBarrel ? true : false;
            if (hasBullet)
                Log.Info("Had Bullet in chamber.");
            else
                Log.Info("Hadn´t Bullet in chamber.");

            if (ev.Firearm.Aiming)
            {
                ShootToPlayer(ev.Player, hasBullet);
            }
            else
            {
                ShootSelf(hasBullet);
            }

            if (hasBullet)
                ReloadWeapon();
            else
                _currentBullet++;


            NextPlayer();
        }

        private void ShootToPlayer(Player player, bool hasBullet = false)
        {
            if (hasBullet) 
            {
                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(player.CameraTransform.position, player.CameraTransform.forward, out hit, Mathf.Infinity))
                {
                    Log.Info("The player shot something.");
                    Player target = Player.Get(hit.transform.GetComponentInParent<ReferenceHub>());
                    if (target != null) 
                    {
                        Log.Info("The player shot another player.");
                        EliminatePlayer(target);
                    }
                    else
                    {
                        SendGlobalMessage(SwitchLanguage.Instance.BULLET_WASTED, 3);
                    }
                }
            }
            else
            {
                EliminatePlayer(_playerOrder[_currentPlayer]);
            }


        }

        private void ShootSelf(bool hasBullet = false)
        {
            Log.Info("Tried shot him self.");
            if (hasBullet)
                EliminatePlayer(_playerOrder[_currentPlayer]);
            else
                SendGlobalMessage(SwitchLanguage.Instance.NO_BULLET, 3);
        }

        private void EliminatePlayer(Player player)
        {
            foreach (Player p in _playerOrder)
            {
                if (p == player)
                {
                    if (Abilities.Instance.ProtectedPlayer != player)
                    {
                        SendGlobalMessage(SwitchLanguage.Instance.PLAYER_DEAD_1 + p.Nickname + SwitchLanguage.Instance.PLAYER_DEAD_2, 3);
                        p.ResetInventory(new List<Item>());
                        p.Kill(DamageType.Revolver);
                        p.Role.Set(RoleTypeId.Spectator);
                    }
                    else
                    {
                        SendGlobalMessage(SwitchLanguage.Instance.PLAYER_SHIELD_1 + p.Nickname + SwitchLanguage.Instance.PLAYER_SHIELD_2, 3);
                    }
              
                    break;
                }
            }

            if (Abilities.Instance.ProtectedPlayer == player)
            {
                Abilities.Instance.ProtectedPlayer = null;
            }
            else
            {
                _playerOrder.Remove(player);
                _currentPlayer -= 1;
            }

            Log.Info("There is " + _playerOrder.Count + " players alive.");
        }

        public void ReloadWeapon()
        {
            _currentBullet = 0;
            _bulletInBarrel = Random.Range(0, 6);

            Log.Info("BulletInBarrel: " + _bulletInBarrel);
        }

        public void NextPlayer()
        {
            if (_playerOrder.Count > 1)
            {
                Log.Info("Next Player.");

                _currentPlayer++;
                if (_currentPlayer >= _playerOrder.Count)
                    _currentPlayer = 0;

                GiveWeaponToPlayer(_currentPlayer);
            }
            else if (_playerOrder.Count == 1)
            {
                RemoveWeaponFromPlayer(0);

                Log.Info(_playerOrder[0].Nickname + " won this round! What a lucky guy.");
                SendGlobalMessage(_playerOrder[0].Nickname + SwitchLanguage.Instance.PLAYER_WON, 5);

                //ServerRound.IsLocked = false;
                //Timing.RunCoroutine(EndRound());

                Abilities.Instance.ProtectedPlayer = null;

                RemoveAllCorpses();
                Lobby.Instance.StartNewLobby();
            }
            else
            {
                Log.Info("The game ended with everyone dead.");
                SendGlobalMessage(SwitchLanguage.Instance.EVERYBODY_DIED, 5);

                //ServerRound.IsLocked = false;
                //Timing.RunCoroutine(EndRound());
                Abilities.Instance.ProtectedPlayer = null;

                RemoveAllCorpses();
                Lobby.Instance.StartNewLobby();
            }
        }

        IEnumerator<float> EndRound()
        {
            Abilities.Instance.ProtectedPlayer = null;
            yield return Timing.WaitForSeconds(5);

            RemoveAllCorpses();
            Lobby.Instance.StartNewLobby();
        }

        public void AddRagdollToList(SpawnedRagdollEventArgs ev)
        {
            _ragdolls.Add(ev.Ragdoll);
        }

        private void RemoveAllCorpses()
        {
            foreach (Ragdoll rd in _ragdolls)
            {
                Object.Destroy(rd.GameObject);
            }

            _ragdolls.Clear();
        }

        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (Lobby.Instance.KeepGoing)
                ev.IsAllowed = false;
        }

        private void SendGlobalMessage(string content, ushort duration)
        {
            _broadcastHandler.Content = content;
            _broadcastHandler.Duration = duration;
            Map.Broadcast(_broadcastHandler, true);
        }
    }
}
