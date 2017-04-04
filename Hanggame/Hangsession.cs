// Decompiled with JetBrains decompiler
// Type: Hanggame.Hangsession
// Assembly: Hanggame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C7A8E22D-179E-4C42-96CE-0CFEF30EB14A
// Assembly location: C:\Users\mitutee\Downloads\Hanggame.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace Hanggame
{
    public class Hangsession
    {
        private List<char> previousGuesses = new List<char>();
        private List<string> words = new List<string>();
        private Random r = new Random();
        private List<string> reserve_words = new List<string>()
        {
      "корова",
      "гриб",
      "обезъяна",
      "лавина",
      "апельсин",
      "свеча",
      "зонт",
      "карман",
      "диплом",
      "бочонок",
      "ручка",
      "ананас",
      "салфетка",
      "йогурт"
    };
        public string mysteryWord;
        private StringBuilder currentGuess;
        private const int MAX_TRIES = 6;
        private int currentTry;

        public Hangsession()
        {
            this.words = Hanggame.Words;
            this.mysteryWord = this.pickWord().ToUpper();
            this.currentGuess = this.initializeCurrentGuess();
        }

        public string getFormalCurrentGuess()
        {
            return "\nСлово: " + this.currentGuess.ToString();
        }

        private StringBuilder initializeCurrentGuess()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < this.mysteryWord.Length * 2; ++index) {
                char ch = index % 2 == 0 ? '_' : ' ';
                stringBuilder.Append(ch);
            }
            return stringBuilder;
        }

        private string pickWord()
        {
            if (this.words.Count == 0)
                return this.reserve_words[this.r.Next(0, this.reserve_words.Count)];
            return this.words[this.r.Next(0, this.words.Count - 1)];
        }

        internal bool IsGuessedAlready(char c)
        {
            return this.previousGuesses.Contains(c);
        }

        internal bool GameOver()
        {
            return this.didWeWin() || this.didWeLose();
        }

        public bool didWeLose()
        {
            return this.currentTry >= 6;
        }

        public bool didWeWin()
        {
            return this.getCondensendCurrentGuess().Equals(this.mysteryWord);
        }

        public string getCondensendCurrentGuess()
        {
            return this.currentGuess.ToString().Replace(" ", "");
        }

        internal bool PlayGuess(char guess)
        {
            bool flag = false;
            this.previousGuesses.Add(guess);
            for (int index = 0; index < this.mysteryWord.Length; ++index) {
                if ((int)this.mysteryWord[index] == (int)guess) {
                    flag = true;
                    this.currentGuess.Replace('_', guess, index * 2, 1);
                }
            }
            if (!flag)
                this.currentTry = this.currentTry + 1;
            return flag;
        }

        public string DrawPicture()
        {
            switch (this.currentTry) {
                case 0:
                    return this.noPersonDraw();
                case 1:
                    return this.addHeadDraw();
                case 2:
                    return this.addBodyDraw();
                case 3:
                    return this.addOneArmDraw();
                case 4:
                    return this.addSecondArmDraw();
                case 5:
                    return this.addFirstLegDraw();
                default:
                    return this.fullPersonDraw();
            }
        }

        private string fullPersonDraw()
        {
            return "   _____\n  |     |\n  |     O\n  |    \\|/\n  |     |\n  |    / \n  |\n__|__\nПопыток осталось: ПОПЫТКИ ЗАКОНЧИЛИСЬ";
        }

        private string addFirstLegDraw()
        {
            return "   _____\n  |     |\n  |     O\n  |    \\|/\n  |     |\n  |    / \\\n  |\n__|__\nПопыток осталось: ❤ || \xD83D\xDDA4\xD83D\xDDA4\xD83D\xDDA4\xD83D\xDDA4\xD83D\xDDA4";
        }

        private string addSecondArmDraw()
        {
            return "   _____\n  |     |\n  |     O\n  |    \\|/\n  |      |\n  |\n  |\n__|__\nПопыток осталось: ❤\xD83D\xDC99 || \xD83D\xDDA4\xD83D\xDDA4\xD83D\xDDA4\xD83D\xDDA4";
        }

        private string addOneArmDraw()
        {
            return "   _____\n  |     |\n  |     O\n  |    \\|\n  |\n  |\n  |\n__|__\nПопыток осталось: ❤\xD83D\xDC99\xD83D\xDC9A || \xD83D\xDDA4\xD83D\xDDA4\xD83D\xDDA4";
        }

        private string addBodyDraw()
        {
            return "   _____\n  |     |\n  |     O\n  |    \\|/n  |\n  |\n  |\n__|__\nПопыток осталось: ❤\xD83D\xDC99\xD83D\xDC9A\xD83D\xDC9B || \xD83D\xDDA4\xD83D\xDDA4";
        }

        private string addHeadDraw()
        {
            return "   _____\n  |     |\n  |     O\n  |     |\n  |\n  |\n  |\n__|__\nПопыток осталось: ❤\xD83D\xDC99\xD83D\xDC9A\xD83D\xDC9B\xD83D\xDC9C || \xD83D\xDDA4";
        }

        private string noPersonDraw()
        {
            return "   _____\n  |     |\n  |     \n  |\n  |\n  |\n  |\n__|__\nПопыток осталось: ❤\xD83D\xDC99\xD83D\xDC9A\xD83D\xDC9B\xD83D\xDC9C❤";
        }
    }
}
