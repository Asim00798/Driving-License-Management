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
    public partial class frmShowPersonInfo : Form
    {
        int _PersonID = -1;

        public frmShowPersonInfo(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID; 
            Initialize(_PersonID);
        }

        private void Initialize(int PersonID)
        {
            ucPersonInfo1.LoadPersonInfo(PersonID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPersonInfo_Load(object sender, EventArgs e)
        {
            
        }

        private void ucPersonInfo1_Load(object sender, EventArgs e)
        {

        }
    }
}
