using FAST.Societies.Desktop.DAL;
using FAST.Societies.Desktop.Models;

namespace FAST.Societies.Desktop.BLL;

public sealed class SocietyBLL
{
    private readonly SocietyDAL _dal = new();
    public void Create(string name, string category) => _dal.Create(name, category);
    public List<Society> GetAll() => _dal.GetAll();
}
