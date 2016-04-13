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
    public partial class frmPosition : Form
    {
        PositionList pl;
        public frmPosition()
        {
            InitializeComponent();
        }

        private void frmPosition_Load(object sender, EventArgs e)
        {
            dgvPosition.AutoGenerateColumns = false;
            pl = new PositionList();
            pl = pl.GetAll();
            dgvPosition.DataSource = pl.List;
            saveToolStripMenuItem.Enabled = false;
            pl.Savable += Pl_Savable;
        }

        private void Pl_Savable(SavableEventArgs e)
        {
            saveToolStripMenuItem.Enabled = e.Savable;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Position pn in pl.List)
            {
                if (pn.IsSavable() == true)
                {
                    pn.Save();
                }
            }
            saveToolStripMenuItem.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
