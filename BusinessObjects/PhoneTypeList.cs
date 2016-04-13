using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseHelper;
using System.Data;

namespace BusinessObjects
{
    public class PhoneTypeList : Event
    {
        #region Private Members
        private BindingList<PhoneType> _List;
        #endregion

        #region Public Properties
        public BindingList<PhoneType> List
        {
            get { return _List; }
        }
        #endregion

        #region Private Methods

        #endregion

        #region Public Methods
        public PhoneTypeList GetAll()
        {
            Database database = new Database("Employer");

            database.Command.Parameters.Clear();
            database.Command.CommandType = CommandType.StoredProcedure;
            database.Command.CommandText = "tblPhoneTypeGetAll";

            DataTable dt = database.ExecuteQuery();
            foreach (DataRow dr in dt.Rows)
            {
                PhoneType p = new PhoneType();
                p.Initialize(dr);
                p.InitializeBusinessData(dr);
                p.IsNew = false;
                p.IsDirty = false;
                p.Savable += Phonetype_Savable;
                _List.Add(p);
            }

            return this;
        }
        public PhoneTypeList Save()
        {
            foreach (PhoneType em in _List)
            {
                if (em.IsSavable() == true)
                {
                    em.Save();
                }
            }

            return this;
        }
        #endregion

        #region Public Events
        private void Phonetype_Savable(SavableEventArgs e)
        {
            RaiseEvent(e);
        }
        #endregion

        #region Public Event Handlers
        private void _List_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new PhoneType();
            PhoneType phonetype = (PhoneType)e.NewObject;
            phonetype.Savable += Phonetype_Savable;
        }
        #endregion

        #region Construction
        public PhoneTypeList()
        {
            _List = new BindingList<PhoneType>();
            _List.AddingNew += _List_AddingNew;
        }
        #endregion

    }
}
