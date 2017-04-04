using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using vk_dotnet.Local;

namespace vk_dotnet.Methods
{
    /// <summary>
    /// Base class for all other methods.
    /// </summary>
    public class Method
    {

        #region Static Members
        private static string _mainURI = "https://api.vk.com/method";

        public static async Task<string> SendGetAsync(string request_uri)
        {
            using (var cl = new HttpClient()) {

                var res = await cl.GetAsync(request_uri);
                return await res.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> SendPostAsync(string request_uri, string login, string password)
        {

            using (var cl = new HttpClient()) {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("act", "login"),
                    new KeyValuePair<string, string>("al_frame", "al_frame"),
                    new KeyValuePair<string, string>("_origin", "https://vk.com"),
                    new KeyValuePair<string, string>("utf8", "1"),
                    new KeyValuePair<string, string>("email", login),
                    new KeyValuePair<string, string>("pass", password),
                    new KeyValuePair<string, string>("lg_h", login),
                });
                var res = await cl.PostAsync(request_uri, content);
                return await res.Content.ReadAsStringAsync();
            }
        }



        #region Autorization with login and pass
        public static async Task<string> LoginPasswordAutorization(string login, string password)
        {
            string token;
            //If we have account settings localy
            if (LocalConfig.TheInstance.TryGetToken(login, out token)) {
                if (await tokenIsValid(token))
                    return token;
            }


            //+++++++++++
            Console.WriteLine("Getting access token: ");
            //+++++++++++
            string REGEX_LOGIN_HASH = @"lg_h=([a-zA-Z0-9]+)";
            string REGEX_ALLOW_TOKEN = "action=\"(.+)\"";
            string REGEX_GET_TOKEN = "access_token=([a-zA-Z0-9]+)";


            Dictionary<string, string> data_for_login = new Dictionary<string, string>();

            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler)) {
                ///by doing this request you store some initial cookie values,
                ///that might be used in the subsequent login request and checked by the server
                //+++++++++++
                Console.WriteLine("Sending get request to collect cookies..");
                //+++++++++++
                HttpResponseMessage res0 = new HttpResponseMessage();
                try {
                    res0 = await client.GetAsync("http://vk.com/");
                }
                catch (System.Net.Http.HttpRequestException e) {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e) {
                    Console.WriteLine("LoL");
                    Console.WriteLine(e.Message);
                    throw;

                }
                string res0_text = await res0.Content.ReadAsStringAsync();
                //+++++++++++
                Console.WriteLine("Status code : {0}", res0.StatusCode);
                //+++++++++++
                //Extracted login hash for sending post requset

                string lg_h = extractWithGivenPattern(REGEX_LOGIN_HASH, res0_text);
                //+++++++++++
                Console.WriteLine("Login hash = {0}", lg_h);
                //+++++++++++
                //+++++++++++
                Console.WriteLine("Login..");
                //+++++++++++
                //Form infrormation
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("act", "login"),
                    new KeyValuePair<string, string>("al_frame", "al_frame"),
                    new KeyValuePair<string, string>("_origin", "https://vk.com"),
                    new KeyValuePair<string, string>("utf8", "1"),
                    new KeyValuePair<string, string>("email", login),
                    new KeyValuePair<string, string>("pass", password),
                    new KeyValuePair<string, string>("lg_h", lg_h),
                });

                var res1 = await client.PostAsync("https://login.vk.com/", content);
                //Default reading stucks*******************
                using (System.IO.StreamReader sr = new System.IO.StreamReader(await res1.Content.ReadAsStreamAsync())) {
                    string s = sr.ReadToEnd();
                    if (s.Contains("onLoginFailed(4")) {
                        Console.WriteLine("INVALID LOGIN/PASS");
                        throw new Exception("INVALID LOGIN/PASS");
                    }
                }
                //*****************************************

                //+++++++++++
                Console.WriteLine("Status code : {0}", res1.StatusCode);
                //+++++++++++
                var cookies_col = cookieContainer.GetCookies(new Uri("https://login.vk.com/"));
                data_for_login.Add("p", cookies_col["p"].Value);
                data_for_login.Add("l", cookies_col["l"].Value);
                data_for_login.Add("remixsid", cookies_col["remixsid"].Value);

                HttpResponseMessage res2 = await client.GetAsync(
                    "https://oauth.vk.com/authorize?client_id=5933165&scope=33554431&response_type=token");
                //+++++++++++
                Console.WriteLine("Status code : {0}", res2.StatusCode);
                //+++++++++++
                string res2_text = await res2.Content.ReadAsStringAsync();

                string allow_token_uri = extractWithGivenPattern(REGEX_ALLOW_TOKEN,
                    res2_text);

                HttpResponseMessage res3 = await client.GetAsync(allow_token_uri);
                string token_uri = res3.RequestMessage.RequestUri.AbsoluteUri;
                token = extractWithGivenPattern(REGEX_GET_TOKEN, token_uri);
                if (await tokenIsValid(token)) {
                    LocalConfig.TheInstance.CasheToken(login, token);
                    return token;
                }


                throw new Exception("Invalid token validation");
            }
        }

        private async static Task<bool> tokenIsValid(string token)
        {
            string validate_req = $"https://api.vk.com/method/account.getInfo?&access_token={token}&v=5.62";
            JObject o;
            using (HttpClient cl = new HttpClient()) {
                var res = await cl.GetAsync(validate_req);
                string json = await res.Content.ReadAsStringAsync();
                o = JObject.Parse(json);
            }
            bool status = o["error"] == null;
            Console.WriteLine("Token is valid: {0}", status);
            return status;
        }

        private static string extractWithGivenPattern(string pattern, string text)
        {
            Regex p = new Regex(pattern);
            Match match = p.Match(text);
            return match.Groups[1].Value;

        }
        #endregion

        #endregion

        #region Private Fields
        protected string _token;

        #endregion

        #region Public Fields

        #endregion

        #region Constructors
        public Method(string token)
        {
            _token = token;
        }
        #endregion

        #region Public Methods
        public static string GetMethodUri(string method, params string[] parameters)
        {

            string prms = "";
            for (int i = 0; i < parameters.Length; i++) {
                prms += parameters[i] + '&';
            }

            string request_uri = $"{_mainURI}/{method}?{prms}&v=5.62";
            return request_uri;


        }
        #endregion


        #region Private Methods 



        private bool _isAnError(string response)
        {

            return false;
        }

        /// <summary>
        /// Makes request to the API with the specified URI;
        /// </summary>
        /// <param name="req">URI to request</param>
        /// <returns> Response in the string(json) format or throws a corresponding exception. </returns>
        public static async Task<string> CallApiAsync(string req)
        {
            string raw_json = await SendGetAsync(req);
            JObject o = JObject.Parse(raw_json);

            #region throwing exception if it is there
            if (o["error"] != null) {
                string s = o["error"].ToString();
                Error error = JsonConvert.DeserializeObject<Error>(s);                
                switch (error.error_code) {
                    case 5:
                        throw new AutorizationException(error);
                    default:
                        throw new ApiException(error);
                }
            } 
            #endregion
            try {

                string res = o["response"].ToString();
                return res;
            }
            catch (Newtonsoft.Json.JsonReaderException e) {
                Console.Write("Catched");
                throw JsonConvert.DeserializeObject<ApiException>(o["error"]
                    .ToString());

            }
            catch (Exception e) {

                throw;
            }
        }


        #endregion


    }
}
