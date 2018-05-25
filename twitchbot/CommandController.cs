using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace twitchbot
{
    class CommandsController
    {
        private List<Command> commands;
        internal List<Command> Commands { get => commands; set => commands = value; }

        private SendMessage _sender;
        private ISerializer _serializer;

        public CommandsController(SendMessage sender, ISerializer serializer)
        {
            _sender = sender;
            _serializer = serializer;

            commands = new List<Command>()
            {
                new Command("!vk", "Ссылка на группу ВК", x => "Подписывайтесь на мою группу Вконтакте: https://site"),
                new Command("!donate", "Поддержка донатом", x => "Поддержать меня вы можете по этой ссылке: https://site")
            };

            serializer.Serialize(commands);
        }

        public void EvaluateCommand(string input)
        {
            if (!input.StartsWith("!")) return;

            string command;
            string[] args;

            int spaceIndex = input.IndexOf(' ');
            if (spaceIndex < 0)
            {
                command = input;
                args = null;
            }
            else
            {
                command = input.Substring(0, spaceIndex);
                args = input.Substring(spaceIndex + 1).Split(' ');
            }

            var arr = Commands.Where(x => x.Name == command);
            if (arr == null || arr.Count() == 0 || arr.Count() > 1)
            {
                return;
            }
            
            _sender(arr.First().Eval(args));
        }
    }
}
