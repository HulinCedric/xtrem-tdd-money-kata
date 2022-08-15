namespace money_problem.Domain;

public readonly record struct ExchangeRate(Currency From, Currency To, double Rate)
{
    internal string ExchangeName
        => $"{From}->{To}";

    public static ExchangeRate Default(Currency from, Currency to)
        => new(from, to, default);

    public bool IsSameExchange(ExchangeRate exchangeRate)
        => ExchangeName == exchangeRate.ExchangeName;

    public bool IsNotSameExchange(ExchangeRate exchangeRate)
        => !IsSameExchange(exchangeRate);
}