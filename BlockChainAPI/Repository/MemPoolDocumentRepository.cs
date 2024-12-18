﻿using BlockChain_DB.DTO;
using BlockChain_DB.General.Message;
using BlockChain_DB;
using BlockChainAPI.Interfaces.IRepository;
using BlockChainAPI.Utilities;
using BlockChain_DB.Response;
using BlockChainAPI.Utilities.ResponseMessage;
using BlockChainAPI.Interfaces.IServices.Utilities;

namespace BlockChainAPI.Repository
{
    public class MemPoolDocumentRepository: IMemPoolDocumentRepository
    {
        private readonly IMempoolRepository _mempoolRepository;
        private readonly Message _message;

        public MemPoolDocumentRepository(IMempoolRepository mempoolRepository, IMessageService message) { 
            _mempoolRepository = mempoolRepository;
            _message = message.Get_Message();
        }

        //Get all user mempool docs
        public async Task<Response<List<MemPoolDocument>>> GetMempoolDocuments(int userId)
        {

            List<MemPoolDocument> documents = new();
            try
            {
                Response<MemPool> result = await _mempoolRepository.GetMempool(userId);
                MemPool memPool = result.Data;
                if (memPool == null)
                {
                    return ResponseResult.CreateResponse<List<MemPoolDocument>>(false, _message.NotFound);
                }
                documents = memPool.Documents.ToList();

                return ResponseResult.CreateResponse(true, _message.Success.Get, documents);
            }
            catch { throw; };
        }

        //Get ById
        public async Task<Response<DocumentDTO>> GetMempoolDocumentById(int userId, int documentId)
        {
            try
            {
                Response<MemPool> result = await _mempoolRepository.GetMempool(userId);
                if (!result.Success) { return ResponseResult.CreateResponse<DocumentDTO>(false, _message.Failure.Get); }
                MemPoolDocument document = result.Data.Documents
                        .FirstOrDefault(d => d.Id == documentId);

                if (document != null)
                {
                    var documentDto = new DocumentDTO
                    {
                        Id = document.Id,
                        Owner = document.Owner,
                        FileType = document.FileType,
                        CreationDate = document.CreationDate,
                        Size = document.Size,
                        Doc_encode = document.Doc_encode,
                    };

                    return ResponseResult.CreateResponse(true, _message.Success.Get, documentDto);
                }
                return ResponseResult.CreateResponse<DocumentDTO>(false, _message.Failure.Get);
            }
            catch { throw; }
        }


        public async Task<Response<List<DocumentDTO>>> GetDocumentsByIds(int userId, List<int> documentIds)
        {
            try
            {
                // Obtener el mempool del usuario
                Response<MemPool> result = await _mempoolRepository.GetMempool(userId);
                if (!result.Success) return ResponseResult.CreateResponse<List<DocumentDTO>>(false, _message.Failure.Get);
                MemPool memPool = result.Data;

                // Filtrar los documentos por los IDs proporcionados
                List<DocumentDTO> documents = memPool.Documents
                    .Where(doc => documentIds.Contains(doc.Id))
                    .Select(doc => new DocumentDTO
                    {
                        Id = doc.Id,
                        Owner = doc.Owner,
                        FileType = doc.FileType,
                        CreationDate = doc.CreationDate,
                        Size = doc.Size,
                        Doc_encode = doc.Doc_encode
                    })
                    .ToList();

                return ResponseResult.CreateResponse(true, _message.Success.Get, documents);
            }
            catch (Exception ex)
            {
                return ResponseResult.CreateResponse<List<DocumentDTO>>(false, _message.Failure.Get, ex.Message);
            }
        }







    }
}
