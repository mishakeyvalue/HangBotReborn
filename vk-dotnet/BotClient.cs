using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vk_dotnet.Local;
using vk_dotnet.Methods;
using vk_dotnet.Objects;

namespace vk_dotnet
{
    /// <summary>
    /// Wrapper for vk API;
    /// Used for automating tasks;
    /// </summary>
    public class BotClient
    {
        #region Static Members
        private static LocalConfig _configurator = LocalConfig.TheInstance;

        public static bool TryGetLogin(out string login, out string password)
        {
            return _configurator.TryGetLoginPass(out login, out password);
        }

        #endregion

        #region Private Fields
        private readonly Session _s;

        #endregion

        #region Public Fields
        public string _my_id { get; private set; }

        #endregion

        #region Constructors
        public BotClient(string login, string password)
        {
            _s = new Session(login, password);
            _s.SignInAsync();//.Wait();
            // _getMyId();
        }
        public BotClient(string token)
        {
            _s = new Session(token);
            _s.SignInAsync();// Wait();
            _getMyId();
        }

        #endregion

        #region Public Methods
        public void StartListening()
        {
            _s.LongPollServer.GetLongPollServer().Wait();
            Task.Factory.StartNew(_startListeningAsync);

        }

        public async Task SendTextMessageAsync(string user_id, string text)
        {
            await _s.Messages.Send(user_id, text);
        }


        #endregion

        #region Events
        public delegate void BotEventHandler<TArgs>(BotClient sender, TArgs e);

        public BotEventHandler<Message> IncomingMessage;

        private void OnIncomingMessage(Message arg)
        {
            if (IncomingMessage != null)
                IncomingMessage(this, arg);
        }

        #endregion

        #region Private Methods
        private async Task _startListeningAsync()
        {
            while (true) {
                List<List<string>> updates = await _s.LongPollServer.CallLongPoll();
                List<Message> incoming_messages = LongPoll_Methods.ParseEventForMessages(updates);
                foreach (var msg in incoming_messages) {
                    if (msg.User_id == 0) continue;
                    Message args = msg;
                    OnIncomingMessage(args);

                }
            }
        }

        private async Task<string> _getMyId()
        {
            string id = "";
            List<User> answer = await _s.Users.Get();
            User me = answer[0];
            return id;
        }
        #endregion

    }
}

/*

#region Static Members

#endregion

#region Private Fields

#endregion

#region Public Fields

#endregion

#region Constructors

#endregion

#region Public Methods

#endregion

#region Events

#endregion

#region Private Methods 

#endregion

*/
