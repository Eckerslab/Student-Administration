using StudentAdministrationDatabase.Models;
using StudentAdministrationServices.Services.Interfaces;

namespace StudentAdministrationServices.Services;

/// <summary>
///     Default implementation of <see cref="IGradeCalculationService" />.
///     All rules delegate to <see cref="GradingScale" /> so the passing threshold is defined in exactly one place.
/// </summary>
public class GradeCalculationService : IGradeCalculationService
{
    /// <inheritdoc />
    public bool IsPassing(int gradeValue)
    {
        return GradingScale.IsPassing(gradeValue);
    }

    /// <inheritdoc />
    public int CalculateEarnedCredits(IEnumerable<StudentGradeCredit> grades)
    {
        ArgumentNullException.ThrowIfNull(grades);

        return grades
            .Where(grade => IsPassing(grade.GradeValue))
            .Sum(grade => grade.CourseCredit);
    }

    /// <inheritdoc />
    public double? CalculateAverageGrade(IEnumerable<int> gradeValues)
    {
        ArgumentNullException.ThrowIfNull(gradeValues);

        // Materialize once so we can both test for emptiness and average without enumerating twice.
        var values = gradeValues.ToList();
        return values.Count == 0 ? null : values.Average();
    }
}