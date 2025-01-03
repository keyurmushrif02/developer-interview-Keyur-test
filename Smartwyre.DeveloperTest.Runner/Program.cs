using System;
using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner
{

    public class Program
    {
    public static void Main(string[] args)
    {
        var rebateDataStore = new RebateDataStore();
        var productDataStore = new ProductDataStore();
        var rebateService = new RebateService(rebateDataStore, productDataStore);

        Console.WriteLine("Enter the Rebate Identifier:");
        var rebateIdentifier = Console.ReadLine();
        Console.WriteLine("Enter the Product Identifier:");
        var productIdentifier = Console.ReadLine();

        decimal volume;
        do
        {
            Console.WriteLine("Enter the Volume:");
        } while (!decimal.TryParse(Console.ReadLine(), out volume) || volume <= 0);

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume
        };

        var result = rebateService.Calculate(request);
        Console.WriteLine($"Rebate Calculation Success: {result.Success}");
        if (result.Success)
        {
            Console.WriteLine($"Rebate Amount: {result.Amount}");
        }
    }
}
}
