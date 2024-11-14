namespace BlockChain_DB.DTO
{
    public class DocumentDTO
    {
        public int? Id { get; set; }
        public string Owner { get; set; }

        public string FileType { get; set; }

        public DateTime CreationDate { get; set; }

        public long Size { get; set; }

        public string Doc_encode { get; set; }
    }
}
