using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanOS.Commands
{
    public class CommandManager
    {
        private List<Command> commands;
        

        public CommandManager()
        {
            this.commands = new List<Command>(1);
        }


        public String processInput(String input)
        {
            String[] split = input.Split(' ');
            String label = split[0];
            List<String> args = new List<String>();
            int ctr = 0;
            foreach (String s in split)
            {
                if (ctr != 0)
                    args.Add(s);
                ++ctr;
            }

            foreach(Command cmd in this.commands)
            {
                if(cmd.name == label)
                    cmd.execute(args.ToArray());
            }
            return "Unknown command";
        }

    }
}
