using FAST.Societies.Desktop.BLL;

namespace FAST.Societies.Desktop.Forms.Student;

public partial class MembershipStatus : Form
{
    private readonly MembershipBLL _bll = new();
    private readonly NumericUpDown _studentId = new() { Minimum = 1, Maximum = 999999 };
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

    public MembershipStatus()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Membership Status";
        Width = 1000;
        Height = 600;
        StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Segoe UI", 9);
        
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 50, Padding = new Padding(15), BackColor = Color.WhiteSmoke };
        top.Controls.Add(new Label { Text = "Student ID:", Width = 80, TextAlign = ContentAlignment.MiddleRight, Margin = new Padding(5) });
        top.Controls.Add(_studentId);
        var btn = new Button { Text = "Load Status", Width = 110, Height = 32, Margin = new Padding(5) };
        btn.Click += (_, _) => _grid.DataSource = _bll.GetByStudent((int)_studentId.Value);
        top.Controls.Add(btn);
        Controls.Add(top);
        
        _grid.Dock = DockStyle.Fill;
        Controls.Add(_grid);
    }
}
