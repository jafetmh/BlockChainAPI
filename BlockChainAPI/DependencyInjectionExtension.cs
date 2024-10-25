using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Services.Auth;
using BlockChainAPI.Services;
using BlockChainAPI.Utilities.ResponseMessage;

namespace BlockChainAPI
{
    public static class DependencyInjectionExtension
    {

        public static IServiceCollection AddCRUDServices(this IServiceCollection services) {

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMemPoolDocumentService, MemPoolDocumentService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddTransient<IAuthService, AuthService>();
            return services;
        }

        public static IServiceCollection AddHelperServices(this IServiceCollection services) {
            services.AddTransient<MessageService>();
            return services;
        }
    }
}
