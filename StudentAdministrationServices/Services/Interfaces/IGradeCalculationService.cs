using StudentAdministrationDatabase.Models;

namespace StudentAdministrationServices.Services.Interfaces;

/// <summary>
///     Centralizes the deterministic grading calculations (passing rule, earned credits, average grade).
/// </summary>
/// <remarks>
///     This service contains no I/O and is intentionally free of side effects so that the grading rules
///     are reusable across services and view models and are trivially unit-testable. It is the single home
///     for calculation logic that was previously scattered through the view models.
/// </remarks>
public interface IGradeCalculationService
{
    /// <summary>
    ///     Determines whether a grade value represents a passing result.
    /// </summary>
    /// <param name="gradeValue">The grade value to evaluate.</param>
    /// <returns><c>true</c> when the grade is passing; otherwise <c>false</c>.</returns>
    bool IsPassing(int gradeValue);

    /// <summary>
    ///     Calculates the total credits a student has earned, i.e. the sum of the course credits for every
    ///     <b>passing</b> grade.
    /// </summary>
    /// <param name="grades">The student's grades paired with their course credit values.</param>
    /// <returns>The total earned credits. Returns <c>0</c> when the sequence is empty.</returns>
    int CalculateEarnedCredits(IEnumerable<StudentGradeCredit> grades);

    /// <summary>
    ///     Calculates the (unweighted) average of the supplied grade values.
    /// </summary>
    /// <param name="gradeValues">The grade values to average.</param>
    /// <returns>The average, or <c>null</c> when the sequence is empty (no grades to average).</returns>
    double? CalculateAverageGrade(IEnumerable<int> gradeValues);
}