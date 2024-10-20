using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_DB
{
    public class Chain
    {
        public int Id { get; set; }

        public int UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }

        public virtual ICollection<Block> Blocks { get; set; } = new List<Block>();
    }
}
