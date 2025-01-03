using Smartwyre.DeveloperTest.Types;

public interface IIncentiveCalculator
{
    

    bool Supports(IncentiveType incentiveType);
    CalculateRebateResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request);
}



