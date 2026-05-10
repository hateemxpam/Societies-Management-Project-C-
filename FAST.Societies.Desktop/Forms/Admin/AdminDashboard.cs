using FAST.Societies.Desktop.BLL;

namespace FAST.Societies.Desktop.Forms.Admin;

public partial class AdminDashboard : Form
{
    private readonly AdminBLL _adminBll = new();
    private readonly StudentBLL _studentBll = new();
    private readonly SocietyBLL _societyBll = new();
    private readonly EventBLL _eventBll = new();
    private readonly DataGridView _studentsGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly DataGridView _societyGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly DataGridView _eventGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly Label _stats = new() { AutoSize = true };

    public AdminDashboard()
    {
        InitializeComponent();
        LoadData();
    }

    private void InitializeComponent()
    {
        Text = "Admin Dashboard";
        Width = 1200;
        Height = 800;
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(245, 247, 250);

        Button CreateNavButton(string text, int width = 140)
        {
            return new Button
            {
                Text = text,
                Width = width,
                Height = 36,
                Margin = new Padding(5, 0, 5, 0),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        var navBar = new FlowLayoutPanel 
        { 
            Dock = DockStyle.Top, 
            Height = 60, 
            Padding = new Padding(15, 10, 15, 10), 
            BackColor = Color.FromArgb(41, 128, 185), 
            AutoScroll = false,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true 
        };

        var refresh = CreateNavButton("Refresh Data", 120);
        refresh.FlatAppearance.BorderSize = 0;
        refresh.Click += (_, _) => LoadData();

        var manageStudents = CreateNavButton("Manage Students", 150);
        manageStudents.FlatAppearance.BorderSize = 0;
        manageStudents.Click += (_, _) => new ManageStudents().ShowDialog();

        var manageSocieties = CreateNavButton("Manage Societies", 150);
        manageSocieties.FlatAppearance.BorderSize = 0;
        manageSocieties.Click += (_, _) => new ManageSocieties().ShowDialog();

        var eventApproval = CreateNavButton("Event Approval", 140);
        eventApproval.FlatAppearance.BorderSize = 0;
        eventApproval.Click += (_, _) => new EventApproval().ShowDialog();

        var reports = CreateNavButton("University Reports", 150);
        reports.FlatAppearance.BorderSize = 0;
        reports.Click += (_, _) => new UniversityReports().ShowDialog();

        var demo = CreateNavButton("Insert Demo Data", 150);
        demo.FlatAppearance.BorderSize = 1;
        demo.FlatAppearance.BorderColor = Color.White;
        demo.Click += (_, _) =>
        {
            _adminBll.SeedDemoData();
            LoadData();
            MessageBox.Show("Demo data inserted/refreshed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };

        navBar.Controls.Add(refresh);
        navBar.Controls.Add(manageStudents);
        navBar.Controls.Add(manageSocieties);
        navBar.Controls.Add(eventApproval);
        navBar.Controls.Add(reports);
        navBar.Controls.Add(demo);
        Controls.Add(navBar);

        var summary = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, BackColor = Color.White, Padding = new Padding(20, 15, 20, 10) };
        _stats.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        _stats.ForeColor = Color.FromArgb(44, 62, 80);
        _stats.Margin = new Padding(5, 0, 10, 0);
        summary.Controls.Add(_stats);
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

        StyleGrid(_studentsGrid);
        StyleGrid(_societyGrid);
        StyleGrid(_eventGrid);

        var tabs = new TabControl { Dock = DockStyle.Fill, ItemSize = new Size(120, 30), Font = new Font("Segoe UI", 10) };
        var t1 = new TabPage("Students") { BackColor = Color.White };
        t1.Controls.Add(_studentsGrid);
        var t2 = new TabPage("Societies") { BackColor = Color.White };
        t2.Controls.Add(_societyGrid);
        var t3 = new TabPage("Events") { BackColor = Color.White };
        t3.Controls.Add(_eventGrid);
        tabs.TabPages.Add(t1);
        tabs.TabPages.Add(t2);
        tabs.TabPages.Add(t3);
        
        Controls.Add(tabs);
        navBar.BringToFront();
    }

    private void LoadData()
    {
        _studentsGrid.DataSource = _studentBll.GetAll();
        _societyGrid.DataSource = _societyBll.GetAll();
        _eventGrid.DataSource = _eventBll.GetUpcoming();
        var s = _adminBll.GetStats();
        _stats.Text = $"Students: {s.Students} | Societies: {s.Societies} | Events: {s.Events} | Memberships: {s.Memberships} | Tasks: {s.Tasks}";
    }
}


