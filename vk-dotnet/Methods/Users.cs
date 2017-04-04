using System;
using System.Net.Http;
using System.Threading.Tasks;
using vk_dotnet.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace vk_dotnet.Methods
{
    public class Users_Methods : Method
    {


        

        public Users_Methods(string token) : base(token)
        {
            
        }

        public async Task<List<User>> Get(params string[] user_ids)
        {
            string request;
            if (user_ids.Length == 0){
                request = GetMethodUri("users.get",
               $"access_token={_token}" );
            }
            else{
            request = GetMethodUri("users.get",
                $"user_ids={String.Join(",", user_ids)}");
            }


            string response = await SendGetAsync(request);

            Console.WriteLine(response);
            List<User> list_of_users = new List<User>();
            try
            {
                JToken json_users_array = JObject.Parse(response)["response"];
                foreach(var user in json_users_array)
                {
                    Console.WriteLine(user.ToString());
                    list_of_users.Add(JsonConvert.DeserializeObject<User>(user.ToString()));
                }
                
            }
            catch (System.Exception e)
            {
                
               Console.WriteLine(e.Message);
            }

 
            return list_of_users;
        }
    }
}