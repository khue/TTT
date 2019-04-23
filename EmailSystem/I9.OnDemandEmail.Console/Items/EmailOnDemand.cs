using System;

namespace I9.OnDemandEmail.Console.Items {

    internal class EmailOnDemand {

        #region Members
        
        private int _EmailOnDemandId = MyConsole.NullId;
        private int _EmailTemplateId = MyConsole.NullId;

        private int _UserId = MyConsole.NullId;
        private int _AgentId = MyConsole.NullId;
        private int _EmployeeId = MyConsole.NullId;
        private int _I9Id = MyConsole.NullId;
        private int _FutureEmployeeId = MyConsole.NullId;
        
        #endregion

        #region Constructors

        internal EmailOnDemand(Sql.spODE_EmailOnDemand_Get spEmailOnDemandGet) {

            if (spEmailOnDemandGet == null) throw new ArgumentNullException("spODE_OnDemandEmail_Get is null");

            _EmailOnDemandId = Convert.ToInt32(spEmailOnDemandGet.GetDataReaderValue(0, MyConsole.NullId));
            _EmailTemplateId = Convert.ToInt32(spEmailOnDemandGet.GetDataReaderValue(1, MyConsole.NullId));

            _UserId = Convert.ToInt32(spEmailOnDemandGet.GetDataReaderValue(2, MyConsole.NullId));
            _AgentId = Convert.ToInt32(spEmailOnDemandGet.GetDataReaderValue(3, MyConsole.NullId));
            _EmployeeId = Convert.ToInt32(spEmailOnDemandGet.GetDataReaderValue(4, MyConsole.NullId));
            _I9Id = Convert.ToInt32(spEmailOnDemandGet.GetDataReaderValue(5, MyConsole.NullId));
            _FutureEmployeeId = Convert.ToInt32(spEmailOnDemandGet.GetDataReaderValue(6, MyConsole.NullId));

        }

        #endregion

        #region Public Properties

        public int EmailOnDemandId { get { return _EmailOnDemandId; } }
        public int EmailTemplateId { get { return _EmailTemplateId; } }

        public int UserId { get { return _UserId; } }
        public int AgentId { get { return _AgentId; } }
        public int EmployeeId { get { return _EmployeeId; } }
        public int I9Id { get { return _I9Id; } }
        public int FutureEmployeeId { get { return _FutureEmployeeId; } }

        #endregion

    }

}
