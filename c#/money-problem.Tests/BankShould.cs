using FluentAssertions;
using FluentAssertions.LanguageExt;
using money_problem.Domain;
using Xunit;
using static money_problem.Domain.Currency;

namespace money_problem.Tests
{
    public class BankShould
    {
        private readonly Bank bank = Bank.WithExchangeRate(EUR, USD, 1.2);

        [Fact(DisplayName = "10 EUR -> USD = 12 USD")]
        public void ConvertEuroToUsd()
            => bank.Convert(10d.Euros(), USD)
                .Should()
                .Be(12d.Dollars());

        [Fact(DisplayName = "10 EUR -> EUR = 10 EUR")]
        public void ConvertMoneyInSameCurrency()
            => bank.ConvertWithConversionResult(10d.Euros(), EUR)
                .Money
                .Should()
                .Be(10d.Euros());

        [Fact(DisplayName = "Returns a missing exchange rate failure")]
        public void ConvertWithMissingExchangeRateShouldReturnsFailure()
            => bank.ConvertWithConversionResult(10d.Euros(), KRW)
                .Error
                .Should()
                .Be($"{EUR}->{KRW}");

        [Fact(DisplayName = "Conversion with different exchange rates EUR -> USD")]
        public void ConvertWithDifferentExchangeRates()
        {
            bank.ConvertWithConversionResult(10d.Euros(), USD)
                .Money
                .Should()
                .Be(12d.Dollars());

            bank.AddExchangeRate(EUR, USD, 1.3)
                .ConvertWithConversionResult(10d.Euros(), USD)
                .Money
                .Should()
                .Be(13d.Dollars());
        }
    }
}