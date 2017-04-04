using System;
using System.Collections.Generic;
using System.Text;

namespace vk_dotnet.Objects
{
    public class User
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string deactivated { get; set; }
        public int hidden { get; set; }
    }
}
