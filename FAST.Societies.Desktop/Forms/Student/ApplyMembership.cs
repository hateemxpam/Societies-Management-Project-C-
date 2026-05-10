using FAST.Societies.Desktop.BLL;
using FAST.Societies.Desktop.Utilities;

namespace FAST.Societies.Desktop.Forms.Student;

public partial class ApplyMembership : Form
{
    private readonly MembershipBLL _membershipBll = new();
    private readonly SocietyBLL _societyBll = new();
    private readonly TextBox _studentName = new() { ReadOnly = true };
    private readonly ComboBox _societyList = new() { DropDownStyle = ComboBoxStyle.DropDownList };

    public ApplyMembership()
    {
        InitializeComponent();
        LoadSocieties();
    }

    private void InitializeComponent()
    {
        Text = "Apply for Membership";
        Width = 450;
        Height = 400; // Increased height to prevent flow wrapping
        StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(245, 247, 250); // Matching theme

        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(40), FlowDirection = FlowDirection.TopDown, AutoSize = false, WrapContents = false };
        
        var titleLabel = new Label 
        { 
            Text = "Join a Society", 
            Font = new Font("Segoe UI", 16, FontStyle.Bold), 
            ForeColor = Color.FromArgb(41, 128, 185), // Theme color
            AutoSize = true, 
            Margin = new Padding(0, 0, 0, 25) 
        };
        panel.Controls.Add(titleLabel);
        
        var studentLabel = new Label { Text = "Student", ForeColor = Color.FromArgb(64,64,64), Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 10, 0, 5) };
        panel.Controls.Add(studentLabel);
        _studentName.Width = 330;
        _studentName.Font = new Font("Segoe UI", 11);
        _studentName.BorderStyle = BorderStyle.FixedSingle;
        _studentName.Margin = new Padding(0, 0, 0, 20);
        panel.Controls.Add(_studentName);
        
        var societyLabel = new Label { Text = "Society", ForeColor = Color.FromArgb(64,64,64), Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 10, 0, 5) };
        panel.Controls.Add(societyLabel);
        _societyList.Width = 330;
        _societyList.Font = new Font("Segoe UI", 11);
        _societyList.Margin = new Padding(0, 0, 0, 30);
        panel.Controls.Add(_societyList);
        
        var btn = new Button 
        { 
            Text = "Submit Application", 
            Width = 330, 
            Height = 40, 
            Margin = new Padding(0, 10, 0, 0),
            BackColor = Color.FromArgb(41, 128, 185),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.Click += (_, _) => {
            if (_societyList.SelectedValue is not int societyId)
            {
                MessageBox.Show("Please select a society.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (SessionManager.CurrentUserId is null)
            {
                MessageBox.Show("Please log in again.", "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _membershipBll.Apply(SessionManager.CurrentUserId.Value, societyId);
            MessageBox.Show("Membership application submitted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            Close();
        };
        panel.Controls.Add(btn);
        Controls.Add(panel);
    }

    private void LoadSocieties()
    {
        var societies = _societyBll.GetAll();
        _societyList.DisplayMember = "Name";
        _societyList.ValueMember = "SocietyId";
        _societyList.DataSource = societies;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        _studentName.Text = SessionManager.CurrentUserName ?? string.Empty;
    }
}
