using BlockChain_DB;
using BlockChain_DB.General.Message;
using BlockChain_DB.Response;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Interfaces.IRepository;
using BlockChainAPI.Interfaces.IServices.IAppServices;
using BlockChainAPI.Interfaces.IServices.ICrypto.AES;
using BlockChainAPI.Interfaces.IServices.ICrypto.SHA256;
using BlockChainAPI.Interfaces.IServices.Utilities;
using BlockChainAPI.Services.AppServices;
using NSubstitute;

namespace BlockChainAPI.test.AppServices.Test
{
    public class BlockServiceTest
    {

        private readonly IUserRepository _userRepository;
        private readonly IMemPoolDocumentService _memPoolDocumentService;
        private readonly IChainRepository _chainRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly IDocumentService _documentService;
        private readonly ILogService _logService;
        private readonly ISHA256Hash _sha256Hash;
        private readonly IAESEncryption _encryption;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMessageService _message;

        public BlockServiceTest(
            IUserRepository userRepository,
            IMemPoolDocumentService memPoolDocumentService,
            IChainRepository chainRepository,
            IBlockRepository blockRepository,
            IDocumentService documentService,
            ILogService logService,
            ISHA256Hash sha256Hash,
            IMessageService message

            ) {
            
            _userRepository = Substitute.For<IUserRepository>();
            _memPoolDocumentService = Substitute.For<IMemPoolDocumentService>();
            _chainRepository = Substitute.For<IChainRepository>();
            _blockRepository = Substitute.For<IBlockRepository>();
            _documentService = Substitute.For<IDocumentService>();
            _logService = Substitute.For<ILogService>();
            _sha256Hash = Substitute.For<ISHA256Hash>();
            _encryption = Substitute.For<IAESEncryption>();
            _configurationRepository = Substitute.For<IConfigurationRepository>();
            _message = Substitute.For<IMessageService>();

        }

        [Fact]
        public async void BuildBlockSuccessfullyOperation()
        {
            //mock filter list
            _memPoolDocumentService.FilterMemPoolDocument(Arg.Any<int>(), Arg.Any<List<MemPoolDocument>>())
                .Returns(Task.FromResult(new List<MemPoolDocument>
                {
                    new MemPoolDocument { Doc_encode="base64docencode" }
                }));
            //mock hash sha256
            _sha256Hash.GenerateHash(Arg.Any<string>()).Returns("0000abcde2406lm");
            //mock data access
            _userRepository.GetUser(Arg.Any<int>()).Returns(Task.FromResult(new Response<User> { Data = new User { Id = 1, Name="UserTestSuccess" } }));
            _chainRepository.CreateChain(Arg.Any<int>()).Returns(Task.FromResult(new Chain { Id = 1, Blocks = new List<Block>() }));
            _blockRepository.CreateBlock(Arg.Any<Block>()).Returns(Task.FromResult(1));
            _documentService.BulkCreateDocuments(Arg.Any<User>(), Arg.Any<List<Document>>(), Arg.Any<Block>()).Returns(Task.FromResult(new Response<Document> { Success = true }));

            BlockService blockservice = new BlockService(
                _userRepository,
                _blockRepository,
                _sha256Hash,
                _chainRepository,
                _documentService,
                _memPoolDocumentService,
                _encryption,
                _configurationRepository,
                _logService,
                _message
                );
            //Arrange
            int userId = 1;
            List<MemPoolDocument> documents = new List<MemPoolDocument>()
            {
                new MemPoolDocument { Doc_encode = "base64EncodedDocument1"},
                new MemPoolDocument { Doc_encode = "base64EncodedDocument2"},
                new MemPoolDocument { Doc_encode = "base64EncodedDocument3"}
            };

            //Act
            var response = await blockservice.BuildBlock(userId, documents);

            //Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal("0000abcde2406lm", response.Data.Hash);


        }
    }
}
