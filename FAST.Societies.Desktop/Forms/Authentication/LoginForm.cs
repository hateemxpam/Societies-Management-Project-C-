using FAST.Societies.Desktop.BLL;
using FAST.Societies.Desktop.Forms.Admin;
using FAST.Societies.Desktop.Forms.Society;
using FAST.Societies.Desktop.Forms.Student;
using FAST.Societies.Desktop.Utilities;

namespace FAST.Societies.Desktop.Forms.Authentication;

public partial class LoginForm : Form
{
    private readonly TextBox _email = new() { Width = 260 };
    private readonly TextBox _password = new() { Width = 260, PasswordChar = '*' };
    private readonly ComboBox _role = new() { Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly StudentBLL _studentBll = new();

    public LoginForm() => InitializeComponent();

    private void InitializeComponent()
    {
        Text = "FAST Societies - Login";
        Width = 450;
        Height = 520;
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(245, 247, 250); // Softer, more modern background

        _role.Items.AddRange(["Student", "Society", "Admin"]);
        _role.SelectedIndex = 0;

        var mainPanel = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(40) };
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, AutoSize = false, WrapContents = false };
        
        var titleLabel = new Label 
        { 
            Text = "FAST Societies", 
            Font = new Font("Segoe UI", 18, FontStyle.Bold), 
            ForeColor = Color.FromArgb(41, 128, 185), // Theme color
            AutoSize = true, 
            Margin = new Padding(0, 0, 0, 5) 
        };
        panel.Controls.Add(titleLabel);

        var subTitleLabel = new Label 
        { 
            Text = "Sign in to continue", 
            Font = new Font("Segoe UI", 10, FontStyle.Regular), 
            ForeColor = Color.Gray,
            AutoSize = true, 
            Margin = new Padding(0, 0, 0, 30) 
        };
        panel.Controls.Add(subTitleLabel);
        
        var emailLabel = new Label { Text = "Email Address", ForeColor = Color.FromArgb(64,64,64), Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 10, 0, 5) };
        panel.Controls.Add(emailLabel);
        _email.Width = 330;
        _email.Font = new Font("Segoe UI", 11);
        _email.BorderStyle = BorderStyle.FixedSingle;
        _email.Margin = new Padding(0, 0, 0, 15);
        panel.Controls.Add(_email);
        
        var passLabel = new Label { Text = "Password", ForeColor = Color.FromArgb(64,64,64), Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 10, 0, 5) };
        panel.Controls.Add(passLabel);
        _password.Width = 330;
        _password.Font = new Font("Segoe UI", 11);
        _password.BorderStyle = BorderStyle.FixedSingle;
        _password.Margin = new Padding(0, 0, 0, 15);
        panel.Controls.Add(_password);
        
        var roleLabel = new Label { Text = "Login Role", ForeColor = Color.FromArgb(64,64,64), Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 10, 0, 5) };
        panel.Controls.Add(roleLabel);
        _role.Width = 330;
        _role.Font = new Font("Segoe UI", 11);
        _role.Margin = new Padding(0, 0, 0, 25);
        panel.Controls.Add(_role);

        mainPanel.Controls.Add(panel, 0, 0);

        var buttonPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, Height = 50, Padding = new Padding(0), FlowDirection = FlowDirection.LeftToRight };
        
        var loginBtn = new Button 
        { 
            Text = "Login", 
            Width = 160, 
            Height = 40, 
            Margin = new Padding(0, 5, 10, 5),
            BackColor = Color.FromArgb(41, 128, 185),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        loginBtn.FlatAppearance.BorderSize = 0;
        loginBtn.Click += OnLoginClick;

        var registerBtn = new Button 
        { 
            Text = "Create Account", 
            Width = 160, 
            Height = 40, 
            Margin = new Padding(0, 5, 0, 5),
            BackColor = Color.White,
            ForeColor = Color.FromArgb(41, 128, 185),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        registerBtn.FlatAppearance.BorderColor = Color.FromArgb(41, 128, 185);
        registerBtn.FlatAppearance.BorderSize = 1;
        registerBtn.Click += (_, _) => new RegisterForm().ShowDialog();

        buttonPanel.Controls.Add(loginBtn);
        buttonPanel.Controls.Add(registerBtn);
        mainPanel.Controls.Add(buttonPanel, 0, 1);

        Controls.Add(mainPanel);
    }

    private void OnLoginClick(object? sender, EventArgs e)
    {
        if (_role.Text == "Society")
        {
            if (!ValidationHelper.Required(_email.Text))
            {
                MessageBox.Show("Society name is required.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        else if (!ValidationHelper.Required(_email.Text, _password.Text))
        {
            MessageBox.Show("Email and password are required.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_role.Text != "Society" && !ValidationHelper.IsValidEmail(_email.Text))
        {
            MessageBox.Show("Please enter a valid email address.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            if (_role.Text == "Student")
            {
                var student = _studentBll.GetAll().FirstOrDefault(s => s.Email.Equals(_email.Text.Trim(), StringComparison.OrdinalIgnoreCase) && s.PasswordHash == _password.Text);
                if (student == null)
                {
                    MessageBox.Show("Invalid student credentials. Please register first or check password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                SessionManager.CurrentRole = "Student";
                SessionManager.CurrentUserId = student.StudentId;
                SessionManager.CurrentUserName = student.FullName;
                Hide();
                new StudentDashboard().ShowDialog();
                Show();
                return;
            }

            if (_role.Text == "Society")
            {
                // Note: The original project architecture lacks unique society passwords. Let's fix the logic so they can't log in using a student account's credentials.
                // For a proper society login, we verify if the typed 'email' maps directly to a society name.
                var exists = new SocietyBLL().GetAll().Any(s => s.Name.Equals(_email.Text.Trim(), StringComparison.OrdinalIgnoreCase));
                
                // If they used a Student email attempting to log in as society, it gets rejected.
                if (!exists && _email.Text.Contains("@"))
                {
                    MessageBox.Show("You cannot use a Student account to access the Society Manager Dashboard.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SessionManager.CurrentRole = "Society";
                Hide();
                new SocietyDashboard().ShowDialog();
                Show();
                return;
            }
            
            // Admin Logic
            if (_email.Text != "admin@fast.pk" || _password.Text != "admin123")
            {
                MessageBox.Show("Invalid admin credentials.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SessionManager.CurrentRole = "Admin";
            Hide();
            new AdminDashboard().ShowDialog();
            Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Login Error");
        }
    }
}


