namespace Tenjin.Data.Models;

/// <summary>
/// The data structure used to create a new SqlException.
/// </summary>
public record MicrosoftSqlExceptionAttributes
{
    /// <summary>
    /// The SQL error code or number.
    /// </summary>
    public int Number { get; init; } = 0;
}