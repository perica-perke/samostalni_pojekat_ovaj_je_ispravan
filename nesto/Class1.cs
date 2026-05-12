using System;
using System.Drawing;
using System.Windows.Forms;

namespace nesto
{
    public class LoginForm : Form
    {
        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;

        private TextBox txtUsername;
        private TextBox txtPassword;

        private Button btnLogin;
        private Button btnSignup;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Login";
            this.Size = new Size(400, 320);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 10f);
            this.BackColor = Color.White;

            lblTitle = new Label
            {
                Text = "LOGIN / SIGN UP",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(85, 30)
            };

            lblUsername = new Label
            {
                Text = "Username",
                AutoSize = true,
                Location = new Point(50, 95)
            };

            txtUsername = new TextBox
            {
                Size = new Size(280, 30),
                Location = new Point(50, 120)
            };

            lblPassword = new Label
            {
                Text = "Password",
                AutoSize = true,
                Location = new Point(50, 160)
            };

            txtPassword = new TextBox
            {
                Size = new Size(280, 30),
                Location = new Point(50, 185),
                UseSystemPasswordChar = true
            };

            btnLogin = new Button
            {
                Text = "Log In",
                Size = new Size(120, 40),
                Location = new Point(50, 235),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnSignup = new Button
            {
                Text = "Sign Up",
                Size = new Size(120, 40),
                Location = new Point(210, 235),
                BackColor = Color.FromArgb(46, 139, 87),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnLogin.Click += BtnLogin_Click;
            btnSignup.Click += BtnSignup_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnSignup);

            this.AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show(
                    "Unesi username i password.",
                    "Upozorenje",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            bool success = DatabaseHelper.LoginUser(username, password);

            if (success)
            {
                MessageBox.Show(
                    "Uspesan login.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                FormMain main = new FormMain();

                main.FormClosed += (s, args) => this.Close();

                main.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show(
                    "Pogresan username ili password.",
                    "Greska",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnSignup_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show(
                    "Unesi username i password.",
                    "Upozorenje",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (DatabaseHelper.UserExists(username))
            {
                MessageBox.Show(
                    "Korisnik vec postoji.",
                    "Greska",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            try
            {
                DatabaseHelper.InsertKorisnik(username, password);

                MessageBox.Show(
                    "Account napravljen.",
                    "Uspeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                txtPassword.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Greska",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
