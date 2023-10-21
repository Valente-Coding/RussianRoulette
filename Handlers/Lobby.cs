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
using Exiled.API.Enums;
using RussianRoulette.Localization;

namespace RussianRoulette.Handlers
{
    class Lobby : MonoBehaviour
    {
        public static Lobby Instance = new Lobby();
        public Lobby() { }

        private Message _broadcastHandler = new Message();
        private List<Item> _defaultInv = new List<Item>();

        public bool KeepGoing = false;


        public void RegisterEvents()
        {
            PlayerEvents.DryfiringWeapon += RouletteRound.Instance.TriggerPulled;
            PlayerEvents.ChangingItem += Abilities.Instance.UsingAbility;
            PlayerEvents.SpawnedRagdoll += RouletteRound.Instance.AddRagdollToList;
            PlayerEvents.DroppingItem += RouletteRound.Instance.OnDroppingItem;
        }

        public void UnregisterEvents()
        {
            PlayerEvents.DryfiringWeapon -= RouletteRound.Instance.TriggerPulled;
            PlayerEvents.ChangingItem -= Abilities.Instance.UsingAbility;
            PlayerEvents.SpawnedRagdoll -= RouletteRound.Instance.AddRagdollToList;
            PlayerEvents.DroppingItem -= RouletteRound.Instance.OnDroppingItem;
        }

        public void StartNewLobby()
        {
            Log.Info("Keepgoing: " + KeepGoing.ToString());
            if (KeepGoing)
            {
                ServerRound.IsLocked = true;
                SendPlayersToLobby();

                Timing.RunCoroutine(WaitingToStart());
            }
        }

        IEnumerator<float> WaitingToStart()
        {
            _broadcastHandler.Duration = 1;
            yield return Timing.WaitForSeconds(5f);

            for (int i = 10; i > 0; i--) 
            {
                _broadcastHandler.Content = SwitchLanguage.Instance.WAIT_TO_START_1 + i + SwitchLanguage.Instance.WAIT_TO_START_2;
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
