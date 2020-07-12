using System;

namespace ChatStockApp.Models
{
    public class Message
    {
        public Message()
        {

        }
        public Message(string user, string messageText)
        {
            User = user;
            MessageText = messageText;
            DateTimeMsg = DateTime.Now.ToString();
        }
        public string User { get; set; }
        public string MessageText { get; set; }
        public string DateTimeMsg { get; set; }
    }
}
