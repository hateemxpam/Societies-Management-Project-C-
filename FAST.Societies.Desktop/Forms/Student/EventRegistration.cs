using FAST.Societies.Desktop.BLL;
using FAST.Societies.Desktop.Utilities;
namespace FAST.Societies.Desktop.Forms.Student;

public partial class EventRegistration : Form
{
    private readonly EventBLL _bll = new();
    private readonly TextBox _studentName = new() { ReadOnly = true };
    private readonly ComboBox _eventList = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    public EventRegistration()
    {
        InitializeComponent();
        LoadEvents();
    }

    private void InitializeComponent()
    {
        Text = "Event Registration";
        Width = 450;
        Height = 400; // Increased height to prevent overlap
        StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(245, 247, 250); // Matching theme

        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(40), FlowDirection = FlowDirection.TopDown, AutoSize = false, WrapContents = false };
        
        var titleLabel = new Label 
        { 
            Text = "Register for Event", 
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
        
        var eventLabel = new Label { Text = "Event", ForeColor = Color.FromArgb(64,64,64), Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 10, 0, 5) };
        panel.Controls.Add(eventLabel);
        _eventList.Width = 330;
        _eventList.Font = new Font("Segoe UI", 11);
        _eventList.Margin = new Padding(0, 0, 0, 30);
        panel.Controls.Add(_eventList);
        
        var btn = new Button 
        { 
            Text = "Register", 
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
        btn.Click += (_, _) => 
        { 
            if (SessionManager.CurrentUserId is null)
            {
                MessageBox.Show("Please log in again.", "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_eventList.SelectedValue is not int eventId)
            {
                MessageBox.Show("Please select an event.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _bll.RegisterStudent(SessionManager.CurrentUserId.Value, eventId); 
            MessageBox.Show("Event registration completed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            Close(); 
        };
        panel.Controls.Add(btn);
        Controls.Add(panel);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        _studentName.Text = SessionManager.CurrentUserName ?? string.Empty;
    }

    private void LoadEvents()
    {
        var events = _bll.GetUpcomingWithSocietyName();
        _eventList.DisplayMember = "DisplayName";
        _eventList.ValueMember = "EventId";
        _eventList.DataSource = events;
    }
}

