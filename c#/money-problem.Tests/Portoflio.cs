using System.Collections.Generic;
using System.Linq;
using money_problem.Domain;

namespace money_problem.Tests;

public class Portoflio
{
    private readonly Dictionary<Currency, double> _amounts;
    private readonly Bank _bank;

    public Portoflio(Bank bank)
    {
        _bank = bank;
        _amounts = new Dictionary<Currency, double>();
    }

    public void Add(int amount, Currency currency)
    {
        if (!_amounts.TryAdd(currency, amount))
            _amounts[currency] += amount;
    }

    public double GetTotalAmount(Currency currency)
    {
        double sum = 0;
        
        var missingExchangeRates = new List<MissingExchangeRateException>();

        foreach (var amount in _amounts)
        {
            try
            {
                sum += _bank.Convert(amount.Value, amount.Key, currency);
            }
            catch (MissingExchangeRateException missingExchangeRate)
            {
                missingExchangeRates.Add(missingExchangeRate);
            }
        }
        
        if (missingExchangeRates.Any())
            throw new MissingExchangeRatesException(missingExchangeRates);

        return sum;
    }
}