namespace ArtOfTime.Data.Entities
{
    public class Image
    {
        public string TimeStamp { get; set; }
        // coma separated hashtags
        public string BasedOnText { get; set; }
        public bool IsFetched { get; set; }
    }
}
