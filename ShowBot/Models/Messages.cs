using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowBot.Models
{
    public class Messages
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public IList<string> Images { get; set; }
    }
}
