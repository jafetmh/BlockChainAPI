using BlockChain_DB.General.Message;
using System.Text.Json;

namespace BlockChainAPI.Utilities.ResponseMessage
{
    public class MessageService
    {
        public Message Get_Message()
        {
            // Obtener la ruta base del directorio actual de la aplicación
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = Path.Combine(basePath, "Utilities", "ResponseMessage", "Message.json");

            Console.WriteLine(relativePath);
            string json = File.ReadAllText(relativePath);
            return JsonSerializer.Deserialize<Message>(json);
        }
    }
}

