namespace ArtOfTime.Services
{
    using System.IO;
    using System.Threading.Tasks;

    using ArtOfTime.Interfaces;

    using Microsoft.Extensions.Configuration;

    using Nethereum.Contracts;
    using Nethereum.Hex.HexTypes;
    using Nethereum.Web3;
    using Nethereum.Web3.Accounts;

    public class EthereumService : IEthereumService
    {
        private readonly IConfiguration configuration;

        private readonly string privateKey;

        private readonly string contractAddress;

        private readonly string infura;

        private readonly string publicKey;

        public EthereumService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.privateKey = this.configuration["Ethereum:PrivateKey"];
            this.contractAddress = "0xBb79bc65360e1d588b2966f7b4B0c25185FB6165";
            this.infura = this.configuration["Ethereum:Infura"];
            this.publicKey = "0xD71d8029C6d070D9D282394B537deA8c4C44e8C4";
        }

        public async Task CreateToken(string tokenURI)
        {
            Contract contract = this.SetupContract(this.contractAddress, this.privateKey, this.infura);
            Function writingFunction = contract.GetFunction("create");
            await writingFunction.SendTransactionAsync(
                this.publicKey,
                new HexBigInteger(700000),
                new HexBigInteger(0),
                tokenURI);
        }

        public async Task<int> GetCollectionSize()
        {
            Contract contract = this.SetupContract(this.contractAddress, this.privateKey, this.infura);
            Function readingFunction = contract.GetFunction("tokenCounter");
            int result = await readingFunction.CallAsync<int>();
            return result;
        }

        private Contract SetupContract(string contractAddress, string privateKey, string infura)
        {
            var account = new Account(privateKey, 4);
            var web3 = new Web3(account, infura);
            string filePath = "LOUVRENFT.json";
            string abi = File.ReadAllText(filePath);
            return web3.Eth.GetContract(abi, contractAddress);
        }
    }
}
