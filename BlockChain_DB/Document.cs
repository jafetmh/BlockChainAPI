using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_DB
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Owner { get; set; }

        public string FileType { get; set; }

        public DateTime CreationDate { get; set; }

        public long Size { get; set; }

        public string Doc_encode { get; set; }

        public int BlockID { get; set; }

        [ForeignKey(nameof(BlockID))]
        public virtual Block Block { get; set; }
    }
}
