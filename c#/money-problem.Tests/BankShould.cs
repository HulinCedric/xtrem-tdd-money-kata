using FluentAssertions;
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
            => bank.ConvertWithException(10d.Euros(), USD)
                .Should()
                .Be(12d.Dollars());

        [Fact(DisplayName = "10 EUR -> EUR = 10 EUR")]
        public void ConvertMoneyInSameCurrency()
            => bank.ConvertWithException(10d.Euros(), EUR)
                .Should()
                .Be(10d.Euros());

        [Fact(DisplayName = "Throws a MissingExchangeRateException in case of missing exchange rates")]
        public void ConvertWithMissingExchangeRateShouldThrowException()
            => bank.Convert(10d.Euros(), KRW)
                .Error
                .Should()
                .Be($"{EUR}->{KRW}");

        [Fact(DisplayName = "Conversion with different exchange rates EUR -> USD")]
        public void ConvertWithDifferentExchangeRates()
        {
            bank.ConvertWithException(10d.Euros(), USD)
                .Should()
                .Be(12d.Dollars());

            bank.AddExchangeRate(EUR, USD, 1.3)
                .ConvertWithException(10d.Euros(), USD)
                .Should()
                .Be(13d.Dollars());
        }
    }
}