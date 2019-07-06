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
                if (x.VICount == y.VICount)
                {
                    return 0;
                }

                return x.VICount < y.VICount ? -1 : 1;
            }

            return x.DQ == false ? -1 : 1;
        }
    }
}
