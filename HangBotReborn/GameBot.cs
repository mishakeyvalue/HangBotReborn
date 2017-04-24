using Hangbot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using vk_dotnet;
using vk_dotnet.Objects;

namespace HangBotReborn
{
    class GameBot
    {
        private BotClient vk_bot;

        private Dictionary<string, CommunicationChannel> games =
            new Dictionary<string, CommunicationChannel>();

        public GameBot(BotClient vk_bot)
        {
            this.vk_bot = vk_bot;
            vk_bot.IncomingMessage += HandleNewMessage;
            Task.Factory.StartNew(_gameGarbageCollector);
            vk_bot.StartListening();
        }

        private void HandleNewMessage(BotClient sender, Message e)
        {
            string from = e.User_id.ToString();
            string msg = e.Body;
            /// Game is already running;
            /// Keep playing;
            if (games.ContainsKey(from)) {

                if (games[from].IsDead) {
                    games.Remove(from);
                    goto start_new_game;
                }
                Console.WriteLine("We are playing. Sending data to game input");
                games[from].Input_Buffer = msg;
                return;
            }


            start_new_game:
            if (WantsStartTheGame(msg)) {
                /// Starting the new game

                CommunicationChannel new_channel = new CommunicationChannel(from);
                new_channel.OutputIsReady += OutputHandler;

                games.Add(from, new_channel);
            }
            else if (DontWantsStartTheGame(msg)) {
                // Dispose
                games.Remove(from);

                vk_bot.SendTextMessageAsync(from, a_for_n_a());
            }
            else {
                string answer = defaultMsg();
                vk_bot.SendTextMessageAsync(from, answer);
            }
        }

        private void OutputHandler(CommunicationChannel source, EventArgs args)
        {
            vk_bot.SendTextMessageAsync(source.Player, source.Output_Buffer);
        }

        private string defaultMsg()
        {
            return defaultMsgs[new Random().Next(0, defaultMsgs.Count - 1)];
        }

        private string a_for_n_a()
        {
            return answersForNegativeAnswers[new Random().Next(0, answersForNegativeAnswers.Count - 1)];
        }


        private bool WantsStartTheGame(string text)
        {
            text = (text.ToLower());
            return answersToInitializeTheGame.Contains(text);
        }

        private bool DontWantsStartTheGame(string text)
        {
            text = (text.ToLower());
            return negativeAnswers.Contains(text);
        }

        private void _gameGarbageCollector()
        {
            while (true) {
                Thread.Sleep(3000);
                foreach (var kvp in games) {
                    if ((DateTime.Now - kvp.Value.LastTouchedByUser).Minutes > 5) {
                        games.Remove(kvp.Key);
                        break;
                    }
                }
            }
        }

        #region Answers For Negative Answers
        private List<string> answersForNegativeAnswers = new List<string>() {
            "Ну что же...Так уж и быть!",
            "Понятно😰",
            "Когда захочешь поиграть - просто напиши мне!",
        };

        #endregion

        #region NegativeAnswers
        private List<string> negativeAnswers = new List<string>() {
            "n",
            "no",
            "net",
            "ne",
            "н",
            "нет",
            "не",
        };
        #endregion

        #region PositiveAnswers
        private List<string> answersToInitializeTheGame = new List<string>() {
            "y",
            "yes",
            "da",
            "go",

            "оккей",
            "может быть",
            "дат",
            "да",
            "давай",
            "д",
            "ок",
            "го",
            ".начать",

            "ебаш",
            "ну можно",
            "давай сыграем",
            "хорошо",
            "я не против",
            "валяй",
            "действуй",
            "было бы неплохо",
            "конечно",
            "хочу",
        };
        #endregion

        #region Default Answers
        private List<string> defaultMsgs = new List<string>() {
            "Хочешь поиграть в виселицу? 😎(напиши 'да', к примеру)",
            "Привет! Можем сыграть с тобой в 'Виселицу', если хочешь 😊",
            "😜 Давай играть в 'Виселицу!' Хочешь?",
            "Мне скучно, может сыграем в висельника?",
            "Просто скажи мне да, и игра начнется!",
            "Хочешь ли ты в игру?"

        };

        #endregion    }
    }
}
