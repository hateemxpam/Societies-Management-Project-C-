using FAST.Societies.Desktop.BLL;

namespace FAST.Societies.Desktop.Forms.Society;

public partial class AssignTasks : Form
{
    private readonly TaskBLL _bll = new();
    private readonly NumericUpDown _societyId = new() { Minimum = 1, Maximum = 999999 };
    private readonly NumericUpDown _studentId = new() { Minimum = 1, Maximum = 999999 };
    private readonly TextBox _title = new() { Width = 180 };
    private readonly DateTimePicker _due = new() { Format = DateTimePickerFormat.Short };
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

    public AssignTasks()
    {
        InitializeComponent();
        LoadData();
    }

    private void InitializeComponent()
    {
        Text = "Assign Tasks";
        Width = 1200; // Increased width so fields fit better in one line on desktop
        Height = 650;
        StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(245, 247, 250);
        
        var top = new FlowLayoutPanel 
        { 
            Dock = DockStyle.Top, 
            AutoSize = true,
            Padding = new Padding(15, 20, 15, 20), 
            BackColor = Color.White,
            WrapContents = true
        };
        
        _societyId.Font = new Font("Segoe UI", 10);
        _societyId.Width = 90;
        _societyId.Margin = new Padding(0, 5, 10, 5);

        _studentId.Font = new Font("Segoe UI", 10);
        _studentId.Width = 90;
        _studentId.Margin = new Padding(0, 5, 10, 5);

        _title.Font = new Font("Segoe UI", 10);
        _title.Margin = new Padding(0, 5, 10, 5);

        _due.Font = new Font("Segoe UI", 10);
        _due.Width = 110;
        _due.Margin = new Padding(0, 5, 20, 5);

        top.Controls.Add(new Label { Text = "Society ID:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5, 10, 5, 5) });
        top.Controls.Add(_societyId);
        top.Controls.Add(new Label { Text = "Student ID:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5, 10, 5, 5) });
        top.Controls.Add(_studentId);
        top.Controls.Add(new Label { Text = "Task Title:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5, 10, 5, 5) });
        top.Controls.Add(_title);
        top.Controls.Add(new Label { Text = "Due Date:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5, 10, 5, 5) });
        top.Controls.Add(_due);
        
        var add = new Button 
        { 
            Text = "Assign Task", 
            Width = 120, 
            Height = 32, 
            Margin = new Padding(5, 2, 5, 5),
            BackColor = Color.FromArgb(41, 128, 185),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        add.FlatAppearance.BorderSize = 0;
        add.Click += (_, _) => { 
            _bll.Assign((int)_societyId.Value, (int)_studentId.Value, _title.Text.Trim(), _due.Value.Date); 
            MessageBox.Show("Task assigned successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            LoadData(); 
        };

        var refresh = new Button 
        { 
            Text = "Refresh List", 
            Width = 110, 
            Height = 32, 
            Margin = new Padding(5, 2, 5, 5),
            BackColor = Color.FromArgb(149, 165, 166),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        refresh.FlatAppearance.BorderSize = 0;
        refresh.Click += (_, _) => LoadData();

        top.Controls.Add(add);
        top.Controls.Add(refresh);
        Controls.Add(top);
        
        // Grid Styling
        _grid.BackgroundColor = Color.White;
        _grid.BorderStyle = BorderStyle.None;
        _grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        _grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
        _grid.DefaultCellStyle.SelectionForeColor = Color.White;
        _grid.DefaultCellStyle.Padding = new Padding(5);
        _grid.RowTemplate.Height = 35;
        _grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(236, 240, 241);
        _grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(44, 62, 80);
        _grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        _grid.ColumnHeadersHeight = 40;
        _grid.EnableHeadersVisualStyles = false;
        
        _grid.Dock = DockStyle.Fill;
        var gridPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        gridPanel.Controls.Add(_grid);
        Controls.Add(gridPanel);
        
        top.BringToFront();
    }

    private void LoadData() => _grid.DataSource = _bll.GetAll();
}
