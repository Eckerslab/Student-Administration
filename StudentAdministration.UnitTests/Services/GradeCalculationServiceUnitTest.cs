using FluentAssertions;
using StudentAdministrationDatabase.Models;
using StudentAdministrationServices.Services;

namespace StudentAdministrationTests.Services;

/// <summary>
///     Unit tests for the centralized, deterministic grading calculations.
/// </summary>
public class GradeCalculationServiceUnitTest
{
    private readonly GradeCalculationService calculationService = new();

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(4, true)]
    [InlineData(5, false)]
    [InlineData(6, false)]
    public void IsPassing_ReturnsExpected(int gradeValue, bool expected)
    {
        calculationService.IsPassing(gradeValue).Should().Be(expected);
    }

    [Fact]
    public void CalculateEarnedCredits_SumsOnlyPassingGrades()
    {
        StudentGradeCredit[] grades =
        [
            new(1, 5), // passing  -> counts
            new(4, 6), // passing  -> counts
            new(5, 8), // failing  -> ignored
            new(6, 10) // failing  -> ignored
        ];

        calculationService.CalculateEarnedCredits(grades).Should().Be(11);
    }

    [Fact]
    public void CalculateEarnedCredits_EmptySequence_ReturnsZero()
    {
        calculationService.CalculateEarnedCredits([]).Should().Be(0);
    }

    [Fact]
    public void CalculateEarnedCredits_ZeroCreditPassingCourse_ContributesNothing()
    {
        StudentGradeCredit[] grades = [new(1, 0), new(2, 4)];

        calculationService.CalculateEarnedCredits(grades).Should().Be(4);
    }

    [Fact]
    public void CalculateEarnedCredits_AllFailing_ReturnsZero()
    {
        StudentGradeCredit[] grades = [new(5, 6), new(6, 4)];

        calculationService.CalculateEarnedCredits(grades).Should().Be(0);
    }

    [Fact]
    public void CalculateAverageGrade_NoGrades_ReturnsNull()
    {
        calculationService.CalculateAverageGrade([]).Should().BeNull();
    }

    [Fact]
    public void CalculateAverageGrade_ComputesArithmeticMean()
    {
        calculationService.CalculateAverageGrade([1, 2, 6]).Should().Be(3);
    }

    [Fact]
    public void CalculateAverageGrade_SingleGrade_ReturnsThatGrade()
    {
        calculationService.CalculateAverageGrade([4]).Should().Be(4);
    }
}