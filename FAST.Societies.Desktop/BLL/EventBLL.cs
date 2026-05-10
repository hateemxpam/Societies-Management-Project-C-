using FAST.Societies.Desktop.DAL;
using FAST.Societies.Desktop.Models;

namespace FAST.Societies.Desktop.BLL;

public sealed class EventBLL
{
    private readonly EventDAL _dal = new();
    public void Create(int societyId, string title, DateTime eventDate) => _dal.Create(societyId, title, eventDate);
    public List<Event> GetUpcoming() => _dal.GetUpcoming();
    public void RegisterStudent(int studentId, int eventId) => _dal.RegisterStudent(studentId, eventId);
}
