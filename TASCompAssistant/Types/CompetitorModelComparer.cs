using System.Collections.Generic;
using TASCompAssistant.Models;

namespace TASCompAssistant.Types
{
    internal sealed class CompetitorModelComparer : IComparer<CompetitorModel>
    {
        public int Compare(CompetitorModel x, CompetitorModel y)
        {
            if (x.DQ == y.DQ)
            {
                if (x.VIs == y.VIs)
                {
                    return 0;
                }

                return x.VIs < y.VIs ? -1 : 1;
            }

            return x.DQ == false ? -1 : 1;
        }
    }
}
