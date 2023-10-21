using CommandSystem;
using RussianRoulette.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RussianRoulette.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    internal class Lang : ICommand
    {
        public string Command { get; } = "rrlang";

        public string[] Aliases { get; } = { };

        public string Description { get; } = "Starts a Russian Roulette event. To stop it do /rrstop.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Done";

            if (arguments.Count == 0)
            {
                response = "rrlang <EN|ES|PT>";

                return false;
            }

            SwitchLanguage.Instance.To(string.Join(" ", arguments.Skip(0)));



            return true;
        }
    }
}
