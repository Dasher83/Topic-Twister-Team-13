using NUnit.Framework;
using System;
using System.Globalization;
using System.Text;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.UseCases;


public class TestGetRandomLetterUseCase 
{
    [Test]
    public void TestGetNewLetterUseCaseSimplePasses()
    {
        IShuffleLetterUseCase useCase = new ShuffleLetterUseCase();

        string actualResult = useCase.GetRandomLetter();

        int actualResultLength = actualResult.Length;

        int expectedLength = 1;

        Assert.AreEqual(expectedLength, actualResultLength);
    }

    [Test]
    public void test_get_new_letter_use_case_returns_letter()
    {
        IShuffleLetterUseCase useCase = new ShuffleLetterUseCase();

        string actualResult = useCase.GetRandomLetter();

        Assert.IsTrue(Char.IsLetter(actualResult[0]));
    }

    [Test]
    public void test_get_new_letter_use_case_returns_normalize_letter()
    {
        IShuffleLetterUseCase useCase = new ShuffleLetterUseCase();

        string actualResult = useCase.GetRandomLetter();

        Assert.AreEqual(IsNormalizedLetter(actualResult), actualResult);
    }

    private string IsNormalizedLetter(string text)
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

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}

