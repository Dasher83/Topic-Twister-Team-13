using NUnit.Framework;
using System;
using System.Globalization;
using System.Text;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.UseCases;


public class TestGetRandomLetterUseCase 
{
    [Test]
    public void Test_get_new_letter_use_case_simple_passes()
    {
        IShuffleLetterUseCase useCase = new ShuffleLetterUseCase();

        string actualResult = useCase.GetRandomLetter();

        int actualResultLength = actualResult.Length;

        int expectedLength = 1;

        Assert.AreEqual(expectedLength, actualResultLength);
    }

    [Test]
    public void Test_get_new_letter_use_case_returns_letter()
    {
        IShuffleLetterUseCase useCase = new ShuffleLetterUseCase();

        string actualResult = useCase.GetRandomLetter();

        Assert.IsTrue(Char.IsLetter(actualResult[0]));
    }

    [Test]
    public void Test_get_new_letter_use_case_returns_normalize_letter()
    {
        IShuffleLetterUseCase useCase = new ShuffleLetterUseCase();

        string actualResult = useCase.GetRandomLetter();

        Assert.AreEqual(GetNormalizedString(actualResult), actualResult);
    }

    [Test]
    public void Test_get_new_letter_use_case_returns_random_letter()
    {
        IShuffleLetterUseCase useCase = new ShuffleLetterUseCase();

        string actualResult = useCase.GetRandomLetter();

        int cycles = 100;

        for (int i = 0; i < cycles; i++)
        {
            string duplicateResult = useCase.GetRandomLetter();

            try
            {
                Assert.IsTrue(String.CompareOrdinal(actualResult, duplicateResult) != 0);
            }
            catch(AssertionException ex)
            {
                if(i == cycles - 1)
                {
                    throw ex;
                }
                continue;
            }

            break;
        }
    }
    
    
    
    private string GetNormalizedString(string text)
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

