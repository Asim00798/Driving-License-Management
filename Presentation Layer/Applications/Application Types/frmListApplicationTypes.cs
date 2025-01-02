using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Business_Layer;

namespace Presentation_Layer
{
    public partial class frmListApplicationTypes : Form
    {
        public frmListApplicationTypes()
        {
            InitializeComponent();
        }
        private void _ResizeTable()
        {
            if (dgvApplicationTypes.Columns["ApplicationTypeID"] != null)
                dgvApplicationTypes.Columns["ApplicationTypeID"].Width = 120;
            if (dgvApplicationTypes.Columns["ApplicationTypetitle"] != null)
                dgvApplicationTypes.Columns["ApplicationTypetitle"].Width = 350;
            if (dgvApplicationTypes.Columns["ApplicationFees"] != null)
                dgvApplicationTypes.Columns["ApplicationFees"].Width = 120;    
        }

        private void _LoadData()
        {
            dgvApplicationTypes.DataSource = clsApplicationType.GetAllApplicationTypes();
            lblRecordsCount.Text = dgvApplicationTypes.Rows.Count.ToString();
            _ResizeTable();
        }

        private void frmListApplications_Load(object sender, EventArgs e)
        {
           _LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvApplicationTypes.CurrentRow.Cells[0].Value;

            frmUpdateApplicationType frm = new frmUpdateApplicationType(ID);
            frm.ShowDialog();

            _LoadData();
        }
    }
}
