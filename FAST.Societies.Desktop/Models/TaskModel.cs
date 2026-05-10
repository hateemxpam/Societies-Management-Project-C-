namespace FAST.Societies.Desktop.Models;
public record TaskModel(int TaskId, int SocietyId, int AssignedToStudentId, string Title, string Status);
