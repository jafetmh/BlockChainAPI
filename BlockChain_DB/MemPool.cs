using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_DB
{
    public class MemPool
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }
        public virtual ICollection<MemPoolDocument> Documents { get; set; } = new List<MemPoolDocument>();
    }
}
