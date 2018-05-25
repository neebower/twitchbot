using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twitchbot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            bot.Connect();
            Console.ReadLine();



            //CommandsController controller = new CommandsController(Send, //new MyXmlSerializer("commands.xml")
               // new MyBinarySerializer("commands.dat"));

            //var s = (Command[])new MyBinarySerializer("commands.dat").Deserialize();

            //foreach(var r in s)
            //{
             //   Console.WriteLine(r.Name);
            //}
           // controller.EvaluateCommand("!s");
        }

        private static void Send(string str)
        {
            Console.WriteLine(str);
        }
    }
}
