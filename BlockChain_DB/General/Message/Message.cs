using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_DB.General.Message
{
    public class Message
    {
        public MessageModel Success {  get; set; }
        public MessageModel Failure { get; set; }
        public string NotFound { get; set; }
    }
}
