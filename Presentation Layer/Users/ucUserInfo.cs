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
    public partial class ucUserInfo : UserControl
    {
        private clsUser _User;
        int _UserID;

        public ucUserInfo(int UserID)
        {
            InitializeComponent();  
            _UserID = UserID;
        }
        public ucUserInfo()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(int UserID)
        {
            _UserID = UserID;
            _User = clsUser.FindByUserID(_UserID);
            
            if (_User == null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No User with UserID = " + _UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ucPersonInfo1.LoadPersonInfo(_User.PersonID);
            _FillUserInfo();
        }

        private void _FillUserInfo()
        {
            ucPersonInfo1.LoadPersonInfo(_User.PersonID);

            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName.ToString();

            if (_User.IsActive)
                lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";
        }

        private void _ResetPersonInfo()
        {
            ucPersonInfo1.ResetPersonInfo();
            lblUserID.Text = "[???]";
            lblUserName.Text = "[???]";
            lblIsActive.Text = "[???]";
        }

        private void ucUserInfo_Load(object sender, EventArgs e)
        {

        }

        private void ucPersonInfo1_Load(object sender, EventArgs e)
        {

        }
    }
}
