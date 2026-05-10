using FAST.Societies.Desktop.Data;
using FAST.Societies.Desktop.Models;

namespace FAST.Societies.Desktop.DAL;

public sealed class StudentDAL
{
    private readonly SqlRepository _repo = new(DBConnection.GetConnectionString());
    public void Register(string fullName, string email, string password) => _repo.RegisterStudent(fullName, email, password);
    public List<Student> GetAll() => _repo.GetStudents();
}
