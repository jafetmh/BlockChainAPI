using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_DB.General.Message
{
    public class MessageModel
    {
        public string Get {  set; get; }
        public string Set { set; get; }
        public string Modify { set; get; }
        public string Remove { set; get; }
    }
}
