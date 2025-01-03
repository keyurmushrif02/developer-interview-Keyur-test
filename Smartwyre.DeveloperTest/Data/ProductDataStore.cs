using System.Collections.Generic;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data
{
    public class ProductDataStore
    {
        private readonly Dictionary<string, Product> _products;

        public ProductDataStore()
        {
            // In a real-world scenario, this data could come from a database
            _products = new Dictionary<string, Product>
        {
            { "XYZ456", new Product { Price = 100, SupportedIncentives = SupportedIncentiveType.FixedCashAmount | SupportedIncentiveType.FixedRateRebate } },
            { "XYZ789", new Product { Price = 150, SupportedIncentives = SupportedIncentiveType.AmountPerUom } }
        };
        }

        public Product GetProduct(string productIdentifier)
        {
            _products.TryGetValue(productIdentifier, out var product);
            return product;  // Returns null if product doesn't exist
        }
    }

}
