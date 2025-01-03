using System.Collections.Generic;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

public class TestRebateDataStore : RebateDataStore
{
    private readonly Dictionary<string, Rebate> _rebates;

    public TestRebateDataStore(Dictionary<string, Rebate> rebates)
    {
        _rebates = rebates;
    }

    public new Rebate GetRebate(string rebateIdentifier)
    {
        _rebates.TryGetValue(rebateIdentifier, out var rebate);
        return rebate;
    }
}