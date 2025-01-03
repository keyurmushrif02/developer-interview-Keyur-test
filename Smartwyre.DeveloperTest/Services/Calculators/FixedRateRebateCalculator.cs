using System;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public class FixedRateRebateCalculator : IIncentiveCalculator
{

    public bool Supports(IncentiveType incentiveType) =>
        incentiveType == IncentiveType.FixedRateRebate;

    public CalculateRebateResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        var result = new CalculateRebateResult();
        if (product == null ||
            !product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate)
            || product.Price <= 0 || request.Volume <= 0 || rebate.Percentage <= 0)
        {
            Console.WriteLine("Product does not support FixedRateRebate or rebate percentage is zero.");
            result.Success = false;
        }
        else
        {
            result.Amount = product.Price * (rebate.Percentage / 100m) * request.Volume;
            result.Success = true;
        }
        return result;
    }
}
