using System;

namespace ChatStockApp.Models
{
    public class Message
    {
        public string User { get; set; }
        public string MessageText { get; set; }
        public DateTime DateTimeMsg { get; set; }
    }
}
