using Exiled.API.Enums;
using Exiled.API.Features;
using RussianRoulette.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using ServerRound = PluginAPI.Core.Round;
using RussianRoulette.Handlers;

namespace RussianRoulette
{
    public class RussianRoulette : Plugin<Config>
    {
        public static RussianRoulette Instance { get; } = new RussianRoulette();
        private RussianRoulette() { }

        

        public override PluginPriority Priority { get; } = PluginPriority.Medium;

        public override void OnEnabled()
        {
            base.OnEnabled();

            RegisterEvents();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            UnregisterEvents();
        }


        private void RegisterEvents()
        {
            Lobby.Instance.RegisterEvents();
            Server.RoundStarted += Lobby.Instance.StartNewLobby;
        }

        private void UnregisterEvents()
        {
            Lobby.Instance.UnregisterEvents();
            Server.RoundStarted -= Lobby.Instance.StartNewLobby;
        }
    }
}
