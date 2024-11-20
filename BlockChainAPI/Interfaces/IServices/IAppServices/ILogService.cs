namespace BlockChainAPI.Interfaces.IServices.IAppServices
{
    public interface ILogService
    {
        Task Log(string action, string user, string details = null);
    }
}
