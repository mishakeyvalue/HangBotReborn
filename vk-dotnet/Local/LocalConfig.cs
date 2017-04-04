using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace vk_dotnet.Local
{
    public class LocalConfig
    {
        private readonly string tokens_file = "loc_toc.json";
        private readonly string login_file = "login_config.json";


        private static LocalConfig _theInstance;
        public static LocalConfig TheInstance {

            get {
                if (_theInstance == null)
                    _theInstance = new LocalConfig();
                return _theInstance;
            }
        }
        private LocalConfig() { }
        private Dictionary<string, string> tokens;

        public bool TryGetLoginPass(out string login, out string pass)
        {
            string json = "";
            login = pass = "None";
            try
            {
                json = System.IO.File.ReadAllText(login_file);
            }
            catch(System.IO.FileNotFoundException err)
            {
                Console.WriteLine("Could not find {0} file with settings. Creting blank config file..", login_file);
                createEmptyLoginConfig();
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return false;
            }           
            Dictionary<string, string> login_dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);            
            return login_dict.TryGetValue("pass", out pass) && login_dict.TryGetValue("login", out login);

        }

        private void createEmptyLoginConfig()
        {
            Dictionary<string, string> login_conf = new Dictionary<string, string>()
            {
                { "login", "my_login/my_number"},
                { "pass", "my_password"},
                { "token", "my_token(optional)(dont use now)"},

            };
            string json = JsonConvert.SerializeObject(login_conf);
            System.IO.File.WriteAllText(login_file, json);
        }

        public bool TryGetToken(string login, out string token)
        {
            loadTokens();
            if (tokens.TryGetValue(login, out token))
                return true;
            return false;
        }

        public void CasheToken(string login, string token)
        {
            loadTokens();
            tokens.Remove(login);
            tokens.Add(login, token);
            string json = JsonConvert.SerializeObject(tokens);
            System.IO.File.WriteAllText(tokens_file, json);
        }

        /// <summary>
        /// Loading tokens from specified config file and instantiate proper dictionary ( tokens )
        /// If file
        /// </summary>
        private void loadTokens()
        {
            string json;
            try
            {
                json = System.IO.File.ReadAllText(tokens_file);
                if (json == String.Empty)
                    throw new ArgumentNullException("Config file is blank.");
            }
            //Something is wrong, instantiate <tokens> as an empty Dict
            catch (Exception e)
            {
                tokens = new Dictionary<string, string>();
                return;
            }
            tokens = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
    }
}
