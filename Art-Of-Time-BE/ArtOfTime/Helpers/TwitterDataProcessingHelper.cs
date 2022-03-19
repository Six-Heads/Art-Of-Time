using ArtOfTime.Models.Twitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArtOfTime.Helpers
{
    public class TwitterDataProcessingHelper
    {
        public async Task<List<string>> ExtractText(List<TrendModel> trendsListModel)
        {
            const string PATTERN_ALL_ENCODED = @"^#\\u[0-9A-F]{4}";
            const string PATTERN_ALL_ASCII = @"^[ -~]*$";

            Regex rg = new Regex(PATTERN_ALL_ENCODED);
            Regex rg2 = new Regex(PATTERN_ALL_ASCII);

            trendsListModel = trendsListModel
                .Where(x => x.TweetVolume != null && !rg.IsMatch(x.Name) && rg2.IsMatch(x.Name))
                .OrderByDescending(x => x.TweetVolume)
                .Take(5)
                .ToList();

            var text = new List<string>();
            foreach (var trend in trendsListModel)
            {
                text.Add(await GetWordsFromHashtag(trend.Name));
            }

            return text;
        }

        private async Task<string> GetWordsFromHashtag(string hashtag)
        {
            var words = hashtag
                .Replace("#", "")
                .Split(new[] { ' ', '-', '_', '.', ',', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            //split hashtags to words
            if (words.Count == 1)
            {
                string word = words[0];
                int WORD_LENGTH = words[0].Length;
                words.Clear();

                int wordsCount = 0;
                string currentWord = word[0].ToString();

                for (int i = 1; i < WORD_LENGTH; i++)
                {
                    if (WORD_LENGTH - 1 - i >= 2)
                    {
                        if (char.IsUpper(word[i]))
                        {
                            if (char.IsUpper(word[i + 1]) && char.IsUpper(word[i-1]))
                            {
                                currentWord += word[i];
                            }
                            else
                            {
                                words.Add(currentWord);
                                wordsCount++;
                                currentWord = word[i].ToString();
                            }
                        }
                        else
                        {
                            currentWord += word[i];
                        }
                    }
                    else
                    {
                        if (WORD_LENGTH - 1 - i == 0)
                        {
                            currentWord += word[WORD_LENGTH - 1].ToString();
                        }
                        if (WORD_LENGTH - 1 - i == 1)
                        {
                            currentWord += word[WORD_LENGTH - 2].ToString();
                            currentWord += word[WORD_LENGTH - 1].ToString();
                        }
                        break;
                    }
                }

                words.Add(currentWord);
            }

            words = words.Where(x => x.Length > 1).ToList();

            return string.Join(" ", words);
        }
    }
}
