using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data
{

    public class RebateDataStore
    {
        private readonly Dictionary<string, Rebate> _rebates;

        public RebateDataStore()
        {
            // In a real-world scenario, this data could come from a database
            _rebates = new Dictionary<string, Rebate>
        {
            { "ABC123", new Rebate { Amount = 50, Incentive = IncentiveType.FixedCashAmount } },
            { "DEF456", new Rebate { Amount = 0, Percentage = 10, Incentive = IncentiveType.FixedRateRebate } },
            { "XYZ789", new Rebate { Amount = 5, Incentive = IncentiveType.AmountPerUom } }
        };
        }

        public Rebate GetRebate(string rebateIdentifier)
        {
            _rebates.TryGetValue(rebateIdentifier, out var rebate);
            return rebate;  // Returns null if rebate doesn't exist
        }

        public void StoreCalculationResult(Rebate rebate, decimal rebateAmount)
        {
            // Simulate storing the result (in a real-world case, store it in a database)
            Console.WriteLine($"Rebate calculation for {rebate.Identifier} stored. Amount: {rebateAmount}");
        }
    }

}
