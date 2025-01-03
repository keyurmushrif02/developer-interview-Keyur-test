using System;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public class FixedCashAmountCalculator : IIncentiveCalculator
{
    public bool Supports(IncentiveType incentiveType) =>
        incentiveType == IncentiveType.FixedCashAmount;

    public CalculateRebateResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        var result = new CalculateRebateResult();
        if (rebate == null || product == null ||
            !product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) ||
            rebate.Amount <= 0)
        {
            Console.WriteLine("Product does not support FixedCashAmount or rebate amount is zero.");
            result.Success = false;
        }
        else
        {
            result.Amount = rebate.Amount;  // Fixed rebate amount
            result.Success = true;
        }
        return result;
    }
}
