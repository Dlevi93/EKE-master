using System.Collections.Generic;
using EKE.Data.Entities.Enums;

namespace EKE.Data.Entities.Consts
{
    public static class VtConsts
    {
        private static readonly Dictionary<VtPaymentCategory, decimal> PaymentCategoryValues = new Dictionary<VtPaymentCategory, decimal>
        {
            {VtPaymentCategory.Alap,120},
            {VtPaymentCategory.Tag,60},
            {VtPaymentCategory.Kisgyerek,0},
            {VtPaymentCategory.GyerekNemTag,60},
            {VtPaymentCategory.GyerekTag,30},
            {VtPaymentCategory.DiakNemTag,70},
            {VtPaymentCategory.DiakTag,35},
            {VtPaymentCategory.GyerekCsalad,0},
            {VtPaymentCategory.Napijegy,30}
        };

        public static decimal GetPaymentValue(VtPaymentCategory value)
        {
            return PaymentCategoryValues.TryGetValue(value, out var result) ? result : -1;
        }
    }
}