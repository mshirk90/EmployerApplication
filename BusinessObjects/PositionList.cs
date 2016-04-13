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
    public class PositionList : Event
    {

        #region  Private Members 
        private BindingList<Position> _List;


        #endregion

        #region  Public Properties
        public BindingList<Position> List
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
        public PositionList GetAll()
        {
            Database database = new Database("Employer");
            database.Command.Parameters.Clear();
            database.Command.CommandType = System.Data.CommandType.StoredProcedure;
            database.Command.CommandText = "tblPositionGetAll";
            DataTable dt = database.ExecuteQuery();
            foreach (DataRow dr in dt.Rows)
            {
                Position e = new Position();
                e.Initialize(dr);
                e.InitializeBusinessData(dr);
                e.IsNew = false;
                e.IsDirty = false;
                e.Savable += Position_Savable;
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
            e.NewObject = new Position();
            Position position = (Position)e.NewObject;
            position.Savable += Position_Savable;
        }

        private void Position_Savable(SavableEventArgs e)
        {
            RaiseEvent(e);
        }
        #endregion

        #region Construction
        public PositionList()
        {
            _List = new BindingList<Position>();
            _List.AddingNew += _List_AddingNew;
        }


        #endregion

    }
}
