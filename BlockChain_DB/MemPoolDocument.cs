using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_DB
{
    public class MemPoolDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Owner { get; set; }

        public string FileType { get; set; }

        public DateTime CreationDate { get; set; }

        public long Size { get; set; }

        public string Doc_encode { get; set; }

        public int MemPoolID { get; set; }

        [ForeignKey(nameof(MemPoolID))]
        public virtual MemPool? MemPool { get; set; }

        //convert to MemPoolDocument
        public static MemPoolDocument FromDocument(Document document)
        {
            return new MemPoolDocument
            {
                Id = document.Id,
                Owner = document.Owner,
                FileType = document.FileType,
                CreationDate = document.CreationDate,
                Size = document.Size,
                Doc_encode = document.Doc_encode
            };
        }
    }
}
