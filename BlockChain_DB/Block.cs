using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_DB
{
    public class Block
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime MiningDate { get; set; }

        public int Attempts { get; set; }

        public int Milliseconds { get; set; }

        public string Previous_Hash { get; set; }

        public string Hash { get; set; }

        public int ChainID { get; set; }

        [ForeignKey(nameof(ChainID))]
        public virtual Chain Chain { get; set; }

        public virtual ICollection<Document> Documents { get; } = new List<Document>();
    }
}
