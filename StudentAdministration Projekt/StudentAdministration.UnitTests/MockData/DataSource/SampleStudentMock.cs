using StudentAdministrationDatabase.Models;

namespace StudentAdministrationTests.MockData.DataSource;

internal static class SampleStudentMock
{
    /// <summary>
    ///     Gets sample students.
    /// </summary>
    /// <returns></returns>
    public static List<Student> GetStudents()
    {
        return
        [
            CreateSample(Guid.Parse("f9be4e8e-f78b-4a0b-9941-0ce8b9bce178"), "Anna", "Schindlmeier", "Anna.Schindlmeier@gmailcom", Guid.Parse("cdc54724-c8c4-44de-a7aa-24104fb482dd"),
                Guid.Parse("fc72afcf-ed7f-46de-b3cb-a173fd379920"), 67, 10000000),

            CreateSample(
                Guid.Parse("7b2680c2-9420-4e7e-a2d4-97f4d9931b26"), "John", "Doe", "John.Doe@gmailcom", Guid.Parse("cdc54724-c8c4-44de-a7aa-24104fb482dd"),
                Guid.Parse("8a20eb66-e15c-4a5f-9c55-8d90c76974d4"), 9, 10000001
            ),

            CreateSample(
                Guid.Parse("e9fdd9ab-2c76-4778-8992-80e35f3a4a03"),
                "Jonas",
                "Meier",
                "Jonas.Meier@gmailcom",
                Guid.Parse("cdc54724-c8c4-44de-a7aa-24104fb482dd"),
                Guid.Parse("ebeb1657-37bd-4781-a69a-6602805bc34f"), 0, 10000002
            ),

            CreateSample(
                Guid.Parse("96d71862-bb38-4e1f-a3fd-12f43582d35e"),
                "Martin",
                "Schmid",
                "Martin.Schmidt@gmailcom",
                Guid.Parse("cdc54724-c8c4-44de-a7aa-24104fb482dd"),
                Guid.Parse("fc72afcf-ed7f-46de-b3cb-a173fd379920"), 15, 10000003),

            CreateSample(
                Guid.Parse("5d2b9b34-f2e8-46b1-83e3-0e39b52ee044"),
                "Lukas",
                "Fröhler",
                "Lukas.Fröhler@gmailcom",
                Guid.Parse("cdc54724-c8c4-44de-a7aa-24104fb482dd"),
                Guid.Parse("8a20eb66-e15c-4a5f-9c55-8d90c76974d4"), 0, 10000004)
        ];

        static Student CreateSample(Guid id, string firstname, string lastname, string email, Guid universityId, Guid degreeProgramId, int credits = 0, int studentnumber = 999999)
        {
            return new Student
                   {
                       Id = id,
                       FirstName = firstname,
                       LastName = lastname,
                       Email = email,
                       UniversityId = universityId,
                       DegreeProgramId = degreeProgramId,
                       StudentNumber = studentnumber,
                       Credits = credits,
                       University = SampleUniversitiesMock.GetUniversities().FirstOrDefault(u => u.Id         == universityId)    ?? throw new InvalidOperationException(),
                       DegreeProgram = SampleDegreeProgramMock.GetDegreePrograms().FirstOrDefault(dp => dp.Id == degreeProgramId) ?? throw new InvalidOperationException(),
                       Grades = SampleGradeMock.GetGrades().Where(g => g.StudentId == id).ToList()
                   };
        }
    }
}