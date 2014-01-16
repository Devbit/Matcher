using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matcher;
using System.Diagnostics;
using System.Threading;

//           ■■■■   ■■■■■■              
//           ■■■■   ■■■■■■       © Copyright 2014         
//           ■■■■         ■■■     _____             _     _ _           _
//           ■■■■   ■■■   ■■■    |  __ \           | |   (_) |         | |
//           ■■■■   ■■■   ■■■    | |  | | _____   _| |__  _| |_   _ __ | |
//           ■■■■         ■■■    | |  | |/ _ \ \ / / '_ \| | __| | '_ \| |     
//           ■■■■■■■■■■■■■       | |__| |  __/\ V /| |_) | | |_ _| | | | |
//           ■■■■■■■■■■■■■       |_____/ \___| \_/ |_.__/|_|\__(_)_| |_|_|


namespace Matcher.Commander
{
    class Program
    {
        private static string _Header = "Devbit Ultimate Super Matcher 2014";

        private static MatcherCommander commander;
        static void Main(string[] args)
        {
            Console.SetWindowSize(_Header.Length, 10);
            Console.BufferWidth = _Header.Length;
            Console.BufferHeight = 10;
            commander = new MatcherCommander("http://127.0.0.1:28000", false);
            WriteMainMenu();
        }

        private static void WriteHead()
        {
            Console.WriteLine();
            Console.WriteLine(_Header);
            Console.WriteLine(" Matchers running: " + commander.GetMatcherCount());
        }

        private static int WriteMatcherList()
        {
            Update();
            string status = commander.GetMatcherStatus();
            int lines = ((status.Length - status.Replace(Environment.NewLine, "").Length) / Environment.NewLine.Length);
            if (lines + 6 > Console.BufferHeight)
            {
                Console.BufferHeight = lines + 6;
                Console.SetWindowSize(Console.BufferWidth, lines + 6);
            }
            Console.Write(status);
            return lines;
        }

        private static void WriteMainMenu()
        {
            Update();
            Console.WriteLine(" [1] - Start a Matcher");
            Console.WriteLine(" [2] - Stop a Matcher");
            Console.WriteLine(" [3] - View running Matchers");
            Console.Write(" Enter input: ");
            string key = Console.ReadKey().KeyChar.ToString();
            if (!new string[3] { "1", "2", "3" }.Contains(key))
            {
                Update();
                Console.WriteLine(" Invalid input, try again");
                Console.ReadKey();
                WriteMainMenu();
                return;
            }
            switch (key)
            {
                case "1":
                    WriteSpawnMenu();
                    break;
                case "2":
                    WriteStopMenu();
                    break;
                case "3":
                    WriteStatusMenu();
                    break;
                default:
                    break;
            }
        }

        private static void WriteSpawnMenu()
        {
            commander.SpawnMatcher();
            Update();
            Console.WriteLine(" Matcher spawned");
            Thread.Sleep(500);
            WriteMainMenu();
        }

        private static void WriteStopMenu()
        {
            int amount = WriteMatcherList();
            if (amount == 0)
            {
                Console.Write(" No Matchers running");
                Thread.Sleep(1000);
                WriteMainMenu();
                return;
            }
            Console.Write(" Enter input: ");
            string input = Console.ReadLine();
            int result;
            if (int.TryParse(input, out result))
            {
                if (result < amount)
                {
                    commander.StopMatcher(result);
                    Console.SetWindowSize(Console.BufferWidth, 10);
                    Console.BufferHeight = 10;
                    Update();
                    Console.WriteLine(" Matcher stopped");
                    Thread.Sleep(1000);
                    WriteMainMenu();
                    return;
                }
            }
            Update();
            Console.WriteLine(" Invalid input, try again");
            Console.ReadKey();
            WriteStopMenu();
        }

        private static void WriteStatusMenu()
        {
            WriteMatcherList();
            Console.ReadKey();
            Console.SetWindowSize(Console.BufferWidth, 10);
            Console.BufferHeight = 10;
            WriteMainMenu();
        }

        private static void Update()
        {
            Console.Clear();
            WriteHead();
            Console.WriteLine();
        }
    }
}
