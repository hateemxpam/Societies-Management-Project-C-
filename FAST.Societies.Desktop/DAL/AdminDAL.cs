using FAST.Societies.Desktop.Data;
using FAST.Societies.Desktop.Models;

namespace FAST.Societies.Desktop.DAL;

public sealed class AdminDAL
{
    private readonly SqlRepository _repo = new(DBConnection.GetConnectionString());
    public List<Event> GetPendingEvents() => _repo.GetPendingEvents();
    public void UpdateEventStatus(int eventId, string status) => _repo.UpdateEventStatus(eventId, status);
    public (int Students, int Societies, int Events, int Memberships, int Tasks) GetStats() => _repo.GetDashboardStats();
    public void SeedDemoData() => _repo.SeedDemoData();
}
