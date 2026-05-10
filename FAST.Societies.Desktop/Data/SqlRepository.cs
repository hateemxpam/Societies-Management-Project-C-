using Microsoft.Data.SqlClient;
using FAST.Societies.Desktop.Models;

namespace FAST.Societies.Desktop.Data;

public sealed class SqlRepository
{
    private readonly string _connectionString;
    private static bool _schemaReady;
    private static readonly object SchemaLock = new();

    public SqlRepository(string connectionString)
    {
        _connectionString = connectionString;
        EnsureSchema();
        EnsureSeedData();
    }

    public void EnsureDatabaseReady()
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT 1";
        cmd.ExecuteScalar();
    }

    private void EnsureSchema()
    {
        if (_schemaReady) return;
        lock (SchemaLock)
        {
            if (_schemaReady) return;
            using var con = new SqlConnection(_connectionString);
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
IF OBJECT_ID('Students', 'U') IS NULL
CREATE TABLE Students (
    StudentId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(120) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(200) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

IF OBJECT_ID('AdminUsers', 'U') IS NULL
CREATE TABLE AdminUsers (
    AdminId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(120) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(200) NOT NULL,
    RoleName NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

IF OBJECT_ID('Societies', 'U') IS NULL
CREATE TABLE Societies (
    SocietyId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(120) NOT NULL UNIQUE,
    Category NVARCHAR(80) NOT NULL,
    Description NVARCHAR(500) NOT NULL DEFAULT '',
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

IF OBJECT_ID('Memberships', 'U') IS NULL
CREATE TABLE Memberships (
    MembershipId INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT NOT NULL FOREIGN KEY REFERENCES Students(StudentId),
    SocietyId INT NOT NULL FOREIGN KEY REFERENCES Societies(SocietyId),
    Status NVARCHAR(30) NOT NULL,
    AppliedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    ApprovedAt DATETIME2 NULL,
    CONSTRAINT UQ_Membership UNIQUE (StudentId, SocietyId)
);

IF OBJECT_ID('Events', 'U') IS NULL
CREATE TABLE Events (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    SocietyId INT NOT NULL FOREIGN KEY REFERENCES Societies(SocietyId),
    Title NVARCHAR(150) NOT NULL,
    Description NVARCHAR(600) NOT NULL DEFAULT '',
    Venue NVARCHAR(150) NOT NULL DEFAULT 'TBD',
    EventDate DATE NOT NULL,
    Capacity INT NOT NULL DEFAULT 100,
    Status NVARCHAR(30) NOT NULL,
    RequiresAdminApproval BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

IF OBJECT_ID('EventRegistrations', 'U') IS NULL
CREATE TABLE EventRegistrations (
    RegistrationId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL FOREIGN KEY REFERENCES Events(EventId),
    StudentId INT NOT NULL FOREIGN KEY REFERENCES Students(StudentId),
    TicketCode NVARCHAR(80) NOT NULL UNIQUE,
    RegisteredAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    AttendanceStatus NVARCHAR(30) NOT NULL DEFAULT 'Registered',
    CONSTRAINT UQ_EventStudent UNIQUE (EventId, StudentId)
);

IF OBJECT_ID('Tasks', 'U') IS NULL
CREATE TABLE Tasks (
    TaskId INT IDENTITY(1,1) PRIMARY KEY,
    SocietyId INT NOT NULL FOREIGN KEY REFERENCES Societies(SocietyId),
    AssignedToStudentId INT NOT NULL FOREIGN KEY REFERENCES Students(StudentId),
    Title NVARCHAR(150) NOT NULL,
    Status NVARCHAR(30) NOT NULL,
    DueDate DATE NULL
);

IF OBJECT_ID('Announcements', 'U') IS NULL
CREATE TABLE Announcements (
    AnnouncementId INT IDENTITY(1,1) PRIMARY KEY,
    SocietyId INT NULL FOREIGN KEY REFERENCES Societies(SocietyId),
    AdminId INT NULL FOREIGN KEY REFERENCES AdminUsers(AdminId),
    Title NVARCHAR(150) NOT NULL,
    Body NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);";
            cmd.ExecuteNonQuery();
            _schemaReady = true;
        }
    }

    public List<Student> GetStudents()
    {
        var list = new List<Student>();
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT StudentId, FullName, Email, PasswordHash FROM Students ORDER BY StudentId DESC";
        using var rd = cmd.ExecuteReader();
        while (rd.Read())
        {
            list.Add(new Student(rd.GetInt32(0), rd.GetString(1), rd.GetString(2), rd.GetString(3)));
        }
        return list;
    }

    public void RegisterStudent(string fullName, string email, string password)
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
INSERT INTO Students (FullName, Email, PasswordHash, IsActive, CreatedAt)
VALUES (@n, @e, @p, 1, SYSDATETIME())";
        cmd.Parameters.AddWithValue("@n", fullName);
        cmd.Parameters.AddWithValue("@e", email);
        cmd.Parameters.AddWithValue("@p", password);
        cmd.ExecuteNonQuery();
    }

    public List<Society> GetSocieties()
    {
        var list = new List<Society>();
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT SocietyId, Name, Category, IsActive FROM Societies ORDER BY Name";
        using var rd = cmd.ExecuteReader();
        while (rd.Read())
        {
            list.Add(new Society(rd.GetInt32(0), rd.GetString(1), rd.GetString(2), rd.GetBoolean(3)));
        }
        return list;
    }

    public void CreateSociety(string name, string category)
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
INSERT INTO Societies (Name, Category, Description, IsActive, CreatedAt)
VALUES (@n, @c, '', 1, SYSDATETIME())";
        cmd.Parameters.AddWithValue("@n", name);
        cmd.Parameters.AddWithValue("@c", category);
        cmd.ExecuteNonQuery();
    }

    public void ApplyMembership(int studentId, int societyId)
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM Memberships WHERE StudentId=@s AND SocietyId=@soc)
INSERT INTO Memberships (StudentId, SocietyId, Status, AppliedAt)
VALUES (@s, @soc, 'Pending', SYSDATETIME())";
        cmd.Parameters.AddWithValue("@s", studentId);
        cmd.Parameters.AddWithValue("@soc", societyId);
        cmd.ExecuteNonQuery();
    }

    public List<Event> GetUpcomingEvents()
    {
        var list = new List<Event>();
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
SELECT EventId, SocietyId, Title, EventDate, Status
FROM Events
WHERE EventDate >= CAST(SYSDATETIME() AS date)
ORDER BY EventDate";
        using var rd = cmd.ExecuteReader();
        while (rd.Read())
        {
            list.Add(new Event(rd.GetInt32(0), rd.GetInt32(1), rd.GetString(2), rd.GetDateTime(3), rd.GetString(4)));
        }
        return list;
    }

    public void CreateEvent(int societyId, string title, DateTime eventDate)
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
INSERT INTO Events (SocietyId, Title, Description, Venue, EventDate, Capacity, Status, RequiresAdminApproval, CreatedAt)
VALUES (@soc, @t, '', 'TBD', @d, 100, 'PendingApproval', 1, SYSDATETIME())";
        cmd.Parameters.AddWithValue("@soc", societyId);
        cmd.Parameters.AddWithValue("@t", title);
        cmd.Parameters.AddWithValue("@d", eventDate);
        cmd.ExecuteNonQuery();
    }

    public void RegisterEvent(int studentId, int eventId)
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM EventRegistrations WHERE StudentId=@s AND EventId=@e)
INSERT INTO EventRegistrations (EventId, StudentId, TicketCode, RegisteredAt, AttendanceStatus)
VALUES (@e, @s, CONCAT('TKT-', @s, '-', @e), SYSDATETIME(), 'Registered')";
        cmd.Parameters.AddWithValue("@s", studentId);
        cmd.Parameters.AddWithValue("@e", eventId);
        cmd.ExecuteNonQuery();
    }

    public List<Membership> GetMembershipsByStudent(int studentId)
    {
        var list = new List<Membership>();
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT MembershipId, StudentId, SocietyId, Status FROM Memberships WHERE StudentId=@s ORDER BY MembershipId DESC";
        cmd.Parameters.AddWithValue("@s", studentId);
        using var rd = cmd.ExecuteReader();
        while (rd.Read())
        {
            list.Add(new Membership(rd.GetInt32(0), rd.GetInt32(1), rd.GetInt32(2), rd.GetString(3)));
        }
        return list;
    }

    public List<Membership> GetPendingMemberships()
    {
        var list = new List<Membership>();
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT MembershipId, StudentId, SocietyId, Status FROM Memberships WHERE Status='Pending' ORDER BY MembershipId DESC";
        using var rd = cmd.ExecuteReader();
        while (rd.Read())
        {
            list.Add(new Membership(rd.GetInt32(0), rd.GetInt32(1), rd.GetInt32(2), rd.GetString(3)));
        }
        return list;
    }

    public void UpdateMembershipStatus(int membershipId, string status)
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "UPDATE Memberships SET Status=@st, ApprovedAt=SYSDATETIME() WHERE MembershipId=@id";
        cmd.Parameters.AddWithValue("@st", status);
        cmd.Parameters.AddWithValue("@id", membershipId);
        cmd.ExecuteNonQuery();
    }

    public List<TaskModel> GetTasks()
    {
        var list = new List<TaskModel>();
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT TaskId, SocietyId, AssignedToStudentId, Title, Status FROM Tasks ORDER BY TaskId DESC";
        using var rd = cmd.ExecuteReader();
        while (rd.Read())
        {
            list.Add(new TaskModel(rd.GetInt32(0), rd.GetInt32(1), rd.GetInt32(2), rd.GetString(3), rd.GetString(4)));
        }
        return list;
    }

    public void AssignTask(int societyId, int studentId, string title, DateTime dueDate)
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
INSERT INTO Tasks (SocietyId, AssignedToStudentId, Title, Status, DueDate)
VALUES (@soc, @st, @t, 'Assigned', @d)";
        cmd.Parameters.AddWithValue("@soc", societyId);
        cmd.Parameters.AddWithValue("@st", studentId);
        cmd.Parameters.AddWithValue("@t", title);
        cmd.Parameters.AddWithValue("@d", dueDate.Date);
        cmd.ExecuteNonQuery();
    }

    public List<Event> GetPendingEvents()
    {
        var list = new List<Event>();
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT EventId, SocietyId, Title, EventDate, Status FROM Events WHERE Status='PendingApproval' ORDER BY EventId DESC";
        using var rd = cmd.ExecuteReader();
        while (rd.Read())
        {
            list.Add(new Event(rd.GetInt32(0), rd.GetInt32(1), rd.GetString(2), rd.GetDateTime(3), rd.GetString(4)));
        }
        return list;
    }

    public void UpdateEventStatus(int eventId, string status)
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "UPDATE Events SET Status=@st WHERE EventId=@id";
        cmd.Parameters.AddWithValue("@st", status);
        cmd.Parameters.AddWithValue("@id", eventId);
        cmd.ExecuteNonQuery();
    }

    public (int Students, int Societies, int Events, int Memberships, int Tasks) GetDashboardStats()
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
SELECT
 (SELECT COUNT(*) FROM Students),
 (SELECT COUNT(*) FROM Societies),
 (SELECT COUNT(*) FROM Events),
 (SELECT COUNT(*) FROM Memberships),
 (SELECT COUNT(*) FROM Tasks)";
        using var rd = cmd.ExecuteReader();
        rd.Read();
        return (rd.GetInt32(0), rd.GetInt32(1), rd.GetInt32(2), rd.GetInt32(3), rd.GetInt32(4));
    }

    public void SeedDemoData()
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM Students WHERE Email='ali.raza@nu.edu.pk')
INSERT INTO Students (FullName, Email, PasswordHash, IsActive, CreatedAt)
VALUES ('Ali Raza', 'ali.raza@nu.edu.pk', '12345', 1, SYSDATETIME());

