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
        private List<Item> items = new List<Item>() { };
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

            _bulletInBarrel = Random.Range(0, 6);
            _currentBullet = 0;
            Log.Info("BulletInBarrel: " + _bulletInBarrel);

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
            bool hasBullet = _currentBullet == _bulletInBarrel ? true : false;
            if (hasBullet)
                Log.Info("Had Bullet in chamber.");
            else
                Log.Info("Hadn´t Bullet in chamber.");

            if (ev.Firearm.Aiming)
            {
                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(ev.Player.GameObject.transform.position, ev.Player.GameObject.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, 8))
                {
                    if (hasBullet)
                    {
                        Player target = hit.collider.gameObject.GetComponent<Player>();
                        EliminatePlayer(target);
                    }
                    else
                    {
                        EliminatePlayer(_playerOrder[_currentPlayer]);
                    }
                }
                else
                {
                    if (hasBullet)
                    {
                        SendGlobalMessage("The bullet was wasted. Reloading a new one.", 3);
                    }
                    else
                    {
                        EliminatePlayer(_playerOrder[_currentPlayer]);
                    }
                }
            }
            else
            {
                Log.Info("Tried shot him self. Player: " + _playerOrder.Count);
                if (hasBullet)
                    EliminatePlayer(_playerOrder[_currentPlayer]);
                else
                    SendGlobalMessage("There was no bullet.", 3);
            }

            if (hasBullet)
                ReloadGun();
            else
                _currentBullet++;


            NextPlayer();
        }

        private void ReloadGun()
        {
            _bulletInBarrel = Random.Range(0, 6);
            _currentBullet = 0;
        }

        private void EliminatePlayer(Player player)
        {
            Log.Info("There is " + _playerOrder.Count + " players alive.");
            foreach (Player p in _playerOrder)
            {
                if (p == player)
                {
                    if (Abilities.Instance.ProtectedPlayer != player)
                    {
                        SendGlobalMessage("The player " + p.Nickname + " died.", 3);
                        p.ResetInventory(new List<Item>());
                        p.Kill(DamageType.Revolver);
                        p.Role.Set(RoleTypeId.Spectator);
                    }
                    else
                    {
                        SendGlobalMessage("The player " + p.Nickname + " had shild activated.", 3);
                    }
              
                    break;
                }
            }

            if (Abilities.Instance.ProtectedPlayer == player)
            {
                Abilities.Instance.ProtectedPlayer = null;
            }
            else
                _playerOrder.Remove(player);

            Log.Info("There is " + _playerOrder.Count + " players alive.");
        }

        public void NextPlayer()
        {
            if (_playerOrder.Count > 1)
            {
                Log.Info("Next Player.");

                RemoveWeaponFromPlayer(_currentPlayer);

                _currentPlayer++;
                if (_currentPlayer >= _playerOrder.Count)
                    _currentPlayer = 0;

                GiveWeaponToPlayer(_currentPlayer);
            }
            else if (_playerOrder.Count == 1)
            {
                Log.Info(_playerOrder[0].Nickname + " won this round! What a lucky guy.");
                SendGlobalMessage(_playerOrder[0].Nickname + " won this round! What a lucky guy.", 5);

                //ServerRound.IsLocked = false;
                Timing.RunCoroutine(EndRound());
            }
            else
            {
                Log.Info("The game ended with everyone dead.");
                SendGlobalMessage("Everybody died!", 5);

                //ServerRound.IsLocked = false;
                Timing.RunCoroutine(EndRound());
            }
        }

        IEnumerator<float> EndRound()
        {
            yield return Timing.WaitForSeconds(5);

            Lobby.Instance.StartNewLobby();
        }


        private void SendGlobalMessage(string content, ushort duration)
        {
            _broadcastHandler.Content = content;
            _broadcastHandler.Duration = duration;
            Map.Broadcast(_broadcastHandler, true);
        }
    }
}
