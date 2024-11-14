using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlockChain_DB
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        public string UserN {  get; set; }   
        public string Name { get; set; }
        public string LastName  { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public byte[]? Salt { get; set; }

        [JsonIgnore] //ignores this property when a JSON serialization occurs on return of an http request
        public virtual Chain? Chain { get; set; }

        public virtual MemPool? MemPool { get; set; }
    }
}