IF NOT EXISTS (SELECT 1 FROM Students WHERE Email='sara.khan@nu.edu.pk')
INSERT INTO Students (FullName, Email, PasswordHash, IsActive, CreatedAt)
VALUES ('Sara Khan', 'sara.khan@nu.edu.pk', '12345', 1, SYSDATETIME());

DECLARE @s1 INT = (SELECT TOP 1 StudentId FROM Students WHERE Email='ali.raza@nu.edu.pk');
DECLARE @s2 INT = (SELECT TOP 1 StudentId FROM Students WHERE Email='sara.khan@nu.edu.pk');
DECLARE @soc1 INT = (SELECT TOP 1 SocietyId FROM Societies ORDER BY SocietyId);
DECLARE @soc2 INT = (SELECT TOP 1 SocietyId FROM Societies ORDER BY SocietyId DESC);

IF @s1 IS NOT NULL AND @soc1 IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM Memberships WHERE StudentId=@s1 AND SocietyId=@soc1)
INSERT INTO Memberships (StudentId, SocietyId, Status, AppliedAt) VALUES (@s1, @soc1, 'Pending', SYSDATETIME());

IF @s2 IS NOT NULL AND @soc2 IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM Memberships WHERE StudentId=@s2 AND SocietyId=@soc2)
INSERT INTO Memberships (StudentId, SocietyId, Status, AppliedAt) VALUES (@s2, @soc2, 'Approved', SYSDATETIME());

