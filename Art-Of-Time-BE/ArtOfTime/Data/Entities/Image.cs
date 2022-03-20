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
    }
}
