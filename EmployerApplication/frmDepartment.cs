using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessObjects;

namespace EmployerApplication
{
    public partial class frmDepartment : Form
    {
        DepartmentList dl;
        public frmDepartment()
        {
            InitializeComponent();
        }
        private void frmDepartment_Load(object sender, EventArgs e)
        {
            dgvDepartment.AutoGenerateColumns = false;
            dl = new DepartmentList();
            dl = dl.GetAll();
            dgvDepartment.DataSource = dl.List;
            saveToolStripMenuItem.Enabled = false;
            dl.Savable += Dl_Savable;
        }


        private void Dl_Savable(SavableEventArgs e)
        {

            saveToolStripMenuItem.Enabled = e.Savable;

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dl.IsSavable()==true)
            {
                dl.Save();
            }
            saveToolStripMenuItem.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }

}

