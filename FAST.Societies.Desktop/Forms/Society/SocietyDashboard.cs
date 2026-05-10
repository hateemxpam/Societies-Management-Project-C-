using FAST.Societies.Desktop.BLL;

namespace FAST.Societies.Desktop.Forms.Society;

public partial class SocietyDashboard : Form
{
    private readonly SocietyBLL _societyBll = new();
    private readonly EventBLL _eventBll = new();
    private readonly DataGridView _societyGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly DataGridView _eventGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly TextBox _societyName = new() { Width = 160 };
    private readonly TextBox _societyCategory = new() { Width = 120 };
    private readonly NumericUpDown _societyId = new() { Minimum = 1, Maximum = 999999 };
    private readonly TextBox _eventTitle = new() { Width = 160 };
    private readonly DateTimePicker _eventDate = new() { Format = DateTimePickerFormat.Short, Width = 120 };
    private readonly Label _societyCount = new() { AutoSize = true };
    private readonly Label _eventCount = new() { AutoSize = true };

    public SocietyDashboard()
    {
        InitializeComponent();
        LoadData();
    }

    private void InitializeComponent()
    {
        Text = "Society Dashboard";
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

        Button CreateActionBtn(string text, int width = 140)
        {
            var btn = new Button
            {
                Text = text,
                Width = width,
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

        // Top Navigation Bar - Actions
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

        var refresh = CreateNavButton("Refresh List", 110);
        refresh.FlatAppearance.BorderSize = 0;
        refresh.Click += (_, _) => LoadData();
        navBar.Controls.Add(refresh);

        var manageSoc = CreateNavButton("Manage Society");
        manageSoc.FlatAppearance.BorderSize = 0;
        manageSoc.Click += (_, _) => new ManageSociety().ShowDialog();
        navBar.Controls.Add(manageSoc);

        var manageEvents = CreateNavButton("Manage Events");
        manageEvents.FlatAppearance.BorderSize = 0;
        manageEvents.Click += (_, _) => new ManageEvents().ShowDialog();
        navBar.Controls.Add(manageEvents);

        var approval = CreateNavButton("Membership Approval", 160);
        approval.FlatAppearance.BorderSize = 0;
        approval.Click += (_, _) => new MembershipApproval().ShowDialog();
        navBar.Controls.Add(approval);

        var tasks = CreateNavButton("Assign Tasks");
        tasks.FlatAppearance.BorderSize = 0;
        tasks.Click += (_, _) => new AssignTasks().ShowDialog();
        navBar.Controls.Add(tasks);

        var report = CreateNavButton("Generate Report");
        report.FlatAppearance.BorderSize = 0;
        report.Click += (_, _) => new GenerateReports().ShowDialog();
        navBar.Controls.Add(report);

        Controls.Add(navBar);

        // Styling helpers for panels
        void StyleTextBox(TextBox txt, int width)
        {
            txt.Width = width;
            txt.Font = new Font("Segoe UI", 10);
            txt.Margin = new Padding(0, 5, 20, 5);
        }

        // First Panel - Create Society
        var createSocPanel = new GroupBox { Text = "Create New Society", Dock = DockStyle.Top, Padding = new Padding(15, 20, 15, 10), Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(41, 128, 185), AutoSize = true };
        var createSocFlow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true, FlowDirection = FlowDirection.LeftToRight };

        StyleTextBox(_societyName, 180);
        StyleTextBox(_societyCategory, 140);

        createSocFlow.Controls.Add(new Label { Text = "Society Name:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), Width = 100, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5) });
        createSocFlow.Controls.Add(_societyName);
        createSocFlow.Controls.Add(new Label { Text = "Category:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), Width = 80, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5) });
        createSocFlow.Controls.Add(_societyCategory);

        var createSocietyBtn = CreateActionBtn("Create Society");
        createSocietyBtn.Click += (_, _) =>
        {
            _societyBll.Create(_societyName.Text.Trim(), _societyCategory.Text.Trim());
            MessageBox.Show("Society created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        };
        createSocFlow.Controls.Add(createSocietyBtn);
        createSocPanel.Controls.Add(createSocFlow);
        Controls.Add(createSocPanel);

        // Second Panel - Create Event
        var createEventPanel = new GroupBox { Text = "Create New Event", Dock = DockStyle.Top, Padding = new Padding(15, 20, 15, 10), Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(41, 128, 185), AutoSize = true };
        var createEventFlow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true, FlowDirection = FlowDirection.LeftToRight };

        _societyId.Font = new Font("Segoe UI", 10);
        _societyId.Width = 100;
        _societyId.Margin = new Padding(0, 5, 20, 5);
        StyleTextBox(_eventTitle, 160);
        _eventDate.Font = new Font("Segoe UI", 10);
        _eventDate.Margin = new Padding(0, 5, 20, 5);

        createEventFlow.Controls.Add(new Label { Text = "Society ID:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), Width = 100, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5) });
        createEventFlow.Controls.Add(_societyId);
        createEventFlow.Controls.Add(new Label { Text = "Event Title:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), Width = 80, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5) });
        createEventFlow.Controls.Add(_eventTitle);
        createEventFlow.Controls.Add(new Label { Text = "Date:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), Width = 50, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5) });
        createEventFlow.Controls.Add(_eventDate);

        var createEventBtn = CreateActionBtn("Create Event");
        createEventBtn.Click += (_, _) =>
        {
            _eventBll.Create((int)_societyId.Value, _eventTitle.Text.Trim(), _eventDate.Value.Date);
            MessageBox.Show("Event created and pending approval.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        };
        createEventFlow.Controls.Add(createEventBtn);
        createEventPanel.Controls.Add(createEventFlow);
        Controls.Add(createEventPanel);

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

        var tabs = new TabControl { Dock = DockStyle.Fill, ItemSize = new Size(120, 30), Font = new Font("Segoe UI", 10) };
        var societiesTab = new TabPage("Societies") { BackColor = Color.White };
        societiesTab.Controls.Add(_societyGrid);
        var eventsTab = new TabPage("Events") { BackColor = Color.White };
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
        _eventCount.Text = $"Total Events: {eventsList.Count}";
    }
}


