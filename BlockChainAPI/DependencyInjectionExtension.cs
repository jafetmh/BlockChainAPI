using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Services.Auth;
using BlockChainAPI.Utilities.ResponseMessage;
using BlockChainAPI.Repository;
using BlockChainAPI.Interfaces.IServices.IAuth;
using BlockChainAPI.Services.AppServices;
using BlockChainAPI.Interfaces.IServices.IAppServices;
using BlockChainAPI.Interfaces.IRepository;
using BlockChainAPI.Interfaces.IServices.ICrypto.AES;
using BlockChainAPI.Services.Crypto.AES;
using BlockChainAPI.Interfaces.IServices.ICrypto.SHA256;
using BlockChainAPI.Services.Crypto.SHA_256;

namespace BlockChainAPI
{
    public static class DependencyInjectionExtension
    {

        public static IServiceCollection AddDataAccesServices(this IServiceCollection services) {

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IBlockRepository, BlockRepository>();
            services.AddScoped<IChainRepository, ChainRepository>();
            services.AddScoped<IMemPoolDocumentRepository, MemPoolDocumentRepository>();
            services.AddScoped<IMempoolRepository, MempoolRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped(typeof(IGenericDocumentRepository<>), typeof(GenericDocumentRepository<>));
            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services) {
            services.AddScoped<IBlockService, BlockService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IMemPoolDocumentService, MemPoolDococumentService>();
            services.AddScoped<ICryptography, Cryptography>();
            services.AddScoped<ISHA256Hash, SHA256Hash>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddScoped<IAESEncryption, AESEncryption>();
            services.AddScoped<ILogService, LogService>();
            services.AddTransient<MessageService>();
            return services;
        }
    }
}
