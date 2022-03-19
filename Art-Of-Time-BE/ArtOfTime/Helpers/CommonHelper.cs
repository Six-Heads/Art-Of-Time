using System;

namespace ArtOfTime.Helpers
{
    public static class CommonHelper
    {
        public static long GetCurrentTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        }
    }
}
