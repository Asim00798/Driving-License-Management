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
    public partial class frmListUsers : Form
    {
        private static DataTable _dtUsers = clsUser.GetAllUsers();

        public frmListUsers()
        {
            InitializeComponent();
            // Attach KeyPress event
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text.Trim() == "Person ID" || cbFilterBy.Text.Trim() == "User ID")
            {
                // Allow only numeric input
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
                
            }

            else if (cbFilterBy.Text.Trim() == "User Name")
            {
                // Allow only alphabetic character for the first input
                if (txtFilterValue.Text.Length == 0)
                {
                    e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar);
                }
                
            }
            else if(cbFilterBy.Text.Trim() == "Full Name")
            {
                // Allow only alphabetic character
                e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _LoadData();
        }
        private void _LoadData()
        {
            _dtUsers = clsUser.GetAllUsers();
            dgvUsers.DataSource = _dtUsers;
            cbFilterBy.SelectedIndex = 0;
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
            _ResizeTable();
        }
        private void _ResizeTable()
        {
            if (dgvUsers.Columns["UserID"] != null)
                dgvUsers.Columns["UserID"].Width = 90;
            if (dgvUsers.Columns["PersonID"] != null)
                dgvUsers.Columns["PersonID"].Width = 90;
            if (dgvUsers.Columns["UserName"] != null)
                dgvUsers.Columns["UserName"].Width = 150;
            if (dgvUsers.Columns["FullName"] != null)
                dgvUsers.Columns["FullName"].Width = 210;
            if (dgvUsers.Columns["IsActive"] != null)
                dgvUsers.Columns["IsActive"].Width = 80;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";  //Clear text to next search
            dgvUsers.DataSource = _dtUsers; //Refresh to default data

            switch (cbFilterBy.Text)
            {
                case "None":
                    txtFilterValue.Visible = false;
                    cbIsActive.Visible = false;
                    dgvUsers.DataSource = _dtUsers;
                    break;

                case "Is Active":
                    txtFilterValue.Visible = false;
                    cbIsActive.Visible = true;
                    cbIsActive.SelectedIndex = 0;
                    cbIsActive.Focus();
                    break;

                default:
                    txtFilterValue.Visible = true;
                    cbIsActive.Visible = false;
                    txtFilterValue.Focus();
                    break;
            }

        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterColumn = cbFilterBy.Text.Trim();

            try
            {
                switch (filterColumn)
                {
                    case "":
                        dgvUsers.DataSource = _dtUsers;
                        break;

                    case "Person ID":
                        if (int.TryParse(txtFilterValue.Text.Trim(), out int personID))
                        {
                            dgvUsers.DataSource = clsUser.FilterUsersByPersonID(personID);
                            _ResizeTable();
                        }
                        else
                        {
                            dgvUsers.DataSource = _dtUsers;
                            _ResizeTable();
                        }
                        break;

                    case "User ID":
                        if (int.TryParse(txtFilterValue.Text.Trim(), out int userID))
                        {
                            dgvUsers.DataSource = clsUser.FilterUsersByUserID(userID);
                            _ResizeTable();
                        }
                        else
                        {
                            dgvUsers.DataSource = _dtUsers;
                            _ResizeTable();
                        }
                        break;

                    case "User Name":
                        dgvUsers.DataSource = clsUser.FilterUsersByUserName(txtFilterValue.Text.Trim());
                        _ResizeTable();
                        break;

                    case "Full Name":
                        dgvUsers.DataSource = clsUser.FilterUsersByFullName(txtFilterValue.Text.Trim());
                        _ResizeTable();
                        break;

                    case "Is Active":
                        bool isActive = cbIsActive.Text == "Yes";
                        dgvUsers.DataSource = clsUser.FilterUsersByStatus(isActive);
                        _ResizeTable();
                        break;

                    default:
                        dgvUsers.DataSource = _dtUsers;
                        _ResizeTable();
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while filtering: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
            
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIsActive.Text.Trim())
            {
                case "Yes":
                    dgvUsers.DataSource = clsUser.FilterUsersByStatus(true);
                    break;

                case "No":
                    dgvUsers.DataSource = clsUser.FilterUsersByStatus(false);
                    break;

                default:
                    dgvUsers.DataSource = clsUser.GetAllUsers();
                    break;
            }
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
            
        }


        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            frmUserInfo frm = new frmUserInfo(UserID);
            frm.ShowDialog();
            //ShowUserInfo(PersonID);
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            frmAddUpdateUser frm = new frmAddUpdateUser(UserID);
            frm.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete Person [" + dgvUsers.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                //Perform Delele and refresh
                if (clsUser.DeleteUser((int)dgvUsers.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("User Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _LoadData();
                }

                else
                    MessageBox.Show("User was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            frmChangeUserPassword frm = new frmChangeUserPassword(UserID);
            frm.ShowDialog();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
        }
    }

}
