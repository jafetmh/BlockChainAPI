using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlockChain_DB
{
    public class Block
    {
        [Key]
        public int Id { get; set; }

        public DateTime MiningDate { get; set; }

        public int Attempts { get; set; }

        public int Milliseconds { get; set; }

        public string Previous_Hash { get; set; }

        public string Hash { get; set; }

        public int ChainID { get; set; }

        [ForeignKey(nameof(ChainID))]
        [JsonIgnore]
        public virtual Chain Chain { get; set; }

        public virtual ICollection<Document> Documents { get; } = new List<Document>();
    }
}
