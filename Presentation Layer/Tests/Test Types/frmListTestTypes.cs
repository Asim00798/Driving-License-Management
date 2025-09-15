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
    public partial class frmListTestTypes : Form
    {
        public frmListTestTypes()
        {
            InitializeComponent();
        }
        private void _ResizeTable()
        {
            if (dgvTestTypes.Columns["TestTypeID"] != null)
                dgvTestTypes.Columns["TestTypeID"].Width = 80;
            if (dgvTestTypes.Columns["TestTypeTitle"] != null)
                dgvTestTypes.Columns["TestTypeTitle"].Width = 150;
            if (dgvTestTypes.Columns["TestTypeDescription"] != null)
                dgvTestTypes.Columns["TestTypeDescription"].Width = 250;
            if (dgvTestTypes.Columns["TestTypeFees"] != null)
                dgvTestTypes.Columns["TestTypeFees"].Width = 100;
        }
        private void _LoadData()
        {
            dgvTestTypes.DataSource = clsTestType.GetAllTestTypes();
            lblRecordsCount.Text = dgvTestTypes.Rows.Count.ToString();
            _ResizeTable();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvTestTypes.CurrentRow.Cells[0].Value;

            frmUpdateTestType frm = new frmUpdateTestType(ID);
            frm.ShowDialog();

            _LoadData();
        }
    }
}
