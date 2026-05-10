using FAST.Societies.Desktop.Data;
using FAST.Societies.Desktop.Models;

namespace FAST.Societies.Desktop.DAL;

public sealed class EventDAL
{
    private readonly SqlRepository _repo = new(DBConnection.GetConnectionString());
    public void Create(int societyId, string title, DateTime eventDate) => _repo.CreateEvent(societyId, title, eventDate);
    public List<Event> GetUpcoming() => _repo.GetUpcomingEvents();
    public void RegisterStudent(int studentId, int eventId) => _repo.RegisterEvent(studentId, eventId);
}
