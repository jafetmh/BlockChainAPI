using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Services.Auth;
using BlockChainAPI.Utilities.ResponseMessage;
using BlockChainAPI.Repository;
using BlockChainAPI.Interfaces.IServices.IAuth;
using BlockChainAPI.Services.AppServices;
using BlockChainAPI.Interfaces.IServices.IAppServices;

namespace BlockChainAPI
{
    public static class DependencyInjectionExtension
    {

        public static IServiceCollection AddDataAccesServices(this IServiceCollection services) {

            services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IMemPoolDocumentService, MemPoolDocumentService>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IBlockRepository, BlockRepository>();
            services.AddScoped(typeof(IGenericDocumentRepository<>), typeof(GenericDocumentRepository<>));            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services) {
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<MessageService>();
            return services;
        }
    }
}
