using BlockChain_DB.Response;

namespace BlockChainAPI.Utilities
{
    public static class ResponseResult
    {

        public static Response<T> CreateResponse<T>(bool success, string? message, T data = default)
        {
            return new Response<T>
            {
                Success = success,
                Message = message,
                Data = data
            };
        }

    }
}
