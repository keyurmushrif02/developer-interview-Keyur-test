using System.Collections.Generic;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

public class TestProductDataStore : ProductDataStore
{
    private readonly Dictionary<string, Product> _products;

    public TestProductDataStore(Dictionary<string, Product> products)
    {
        _products = products;
    }

    public new Product GetProduct(string productIdentifier)
    {
        _products.TryGetValue(productIdentifier, out var product);
        return product;
    }
}