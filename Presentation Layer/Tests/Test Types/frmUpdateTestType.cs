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
    public partial class frmUpdateTestType : Form
    {
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsTestType _TestType;
        public frmUpdateTestType(int TestTypeID)
        {
            InitializeComponent();

            //Keyboard events to prevent incompitable datatype
            txtTitle.KeyPress += txtPreventNumericValues_KeyPress;
            txtFees.KeyPress += txtFees_KeyPress; 
        }
        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numeric input
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void txtPreventNumericValues_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Prevent numeric input
            e.Handled = !(!char.IsDigit(e.KeyChar));
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

                _TestType.Title = txtTitle.Text.Trim();
                _TestType.Description = txtDescription.Text.Trim();
                _TestType.Fees = Convert.ToSingle(txtFees.Text.Trim());


                if (_TestType.Save())
                {
                    MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else if (result == DialogResult.No)
            {
                // The user clicked No
                MessageBox.Show("Operation Cancelled!", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim() != "" && txtFees.Text.Trim() != "" && txtDescription.Text.Trim() != "")
            {
                if (clsTestType.Find(_TestTypeID) != null)
                {
                    ShowConfirmationDialog();
                }
                else
                {
                    MessageBox.Show("An Error Ocuured!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {

                MessageBox.Show("One or More of the required fields are empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            _TestType = clsTestType.Find(_TestTypeID);

            if (_TestType != null)
            {
                lblID.Text = ((int)_TestTypeID).ToString();
                txtTitle.Text = _TestType.Title;
                txtDescription.Text = _TestType.Description;
                txtFees.Text = _TestType.Fees.ToString();
            }

            else
            {
                MessageBox.Show("Could not find Test Type with id = " + _TestTypeID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }


    }
}
