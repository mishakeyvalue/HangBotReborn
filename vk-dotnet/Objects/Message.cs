using System;
using System.Collections.Generic;
using System.Text;

namespace vk_dotnet.Objects
{
    public class Message
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int From_id { get; set; }
        public int Date { get; set; }
        public int Read_state { get; set; }
        public int Out { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public List<Message> Fwd_messages { get; set; }
        public int Emoji { get; set; }
        public int Important { get; set; }
        public int Deleted { get; set; }
        public string Random_id { get; set; }


    }
}
