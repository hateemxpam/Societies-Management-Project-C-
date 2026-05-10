using FAST.Societies.Desktop.BLL;

namespace FAST.Societies.Desktop.Forms.Student;

public partial class StudentDashboard : Form
{
    private readonly SocietyBLL _societyBll = new();
    private readonly EventBLL _eventBll = new();
    private readonly MembershipBLL _membershipBll = new();
    private readonly DataGridView _societyGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly DataGridView _eventGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly NumericUpDown _membershipStudentId = new() { Minimum = 1, Maximum = 999999 };
    private readonly NumericUpDown _eventStudentId = new() { Minimum = 1, Maximum = 999999 };
    private readonly NumericUpDown _societyId = new() { Minimum = 1, Maximum = 999999 };
    private readonly NumericUpDown _eventId = new() { Minimum = 1, Maximum = 999999 };
    private readonly Label _societyCount = new() { AutoSize = true };
    private readonly Label _eventCount = new() { AutoSize = true };

    public StudentDashboard()
    {
        InitializeComponent();
        LoadData();
    }

    private void InitializeComponent()
    {
        Text = "Student Dashboard";
        Width = 1200;
        Height = 800;
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(245, 247, 250);

        // Top Navigation Bar
        var navBar = new FlowLayoutPanel 
        { 
            Dock = DockStyle.Top, 
            Height = 60, // Keep height
            Padding = new Padding(15, 10, 15, 10), 
            BackColor = Color.FromArgb(41, 128, 185), // Theme Color
            AutoScroll = false,
            WrapContents = true,  // Important: allow wrapping if form gets small
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true // Important: autosize height if elements wrap
        };

        Button CreateNavButton(string text)
        {
            return new Button
            {
                Text = text,
                Width = 110,
                Height = 36,
                Margin = new Padding(5, 0, 5, 0),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        var refresh = CreateNavButton("Refresh");
        refresh.FlatAppearance.BorderSize = 0;
        refresh.Click += (_, _) => LoadData();
        navBar.Controls.Add(refresh);

        var browse = CreateNavButton("Browse");
        browse.FlatAppearance.BorderSize = 0;
        browse.Click += (_, _) => new BrowseSocieties().ShowDialog();
        navBar.Controls.Add(browse);

        var applyScreen = CreateNavButton("Apply");
        applyScreen.FlatAppearance.BorderSize = 0;
        applyScreen.Click += (_, _) => new ApplyMembership().ShowDialog();
        navBar.Controls.Add(applyScreen);

        var eventsScreen = CreateNavButton("Events");
        eventsScreen.FlatAppearance.BorderSize = 0;
        eventsScreen.Click += (_, _) => new ViewEvents().ShowDialog();
        navBar.Controls.Add(eventsScreen);

        var membershipStatus = CreateNavButton("Status");
        membershipStatus.FlatAppearance.BorderSize = 0;
        membershipStatus.Click += (_, _) => new MembershipStatus().ShowDialog();
        navBar.Controls.Add(membershipStatus);

        var eventReg = CreateNavButton("Register");
        eventReg.FlatAppearance.BorderSize = 0;
        eventReg.Click += (_, _) => new EventRegistration().ShowDialog();
        navBar.Controls.Add(eventReg);
        
        Controls.Add(navBar);

        // Styling for input panels
        void StyleNumericUpDown(NumericUpDown num)
        {
            num.Font = new Font("Segoe UI", 10);
            num.Width = 100;
            num.Margin = new Padding(0, 5, 20, 5);
        }

        StyleNumericUpDown(_membershipStudentId);
        StyleNumericUpDown(_societyId);
        StyleNumericUpDown(_eventStudentId);
        StyleNumericUpDown(_eventId);

        Button CreateActionBtn(string text)
        {
            var btn = new Button 
            { 
                Text = text, 
                Width = 100, 
                Height = 32, 
                Margin = new Padding(10, 2, 5, 5),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        // Membership Application Panel
        var membershipPanel = new GroupBox { Text = "Apply Membership", Dock = DockStyle.Top, Padding = new Padding(15, 20, 15, 10), Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(41, 128, 185), AutoSize = true };
        var membershipFlow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true, FlowDirection = FlowDirection.LeftToRight };
        
        membershipFlow.Controls.Add(new Label { Text = "Student ID:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), Width = 80, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5) });
        membershipFlow.Controls.Add(_membershipStudentId);
        membershipFlow.Controls.Add(new Label { Text = "Society ID:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), Width = 80, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5) });
        membershipFlow.Controls.Add(_societyId);
        
        var apply = CreateActionBtn("Apply");
        apply.Click += (_, _) =>
        {
            _membershipBll.Apply((int)_membershipStudentId.Value, (int)_societyId.Value);
            MessageBox.Show("Membership request submitted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };
        membershipFlow.Controls.Add(apply);
        membershipPanel.Controls.Add(membershipFlow);
        Controls.Add(membershipPanel);

        // Event Registration Panel
        var eventPanel = new GroupBox { Text = "Register Event", Dock = DockStyle.Top, Padding = new Padding(15, 20, 15, 10), Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(41, 128, 185), AutoSize = true };
        var eventFlow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true, FlowDirection = FlowDirection.LeftToRight };
        
        eventFlow.Controls.Add(new Label { Text = "Student ID:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), Width = 80, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5) });
        eventFlow.Controls.Add(_eventStudentId);
        eventFlow.Controls.Add(new Label { Text = "Event ID:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), Width = 80, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5) });
        eventFlow.Controls.Add(_eventId);
        
        var reg = CreateActionBtn("Register");
        reg.Click += (_, _) =>
        {
            _eventBll.RegisterStudent((int)_eventStudentId.Value, (int)_eventId.Value);
            MessageBox.Show("Event registration done.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };
        eventFlow.Controls.Add(reg);
        eventPanel.Controls.Add(eventFlow);
        Controls.Add(eventPanel);

        // Summary Panel
        var summary = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, BackColor = Color.White, Padding = new Padding(20, 15, 20, 10) };
        _societyCount.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        _societyCount.ForeColor = Color.FromArgb(44, 62, 80);
        _societyCount.Margin = new Padding(5, 0, 10, 0);
        
        _eventCount.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        _eventCount.ForeColor = Color.FromArgb(44, 62, 80);
        _eventCount.Margin = new Padding(10, 0, 5, 0);

        summary.Controls.Add(_societyCount);
        summary.Controls.Add(new Label { Text = " | ", Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.LightGray, AutoSize = true, Margin = new Padding(5, 0, 5, 0) });
        summary.Controls.Add(_eventCount);
        Controls.Add(summary);

        // Grid Styling
        void StyleGrid(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.Padding = new Padding(5);
            grid.RowTemplate.Height = 35;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(236, 240, 241);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(44, 62, 80);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.ColumnHeadersHeight = 40;
            grid.EnableHeadersVisualStyles = false;
        }

        StyleGrid(_societyGrid);
        StyleGrid(_eventGrid);

        // Tabs for Data Display
        var tabs = new TabControl { Dock = DockStyle.Fill };
        var societiesTab = new TabPage("Societies");
        societiesTab.Controls.Add(_societyGrid);
        var eventsTab = new TabPage("Events");
        eventsTab.Controls.Add(_eventGrid);
        tabs.TabPages.Add(societiesTab);
        tabs.TabPages.Add(eventsTab);
        Controls.Add(tabs);
    }

    private void LoadData()
    {
        var societies = _societyBll.GetAll();
        var eventsList = _eventBll.GetUpcoming();
        _societyGrid.DataSource = societies;
        _eventGrid.DataSource = eventsList;
        _societyCount.Text = $"Total Societies: {societies.Count}";
        _eventCount.Text = $"Upcoming Events: {eventsList.Count}";
    }
}


