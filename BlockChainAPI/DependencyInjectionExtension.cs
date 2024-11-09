using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Services.Auth;
using BlockChainAPI.Utilities.ResponseMessage;
using BlockChainAPI.Repository;
using BlockChainAPI.Interfaces.IServices.IAuth;

namespace BlockChainAPI
{
    public static class DependencyInjectionExtension
    {

        public static IServiceCollection AddCRUDServices(this IServiceCollection services) {

            services.AddScoped<IUserRepository, UserService>();
            services.AddScoped<IMemPoolDocumentService, MemPoolDocumentService>();
            services.AddScoped<IConfigurationRepository, ConfigurationService>();
            services.AddTransient<IAuthService, AuthService>();
            return services;
        }

        public static IServiceCollection AddHelperServices(this IServiceCollection services) {
            services.AddTransient<MessageService>();
            return services;
        }
    }
}
