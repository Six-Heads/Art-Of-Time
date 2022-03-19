// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "base64-sol/base64.sol";

struct TokenOutputModel {
    uint256 id;
    string uri;
}

contract LOUVRENFT is ERC721URIStorage, Ownable {
    uint256 public tokenCounter;
    event CreatedLOUVRENFT(uint256 indexed tokenId, string tokenURI);

    constructor() ERC721("LOUVRE NFT", "louvreNFT") 
    {
        tokenCounter = 0;
    }

    function create(string memory tokenURI) public onlyOwner  {
        _safeMint(msg.sender, tokenCounter);
        _setTokenURI(tokenCounter, tokenURI);
        emit CreatedLOUVRENFT(tokenCounter, tokenURI);
        tokenCounter++;
    }

    function getAllTokens() public view returns (TokenOutputModel[] memory) {
        uint256 collectionCount = tokenCounter + 1;
        TokenOutputModel[] memory allTokens = new TokenOutputModel[](collectionCount);
        for (uint256 i = 0; i < collectionCount; i++) {
            string memory uri = tokenURI(i);
            allTokens[i] = TokenOutputModel(i, uri);
        }
        return allTokens; 
    }

    //фукнция за бройката на items
}