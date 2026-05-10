using FAST.Societies.Desktop.BLL;
using FAST.Societies.Desktop.Forms.Reports;

namespace FAST.Societies.Desktop.Forms.Society;

public partial class GenerateReports : Form
{
    private readonly AdminBLL _adminBll = new();

    public GenerateReports()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Generate Reports";
        Width = 900;
        Height = 600;
        StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(245, 247, 250);
        
        var mainLayout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(20) };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        
        var btn = new Button 
        { 
            Text = "Pull Text Summary metrics", 
            Height = 40,
            BackColor = Color.FromArgb(41, 128, 185),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 0, 10)
        };
        btn.FlatAppearance.BorderSize = 0;
        
        var box = new TextBox { Multiline = true, Dock = DockStyle.Fill, ReadOnly = true, Font = new Font("Consolas", 11), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(0) };
        
        btn.Click += (_, _) =>
        {
            var s = _adminBll.GetStats();
            box.Text = $"=====================================\r\n     SOCIETY SUMMARY REPORT      \r\n=====================================\r\n\r\nTotal Students:    {s.Students}\r\nSocieties:         {s.Societies}\r\nTotal Events:      {s.Events}\r\nTotal Memberships: {s.Memberships}\r\nTotal Tasks:       {s.Tasks}\r\n\r\n-------------------------------------\r\nReport Generated:  {DateTime.Now:g}\r\n=====================================";
        };

        var buttons = new FlowLayoutPanel 
        { 
            Dock = DockStyle.Fill, 
            Height = 55, 
            Padding = new Padding(0, 0, 0, 20),
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true
        };

        Button CreateReportBtn(string text)
        {
            return new Button
            {
                Text = text,
                Width = 140,
                Height = 36,
                Margin = new Padding(0, 0, 15, 10),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        var students = CreateReportBtn("Students PDF");
        students.FlatAppearance.BorderSize = 0;
        students.Click += (_, _) => new RdlcReportForm("Students").ShowDialog();

        var societies = CreateReportBtn("Societies PDF");
        societies.FlatAppearance.BorderSize = 0;
        societies.Click += (_, _) => new RdlcReportForm("Societies").ShowDialog();

        var eventsBtn = CreateReportBtn("Events PDF");
        eventsBtn.FlatAppearance.BorderSize = 0;
        eventsBtn.Click += (_, _) => new RdlcReportForm("Events").ShowDialog();

        buttons.Controls.Add(students);
        buttons.Controls.Add(societies);
        buttons.Controls.Add(eventsBtn);
        
        mainLayout.Controls.Add(buttons, 0, 0);
        mainLayout.Controls.Add(btn, 0, 1);
        mainLayout.Controls.Add(box, 0, 2);

        Controls.Add(mainLayout);
    }
}
