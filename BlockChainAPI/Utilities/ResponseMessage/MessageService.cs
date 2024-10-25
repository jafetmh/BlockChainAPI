using BlockChain_DB.General.Message;
using System.Text.Json;

namespace BlockChainAPI.Utilities.ResponseMessage
{
    public class MessageService
    {

        public Message Get_Message()
        {
            string rootPath = @"C:\Users\jafet\Documents\II Ciclo 2024\Info Aplicada\Proyecto\BlockChainAPI\Utilities\ResponseMessage\Message.json";
            var path = Path.Combine(rootPath);
            Console.WriteLine(path);
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Message>(json);
        }
    }
}
