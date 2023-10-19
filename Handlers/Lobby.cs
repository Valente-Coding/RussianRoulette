using Exiled.API.Features;
using Exiled.API.Features.Items;
using MEC;
using PlayerRoles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Message = Exiled.API.Features.Broadcast;
using ServerRound = PluginAPI.Core.Round;
using PlayerEvents = Exiled.Events.Handlers.Player;
using RouletteRound = RussianRoulette.Handlers.Round;


namespace RussianRoulette.Handlers
{
    class Lobby : MonoBehaviour
    {
        private Message _broadcastHandler = new Message();
        private List<Item> _defaultInv = new List<Item>();

        public static Lobby Instance = new Lobby();
        public Lobby() { }


        public void RegisterEvents()
        {
            PlayerEvents.DryfiringWeapon += RouletteRound.Instance.TriggerPulled;
            PlayerEvents.ChangingItem += Abilities.Instance.UsingAbility;
        }

        public void UnregisterEvents()
        {
            PlayerEvents.DryfiringWeapon -= RouletteRound.Instance.TriggerPulled;
            PlayerEvents.ChangingItem -= Abilities.Instance.UsingAbility;
        }

        public void StartNewLobby()
        {
            ServerRound.IsLocked = true;
            SendPlayersToLobby();

            Timing.RunCoroutine(WaitingToStart());
        }

        IEnumerator<float> WaitingToStart()
        {
            _broadcastHandler.Duration = 1;

            for (int i = 10; i > 0; i--) 
            {
                _broadcastHandler.Content = "Next round is starting in " + i + " seconds";
                Map.Broadcast(_broadcastHandler);

                yield return Timing.WaitForSeconds(1f);
            }

            Log.Info("10 Seconds Passed");
            StartNewRound();
        }

        private void SendPlayersToLobby()
        {
            foreach (Player player in Player.List)
            {
                player.Role.Set(RoleTypeId.Tutorial);
                player.ResetInventory(_defaultInv);
            }
        }

        public void StartNewRound()
        {
            Log.Info("Creating a new round.");
            RouletteRound.Instance.StartRound();
        }
    }
}
