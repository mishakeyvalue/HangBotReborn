// Decompiled with JetBrains decompiler
// Type: Hanggame.Hanggame
// Assembly: Hanggame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C7A8E22D-179E-4C42-96CE-0CFEF30EB14A
// Assembly location: C:\Users\mitutee\Downloads\Hanggame.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hanggame
{
    public class Hanggame
    {
        public static List<string> Words = new List<string>();
        private static string words_src = "https://raw.githubusercontent.com/mitutee/GineAPI/master/Words.txt";
        private static bool wordsLoaded = false;
        public Encoding enc = Encoding.GetEncoding(65001);
        private List<string> positiveAnswers = new List<string>()
        {
      "y",
      "yes",
      "da",
      "d",
      "д",
      "да",
      "давай"
    };
        private List<string> negativeAnswers = new List<string>()
        {
      "n",
      "no",
      "net",
      "ne",
      "н",
      "нет",
      "не"
    };
        public IChannel Chat;
        private string _player;
        private bool _gameIsOn;

        public string Player {
            get {
                return this._player;
            }
            set {
                this._player = value;
            }
        }

        public bool GameIsOn {
            get {
                return this._gameIsOn;
            }
            set {
                this._gameIsOn = value;
            }
        }

        public Hanggame(IChannel ch)
        {
            this.Chat = ch;
        }

        private static async void loadWordsFromGithub()
        {
            try {
                HttpClient loader = new HttpClient();
                try {
                    using (StreamReader streamReader = new StreamReader(await loader.GetStreamAsync(Hanggame.words_src))) {
                        for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
                            Hanggame.Words.Add(str);
                    }
                }
                finally {
                    if (loader != null)
                        loader.Dispose();
                }
                loader = (HttpClient)null;
            }
            catch (Exception ex) {
            }
            Hanggame.wordsLoaded = true;
        }

        public void PlayGame()
        {
            if (!Hanggame.wordsLoaded)
                Hanggame.loadWordsFromGithub();
            this.GameIsOn = true;
            this.Chat.WriteLine("Приветствую тебя возле 'Виселицы'!Правила игры следующие:\n 1. Я загадываю слово;\n 2. Ты пытаешься отгадать его по одной букве..\n  3. За каждую неудачную попытку я добавляю часть тела к виселице\n 4. Ты проиграешь, когда на виселице появится полное тело!;\n\xD83D\xDC80");
            Task.Delay(TimeSpan.FromSeconds(2)).Wait();
            this.Chat.WriteLine("Я загадал слово!");
            bool flag = true;
            while (flag) {
                Hangsession hangsession = new Hangsession();
                do {
                    this.Chat.WriteLine(hangsession.DrawPicture() + hangsession.getFormalCurrentGuess() + "\nПопытайся догадаться, какая буква есть в этом слове..");
                    char ch;
                    while (true) {
                        string upper = this.Chat.Read().ToUpper();
                        if (upper.Length > 1) {
                            this.Chat.WriteLine("Угадывать нужно по одной букве! Попробуй еще раз!");
                        }
                        else {
                            ch = upper[0];
                            if (hangsession.IsGuessedAlready(ch))
                                this.Chat.WriteLine("Эта буква уже была! Попробуй еще раз!");
                            else
                                break;
                        }
                    }
                    if (hangsession.PlayGuess(ch)) {
                        if (hangsession.didWeWin()) {
                            this.Chat.WriteLine("Урра! ты победил!");
                            break;
                        }
                        this.Chat.WriteLine("Хм...");
                        Task.Delay(TimeSpan.FromSeconds(2)).Wait();
                        this.Chat.WriteLine("Действительно.. Эта буква есть в слове, ты угадал!");
                        Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                    }
                    else {
                        if (hangsession.didWeLose()) {
                            this.Chat.WriteLine(string.Format("Попытки закончились и ты проиграл!\n\xD83D\xDDA4\xD83D\xDDA4\xD83D\xDDA4\xD83D\xDDA4\xD83D\xDDA4\xD83D\xDDA4\n Загаданное слово: \n{0}", (object)hangsession.mysteryWord));
                            break;
                        }
                        this.Chat.WriteLine(string.Format("Не-а.. -* {0} *- в слове нет!\n", (object)ch) + "Минус одна попытка, на шаг ближе к проигрышу!");
                        Task.Delay(TimeSpan.FromSeconds(2)).Wait();
                    }
                }
                while (!hangsession.GameOver());
                while (true) {
                    this.Chat.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\nЖелаешь сыграть еще?");
                    string lower = this.Chat.Read().ToLower();
                    if (!this.positiveAnswers.Contains(lower)) {
                        if (!this.negativeAnswers.Contains(lower))
                            this.Chat.WriteLine("Я не расслышал твоего ответа.. Будь добр, повтори.");
                        else
                            goto label_20;
                    }
                    else
                        break;
                }
                this.Chat.WriteLine("Отлично! Начинаем по новой!");
                flag = true;
                continue;
                label_20:
                this.Chat.WriteLine("Ну что ж! Увидимся в следующий раз!\nНапиши .начать , когда захочешь сыграть еще!");
                flag = false;
                this.GameIsOn = false;
            }
        }
    }
}
