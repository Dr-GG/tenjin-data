using Microsoft.Data.SqlClient;
using Tenjin.Data.Utilities;

namespace Tenjin.Data.Extensions;

/// <summary>
/// A collection of extensions for Exception instances that relate to data or database exceptions.
/// </summary>
public static class DataExceptionExtensions
{
    /// <summary>
    /// Determines if an Exception is an SqlException instance.
    /// </summary>
    public static bool IsDuplicateDataException(this Exception error)
    {
        if (error is SqlException sqlError)
        {
            return MicrosoftSqlServerUtilities.IsDuplicateDataErrorCode(sqlError.Number);
        }

        return error.InnerException != null &&
               error.InnerException.IsDuplicateDataException();
    }
}