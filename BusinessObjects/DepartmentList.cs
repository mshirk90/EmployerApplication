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
    public class DepartmentList : Event
    {


        #region  Private Members 
        private BindingList<Department> _List;


        #endregion

        #region  Public Properties
        public BindingList<Department> List
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

        public bool IsSavable()
        {
            bool result = false;
            foreach (Department d in _List)
            {
                if (d.IsSavable() == true)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        public DepartmentList Save()
        {

            foreach (Department dn in _List)
            {
                if (dn.IsSavable() == true)
                {
                    dn.Save();
                }
            }
            return this;
        }
        public DepartmentList GetAll()
        {
            Database database = new Database("Employer");
            database.Command.Parameters.Clear();
            database.Command.CommandType = System.Data.CommandType.StoredProcedure;
            database.Command.CommandText = "tblDepartmentsGetAll";
            DataTable dt = database.ExecuteQuery();
            foreach (DataRow dr in dt.Rows)
            {
                Department e = new Department();
                e.Initialize(dr);
                e.InitializeBusinessData(dr);
                e.IsNew = false;
                e.IsDirty = false;
                e.Savable += Department_Savable;
                _List.Add(e);
            }

            return this;

        }


        #endregion

        #region Public Events

        #endregion

        #region Public Event Handlers
        private void _List_AddingNew(object sender, AddingNewEventArgs e)
        {

            e.NewObject = new Department();
            Department department = (Department)e.NewObject;
            department.Savable += Department_Savable;

        }

        private void Department_Savable(SavableEventArgs e)
        {
            RaiseEvent(e);
        }



        #endregion

        #region Construction
        public DepartmentList()
        {
            _List = new BindingList<Department>();
            _List.AddingNew += _List_AddingNew;

        }
        #endregion
    }
}