using System;
using FluentAssertions;
using NUnit.Framework;
using Tenjin.Data.Extensions;
using Tenjin.Data.Models;
using Tenjin.Data.Utilities;

namespace Tenjin.Data.Tests.ExtensionsTests;

[TestFixture, Parallelizable(ParallelScope.Children)]
public class DataExceptionExtensionsTests
{
    private static readonly Func<int, Exception> MicrosoftSqlServerExceptionFactory = sqlErrorCode =>
    {
        var attributes = new MicrosoftSqlExceptionAttributes
        {
            Number = sqlErrorCode
        };

        return MicrosoftSqlServerUtilities.CreateSqlException(attributes);
    };

    [TestCase(typeof(Exception))]
    [TestCase(typeof(InvalidOperationException))]
    [TestCase(typeof(ArgumentException))]
    [TestCase(typeof(ArgumentNullException))]
    public void IsDuplicateDataException_WhenProvidedWithANonDbExceptions_ReturnsFalse(Type type)
    {
        var exception = Activator.CreateInstance(type) as Exception;

        exception.Should().NotBeNull();
        exception!.IsDuplicateDataException().Should().BeFalse();
    }

    [TestCase(1, 0)]
    [TestCase(2, 1)]
    [TestCase(3, 2)]
    [TestCase(4, 3)]
    [TestCase(5, 4)]
    public void IsDuplicateDataException_WhenProvidedWithASqlExceptionButIncorrectCode_ReturnsFalse(
        int nestedCount,
        int sqlNumber)
    {
        var exception = GenerateNestedDataException(nestedCount, sqlNumber, MicrosoftSqlServerExceptionFactory);

        exception.IsDuplicateDataException().Should().BeFalse();
    }

    [TestCase(1, 2627)]
    [TestCase(2, 2627)]
    [TestCase(3, 2627)]
    [TestCase(4, 2627)]
    [TestCase(5, 2627)]
    [TestCase(1, 2601)]
    [TestCase(2, 2601)]
    [TestCase(3, 2601)]
    [TestCase(4, 2601)]
    [TestCase(5, 2601)]
    public void IsDuplicateDataException_WhenProvidedWithAMicrosoftSqlExceptionButAndCorrectCode_ReturnsTrue(
        int nestedCount,
        int sqlNumber)
    {
        var exception = GenerateNestedDataException(nestedCount, sqlNumber, MicrosoftSqlServerExceptionFactory);

        exception.IsDuplicateDataException().Should().BeTrue();
    }

    private static Exception GenerateNestedDataException(
        int count,
        int sqlErrorCode,
        Func<int, Exception> dataExceptionFactory)
    {
        if (count == 1)
        {
            return dataExceptionFactory(sqlErrorCode);
        }

        var innerException = GenerateNestedDataException(count - 1, sqlErrorCode, dataExceptionFactory);

        return new Exception("inner", innerException);
    }
}