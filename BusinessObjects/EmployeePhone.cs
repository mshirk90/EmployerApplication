using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseHelper;
using System.Data;

namespace BusinessObjects
{
    public class EmployeePhone : HeaderData
    {

        #region  Private Members 
        private Guid _EmployeeID = Guid.Empty;
        private string _Phone = string.Empty;
        private Guid _PhoneTypeID = Guid.Empty;
        #endregion

        #region  Public Properties

        public Guid EmployeeID
        {
            get
            {
                return _EmployeeID;
            }
            set
            {
                if (_EmployeeID != value)
                {
                    _EmployeeID = value;
                    base.IsDirty = true;
                    bool Savable = IsSavable();
                    SavableEventArgs e = new SavableEventArgs(Savable);
                    RaiseEvent(e);
                }
            }
        }
        public string Phone
        {
            get
            {
                return _Phone;
            }
            set
            {
                if (_Phone != value)
                {
                    _Phone = value;
                    base.IsDirty = true;
                    bool Savable = IsSavable();
                    SavableEventArgs e = new SavableEventArgs(Savable);
                    RaiseEvent(e);
                }
            }
        }
        public Guid PhoneTypeID
        {
            get
            {
                return _PhoneTypeID;
            }
            set
            {
                if (_PhoneTypeID != value)
                {
                    _PhoneTypeID = value;
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
                database.Command.CommandText = "tblEmployeePhoneINSERT";
                database.Command.Parameters.Add("@EmployeeID", SqlDbType.UniqueIdentifier).Value = _EmployeeID;
                database.Command.Parameters.Add("@Phone", SqlDbType.VarChar).Value = _Phone;
                database.Command.Parameters.Add("@PhoneTypeID", SqlDbType.UniqueIdentifier).Value = _PhoneTypeID;

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
                database.Command.CommandText = "tblEmployeePhoneUPDATE";
                database.Command.Parameters.Add("@EmployeeID", SqlDbType.UniqueIdentifier).Value = _EmployeeID;
                database.Command.Parameters.Add("@Phone", SqlDbType.VarChar).Value = _Phone;
                database.Command.Parameters.Add("@PhoneTypeID", SqlDbType.UniqueIdentifier).Value = _PhoneTypeID;
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
                database.Command.CommandText = "tblEmployeePhoneDELETE";
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

            if (_Phone == null || _Phone.Trim() == string.Empty)
            {
                result = false;
            }
          
            if (_Phone == null || _Phone.Length > 20)
            {
                result = false;
            }
            
            if (_PhoneTypeID == null || _PhoneTypeID != Guid.Empty)
            {
                result = false;
            }
           
            return result;
        }
        #endregion

        #region Public Methods

        public EmployeePhone GetById(Guid id)
        {
            Database database = new Database("Employee");
            DataTable dt = new DataTable();
            database.Command.CommandType = CommandType.StoredProcedure;
            database.Command.CommandText = "tblEmployeePhoneGetById";
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
            _EmployeeID = (Guid) dr["EmployeeID"];
            _Phone = dr["Phone"].ToString();
            _PhoneTypeID = (Guid)dr["PhoneTypeID"];


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
        public EmployeePhone Save(Database database)
        {
            bool result = true;
           // Database database = new Database("Employer");
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
            return this;
        }

        #endregion

        #region Public Events

        #endregion

        #region Public Event Handlers

        #endregion

        #region Construction

        public EmployeePhone()
        {

        }
        #endregion

    }
}