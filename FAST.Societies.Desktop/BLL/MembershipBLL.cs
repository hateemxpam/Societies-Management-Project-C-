using FAST.Societies.Desktop.DAL;

namespace FAST.Societies.Desktop.BLL;

public sealed class MembershipBLL
{
    private readonly MembershipDAL _dal = new();
    public void Apply(int studentId, int societyId) => _dal.Apply(studentId, societyId);
    public List<Models.Membership> GetByStudent(int studentId) => _dal.GetByStudent(studentId);
    public List<Models.Membership> GetPending() => _dal.GetPending();
    public void UpdateStatus(int membershipId, string status) => _dal.UpdateStatus(membershipId, status);
}
