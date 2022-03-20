using System;

namespace ArtOfTime.Data.Entities
{
    public class Image
    {
        /// <summary>
        /// TimeStamp used as id
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// Comma separated hashtags
        /// </summary>
        public string BasedOnText { get; set; }
        /// <summary>
        /// Is image fetched from python api
        /// </summary>
        public bool IsFetched { get; set; }

        /// <summary>
        /// Is image uploaded to the IPFS
        /// </summary>
        public bool IsUploadedImage { get; set; }

        /// <summary>
        /// Is image uploaded to the IPFS
        /// </summary>
        public bool IsUploadedJson { get; set; }

        /// <summary>
        /// Is image minted to the blockchain
        /// </summary>
        public bool IsMinted { get; set; }

        /// <summary>
        /// Image raw data
        /// </summary>
        public byte[] ImageBase64 { get; set; }

        /// <summary>
        /// Image IPFS url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// NFT metadata url to IPFS
        /// </summary>
        public string JsonUrl { get; set; }

        /// <summary>
        /// Unique filename
        /// </summary>
        public string UidFilename { get; set; } = Guid.NewGuid().ToString();
    }
}
