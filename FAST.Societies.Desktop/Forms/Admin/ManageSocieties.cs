using FAST.Societies.Desktop.BLL;

namespace FAST.Societies.Desktop.Forms.Admin;

public partial class ManageSocieties : Form
{
    private readonly SocietyBLL _bll = new();
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    public ManageSocieties()
    {
        InitializeComponent();
        _grid.DataSource = _bll.GetAll();
    }

    private void InitializeComponent()
    {
        Text = "Manage Societies";
        Width = 1000;
        Height = 650;
        StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(245, 247, 250);
        
        var topPanel = new FlowLayoutPanel 
        { 
            Dock = DockStyle.Top, 
            AutoSize = true,
            Padding = new Padding(15, 12, 15, 12),
            BackColor = Color.White,
            WrapContents = true
        };
        
        var refreshBtn = new Button 
        { 
            Text = "Refresh List", 
            Width = 120, 
            Height = 36,
            BackColor = Color.FromArgb(41, 128, 185),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Margin = new Padding(5, 2, 5, 2)
        };
        refreshBtn.FlatAppearance.BorderSize = 0;
        refreshBtn.Click += (_, _) => _grid.DataSource = _bll.GetAll();
        topPanel.Controls.Add(refreshBtn);
        
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

        var mainLayout = new TableLayoutPanel { Dock = DockStyle.Fill };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        
        mainLayout.Controls.Add(topPanel, 0, 0);
        mainLayout.Controls.Add(gridPanel, 0, 1);

        Controls.Add(mainLayout);
    }
}
