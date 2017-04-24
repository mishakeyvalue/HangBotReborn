using Hanggame;
using System;

using System.Threading;
using System.Threading.Tasks;
using vk_dotnet;

namespace HangBotReborn
{
    class Program
    {
        private static string _t = "cb8a8c455efa18e4d57cde56e25bb66dc56a33d56eae17d9ddade834c88fe21c5e052b114d1ab89869fbe";
        private static BotClient tootee = new BotClient(_t);
        static void Main(string[] args)
        {

            Task.Factory.StartNew( () => {GameBot b = new GameBot(tootee);});
            while (true) {
                Thread.Sleep(35000);
                tootee.SendTextMessageAsync("414460724", $"Alive at {DateTime.UtcNow}");
            }

        }


    }
}
