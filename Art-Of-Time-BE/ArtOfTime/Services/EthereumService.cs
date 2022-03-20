using System.IO;
using System.Threading.Tasks;

using ArtOfTime.Interfaces;

using Microsoft.Extensions.Configuration;

using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace ArtOfTime.Services
{

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
            this.contractAddress = this.configuration["Ethereum:ContractAddress"];
            this.infura = this.configuration["Ethereum:Infura"];
            this.publicKey = this.configuration["Ethereum:PublicKey"];
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
