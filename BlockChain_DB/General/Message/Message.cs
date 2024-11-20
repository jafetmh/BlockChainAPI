namespace BlockChain_DB.General.Message
{
    public class Message
    {
        public MessageModel Success { get; set; }
        public MessageModel Failure { get; set; }
        public string NotFound { get; set; }
        public string InvalidCredential { get; set; }
        public string InvalidMaxDocuments { get; set; }
        public string ChainInconsistency { get; set; }
        public string CorruptedBlocks { get; set; }
        public LogMessages LogMessages { get; set; }
        public string RepeatedDocument {  get; set; }
    }
}
