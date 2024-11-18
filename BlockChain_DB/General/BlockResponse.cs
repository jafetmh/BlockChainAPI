namespace BlockChain_DB.General
{
    public class BlockResponse
    {
        public List<Block>? Blocks { get; set; }
        public List<Block>? InconsistentBlocks { get; set; }
        public List<Block>? AlteredBlocks { get; set; }
    }
}
