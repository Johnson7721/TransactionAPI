namespace TransactionAPI.Services
{
    public interface IDiscountCalculator
    {
        (long TotalDiscount, long FinalAmount) CalculateDiscount(long totalAmount);
    }
}
