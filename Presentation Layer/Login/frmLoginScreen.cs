using Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation_Layer
{
    public partial class frmLoginScreen : Form
    {
        private static clsUser _CurrentUser;

        public frmLoginScreen()
        {
            InitializeComponent();
        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = "";

            if (clsGlobal.GetStoredCredential(ref UserName, ref Password))
            {
                txtUsername.Text = UserName;
                txtPassword.Text = Password;
                chkRememberMe.Checked = true;
            }
            else
                chkRememberMe.Checked = false;
        }
        private bool AreAllControlsValid(ErrorProvider errorProvider, Control controlContainer)
        {
            foreach (Control control in controlContainer.Controls)
            {
                // Check for nested controls (e.g., Panels, GroupBoxes)
                if (control.HasChildren)
                {
                    if (!AreAllControlsValid(errorProvider, control))
                    {
                        return false;
                    }
                }

                // Check if there's an error associated with the control
                if (!string.IsNullOrEmpty(errorProvider.GetError(control)))
                {
                    return false;
                }
            }
            return true;
        }

        //Login Trials Counter
        private static byte LoginTrialsCounter = 0;
        private void CloseSystem()
        {
            if (LoginTrialsCounter >= 3)
            {
                MessageBox.Show("This system is closed for security reasons after (3) login trials, (Call IT team for assistance)", "System Closed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = false;
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {

            if(AreAllControlsValid(errorProvider1,this))
            {
                _CurrentUser = clsUser.FindByUsernameAndPassword(txtUsername.Text.ToString(), txtPassword.Text.ToString());

                if (_CurrentUser != null)
                {

                    if (chkRememberMe.Checked)
                    {
                        //store username and password
                        clsGlobal.RememberUsernameAndPassword(txtUsername.Text.Trim(), txtPassword.Text.Trim());

                    }
                    else
                    {
                        //store empty username and password
                        clsGlobal.RememberUsernameAndPassword("", "");

                    }

                    //incase the user is not active
                    if (!_CurrentUser.IsActive)
                    {

                        txtUsername.Focus();
                        MessageBox.Show("Your accound is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    clsGlobal.CurrentUser = _CurrentUser;
                    this.Hide();
                    MainForm frm = new MainForm(this);
                    frm.ShowDialog();


                }
                else
                {
                    MessageBox.Show("Invalid username or password !", "Wrong Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoginTrialsCounter++;
                    CloseSystem();
                }
            }

            else
            {
                MessageBox.Show("Username or password cannot be empty!", "Invalid Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoginTrialsCounter++;
                CloseSystem();
            }
           
        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                e.Cancel = true;
                txtUsername.Focus();
                errorProvider1.SetError(txtUsername, "Username must not be empty!");
               
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtUsername, "");
               
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                e.Cancel = true;
                txtPassword.Focus();
                errorProvider1.SetError(txtPassword, "Password must not be empty");  
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPassword, "");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
