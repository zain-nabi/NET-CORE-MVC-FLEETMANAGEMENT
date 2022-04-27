using System.Collections.Generic;

namespace Triton.FleetManagement.Web.Helper
{
    public class PeriodHelper
    {
        public int Peroid { get; set; }

        public static List<PeriodHelper> Peroids()
        {
            var periods = new List<PeriodHelper>();
            for (int i = 1; i < 13; i++)
            {
                periods.Add(
                    new PeriodHelper { Peroid = i }
                );
            }

            return periods;
        }
    }
}
