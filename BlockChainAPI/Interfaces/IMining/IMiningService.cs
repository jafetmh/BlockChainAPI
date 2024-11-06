using BlockChain_DB;



namespace BlockChainAPI.Interfaces.IMining
{
    public interface IMiningService
    {
        Task MineBlockAsync(int blockId, int requiredZeros);
    }
}
