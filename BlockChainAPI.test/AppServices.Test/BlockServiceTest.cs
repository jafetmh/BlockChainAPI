using BlockChain_DB;
using BlockChain_DB.General;
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
using System.Text;

namespace BlockChainAPI.test.AppServices.Test
{
    public class BlockServiceTest
    {

        private IUserRepository _userRepository;
        private IMemPoolDocumentService _memPoolDocumentService;
        private IChainRepository _chainRepository;
        private IBlockRepository _blockRepository;
        private IDocumentService _documentService;
        private ILogService _logService;
        private ISHA256Hash _sha256Hash;
        private IAESEncryption _encryption;
        private IConfigurationRepository _configurationRepository;
        private IMessageService _message;

        private BlockService _blockService;

        public BlockServiceTest() {
            
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

        // build block successfully
        [Fact]
        public async void BuildBlock_ShouldBe_SuccessfullyOperation()
        {


            //mock filter list
            _memPoolDocumentService.FilterMemPoolDocument(Arg.Any<int>(), Arg.Any<List<MemPoolDocument>>())
                .Returns(Task.FromResult(new List<MemPoolDocument>
                {
                    new MemPoolDocument {Id=1, Doc_encode="base64docencode" }
                }));
            //mock hash sha256
            _sha256Hash.GenerateHash(Arg.Any<string>()).Returns("0000abcde2406lm");
            //mock data access
            _userRepository.GetUser(Arg.Any<int>()).Returns(Task.FromResult(new Response<User> { Success=true, Data = new User { Id = 1, Name="UserTestSuccess" } }));
            _chainRepository.CreateChain(Arg.Any<int>()).Returns(Task.FromResult(new Chain { Id = 1, Blocks = new List<Block>() }));
            _blockRepository.CreateBlock(Arg.Any<Block>()).Returns(Task.FromResult(1));
            _documentService.BulkCreateDocuments(Arg.Any<User>(), Arg.Any<List<Document>>(), Arg.Any<Block>()).Returns(Task.FromResult(new Response<Document> { Success = true }));

            //message mock
            _message.Get_Message().Returns(new Message { 
                Success = new MessageModel { Set = "Guardado correctamente" },
                Failure = new MessageModel { Set = "Error al crear" },
                LogMessages = new LogMessages { CreateBlock = "Creación de bloques."}
                });


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
                new MemPoolDocument { Id=1, Doc_encode = "base64docencode"}
            };

            //Act
            var response = await blockservice.BuildBlock(userId, documents);

            //Assert
            await _logService.Received().Log(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<object>());
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal("0000abcde2406lm", response.Data.Hash);

        }

        // MiningBlock successfully
        [Fact]
        public void MiningBlock_ShouldMineBlock_WithValidHash()
        {
            //Arrange
            Block block = new Block() { Previous_Hash = "00000", MiningDate = DateTime.UtcNow, Attempts = 0 };
            User user = new User() { Id = 1, Name = "MiningTestUser" };
            string docsBase64 = "exampleBase64data";

            string expectedHash = "0000abcdef123456"; // Hash válido con "0000"
            _sha256Hash.GenerateHash(Arg.Any<string>()).Returns(expectedHash);

            BlockService blockService = new BlockService(
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
            // Act
            blockService.MiningBlock(block, user, docsBase64);

            // Assert
            Assert.Equal(expectedHash, block.Hash);
            Assert.True(block.Attempts > 0); 
            Assert.True(block.Milliseconds > 0);
        }

        //Get Blocks Successfully
        [Fact]
        public async void GetBlocks_ShouldWorkCorrectly()
        {
            _blockRepository.GetBlocks(Arg.Any<int>()).Returns(Task.FromResult( new Response<List<Block>>
            {
                Success = true,
                Data = new List<Block>() {
                new Block() { Id=1, Documents = new List<Document>(){
                    new Document() { Id = 1, Doc_encode = "SGVsbG8sIHdvcmxkIQ==" },
                    new Document() { Id = 2, Doc_encode = "VGhpcyBpcyBhIHRlc3Qu" }
                }}}}));
            _encryption.DecryptDocument(Arg.Any<byte[]>()).Returns(call =>
            {
                var inputBytes = call.Arg<byte[]>();
                return Task.FromResult(Encoding.UTF8.GetString(inputBytes));
            });

            _userRepository.GetUser(Arg.Any<int>()).Returns(Task.FromResult(new Response<User> { Success = true, Data = new User { Id = 1, Name = "UserTest" }}));
            //message mock
            _message.Get_Message().Returns(new Message
            {
                Success = new MessageModel { Get = "Recuperado correctamente" },
                LogMessages = new LogMessages { ChainValidation = "Validacion de cadena.", AlteredBlocks = "Integridad de cadena." }

            });

            BlockService blockService = new BlockService(
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

            //Act
            Response<BlockResponse> blocks = await blockService.GetBlocks(userId);

            //Assert
            Assert.True( blocks.Success);
            Assert.IsType<BlockResponse>(blocks.Data);
            Assert.True(blocks.Data.Blocks.Count > 0);

        }

    }
}
