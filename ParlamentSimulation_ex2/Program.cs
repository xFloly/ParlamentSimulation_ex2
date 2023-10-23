using Microsoft.VisualBasic;
using ParlamentSimulation_ex2.ParlamentSimulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ParlamentSimulation_ex2
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Podaj liczbę parlamentarzystów");

            string input = Console.ReadLine() ?? "3";
            if (int.TryParse(input, out int x))
            {

                Console.WriteLine("Podaj temat głosowania:");
                string topic = Console.ReadLine() ?? "pieski lepsze niż kotki";
                topic = topic.Trim().ToLower();
                Parlament parlament = new(x, topic);

                while (true)
                {
                    string command = Console.ReadLine() ?? "X";
                    string[] commandParts = command.Split(new char[] { ' ' }, 2);
                    switch (commandParts[0])
                    {
                        case "POCZĄTEK":
                            parlament.StartVoting(commandParts[1] ?? "pieski lepsze niż kotki");
                            break;
                        case "KONIEC":
                            parlament.EndVoting();
                            break;
                        case "GŁOS":
                            if (int.TryParse(commandParts[1], out int index))
                            {
                                parlament.ParlimentarianList[index].Vote();
                            }
                            break;
                        default:
                            return;
                    }
                }
            }
        }
    }
}