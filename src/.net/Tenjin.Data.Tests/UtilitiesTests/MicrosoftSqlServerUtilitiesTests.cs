using NUnit.Framework;
using Tenjin.Data.Models;
using Tenjin.Data.Utilities;

namespace Tenjin.Data.Tests.UtilitiesTests;

[TestFixture]
public class MicrosoftSqlServerUtilitiesTests
{
    [TestCase(-1, false)]
    [TestCase(0, false)]
    [TestCase(1, false)]
    [TestCase(2, false)]
    [TestCase(3, false)]
    [TestCase(2627, true)]
    [TestCase(2601, true)]
    public static void IsDuplicateDataErrorCode_WhenProvidedAnErrorCode_Matches(int errorCode, bool expectedResult)
    {
        Assert.AreEqual(expectedResult, MicrosoftSqlServerUtilities.IsDuplicateDataErrorCode(errorCode));
    }

    [TestCase(-1)]
    [TestCase(-2)]
    [TestCase(-3)]
    [TestCase(-4)]
    [TestCase(-5)]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(101)]
    [TestCase(202)]
    [TestCase(303)]
    [TestCase(404)]
    [TestCase(505)]
    public static void CreateSqlException_WhenProvidedWithNumber_ReturnsCorrectNumber(int number)
    {
        var attributes = new MicrosoftSqlExceptionAttributes
        {
            Number = number
        };
        var exception = MicrosoftSqlServerUtilities.CreateSqlException(attributes);

        Assert.IsNotNull(exception);
        Assert.AreEqual(number, exception.Number);
    }
}