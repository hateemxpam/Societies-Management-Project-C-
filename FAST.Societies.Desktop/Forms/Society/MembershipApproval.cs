using FAST.Societies.Desktop.BLL;

namespace FAST.Societies.Desktop.Forms.Society;

public partial class MembershipApproval : Form
{
    private readonly MembershipBLL _bll = new();
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
    private readonly NumericUpDown _membershipId = new() { Minimum = 1, Maximum = 999999 };

    public MembershipApproval()
    {
        InitializeComponent();
        LoadData();
    }

    private void InitializeComponent()
    {
        Text = "Membership Approval";
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
        
        var refresh = new Button 
        { 
            Text = "Refresh List", 
            Width = 110, 
            Height = 32, 
            Margin = new Padding(5, 2, 20, 5),
            BackColor = Color.FromArgb(149, 165, 166),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        refresh.FlatAppearance.BorderSize = 0;
        refresh.Click += (_, _) => LoadData();
        top.Controls.Add(refresh);

        _membershipId.Font = new Font("Segoe UI", 10);
        _membershipId.Width = 100;
        _membershipId.Margin = new Padding(0, 5, 20, 5);

        top.Controls.Add(new Label { Text = "Membership ID:", ForeColor = Color.Black, Font = new Font("Segoe UI", 9), Width = 110, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(5) });
        top.Controls.Add(_membershipId);
        
        var approve = new Button 
        { 
            Text = "Approve", 
            Width = 100, 
            Height = 32, 
            Margin = new Padding(5, 2, 10, 5),
            BackColor = Color.FromArgb(39, 174, 96), // Green
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        approve.FlatAppearance.BorderSize = 0;
        approve.Click += (_, _) => { 
            _bll.UpdateStatus((int)_membershipId.Value, "Approved"); 
            MessageBox.Show("Approved."); 
            LoadData(); 
        };

        var reject = new Button 
        { 
            Text = "Reject", 
            Width = 100, 
            Height = 32, 
            Margin = new Padding(5, 2, 5, 5),
            BackColor = Color.FromArgb(231, 76, 60), // Red
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        reject.FlatAppearance.BorderSize = 0;
        reject.Click += (_, _) => { 
            _bll.UpdateStatus((int)_membershipId.Value, "Rejected"); 
            MessageBox.Show("Rejected."); 
            LoadData(); 
        };
        
        top.Controls.Add(approve);
        top.Controls.Add(reject);
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

    private void LoadData() => _grid.DataSource = _bll.GetPending();
}
