using FAST.Societies.Desktop.Data;

namespace FAST.Societies.Desktop.DAL;

public sealed class MembershipDAL
{
    private readonly SqlRepository _repo = new(DBConnection.GetConnectionString());
    public void Apply(int studentId, int societyId) => _repo.ApplyMembership(studentId, societyId);
    public List<Models.Membership> GetByStudent(int studentId) => _repo.GetMembershipsByStudent(studentId);
    public List<Models.StudentMembershipStatus> GetByStudentWithSocietyName(int studentId) => _repo.GetMembershipsByStudentWithSocietyName(studentId);
    public List<Models.Membership> GetPending() => _repo.GetPendingMemberships();
    public List<Models.PendingMembershipDisplay> GetPendingWithSocietyName() => _repo.GetPendingMembershipsWithSocietyName();
    public void UpdateStatus(int membershipId, string status) => _repo.UpdateMembershipStatus(membershipId, status);
}
