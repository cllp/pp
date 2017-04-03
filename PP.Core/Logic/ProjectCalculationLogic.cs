using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PP.Core.Model;
using PP.Core.Interfaces;

namespace PP.Core.Logic
{
    public static class ProjectCalculationLogic
    {
        static IAppRepository appRepository = CoreFactory.AppRepository;


        public static decimal CalculateActivityRow(ProjectActivity activity)
        {
            IEnumerable<Settings> financeSettings = appRepository.GetSettings("FinanceRatio");
            var InternalCost = ((from internalCost in financeSettings
                                 where internalCost.Name.Equals("financeInternalCost")
                                 select internalCost).FirstOrDefault());

            var ExternalCost = (from internalCost in financeSettings
                                where internalCost.Name.Equals("financeExternalCost")
                                select internalCost).FirstOrDefault();

            decimal InternalHourCost = decimal.Parse(InternalCost.Value, System.Globalization.CultureInfo.InvariantCulture);
            decimal ExternalHourCost = decimal.Parse(ExternalCost.Value, System.Globalization.CultureInfo.InvariantCulture);
            decimal CostInternalHours = activity.InternalHours * InternalHourCost;
            decimal CostExternalHours = activity.ExternalHours * ExternalHourCost;
            decimal Cost = activity.Cost;

            decimal TotalSum = CostInternalHours + CostExternalHours + Cost;

            return decimal.Round(TotalSum, 1);
        }

        public static IEnumerable<ProjectFollowUp> CalculateFollowUpRow(IEnumerable<ProjectFollowUp> followUps)
        {
            IEnumerable<Settings> financeSettings = appRepository.GetSettings("FinanceRatio");
            var InternalCost = ((from internalCost in financeSettings
                                 where internalCost.Name.Equals("financeInternalCost")
                                 select internalCost).FirstOrDefault());

            var ExternalCost = (from internalCost in financeSettings
                                where internalCost.Name.Equals("financeExternalCost")
                                select internalCost).FirstOrDefault();

            decimal totalSum = 0;
            decimal previousSum = 0;

            decimal internalHourCost = decimal.Parse(InternalCost.Value, System.Globalization.CultureInfo.InvariantCulture);
            decimal externalHourCost = decimal.Parse(ExternalCost.Value, System.Globalization.CultureInfo.InvariantCulture);
            decimal RowActivityTotal = 0;

            foreach (var followUp in followUps)
            {

                decimal OtherCosts = followUp.OtherCosts;
                decimal? activityTotal = followUp.ActivityTotal;
                decimal rowInternalTime = followUp.InternalHours;
                decimal rowExternalTime = followUp.ExternalHours;
                decimal rowMisc = followUp.OtherCosts;

                decimal rowcalculatedInternalCost = rowInternalTime * internalHourCost;
                decimal rowcalculatedExternalCost = rowExternalTime * externalHourCost;
                decimal rowTotal = rowcalculatedInternalCost + rowcalculatedExternalCost + rowMisc;


                if (!followUp.Canceled)
                {
                    previousSum = totalSum;
                    totalSum += rowTotal;
                    RowActivityTotal = activityTotal.GetValueOrDefault() - totalSum;
                    followUp.Balance = RowActivityTotal;
                }
                else
                {
                    RowActivityTotal = activityTotal.GetValueOrDefault() - (totalSum + rowTotal);
                }
            }
            return followUps;
        }


    }
}
