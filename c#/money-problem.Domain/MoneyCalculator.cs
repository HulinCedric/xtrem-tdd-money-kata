namespace money_problem.Domain;

public static class MoneyCalculator
{
    public static double Add(double amount, Currency currency, double addedAmount) => amount + addedAmount;
}