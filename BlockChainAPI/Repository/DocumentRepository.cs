using BlockChain_DB;
using BlockChainAPI.Interfaces.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BlockChainAPI.Repository
{
    public class DocumentRepository: IDocumentRepository
    {
        private readonly BlockChainContext _context;

        public DocumentRepository(BlockChainContext context)
        {
            _context = context;
        }

        public async Task BulkCreateDocuments(List<Document> documents)
        {
            if (documents == null || !documents.Any())
            {
                Console.WriteLine("No hay documentos para insertar");
                return;
            }

            try
            {
                await _context.Documents.AddRangeAsync(documents);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar documentos: {ex.Message}");
            }
        }

    }
}
