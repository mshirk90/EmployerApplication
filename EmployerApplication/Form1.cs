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
    public partial class Form1 : Form
    {
        EmployeeList el = new EmployeeList();
        PhoneTypeList phoneTypes;

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            phoneTypes = new PhoneTypeList();
            phoneTypes = phoneTypes.GetAll();
            dgvPhone.CellFormatting += DgvPhone_CellFormatting;
            dgvPhone.RowValidated += DgvPhone_RowValidated;
            dgvPhone.DataError += DgvPhone_DataError;
            dgvEmployee.AutoGenerateColumns = false;
            dgvPhone.AutoGenerateColumns = false;
            el = new EmployeeList();
            dgvEmployee.DataSource = el.List;
            saveToolStripMenuItem.Enabled = false;
            el.Savable += El_Savable;
            dgvEmployee.RowHeaderMouseDoubleClick += DgvEmployee_RowHeaderMouseDoubleClick;
        }

        private void DgvPhone_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                PopulatePhoneTypes((DataGridViewComboBoxColumn)dgvPhone.Columns[e.ColumnIndex]);
            }
        }

        private void DgvPhone_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
          
        }
        private void PopulatePhoneTypes(DataGridViewComboBoxColumn column)
        {
            if (column.DataSource == null)
            {
                column.DisplayMember = "Type";
                column.ValueMember = "Id";
                column.DataSource = phoneTypes.List;
            }
       
        }

        private void DgvPhone_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //ingnore error
        }

        private void DgvEmployee_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Employee employee = (Employee)dgvEmployee.Rows[e.RowIndex].DataBoundItem;
            dgvPhone.DataSource = employee.Phones.List;
        }

        private void El_Savable(SavableEventArgs e)
        {
            saveToolStripMenuItem.Enabled = e.Savable;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Employee em in el.List)
            {
                if (em.IsSavable() == true)
                {
                    em.Save();
                }
            }
            saveToolStripMenuItem.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            el = el.GetAll();
            dgvEmployee.DataSource = el.List;
        }
    }
}
