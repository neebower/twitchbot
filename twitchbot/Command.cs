using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twitchbot
{
   
    [Serializable]
    public class Command : ICommand
    {
        public string Name { get; }

        public string Description { get; }

        public Evaluate Eval { get; }

        public Command(string name, string description, Evaluate eval)
        {
            Name = name;
            Description = description;
            Eval = eval;
        }

        public Command()
        {

        }
    }


    interface ICommand
    {
        Evaluate Eval { get; }

        String Name { get; }

        String Description { get; }
    }
}
