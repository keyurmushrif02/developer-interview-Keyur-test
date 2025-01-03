using System;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public class AmountPerUomCalculator : IIncentiveCalculator
{
    public bool Supports(IncentiveType incentiveType) =>
        incentiveType == IncentiveType.AmountPerUom;

    public CalculateRebateResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        var result = new CalculateRebateResult();
        if (rebate == null || product == null ||
            !product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) ||
            rebate.Amount <= 0 || request.Volume <= 0)
        {
            Console.WriteLine("Rebate amount or volume is zero for AmountPerUom.");
            result.Success = false;
        }
        else
        {
            result.Amount = rebate.Amount * request.Volume;
            result.Success = true;
        }
        return result;
    }
}
