using Hanggame;
using System;
using System.Threading;
using vk_dotnet;

namespace HangBotReborn
{
    class Program
    {
        private static string _t = "";
        private static BotClient tootee = new BotClient(_t);
        static void Main(string[] args)
        {

            GameBot b = new GameBot(tootee);
            while(true)
            Thread.Sleep(9999999);
        }


    }
}