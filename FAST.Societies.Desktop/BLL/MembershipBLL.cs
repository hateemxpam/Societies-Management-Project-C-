using FAST.Societies.Desktop.DAL;

namespace FAST.Societies.Desktop.BLL;

public sealed class MembershipBLL
{
    private readonly MembershipDAL _dal = new();
    public void Apply(int studentId, int societyId) => _dal.Apply(studentId, societyId);
    public List<Models.Membership> GetByStudent(int studentId) => _dal.GetByStudent(studentId);
    public List<Models.StudentMembershipStatus> GetByStudentWithSocietyName(int studentId) => _dal.GetByStudentWithSocietyName(studentId);
    public List<Models.Membership> GetPending() => _dal.GetPending();
    public List<Models.PendingMembershipDisplay> GetPendingWithSocietyName() => _dal.GetPendingWithSocietyName();
    public void UpdateStatus(int membershipId, string status) => _dal.UpdateStatus(membershipId, status);
}
