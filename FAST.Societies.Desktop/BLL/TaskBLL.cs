using FAST.Societies.Desktop.DAL;
using FAST.Societies.Desktop.Models;

namespace FAST.Societies.Desktop.BLL;

public sealed class TaskBLL
{
    private readonly TaskDAL _dal = new();
    public List<TaskModel> GetAll() => _dal.GetAll();
    public List<TaskDisplay> GetAllWithSocietyName() => _dal.GetAllWithSocietyName();
    public void Assign(int societyId, int studentId, string title, DateTime dueDate) => _dal.Assign(societyId, studentId, title, dueDate);
}
