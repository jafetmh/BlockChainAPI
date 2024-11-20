using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlockChain_DB
{
    public class Document
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Owner { get; set; }

        public string FileType { get; set; }

        public DateTime CreationDate { get; set; }

        public long Size { get; set; }

        public string Doc_encode { get; set; }

        public int BlockID { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(BlockID))]
        public virtual Block? Block { get; set; }

        //convert to Document
        public static Document FromMempoolDocument(MemPoolDocument document)
        {
            return new Document
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
