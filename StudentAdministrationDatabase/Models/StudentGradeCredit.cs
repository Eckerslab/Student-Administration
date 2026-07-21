namespace StudentAdministrationDatabase.Models;

/// <summary>
///     A lightweight projection pairing a grade's value with the credit value of the course it belongs to.
/// </summary>
/// <remarks>
///     Used to feed credit calculations without depending on the <see cref="Grade.Course" /> navigation
///     property being loaded, which keeps the calculation deterministic and free of lazy-loading surprises.
/// </remarks>
/// <param name="GradeValue">The value of the grade.</param>
/// <param name="CourseCredit">The credit value of the course the grade was awarded for.</param>
public readonly record struct StudentGradeCredit(int GradeValue, int CourseCredit);