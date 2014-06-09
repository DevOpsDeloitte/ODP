using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODPTaxonomyDAL_TT
{
    public class SubmissionLinkData
    {
        private int _EvaluationId;
        private Guid _UserId;
        private int _SubmissionTypeId;
        private string _UserName;

        public SubmissionLinkData(int p_EvaluationId, Guid p_UserId, int p_SubmissionTypeId, string s_UserName)
        {
            this._EvaluationId = p_EvaluationId;
            this._UserId = p_UserId;
            this._SubmissionTypeId = p_SubmissionTypeId;
            this._UserName = s_UserName;
        }

        public int EvaluationId
        {
            get { return _EvaluationId; }
            set { _EvaluationId = value; }
        }


        public Guid UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        public int SubmissionTypeId
        {
            get { return _SubmissionTypeId; }
            set { _SubmissionTypeId = value; }
        }

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

    }
}
