using System;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{

    public class RebateService : IRebateService
    {
        private readonly RebateDataStore _rebateDataStore;
        private readonly ProductDataStore _productDataStore;

        private IIncentiveCalculator incentiveCalculator;

        public RebateService(RebateDataStore rebateDataStore, ProductDataStore productDataStore)
        {
            _rebateDataStore = rebateDataStore;
            _productDataStore = productDataStore;
        }

        public CalculateRebateResult Calculate(CalculateRebateRequest request)
        {
            var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
            var product = _productDataStore.GetProduct(request.ProductIdentifier);

            var result = new CalculateRebateResult();

            // Check if rebate or product is null
            if (rebate == null)
            {
                Console.WriteLine("Rebate is null.");
                result.Success = false;
                return result;
            }

            if (product == null)
            {
                Console.WriteLine("Product is null.");
                result.Success = false;
                return result;
            }

            // Validate volume is positive
            if (request.Volume <= 0)
            {
                Console.WriteLine("Please enter a valid positive number for volume.");
                result.Success = false;
                return result;
            }


            // Rebate Calculation Logic
            switch (rebate.Incentive)
            {
                case IncentiveType.FixedCashAmount:
                    incentiveCalculator = new FixedCashAmountCalculator();
                    break;
                case IncentiveType.FixedRateRebate:
                    incentiveCalculator = new FixedRateRebateCalculator();
                    break;
                case IncentiveType.AmountPerUom:
                    incentiveCalculator = new AmountPerUomCalculator();
                    break;
                default:
                    Console.WriteLine("Unknown incentive type.");
                    result.Success = false;
                    break;
            }

            result = incentiveCalculator.Calculate(rebate, product, request);
            
            // If calculation is successful, store and return the result
            if (result.Success)
            {
                _rebateDataStore.StoreCalculationResult(rebate, result.Amount);
                Console.WriteLine($"Rebate Calculation Result: {result.Amount}");
            }
            else
            {
                Console.WriteLine("Rebate Calculation Failed.");
            }

            return result;
        }
    }

}
