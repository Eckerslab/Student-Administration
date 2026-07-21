namespace StudentAdministrationDatabase.Models;

/// <summary>
///     Single source of truth for the grading scale's business rules.
/// </summary>
/// <remarks>
///     The application uses the Austrian/German grading scale where a <b>lower</b> value is a better
///     result. A grade is considered passing when its value is strictly below
///     <see cref="FirstFailingValue" />. Centralizing the rule here removes the duplicated literals and
///     <c>IsPassing</c> implementations that previously lived on <see cref="Grade" />, on the grade
///     binding model and inline in the view models.
/// </remarks>
public static class GradingScale
{
    /// <summary>
    ///     The lowest grade value that is still considered a failure. Any value below this is passing.
    /// </summary>
    public const int FirstFailingValue = 5;

    /// <summary>
    ///     The smallest valid grade value (best possible result).
    /// </summary>
    public const int MinValue = 0;

    /// <summary>
    ///     The largest valid grade value (worst possible result).
    /// </summary>
    public const int MaxValue = 6;

    /// <summary>
    ///     Determines whether the supplied grade value represents a passing result.
    /// </summary>
    /// <param name="gradeValue">The grade value to evaluate.</param>
    /// <returns><c>true</c> when the grade is passing; otherwise <c>false</c>.</returns>
    public static bool IsPassing(int gradeValue)
    {
        return gradeValue < FirstFailingValue;
    }

    /// <summary>
    ///     Determines whether the supplied grade value is within the valid range
    ///     [<see cref="MinValue" />, <see cref="MaxValue" />].
    /// </summary>
    /// <param name="gradeValue">The grade value to validate.</param>
    /// <returns><c>true</c> when the value is a valid grade; otherwise <c>false</c>.</returns>
    public static bool IsValidValue(int gradeValue)
    {
        return gradeValue is >= MinValue and <= MaxValue;
    }
}