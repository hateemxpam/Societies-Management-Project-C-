using FAST.Societies.Desktop.BLL;

namespace FAST.Societies.Desktop.Forms.Society;

public partial class ManageSociety : Form
{
    private readonly SocietyBLL _bll = new();
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly TextBox _name = new() { Width = 180 };
    private readonly TextBox _cat = new() { Width = 140 };

    public ManageSociety()
    {
        InitializeComponent();
        LoadData();
    }

    private void InitializeComponent()
    {
        Text = "Manage Society";
        Width = 1000;
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
        
        _name.Font = new Font("Segoe UI", 10);
        _name.Margin = new Padding(0, 5, 10, 5);
        _cat.Font = new Font("Segoe UI", 10);
        _cat.Margin = new Padding(0, 5, 15, 5);

        top.Controls.Add(new Label { Text = "Society Name:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), Width = 100, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5) });
        top.Controls.Add(_name);
        top.Controls.Add(new Label { Text = "Category:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), Width = 80, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5) });
        top.Controls.Add(_cat);
        
        var btn = new Button 
        { 
            Text = "Create", 
            Width = 110, 
            Height = 32, 
            Margin = new Padding(5, 2, 5, 5),
            BackColor = Color.FromArgb(41, 128, 185),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.Click += (_, _) => { 
            _bll.Create(_name.Text.Trim(), _cat.Text.Trim()); 
            MessageBox.Show("Society Created.", "Success"); 
            LoadData(); 
        };
        top.Controls.Add(btn);
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
        
        var mainLayout = new TableLayoutPanel { Dock = DockStyle.Fill };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        
        mainLayout.Controls.Add(top, 0, 0);
        mainLayout.Controls.Add(gridPanel, 0, 1);

        Controls.Add(mainLayout);
    }

    private void LoadData() => _grid.DataSource = _bll.GetAll();
}
