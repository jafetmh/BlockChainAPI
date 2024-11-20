using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public int? MemPoolID { get; set; }

        public bool isMined { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(MemPoolID))]
        public virtual MemPool? MemPool { get; set; }

    }
}
