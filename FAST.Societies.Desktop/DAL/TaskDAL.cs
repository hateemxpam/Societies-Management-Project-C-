using FAST.Societies.Desktop.Data;
using FAST.Societies.Desktop.Models;

namespace FAST.Societies.Desktop.DAL;

public sealed class TaskDAL
{
    private readonly SqlRepository _repo = new(DBConnection.GetConnectionString());
    public List<TaskModel> GetAll() => _repo.GetTasks();
    public List<TaskDisplay> GetAllWithSocietyName() => _repo.GetTasksWithSocietyName();
    public void Assign(int societyId, int studentId, string title, DateTime dueDate) => _repo.AssignTask(societyId, studentId, title, dueDate);
}
