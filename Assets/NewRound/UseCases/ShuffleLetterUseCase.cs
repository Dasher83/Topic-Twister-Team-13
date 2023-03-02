using System.Globalization;
using System.Text;
using TopicTwister.NewRound.Shared.Interfaces;


namespace TopicTwister.NewRound.UseCases
{
    public class ShuffleLetterUseCase : IShuffleLetterUseCase
    {
        public string GetRandomLetter()
        {
            /*int number = Random.Range(0, 26);
            char letter = (char)(((int)'A') + number);
            return $"{ letter }";*/
            return RemoveDiacritics("á");
        }

        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }
    }
}
