using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseHelper;
using System.Data;

namespace BusinessObjects
{
     public class Employee : HeaderData
    {

        #region  Private Members 
        private string _FirstName = string.Empty;
        private string _LastName = string.Empty;
        private EmployeePhoneList _Phones = null;
        #endregion

        #region  Public Properties
        public EmployeePhoneList Phones
        {
            get
            {
                if (_Phones == null)
                {
                    _Phones = new EmployeePhoneList();
                    _Phones = _Phones.GetByEmployeeId(base.Id);
                }
                return _Phones;
            }

        }

        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    base.IsDirty = true;
                    bool Savable = IsSavable();
                    SavableEventArgs e = new SavableEventArgs(Savable);
                    RaiseEvent(e);
                }
            }
        } 

        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    base.IsDirty = true;
                    bool Savable = IsSavable();
                    SavableEventArgs e = new SavableEventArgs(Savable);
                    RaiseEvent(e);
                }
            }
        }
        #endregion

        #region  Private Methods 
        private bool Insert(Database database)
        {
            bool result = true;
            try
            {
                database.Command.Parameters.Clear();
                database.Command.CommandType = System.Data.CommandType.StoredProcedure;
                database.Command.CommandText = "tblEmployeeINSERT";
                database.Command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = _FirstName;
                database.Command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = _LastName;
                base.Initialize(database, Guid.Empty);
                database.ExecuteNonQueryWithTransaction();
                base.Initialize(database.Command);
               
            }
            catch (Exception e)
            {                
                result = false;             
                throw;
            }
            return result;
        }

        private bool Update(Database database)
        {
            bool result = true;
            try
            {
                database.Command.Parameters.Clear();
                database.Command.CommandType = System.Data.CommandType.StoredProcedure;
                database.Command.CommandText = "tblEmployeeUPDATE";
                database.Command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = _FirstName;
                database.Command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = _LastName;
                base.Initialize(database, base.Id);
                database.ExecuteNonQueryWithTransaction();
                base.Initialize(database.Command);

            }
            catch (Exception e)
            {
                result = false;
                throw;
            }
            return result;            
        }

        private bool Delete(Database database)
        {
            bool result = true;
            try
            {
                database.Command.Parameters.Clear();
                database.Command.CommandType = System.Data.CommandType.StoredProcedure;
                database.Command.CommandText = "tblEmployeeDELETE";
                base.Initialize(database, base.Id);
                database.ExecuteNonQueryWithTransaction();
                base.Initialize(database.Command);

            }
            catch (Exception e)
            {
                result = false;
                throw;
            }
            return result;
        }

        private bool IsValid()
        {
            bool result = true;

            if (_FirstName.Trim() == string.Empty)
            {
                result = false;
            }
            if (_LastName.Trim() == string.Empty)
            {
                result = false;
            }
            if (_FirstName.Length > 20)
            {
                result = false;
            }
            if (_LastName.Length > 20)
            {
                result = false;
            }
            if (_FirstName.Trim().Length < 1 )
            {
                result = false;
            }
            if (_LastName.Trim().Length < 1)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region Public Methods

        public Employee GetById(Guid id)
        {
            Database database = new Database("Employee");
            DataTable dt = new DataTable();
            database.Command.CommandType = CommandType.StoredProcedure;
            database.Command.CommandText = "tblEmployeeGetById";
            base.Initialize(database, base.Id);
            dt = database.ExecuteQuery();
            if (dt != null || dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                base.Initialize(dr);
                InitializeBusinessData(dr);
                base.IsNew = false;
                base.IsDirty = false;
            }
            return this;
        }

        public void InitializeBusinessData(DataRow dr)
        {
            _FirstName = dr["FirstName"].ToString();
            _LastName = dr["LastName"].ToString();
        }

        public bool IsSavable()
        {
            bool result = false;
            if (base.IsDirty == true && IsValid() == true)
            {
               result = true;
            }

            return result;
        }
        public Employee Save()
        {
            bool result = true;
            Database database = new Database("Employer");
            database.BeginTransaction();

            if (base.IsNew == true && IsSavable())
            {
                result = Insert(database);
            }
            else if (base.Deleted == true && base.IsDirty == true)
            {
                result = Delete(database);
                    }
            else if (base.IsNew == false && IsSavable() == true)
            {
                result = Update(database);
            }
            if (result == true)
            {
                base.IsDirty = false;
                base.IsNew = false;
            }
            //save the children
            if (result == true && _Phones != null && _Phones.IsSavable() == true)
            {
                result = Phones.Save(database);
            }
            if (result == true)
            {
                database.EndTransaction();
            }
            else
            {
                database.RollBack();
            }
            return this;
        }

        #endregion

        #region Public Events

        #endregion

        #region Public Event Handlers

        #endregion

        #region Construction

        #endregion

    }
}
