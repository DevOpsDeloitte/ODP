using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODPTaxonomyDAL_JY
{
    public enum AbstractStatusEnum
    {
        OPEN = 1,
        CODED_BY_CODER = 3,
        CONSENSUS_COMPLETE_1B = 4,
        CONSENSUS_COMPLETE_WITH_NOTES_1N = 6
    }

    public enum SubmissionTypeEnum
    {
        CODER_EVALUATION = 1,
        CODER_CONSENSUS = 2
    }

    public enum EvaluationTypeEnum
    {
        CODER_EVALUATION = 1,
        ODP_EVALUATION = 2
    }
}
