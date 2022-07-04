using System.Reflection;
using Microsoft.Data.SqlClient;
using Tenjin.Data.Models;
using Tenjin.Extensions;

namespace Tenjin.Data.Utilities;

public static class MicrosoftSqlServerUtilities
{
    private const int SqlServerPrimaryKeyViolationErrorCode = 2627;
    private const int SqlServerUniqueKeyViolationErrorCode = 2601;

    public static bool IsDuplicateDataErrorCode(int errorCode)
    {
        return errorCode.EqualsAny(
            SqlServerPrimaryKeyViolationErrorCode,
            SqlServerUniqueKeyViolationErrorCode);
    }

    public static SqlException CreateSqlException(MicrosoftSqlExceptionAttributes attributes)
    {
        var error = ConstructFromInternalConstructor<SqlError>(
            attributes.Number, (byte)0, (byte)0, string.Empty, string.Empty, string.Empty, 0, null);
        var errorCollection = ConstructFromInternalConstructor<SqlErrorCollection>();
        var addErrorMethod = typeof(SqlErrorCollection)
            .GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);

        if (addErrorMethod == null)
        {
            throw new InvalidOperationException("Could not find all methods to create SqlException");
        }

        addErrorMethod.Invoke(errorCollection, new object?[] { error });

        return ConstructFromInternalConstructor<SqlException>(
            string.Empty, errorCollection, null, Guid.NewGuid());
    }

    private static T ConstructFromInternalConstructor<T>(params object?[] parameters)
    {
        var constructors = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

        return (T)constructors
            .First(ctor => ctor.GetParameters().Length == parameters.Length)
            .Invoke(parameters);
    }
}