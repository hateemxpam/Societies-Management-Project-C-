using FAST.Societies.Desktop.Data;
using FAST.Societies.Desktop.Models;

namespace FAST.Societies.Desktop.DAL;

public sealed class SocietyDAL
{
    private readonly SqlRepository _repo = new(DBConnection.GetConnectionString());
    public void Create(string name, string category) => _repo.CreateSociety(name, category);
    public List<Society> GetAll() => _repo.GetSocieties();
    public void UpdateStatus(int societyId, bool isActive) => _repo.UpdateSocietyStatus(societyId, isActive);
}
