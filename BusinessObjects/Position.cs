using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseHelper;
using System.Data;

namespace BusinessObjects
{
    public class Position : HeaderData
    {

        #region  Private Members 
        private string _PositionName = string.Empty;
        private decimal _MinimumSalary = 0.0m;
        private decimal _MaximumSalary = 0.0m;
        #endregion

        #region  Public Properties
        public string PositionName
        {
            get
            {
                return _PositionName;
            }
            set
            {
                if (_PositionName != value)
                {
                    _PositionName = value;
                    base.IsDirty = true;
                    bool Savable = IsSavable();
                    SavableEventArgs e = new SavableEventArgs(Savable);
                    RaiseEvent(e);
                }
            }
        }

        public decimal MinimumSalary
        {
            get
            {
                return _MinimumSalary;
            }
            set
            {
                if (_MinimumSalary != value)
                {
                    _MinimumSalary = value;
                    base.IsDirty = true;
                    bool Savable = IsSavable();
                    SavableEventArgs e = new SavableEventArgs(Savable);
                    RaiseEvent(e);
                }
            }
        }

        public decimal MaximumSalary
        {
            get
            {
                return _MaximumSalary;
            }
            set
            {
                if (_MaximumSalary != value)
                {
                    _MaximumSalary = value;
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
                database.Command.CommandText = "tblPositionINSERT";
                database.Command.Parameters.Add("@PositionName", SqlDbType.VarChar).Value = _PositionName;
                database.Command.Parameters.Add("@MinimumSalary", SqlDbType.VarChar).Value = _MinimumSalary;
                database.Command.Parameters.Add("@MaximumSalary", SqlDbType.VarChar).Value = _MaximumSalary;
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
                database.Command.CommandText = "tblPositionUPDATE";
                database.Command.Parameters.Add("@PositionName", SqlDbType.VarChar).Value = _PositionName;
                database.Command.Parameters.Add("@MinimumSalary", SqlDbType.VarChar).Value = _MinimumSalary;
                database.Command.Parameters.Add("@MaximumSalary", SqlDbType.VarChar).Value = _MaximumSalary;
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
                database.Command.CommandText = "tblPositionDELETE";
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

            if (_PositionName.Trim() == string.Empty)
            {
                result = false;
            }
            if (_MinimumSalary == decimal.MaxValue)
            {
                result = false;
            }
            if (_PositionName.Length > 20)
            {
                result = false;
            }          
            if (_PositionName.Trim().Length < 1)
            {
                result = false;
            }
            if (_MinimumSalary == decimal.MaxValue)
            {
                result = false;
            }
            if (_MinimumSalary < 0)
            {
                result = false;
            }
            if (_MaximumSalary < 0)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region Public Methods

        private Position GetById(Guid id)
        {
            Database database = new Database("Employee");
            DataTable dt = new DataTable();
            database.Command.CommandType = CommandType.StoredProcedure;
            database.Command.CommandText = "tblPositionGetById";
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
            _PositionName = dr["PositionName"].ToString();
            _MinimumSalary = (decimal)dr["MinimumSalary"];
            _MaximumSalary = (decimal)dr["MaximumSalary"];
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
        public Position Save()
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
