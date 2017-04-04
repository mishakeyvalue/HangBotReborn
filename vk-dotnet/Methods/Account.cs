using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using vk_dotnet.Objects;

namespace vk_dotnet.Methods
{
    public class Account_Methods : Method
    {
        public Account_Methods(string token) : base(token)
        {
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetInfo()
        {
            string request = GetMethodUri("account.getInfo",
                $"access_token={_token}");
            string response = await SendGetAsync(request);

            User me = new User();

            Console.WriteLine(response);
            return me;
        }

        public async Task<User> GetProfileInfo()
        {

            string request = GetMethodUri("account.getInfo",
                $"access_token={_token}");
            string response = await SendGetAsync(request);
            Console.WriteLine(response);
            User me = new User();

            return me;

        }
    }
}