IF @soc1 IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM Events WHERE Title='Coding Sprint')
INSERT INTO Events (SocietyId, Title, Description, Venue, EventDate, Capacity, Status, RequiresAdminApproval, CreatedAt)
VALUES (@soc1, 'Coding Sprint', '24-hour coding challenge', 'Lab 3', DATEADD(DAY, 10, CAST(SYSDATETIME() AS date)), 120, 'PendingApproval', 1, SYSDATETIME());

DECLARE @eventId INT = (SELECT TOP 1 EventId FROM Events WHERE Title='Coding Sprint');
IF @eventId IS NOT NULL AND @s1 IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM EventRegistrations WHERE EventId=@eventId AND StudentId=@s1)
INSERT INTO EventRegistrations (EventId, StudentId, TicketCode, RegisteredAt, AttendanceStatus)
VALUES (@eventId, @s1, CONCAT('TKT-', @s1, '-', @eventId), SYSDATETIME(), 'Registered');

IF @soc1 IS NOT NULL AND @s2 IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM Tasks WHERE Title='Poster Design' AND AssignedToStudentId=@s2)
INSERT INTO Tasks (SocietyId, AssignedToStudentId, Title, Status, DueDate)
VALUES (@soc1, @s2, 'Poster Design', 'Assigned', DATEADD(DAY, 5, CAST(SYSDATETIME() AS date)));";
        cmd.ExecuteNonQuery();
    }

    private void EnsureSeedData()
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM Societies)
BEGIN
    INSERT INTO Societies (Name, Category, Description, IsActive, CreatedAt) VALUES
    ('Gaming Society','Gaming','Esports and gaming events',1,SYSDATETIME()),
    ('Developers Club','Technology','Coding workshops and hackathons',1,SYSDATETIME()),
    ('Media Society','Media','Content and media activities',1,SYSDATETIME());
END

IF NOT EXISTS (SELECT 1 FROM Events)
BEGIN
    DECLARE @sid INT = (SELECT TOP 1 SocietyId FROM Societies ORDER BY SocietyId);
    INSERT INTO Events (SocietyId, Title, Description, Venue, EventDate, Capacity, Status, RequiresAdminApproval, CreatedAt)
    VALUES (@sid, 'Orientation Meetup', 'Welcome event', 'Auditorium', DATEADD(DAY, 7, CAST(SYSDATETIME() AS date)), 200, 'Approved', 0, SYSDATETIME());
END";
        cmd.ExecuteNonQuery();
    }
}
