using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using DatabaseHelper;
using System.Data;
namespace BusinessObjects
{
    public class EmployeeList : Event
    {

        #region  Private Members 
        private BindingList<Employee> _List;
           
            
        #endregion

        #region  Public Properties
        public BindingList<Employee> List
        {
            get
            {
                return _List;
            }
        }
        #endregion

        #region  Private Methods 

        #endregion

        #region Public Methods

        public EmployeeList GetAll()
        {
            Database database = new Database("Employer");
            database.Command.Parameters.Clear();
            database.Command.CommandType = System.Data.CommandType.StoredProcedure;
            database.Command.CommandText = "tblEmployeeGetAll";
            DataTable dt = database.ExecuteQuery();
            foreach (DataRow dr in dt.Rows)
            {
                Employee e = new Employee();
                e.Initialize(dr);
                e.InitializeBusinessData(dr);
                e.IsNew = false;
                e.IsDirty = false;
                e.Savable += E_Savable;
                _List.Add(e);
            }
            
            return this;

        }

        private void E_Savable(SavableEventArgs e)
        {
            RaiseEvent(e);
        }
        #endregion

        #region Public Events

        #endregion

        #region Construction
        public EmployeeList()
        {
            _List = new BindingList<Employee>();
            _List.AddingNew += _List_AddingNew;
        }

        private void _List_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new Employee();
            Employee employee = (Employee)e.NewObject;
            employee.Savable += E_Savable;
        }

        


        #endregion

    }
}
