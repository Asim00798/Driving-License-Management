using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Business_Layer;
using Presentation_Layer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLD.People
{
    public partial class frmListPeople : Form
    {
        // Define a delegate for showing the UserControl with a PersonID parameter
        public delegate void ShowUserControlHandler(int personID);
        // Add event
        public event ShowUserControlHandler OnShowUserControl;

        // Event handler for OnShowUserControl event
        private void frmListPeople_OnShowUserControl(int personID)
        {  
            if (personID > 0)
            {
                frmAddUpdatePerson frm = new frmAddUpdatePerson(personID);
                frm.ShowDialog();
            }
            else
            {
                frmAddUpdatePerson frm = new frmAddUpdatePerson();
                frm.ShowDialog();
            }        
        }

        public frmListPeople()
        {
            InitializeComponent();

            // Subscribe to the event
            OnShowUserControl += frmListPeople_OnShowUserControl;

            // Attach KeyPress event
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text.Trim() == "Person ID" || cbFilterBy.Text.Trim() == "Phone")
            {
                // Allow only numeric input
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }

            else if
                (cbFilterBy.Text.Trim() == "First Name"
                || cbFilterBy.Text.Trim() == "Second Name"
                || cbFilterBy.Text.Trim() == "Third Name"
                || cbFilterBy.Text.Trim() == "Last Name")
               
            {
                // Allow only alphabetic character
                e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar);
            }

        }

        private static DataTable _dtPeople = clsPerson.GetAllPeople();
                            
        private void _LoadData()
        {
            _dtPeople = clsPerson.GetAllPeople();
            dgvPeople.DataSource = _dtPeople;
            cbFilterBy.SelectedIndex = 0;
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
            _ResizeTable();
        }
        private void _ResizeTable()
        {

            if (dgvPeople.Columns["PersonID"] != null)
                dgvPeople.Columns["PersonID"].Width = 100;
            if (dgvPeople.Columns["NationalNo"] != null)
                dgvPeople.Columns["NationalNo"].Width = 100;
            if (dgvPeople.Columns["FirstName"] != null)
                dgvPeople.Columns["FirstName"].Width = 100;
            if (dgvPeople.Columns["SecondName"] != null)
                dgvPeople.Columns["SecondName"].Width = 100;
            if (dgvPeople.Columns["ThirdName"] != null)
                dgvPeople.Columns["ThirdName"].Width = 100;
            if (dgvPeople.Columns["LastName"] != null)
                dgvPeople.Columns["LastName"].Width = 100;
            if (dgvPeople.Columns["DateOfBirth"] != null)
                dgvPeople.Columns["DateOfBirth"].Width = 150;
            if (dgvPeople.Columns["Nationality"] != null)
                dgvPeople.Columns["Nationality"].Width = 100;
            if (dgvPeople.Columns["Gendor"] != null)
                dgvPeople.Columns["Gendor"].Width = 100;
            if (dgvPeople.Columns["Phone"] != null)
                dgvPeople.Columns["Phone"].Width = 100;
            if (dgvPeople.Columns["Email"] != null)
                dgvPeople.Columns["Email"].Width = 150;

        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
             _LoadData();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterColumn = cbFilterBy.Text.Trim();

            try
            {
                switch (filterColumn)
                {
                    case "":
                        dgvPeople.DataSource = clsPerson.GetAllPeople();
                        break;

                    case "Person ID":
                        if (int.TryParse(txtFilterValue.Text.Trim(), out int personID))
                        {
                            dgvPeople.DataSource = clsPerson.FilterPeopleByPersonID(personID);
                            _ResizeTable();
                        }
                        else
                        {
                            dgvPeople.DataSource = _dtPeople;
                            _ResizeTable();
                        }
                        break;

                    case "National No":
                        dgvPeople.DataSource = clsPerson.FilterPeopleByNationalNo(txtFilterValue.Text.Trim());
                        _ResizeTable();
                        break;

                    case "First Name":
                        dgvPeople.DataSource = clsPerson.FilterPeopleByFirstName(txtFilterValue.Text.Trim());
                        _ResizeTable();
                        break;

                    case "Second Name":
                        dgvPeople.DataSource = clsPerson.FilterPeopleBySecondName(txtFilterValue.Text.Trim());
                        _ResizeTable();
                        break;

                    case "Third Name":
                        dgvPeople.DataSource = clsPerson.FilterPeopleByThirdName(txtFilterValue.Text.Trim());
                        _ResizeTable();
                        break;

                    case "Last Name":
                        dgvPeople.DataSource = clsPerson.FilterPeopleByLastName(txtFilterValue.Text.Trim());
                        _ResizeTable();
                        break;

                    case "Nationality":
                        dgvPeople.DataSource = clsPerson.FilterPeopleByNationality(txtFilterValue.Text.Trim());
                        _ResizeTable();
                        break;

                    case "Phone":
                        dgvPeople.DataSource = clsPerson.FilterPeopleByPhone(txtFilterValue.Text.Trim());
                        _ResizeTable();
                        break;

                    case "Email":
                        dgvPeople.DataSource = clsPerson.FilterPeopleByEmail(txtFilterValue.Text.Trim());
                        _ResizeTable();
                        break;

                    default:
                        dgvPeople.DataSource = _dtPeople;
                        _ResizeTable();
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while filtering: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";  //Clear text to next search
            dgvPeople.DataSource = _dtPeople; //Refresh to default data

            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            switch (cbFilterBy.Text)
            {
                case "None":
                    txtFilterValue.Visible = false;
                    
                    dgvPeople.DataSource = _dtPeople;
                    break;

                case "Gender":
                    txtFilterValue.Visible = false;
                    cbGender.Visible = true;
                    cbGender.SelectedIndex = 0;
                    dgvPeople.DataSource = _dtPeople;
                    break;

                default:
                    txtFilterValue.Visible = true;
                    cbGender.Visible = false;
                    txtFilterValue.Focus();
                    break;
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }
        private void EditPerson(int personID)
        {
            // Raise the event to show the UserControl
            OnShowUserControl?.Invoke(personID);
            _LoadData();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPeople.CurrentRow != null && dgvPeople.CurrentRow.Cells["PersonID"].Value != null)
            {
                int personID = Convert.ToInt32(dgvPeople.CurrentRow.Cells["PersonID"].Value);
                EditPerson(personID);
            }
            else
            {
                MessageBox.Show("Please select a valid person from the list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            _LoadData();
        }
        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete Person [" + dgvPeople.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {

                //Perform Delele and refresh
                if (clsPerson.DeletePerson((int)dgvPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _LoadData();
                }

                else
                    MessageBox.Show("Person was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

        }

        private void _AddNewPerson()
        {
            // Raise the event for adding a new person (PersonID = -1)
            OnShowUserControl?.Invoke(-1);
            _LoadData();
        }
        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            _AddNewPerson();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _AddNewPerson();
        }

        private void cbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbGender.SelectedIndex == 0)
            {
                dgvPeople.DataSource = clsPerson.FilterPeopleByGender("Male");
                _ResizeTable();
            }
            else
            {
                dgvPeople.DataSource = clsPerson.FilterPeopleByGender("Female");
                _ResizeTable();
            }

        }
    }
}
