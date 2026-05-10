using FAST.Societies.Desktop.DAL;
using FAST.Societies.Desktop.Models;

namespace FAST.Societies.Desktop.BLL;

public sealed class StudentBLL
{
    private readonly StudentDAL _dal = new();
    public void Register(string fullName, string email, string password) => _dal.Register(fullName, email, password);
    public List<Student> GetAll() => _dal.GetAll();
}
