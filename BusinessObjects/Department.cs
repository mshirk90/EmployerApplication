using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseHelper;
using System.Data;

namespace BusinessObjects
{
    public class Department : HeaderData
    {


        #region  Private Members 
        private string _Name = string.Empty;
        private string _Abbreviation = string.Empty;
        #endregion

        #region  Public Properties
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    base.IsDirty = true;
                    bool Savable = IsSavable();
                    SavableEventArgs e = new SavableEventArgs(Savable);
                    RaiseEvent(e);
                }
            }
        }

        public string Abbreviation
        {
            get
            {
                return _Abbreviation;
            }
            set
            {
                if (_Abbreviation != value)
                {
                    _Abbreviation = value;
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
                database.Command.CommandText = "tblDepartmentsINSERT";
                database.Command.Parameters.Add("@Name", SqlDbType.VarChar).Value = _Name;
                database.Command.Parameters.Add("@Abbreviation", SqlDbType.VarChar).Value = _Abbreviation;
                base.Initialize(database, Guid.Empty);
                database.ExecuteNonQuery();
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
                database.Command.CommandText = "tblDepartmentsUPDATE";
                database.Command.Parameters.Add("@Name", SqlDbType.VarChar).Value = _Name;
                database.Command.Parameters.Add("@Abbreviation", SqlDbType.VarChar).Value = _Abbreviation;
                base.Initialize(database, base.Id);
                database.ExecuteNonQuery();
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
                database.Command.CommandText = "tblDepartmentsDELETE";
                base.Initialize(database, base.Id);
                database.ExecuteNonQuery();
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

            if (_Name.Trim() == string.Empty)
            {
                result = false;
            }
            if (_Abbreviation.Trim() == string.Empty)
            {
                result = false;
            }
            if (_Name.Length > 35)
            {
                result = false;
            }
           if (_Abbreviation.Length > 20)
           {
               result = false;
            }
              if (_Name.Trim().Length < 1)
            {
                result = false;
            }
                if (_Abbreviation.Trim().Length < 1)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region Public Methods

        private Department GetById(Guid id)
        {
            Database database = new Database("Employee");
            DataTable dt = new DataTable();
            database.Command.CommandType = CommandType.StoredProcedure;
            database.Command.CommandText = "tblDepartmentsGetById";
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
            _Name = dr["Name"].ToString();
            _Abbreviation = dr["Abbreviation"].ToString();
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
        public Department Save()
        {
            bool result = true;
            Database database = new Database("Employer");
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

        #endregion

    }
}





