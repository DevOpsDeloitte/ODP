using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODPTaxonomyCommon
{
    public class ViewAbstractToEvaluation
    {
        public int EvaluationId;       
        

        public Guid UserId;
        

        public int SubmissionTypeId;

        public Mode ViewMode;
        
    }

    public enum Mode
    {
        view = 0,
        code = 1

    }
}
