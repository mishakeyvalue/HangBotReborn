using Hanggame;
using System;
using System.Threading;

namespace HangBotReborn
{
    class Program
    {
        static void Main(string[] args)
        {

            Hanggame.Hanggame g = new Hanggame.Hanggame(new Ch());
            g.PlayGame();
            Thread.Sleep(9999999);
        }

        public class Ch : IChannel
        {
            public string Read()
            {
                return Console.ReadLine();
            }

            public void WriteLine(string s)
            {
                Console.WriteLine(s);
            }
        }
    }
}