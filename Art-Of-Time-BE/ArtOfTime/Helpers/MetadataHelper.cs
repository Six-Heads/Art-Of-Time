using System.Collections.Generic;

namespace ArtOfTime.Helpers
{
    public static class MetadataHelper
    {
        public static string GenerateJsonMetadata(string id, string imageUri, string timestamp, List<string> twitterAttributes)
        {
            return
$@"{{
    ""name"": ""ArtOfTime #{id}"",
    ""description"": ""AI generated snapshot of the Twitter trends. Timestamp: {timestamp}"",
    ""image"": ""{imageUri}"",
    ""attributes"": [
        {{
                ""value"": ""{ twitterAttributes[0]}""
        }},
        {{
                ""value"": ""{ twitterAttributes[1]}""
        }},
        {{
                ""value"": ""{ twitterAttributes[2]}""
        }},
        {{
                ""value"": ""{ twitterAttributes[3]}""
        }},
        {{
                ""value"": ""{ twitterAttributes[4]}""
        }},
        {{
            ""display_type"": ""date"",
            ""trait_type"": ""birthday"", 
            ""value"": {timestamp}
        }}
    ]
}}";

        }
    }
}
