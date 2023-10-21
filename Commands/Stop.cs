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
using Round = PluginAPI.Core.Round;

namespace RussianRoulette.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Stop : ICommand
    {
        public string Command { get; } = "rrstop";

        public string[] Aliases { get; } = { };

        public string Description { get; } = "Stops a current Russian Roulette event.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Lobby.Instance.KeepGoing = false;
            Round.IsLocked = false;

            Round.Restart();

            response = "Done";
            return true;
        }
    }
}
