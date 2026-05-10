using FAST.Societies.Desktop.DAL;
using FAST.Societies.Desktop.Models;

namespace FAST.Societies.Desktop.BLL;

public sealed class AdminBLL
{
    private readonly AdminDAL _dal = new();
    public List<Event> GetPendingEvents() => _dal.GetPendingEvents();
    public void UpdateEventStatus(int eventId, string status) => _dal.UpdateEventStatus(eventId, status);
    public (int Students, int Societies, int Events, int Memberships, int Tasks) GetStats() => _dal.GetStats();
    public void SeedDemoData() => _dal.SeedDemoData();
}
