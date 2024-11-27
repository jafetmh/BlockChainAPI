namespace BlockChainAPI.Interfaces.IServices.IAppServices
{
    public interface ILogService
    {
        Task Log(string action, string user, object? details = default);
    }
}
