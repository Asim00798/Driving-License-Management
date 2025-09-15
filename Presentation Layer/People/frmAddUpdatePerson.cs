using Business_Layer;
using Presentation_Layer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation_Layer
{
    public partial class frmAddUpdatePerson : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int PersonID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;


        public enum enMode { AddNew = 0, Update = 1 };
        public enum enGendor { Male = 0, Female = 1 };

        private enMode _Mode;

        int _PersonID = -1;
        clsPerson _Person;

        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _PersonID = PersonID;
        }

        private void _FillCountriesInComoboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            foreach (DataRow row in dtCountries.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }

        }
        private void _ResetDefualtValues()
        {
            //this will initialize the reset the defaule values
            _FillCountriesInComoboBox();

            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else
            {
                lblMode.Text = "Update Person";
            }

            //set default image for the person.
            if (rbMale.Checked)
                pbPersonalImage.Image = Resources.flight_attendant_male;
            else
                pbPersonalImage.Image = Resources.user_female;

            //we set the max date to 18 years from today, and set the default value the same.
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            //should not allow adding age more than 100 years
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            //this will set default country to jordan.
            cbCountry.SelectedIndex = cbCountry.FindString("United Arab Emirates");

            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            rbMale.Checked = true;
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
        }
        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("No Person with ID = " + _PersonID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Dispose();
                return;
            }

            //the following code will not be executed if the person was not found
            lblPersonID.Text = _PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            if (_Person.Gendor == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            txtAddress.Text = _Person.Address;
            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);


            //load person image incase it was set.
            if (_Person.ImagePath != "")
            {
                pbPersonalImage.ImageLocation = _Person.ImagePath;

            }

            //hide/show the remove linke incase there is no image for the person.
            llRemoveImage.Visible = (_Person.ImagePath != "");

        }

        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            cbCountry.Items.Clear(); // Ensure the combobox is clear before adding items
            foreach (DataRow row in dtCountries.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
        }

        private bool _HandlePersonImage()
        {

            //this procedure will handle the person image,
            //it will take care of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and 
            // place it in the images folder.


            //_Person.ImagePath contains the old Image, we check if it changed then we copy the new image
            if (_Person.ImagePath != pbPersonalImage.ImageLocation)
            {
                if (_Person.ImagePath != "")
                {
                    //first we delete the old image from the folder in case there is any.

                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException)
                    {
                        // We could not delete the file.
                        //log it later   
                    }
                }

                if (pbPersonalImage.ImageLocation != null)
                {
                    //then we copy the new image to the image folder after we rename it
                    string SourceImageFile = pbPersonalImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    {
                        pbPersonalImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

            }
            return true;
        }




        //Method to Generate Random Filename Using GUID:
        private string GenerateRandomFilename(string extension)
        {
            // Generate a random filename using GUID
            string guid = Guid.NewGuid().ToString();
            return $"{guid}{extension}";
        }

        /*private string GenerateRandomFilename(string extension) //using system.io
        {
            // Generate a random string for the filename
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            string randomString = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return $"{randomString}{extension}";
        }*/

        private bool AreAllControlsValid()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("First Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSecondName.Text))
            {
                MessageBox.Show("Second Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSecondName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtThirdName.Text))
            {
                MessageBox.Show("Third Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtThirdName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Last Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLastName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNationalNo.Text) || (clsPerson.IsPersonExist(txtNationalNo.Text) && _Mode == enMode.AddNew))
            {
                MessageBox.Show("National No is required and must be unique.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNationalNo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Phone number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhone.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("A valid Email is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAddress.Focus();
                return false;
            }

            if (!rbFemale.Checked && !rbMale.Checked)
            {
                MessageBox.Show("Gender selection is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true; // All validations passed
        }


        public static bool IsValidEmail(string email)   //using system.text.RegularExpressions
        { // Regular expression pattern for validating email addresses
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            // Use Regex.IsMatch to validate the email string against the pattern
            return Regex.IsMatch(email, pattern);
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!AreAllControlsValid())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if (!_HandlePersonImage())
                return;

            int NationalityCountryID = clsCountry.Find(cbCountry.Text).ID;

            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;

            if (rbMale.Checked)
                _Person.Gendor = (short)enGendor.Male;
            else
                _Person.Gendor = (short)enGendor.Female;

            _Person.NationalityCountryID = NationalityCountryID;

            if (pbPersonalImage.ImageLocation != null)
                _Person.ImagePath = pbPersonalImage.ImageLocation;
            else
                _Person.ImagePath = "";

            if (_Person.Save())
            {
                lblPersonID.Text = _Person.PersonID.ToString();
                //change form mode to update.
                _Mode = enMode.Update;
                lblMode.Text = "Update Person";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


                // Trigger the event to send data back to the caller form.
                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llOpenFileDialog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Process the selected file
                    string selectedFilePath = openFileDialog1.FileName;

                    // Load the selected image into the PictureBox
                    Image selectedImage = Image.FromFile(selectedFilePath);
                    pictureBox1.Image = selectedImage;

                    // Generate a random filename
                    string randomFilename = GenerateRandomFilename(Path.GetExtension(selectedFilePath));
                    string saveDirectory = @"C:\Users\HP GHH\Desktop\DVLD System Solution\Personal Images";

                    // Ensure the directory exists
                    if (!Directory.Exists(saveDirectory))
                    {
                        Directory.CreateDirectory(saveDirectory);
                    }

                    // Save the image with the random filename
                    string savePath = Path.Combine(saveDirectory, randomFilename);
                    selectedImage.Save(savePath);

                    // Optionally, set the PictureBox's image location property to the saved path
                    pbPersonalImage.ImageLocation = savePath;

                    // Optionally, display the saved image path
                    MessageBox.Show($"Image saved as: {randomFilename}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    llRemoveImage.Visible = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving the image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorProvider1.SetError(llOpenFileDialog, "Failed to save image.");
                }
            }


        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            //change the defualt image to male incase there is no image set.
            if (pbPersonalImage.ImageLocation == null)
                pbPersonalImage.Image = Resources.flight_attendant_male;

        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            //change the defualt image to female incase there is no image set.
            if (pbPersonalImage.ImageLocation == null)
                pbPersonalImage.Image = Resources.user_female;
        }

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                e.Cancel = true;
                txtFirstName.Focus();
                errorProvider1.SetError(txtFirstName, "First Name should have a value!");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFirstName, "");

            }
        }

        private void txtSecondName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSecondName.Text))
            {
                e.Cancel = true;
                txtSecondName.Focus();
                errorProvider1.SetError(txtSecondName, "Second Name should have a value!");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtSecondName, "");

            }
        }

        private void txtThirdName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtThirdName.Text))
            {
                e.Cancel = true;
                txtThirdName.Focus();
                errorProvider1.SetError(txtThirdName, "Third Name should have a value!");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtThirdName, "");

            }
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                e.Cancel = true;
                txtLastName.Focus();
                errorProvider1.SetError(txtLastName, "LastName should have a value!");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLastName, "");

            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            string NationalNumber = txtNationalNo.Text;

            if ((string.IsNullOrWhiteSpace(txtNationalNo.Text) || clsPerson.IsPersonExist(NationalNumber)) && _Mode == enMode.AddNew)  //if Person Exisits give error
            {
                e.Cancel = true;
                txtNationalNo.Focus();
                errorProvider1.SetError(txtNationalNo, "This person national number already existed");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, "");

            }
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                e.Cancel = true;
                txtPhone.Focus();
                errorProvider1.SetError(txtPhone, "Phone Number should have a value!");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPhone, "");

            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                e.Cancel = true;
                txtEmail.Focus();
                errorProvider1.SetError(txtEmail, "Email form not Correct");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtEmail, "");

            }
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                e.Cancel = true;
                txtAddress.Focus();
                errorProvider1.SetError(txtAddress, "Address should have a value!");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtAddress, "");

            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonalImage.Image = null;

            if (rbMale.Checked)
            {
                pbPersonalImage.Image = Properties.Resources.flight_attendant_male;
            }
            else if (rbFemale.Checked)
            {
                pbPersonalImage.Image = Properties.Resources.user_female;
            }
            else
            {
                pbPersonalImage.Image = Properties.Resources.Person;
            }

            llRemoveImage.Visible = false;
        }
    }
}

