using System.Collections.Immutable;
using LanguageExt;

namespace money_problem.Domain;

public class Portfolio
{
    private readonly IReadOnlyList<Money> moneys;

    private Portfolio() : this(new List<Money>())
    {
    }

    private Portfolio(IEnumerable<Money> moneys)
        => this.moneys = moneys.ToImmutableList();

    public static Portfolio Empty
        => new();

    public static Portfolio WithMoneys(params Money[] moneys)
        => new(moneys);

    public Portfolio Add(Money money)
    {
        var newMoneys = new List<Money>(moneys) { money };
        return new Portfolio(newMoneys);
    }

    public ConversionResults EvaluateWithConversionResult(Bank bank, Currency currency)
        => ConvertMoneys(bank, currency);

    private ConversionResults ConvertMoneys(Bank bank, Currency currency)
        => new(
            moneys
                .Select(money => bank.Convert(money, currency)),
            currency);

    public Either<string, Money> Evaluate(Bank bank, Currency currency)
    {
        var conversionResult = EvaluateWithConversionResult(bank, currency);
        return conversionResult.IsFailure
                   ? Either<string, Money>.Left(conversionResult.Error)
                   : Either<string, Money>.Right(conversionResult.Money);
    }
}