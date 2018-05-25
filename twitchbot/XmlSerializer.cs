using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace twitchbot
{
    class MyBinarySerializer : ISerializer
    {
        private string _file;

        BinaryFormatter formatter;

        public MyBinarySerializer(string file)
        {
            formatter = new BinaryFormatter();
            _file = file;
        }


        public IEnumerable<Command> Deserialize()
        {
            using (FileStream fs = new FileStream(_file, FileMode.OpenOrCreate))
            {
                IEnumerable<Command> deserilizeCommands = (IEnumerable<Command>)formatter.Deserialize(fs);

                return deserilizeCommands;
            }
        }

        public void Serialize(IEnumerable<Command> commands)
        {
            using (FileStream fs = new FileStream(_file, FileMode.OpenOrCreate))
            {
                // сериализуем весь массив people
                formatter.Serialize(fs, commands);

                Console.WriteLine("Объект сериализован");
            }
        }
    }

    class MyXmlSerializer : ISerializer
    {
        private string _file;

        public MyXmlSerializer(string file)
        {
            _file = file;   
        }

        public IEnumerable<Command> Deserialize()
        {
            throw new NotImplementedException();
        }

        public void Serialize(IEnumerable<Command> commands)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Command));
            using (TextWriter writer = new StreamWriter(_file))
            {
                serializer.Serialize(writer, commands.First());
            }
        }
    }

    interface ISerializer
    {
        void Serialize(IEnumerable<Command> commands);
        IEnumerable<Command> Deserialize();
    }
}
