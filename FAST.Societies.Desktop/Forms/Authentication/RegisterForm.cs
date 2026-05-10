using FAST.Societies.Desktop.BLL;
using FAST.Societies.Desktop.Utilities;

namespace FAST.Societies.Desktop.Forms.Authentication;

public partial class RegisterForm : Form
{
    private readonly StudentBLL _studentBll = new();
    private readonly TextBox _name = new() { Width = 260 };
    private readonly TextBox _email = new() { Width = 260 };
    private readonly TextBox _password = new() { Width = 260, PasswordChar = '*' };

    public RegisterForm() => InitializeComponent();

    private void InitializeComponent()
    {
        Text = "Student Registration";
        Width = 450;
        Height = 520; // Increased height to ensure all fields fit vertically
        StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(245, 247, 250); // Match Login Form background

        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(40), FlowDirection = FlowDirection.TopDown, AutoSize = false, WrapContents = false };
        
        var titleLabel = new Label 
        { 
            Text = "Create Account", 
            Font = new Font("Segoe UI", 18, FontStyle.Bold), 
            ForeColor = Color.FromArgb(41, 128, 185), // Theme color
            AutoSize = true, 
            Margin = new Padding(0, 0, 0, 5) 
        };
        panel.Controls.Add(titleLabel);

        var subTitleLabel = new Label 
        { 
            Text = "Join a society and stay updated", 
            Font = new Font("Segoe UI", 10, FontStyle.Regular), 
            ForeColor = Color.Gray,
            AutoSize = true, 
            Margin = new Padding(0, 0, 0, 25) 
        };
        panel.Controls.Add(subTitleLabel);
        
        var nameLabel = new Label { Text = "Full Name", ForeColor = Color.FromArgb(64,64,64), Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 10, 0, 5) };
        panel.Controls.Add(nameLabel);
        _name.Width = 330;
        _name.Font = new Font("Segoe UI", 11);
        _name.BorderStyle = BorderStyle.FixedSingle;
        _name.Margin = new Padding(0, 0, 0, 15);
        panel.Controls.Add(_name);
        
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
        _password.Margin = new Padding(0, 0, 0, 25);
        panel.Controls.Add(_password);

        var btn = new Button 
        { 
            Text = "Register", 
            Width = 330, 
            Height = 40, 
            Margin = new Padding(0, 10, 0, 0),
            BackColor = Color.FromArgb(41, 128, 185),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.Click += (_, _) =>
        {
            if (!ValidationHelper.Required(_name.Text, _email.Text, _password.Text))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidationHelper.IsValidEmail(_email.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidationHelper.IsValidPassword(_password.Text))
            {
                MessageBox.Show("Password must be at least 6 characters long and contain both letters and numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _studentBll.Register(_name.Text.Trim(), _email.Text.Trim(), _password.Text.Trim());
                MessageBox.Show("Account created successfully.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Registration Error");
            }
        };
        panel.Controls.Add(btn);
        Controls.Add(panel);
    }
}


