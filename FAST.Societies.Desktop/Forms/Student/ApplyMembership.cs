using FAST.Societies.Desktop.BLL;

namespace FAST.Societies.Desktop.Forms.Student;

public partial class ApplyMembership : Form
{
    private readonly MembershipBLL _membershipBll = new();
    private readonly NumericUpDown _studentId = new() { Minimum = 1, Maximum = 999999 };
    private readonly NumericUpDown _societyId = new() { Minimum = 1, Maximum = 999999 };

    public ApplyMembership()
    {
        InitializeComponent();
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
        
        var studentLabel = new Label { Text = "Student ID", ForeColor = Color.FromArgb(64,64,64), Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 10, 0, 5) };
        panel.Controls.Add(studentLabel);
        _studentId.Width = 330;
        _studentId.Font = new Font("Segoe UI", 11);
        _studentId.Margin = new Padding(0, 0, 0, 20);
        panel.Controls.Add(_studentId);
        
        var societyLabel = new Label { Text = "Society ID", ForeColor = Color.FromArgb(64,64,64), Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 10, 0, 5) };
        panel.Controls.Add(societyLabel);
        _societyId.Width = 330;
        _societyId.Font = new Font("Segoe UI", 11);
        _societyId.Margin = new Padding(0, 0, 0, 30);
        panel.Controls.Add(_societyId);
        
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
            _membershipBll.Apply((int)_studentId.Value, (int)_societyId.Value); 
            MessageBox.Show("Membership application submitted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            Close();
        };
        panel.Controls.Add(btn);
        Controls.Add(panel);
    }
}
