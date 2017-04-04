using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using vk_dotnet.Methods;
using System.Threading.Tasks;

namespace vk_dotnet
{
    public class Session
    {

        #region Static Members

        #endregion

        #region Private Fields
        private string _token { get; set; }
        private string _login { get; set; }
        private string _pass { get; set; }
        #endregion

        #region API METHODS
        public Users_Methods Users;

        public Messages_Methods Messages;

        public LongPoll_Methods LongPollServer;

        public Account_Methods Account;
        private object my_id;

        /// <summary>
        /// Initializing Methods with the given token
        /// </summary>
        /// <param name="token">Token to be passed into methods</param>
        private void init(string token)
        {
            Users = new Users_Methods(token);
            Messages = new Messages_Methods(token);
            LongPollServer = new LongPoll_Methods(token);
            Account = new Account_Methods(token);
        }
        #endregion

        #region Constructors
        public Session(string token)
        {
            _token = token;
        }

        public Session(string login, string password)
        {
            _login = login;
            _pass = password;
        }

        #endregion

        #region Public Methods
        public void SignInAsync()
        {

            //if (_token == null) {
            //    _token = await Method.LoginPasswordAutorization(_login, _pass);
            //}
          //  my_id = Method.ValidateToken(_token);
            init(_token);
        }
        #endregion
    }
}
