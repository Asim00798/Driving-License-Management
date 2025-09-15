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
    public partial class frmUpdateApplicationType : Form
    {
        private int _ApplicationTypeID = -1;
        private clsApplicationType _ApplicationType;
        public frmUpdateApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = ApplicationTypeID;

            //Keyboard events to prevent incompitable datatype
            txtFees.KeyPress += txtFees_KeyPress;
            txtTitle.KeyPress += txtTitle_KeyPress;
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            lblID.Text = _ApplicationTypeID.ToString();

            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);

            if (_ApplicationType != null)
            {
                txtTitle.Text = _ApplicationType.Title;
                txtFees.Text = _ApplicationType.Fees.ToString();


            }
        }
        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numeric input
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void txtTitle_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only alphabetic character
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ShowConfirmationDialog()
        {
            // Display a MessageBox with Yes and No buttons
            DialogResult result = MessageBox.Show("Do you want to proceed?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Handle the result based on the user's choice
            if (result == DialogResult.Yes)
            {
                // The user clicked Yes
                if (!this.ValidateChildren())
                {
                    //Here we dont continue becuase the form is not valid
                    MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }

                _ApplicationType.Title = txtTitle.Text.Trim();
                _ApplicationType.Fees = Convert.ToSingle(txtFees.Text.Trim());


                if (_ApplicationType.Save())
                {
                    MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else if (result == DialogResult.No)
            {
                // The user clicked No
                MessageBox.Show("You choose No!", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtTitle.Text.Trim() != "" && txtFees.Text.Trim() != "")
            {
                ShowConfirmationDialog();
            }   
            else
            {
                
                MessageBox.Show("One or both of the required fields are empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                e.Cancel = true;
                txtTitle.Focus();
                errorProvider1.SetError(txtTitle, "Title shouldn,t be empty!");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtTitle, "");

            }
        }
        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFees.Text))
            {
                e.Cancel = true;
                txtFees.Focus();
                errorProvider1.SetError(txtFees, "Fees shouldn,t be empty!");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFees, "");

            }
        }

    }
}
