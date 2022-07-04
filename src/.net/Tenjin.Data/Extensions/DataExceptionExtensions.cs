using Microsoft.Data.SqlClient;
using Tenjin.Data.Utilities;

namespace Tenjin.Data.Extensions;

public static class DataExceptionExtensions
{
    public static bool IsDuplicateDataException(this Exception error)
    {
        if (error is SqlException sqlError)
        {
            return MicrosoftSqlServerUtilities.IsDuplicateDataErrorCode(sqlError.Number);
        }

        return error.InnerException != null
               && error.InnerException.IsDuplicateDataException();
    }
}