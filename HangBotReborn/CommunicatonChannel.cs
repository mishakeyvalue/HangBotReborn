using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Hanggame;

namespace Hangbot
{
    public class CommunicationChannel : Hanggame.IChannel
    {
        //*****************************************************IChannel implementation**********************

        public bool IsDead { get; set; }


        public void WriteLine(string s = "\n")
        {
            Output_Buffer = (s + "\n");
        }
        public string Read()
        {

            waiter.WaitOne();


            return Input_Buffer;
        }
        //*************************************************************************************************
        private EventWaitHandle waiter;
        private EventWaitHandle wait_for_bot_writes;

        private string _output_buffer;
        private string _input_buffer;
        private string _player;
        private Hanggame.Hanggame _game;
        public DateTime LastTouchedByUser;

        //*************************************************************************************************
        //                         Event sending
        public delegate void OutputBufferEventHandler(CommunicationChannel source, EventArgs args);

        public event OutputBufferEventHandler OutputIsReady;


        /// <summary>
        /// Notifies subscribers that Message is ready
        /// </summary>
        protected virtual void OnOutputIsReady()
        {
            if (OutputIsReady != null) {
                OutputIsReady(this, EventArgs.Empty);
            }
        }

        //*************************************************************************************************

        private CommunicationChannel()
        {
            waiter = new EventWaitHandle(false, EventResetMode.AutoReset);
            Game = new Hanggame.Hanggame(this);
            IsDead = false;

        }

        public CommunicationChannel(string target) : this()
        {

            this.Player = target;
            Task.Factory.StartNew(Game.PlayGame);
        }

        /// <summary>
        /// To read from
        /// </summary>
        public string Output_Buffer {
            get {
                // Accessed by user
                string temp = _output_buffer;
                _output_buffer = "";
                LastTouchedByUser = DateTime.Now;
                return temp;
            }

            set {
                // Accessed by game
                _output_buffer = value;
                Console.WriteLine("Game hase written something..");
                OnOutputIsReady();
                //_buffer_tower.ChimeBufferIncome(value);
            }
        }

        /// <summary>
        /// To write there 
        /// </summary>
        public string Input_Buffer {
            get {
                // Accessed by game
                string temp = _input_buffer;
                _input_buffer = "";
                return temp;
            }

            set {
                // Accessed by user
                _input_buffer = (value);
                LastTouchedByUser = DateTime.Now;
                waiter.Set();
            }
        }

        /// <summary>
        /// Ommiting double quotes and backslashes
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private string FuckingDeserealizationOfQuotesAndSlashesKostyl(string v)
        {
            string na_vyhod = "";
            for (int i = 0; i < v.Length; i++) {
                if (v[i] != '\\' && v[i] != '"') na_vyhod += v[i];
            }
            return na_vyhod;
        }

        public string Player {
            get {
                return _player;
            }

            set {
                _player = value;
            }
        }

        public Hanggame.Hanggame Game {
            get {
                return _game;
            }

            set {
                _game = value;
            }
        }

       
    }
}
