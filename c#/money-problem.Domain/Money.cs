namespace money_problem.Domain;

public readonly record struct Money(double Amount, Currency Currency)
{
    public Money Times(int times)
        => this with { Amount = Amount * times };

    public Money Divide(int divisor)
        => this with { Amount = Amount / divisor };
}