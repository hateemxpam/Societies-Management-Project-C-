using FAST.Societies.Desktop.BLL;

namespace FAST.Societies.Desktop.Forms.Student;

public partial class ViewEvents : Form
{
    private readonly EventBLL _bll = new();
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    public ViewEvents()
    {
        InitializeComponent();
        _grid.DataSource = _bll.GetUpcoming();
    }

    private void InitializeComponent()
    {
        Text = "Upcoming Events";
        Width = 1000;
        Height = 650;
        StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(245, 247, 250);
        
        var topPanel = new FlowLayoutPanel 
        { 
            Dock = DockStyle.Top, 
            Height = 60, 
            Padding = new Padding(15, 12, 15, 12),
            BackColor = Color.FromArgb(41, 128, 185) 
        };
        
        var refreshBtn = new Button 
        { 
            Text = "Refresh List", 
            Width = 120, 
            Height = 36,
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        refreshBtn.FlatAppearance.BorderSize = 0;
        refreshBtn.Click += (_, _) => _grid.DataSource = _bll.GetUpcoming();
        topPanel.Controls.Add(refreshBtn);
        Controls.Add(topPanel);

        // Styling the DataGridView
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
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        
        // Create a wrapper panel to contain the grid for proper docking below the top panel
        var gridPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        gridPanel.Controls.Add(_grid);
        
        Controls.Add(gridPanel);
        
        // Push top panel to top just in case
        topPanel.BringToFront();
    }
}
