let { networkConfig } = require('../helper-hardhat-config')

module.exports = async ({
    getNamedAccounts,
    deployments,
    getChainId
}) => {

    const { deploy, log } = deployments
    const { deployer } = await getNamedAccounts()
    const chainId = await getChainId()

    log("----------------------------------------------------");

    const LOUVRENFT = await deploy("LOUVRENFT", {
        from: deployer,
        log: true
    })
    log(`You have deployed an NFT contract to ${LOUVRENFT.address}`);

    const louvreNFTContract = await ethers.getContractFactory("LOUVRENFT");
    const accounts = await hre.ethers.getSigners()
    const signer = accounts[0]
    const louvreNFT = new ethers.Contract(LOUVRENFT.address, louvreNFTContract.interface, signer)
    const networkName = networkConfig[chainId]['name']
    log(`Verify with:\n npx hardhat verify --network ${networkName} ${louvreNFT.address}`)
}