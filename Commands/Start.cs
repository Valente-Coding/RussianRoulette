using CommandSystem;
using Exiled.API.Features;
using Exiled.CustomRoles.API;
using InventorySystem.Items.Usables;
using PlayerRoles;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RussianRoulette.Handlers;

using Message = Exiled.API.Features.Broadcast;

namespace RussianRoulette.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Start : ICommand
    {
        public string Command { get; } = "rrstart";

        public string[] Aliases { get; } = { };

        public string Description { get; } = "Starts a Russian Roulette event. To stop it do /rrstop.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Lobby.Instance.KeepGoing = true;

            Message broadcast = new Message();
            broadcast.Content = "Russian Roulette Event Activated. Starts Next Round.";
            broadcast.Duration = 5;

            Map.Broadcast(broadcast);

            response = "Done";
            return true;
        }
    }
}
