using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Message = Exiled.API.Features.Broadcast;
using RouletteRound = RussianRoulette.Handlers.Round;

namespace RussianRoulette.Handlers
{
    class Abilities
    {
        public static Abilities Instance = new Abilities();


        private List<ItemType> _abilities = new List<ItemType>()
        {
            ItemType.Adrenaline,
            ItemType.Coin,
            ItemType.KeycardJanitor
        };
        private Player _protectedPlayer;
        private Message _broadcastHandler = new Message();

        public Player ProtectedPlayer { get { return _protectedPlayer; } set { _protectedPlayer = value; } }


        public void GiveAbilitiesToPlayers(List<Player> players)
        {
            foreach (ItemType ability in _abilities) 
            {
                players[Random.Range(0, players.Count)].Inventory.ServerAddItem(ability, (ushort)Random.Range(0, 999));
            }
        }

        public void UsingAbility(ChangingItemEventArgs ev)
        {
            if (!Lobby.Instance.KeepGoing) return;
            
            switch (ev.Item.Type)
            {
                case ItemType.Adrenaline:
                    UseAdremaline(ev);
                    break;
                case ItemType.Coin:
                    UseCoin(ev);
                    break;
                case ItemType.KeycardJanitor:
                    UseKeycard(ev);
                    break;
            }
        }

        private void UseAdremaline(ChangingItemEventArgs ev) // Gives a player protection
        {
            Log.Info(RouletteRound.Instance.PlayerOrder[RouletteRound.Instance.CurrentPlayer].Nickname);
            if (RouletteRound.Instance.PlayerOrder[RouletteRound.Instance.CurrentPlayer] == ev.Player)
            {
                _protectedPlayer = ev.Player;

                _broadcastHandler.Content = "You are protected.";
                _broadcastHandler.Duration = 2;

                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(_broadcastHandler);

                ev.Player.Inventory.ServerRemoveItem(ev.Item.Serial, null);
            }
        }

        private void UseCoin(ChangingItemEventArgs ev) // Spins the barrel.
        {
            RouletteRound.Instance.CurrentBullet = 0;
            RouletteRound.Instance.BulletInBarrel = Random.Range(0, 6);

            _broadcastHandler.Content = "Someone spinned the barrel! Everyone lost the bullet.";
            _broadcastHandler.Duration = 5;

            Map.ClearBroadcasts();
            Map.Broadcast(_broadcastHandler);

            ev.Player.Inventory.ServerRemoveItem(ev.Item.Serial, null);
        }

        private void UseKeycard(ChangingItemEventArgs ev) // Makes a player skip his turn
        {
            if (RouletteRound.Instance.PlayerOrder[RouletteRound.Instance.CurrentPlayer] == ev.Player)
            {
                RouletteRound.Instance.NextPlayer();

                _broadcastHandler.Content = "Not today! Turn skipped.";
                _broadcastHandler.Duration = 5;

                Map.ClearBroadcasts();
                Map.Broadcast(_broadcastHandler);

                ev.Player.Inventory.ServerRemoveItem(ev.Item.Serial, null);
            }
        }
    }
}
