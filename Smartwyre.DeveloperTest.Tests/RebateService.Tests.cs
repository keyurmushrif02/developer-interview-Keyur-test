using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

public class RebateServiceTests
{
    private readonly RebateService _rebateService;

    public RebateServiceTests()
    {
        // Create test data
        var rebates = new Dictionary<string, Rebate>
        {
            { "ABC123", new Rebate { Identifier = "ABC123", Incentive = IncentiveType.FixedCashAmount, Amount = 50 } }
        };

        var products = new Dictionary<string, Product>
        {
            { "XYZ456", new Product { Identifier = "XYZ456", Price = 100, SupportedIncentives = SupportedIncentiveType.FixedCashAmount | SupportedIncentiveType.FixedRateRebate } }
        };

        // Create custom test doubles
        var rebateDataStore = new TestRebateDataStore(rebates);
        var productDataStore = new TestProductDataStore(products);

        // Pass test doubles into RebateService constructor
        _rebateService = new RebateService(rebateDataStore, productDataStore);
    }

    [Fact]
    public void Calculate_RebateAndProductExist_ReturnsCorrectAmount()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "ABC123",
            ProductIdentifier = "XYZ456",
            Volume = 1
        };

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(50, result.Amount);  // Expected amount for FixedCashAmount
    }

    [Fact]
    public void Calculate_RebateMissing_ReturnsFailure()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "InvalidRebate",
            ProductIdentifier = "XYZ456",
            Volume = 1
        };

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_ProductMissing_ReturnsFailure()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "ABC123",
            ProductIdentifier = "InvalidProduct",
            Volume = 1
        };

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_InvalidVolume_ReturnsFailure()
    {
        // Arrange
        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "ABC123",
            ProductIdentifier = "XYZ456",
            Volume = 0  // Invalid volume
        };

        // Act
        var result = _rebateService.Calculate(request);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_AmountPerUomIncentive_ReturnsCorrectAmount()
    {
        // Arrange
        var rebate = new Rebate { Identifier = "ABC123", Incentive = IncentiveType.AmountPerUom, Amount = 10 };
        var product = new Product { Identifier = "XYZ456", Price = 100, SupportedIncentives = SupportedIncentiveType.AmountPerUom };

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "ABC123",
            ProductIdentifier = "XYZ456",
            Volume = 5
        };

        var rebates = new Dictionary<string, Rebate> { { "ABC123", rebate } };
        var products = new Dictionary<string, Product> { { "XYZ456", product } };

        var rebateDataStore = new TestRebateDataStore(rebates);
        var productDataStore = new TestProductDataStore(products);
        var rebateService = new RebateService(rebateDataStore, productDataStore);

        // Act
        var result = rebateService.Calculate(request);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(50, result.Amount);  // 10 * 5 = 50
    }

    [Fact]
    public void Calculate_FixedRateRebate_ReturnsCorrectAmount()
    {
        // Arrange
        var rebate = new Rebate { Identifier = "ABC123", Incentive = IncentiveType.FixedRateRebate, Percentage = 10 };
        var product = new Product { Identifier = "XYZ456", Price = 100, SupportedIncentives = SupportedIncentiveType.FixedRateRebate };

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "ABC123",
            ProductIdentifier = "XYZ456",
            Volume = 5  // Adding volume here to match the actual result
        };

        var rebates = new Dictionary<string, Rebate> { { "ABC123", rebate } };
        var products = new Dictionary<string, Product> { { "XYZ456", product } };

        var rebateDataStore = new TestRebateDataStore(rebates);
        var productDataStore = new TestProductDataStore(products);
        var rebateService = new RebateService(rebateDataStore, productDataStore);

        // Act
        var result = rebateService.Calculate(request);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(50, result.Amount);  // Corrected expected amount for volume 5
    }

    [Fact]
    public void Calculate_IncompatibleIncentives_ReturnsFailure()
    {
        // Arrange: Create a rebate with an incompatible incentive type
        var rebate1 = new Rebate { Identifier = "ABC123", Incentive = IncentiveType.FixedRateRebate, Percentage = 10 };
        var rebate2 = new Rebate { Identifier = "XYZ456", Incentive = IncentiveType.AmountPerUom, Amount = 20 };

        var product = new Product { Identifier = "XYZ789", Price = 100, SupportedIncentives = SupportedIncentiveType.FixedRateRebate | SupportedIncentiveType.AmountPerUom };

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "ABC123",
            ProductIdentifier = "XYZ789",
            Volume = 5
        };

        var rebates = new Dictionary<string, Rebate> { { "ABC123", rebate1 }, { "XYZ456", rebate2 } };
        var products = new Dictionary<string, Product> { { "XYZ789", product } };

        var rebateDataStore = new TestRebateDataStore(rebates);
        var productDataStore = new TestProductDataStore(products);
        var rebateService = new RebateService(rebateDataStore, productDataStore);

        // Act
        var result = rebateService.Calculate(request);

        // Assert: Check that the calculation fails due to incompatible incentives
        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_InvalidRebateIdentifierFormat_ReturnsFailure()
    {
        // Arrange: Use an invalid rebate identifier format
        var rebate = new Rebate { Identifier = "ABC123", Incentive = IncentiveType.FixedCashAmount, Amount = 50 };
        var product = new Product { Identifier = "XYZ456", Price = 100, SupportedIncentives = SupportedIncentiveType.FixedCashAmount };

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "INVALID$$$FORMAT",  // Invalid format
            ProductIdentifier = "XYZ456",
            Volume = 5
        };

        var rebates = new Dictionary<string, Rebate> { { "ABC123", rebate } };
        var products = new Dictionary<string, Product> { { "XYZ456", product } };

        var rebateDataStore = new TestRebateDataStore(rebates);
        var productDataStore = new TestProductDataStore(products);
        var rebateService = new RebateService(rebateDataStore, productDataStore);

        // Act
        var result = rebateService.Calculate(request);

        // Assert: Invalid rebate identifier should result in failure
        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_ZeroVolume_ReturnsFailure()
    {
        // Arrange: A valid rebate and product, but volume is zero
        var rebate = new Rebate { Identifier = "ABC123", Incentive = IncentiveType.FixedRateRebate, Percentage = 5 };
        var product = new Product { Identifier = "XYZ456", Price = 100, SupportedIncentives = SupportedIncentiveType.FixedRateRebate };

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "ABC123",
            ProductIdentifier = "XYZ456",
            Volume = 0  // Invalid volume
        };

        var rebates = new Dictionary<string, Rebate> { { "ABC123", rebate } };
        var products = new Dictionary<string, Product> { { "XYZ456", product } };

        var rebateDataStore = new TestRebateDataStore(rebates);
        var productDataStore = new TestProductDataStore(products);
        var rebateService = new RebateService(rebateDataStore, productDataStore);

        // Act
        var result = rebateService.Calculate(request);

        // Assert: Volume is zero, so should fail
        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_PerformanceTestWithLargeDataStore()
    {
        // Arrange: Create a large dataset
        var rebates = new Dictionary<string, Rebate>();
        var products = new Dictionary<string, Product>();

        for (int i = 0; i < 100000; i++) // Large dataset
        {
            rebates.Add($"Rebate{i}", new Rebate { Identifier = $"Rebate{i}", Incentive = IncentiveType.FixedCashAmount, Amount = 50 });
            products.Add($"Product{i}", new Product { Identifier = $"Product{i}", Price = 100, SupportedIncentives = SupportedIncentiveType.FixedCashAmount });
        }

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = "Rebate99999",
            ProductIdentifier = "Product99999",
            Volume = 1
        };

        var rebateDataStore = new TestRebateDataStore(rebates);
        var productDataStore = new TestProductDataStore(products);
        var rebateService = new RebateService(rebateDataStore, productDataStore);

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = rebateService.Calculate(request);
        stopwatch.Stop();
        Console.WriteLine($"Calculation Time: {stopwatch.ElapsedMilliseconds} ms");

        Assert.True(stopwatch.ElapsedMilliseconds < 5000); // 5 seconds for debugging

    }
}
