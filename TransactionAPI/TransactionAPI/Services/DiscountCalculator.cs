using TransactionAPI.Helpers.Validator;

namespace TransactionAPI.Services
{
    public class DiscountCalculator : IDiscountCalculator
    {
        public (long TotalDiscount, long FinalAmount) CalculateDiscount(long totalAmount)
        {
            long baseDiscountPercent = CalculateBaseDiscount(totalAmount);
            long conditionalDiscountPercent = CalculateConditionalDiscount(totalAmount);

            long totalDiscountPercent = Math.Min(
                baseDiscountPercent + conditionalDiscountPercent,
                ValidationConstants.Numeric.MaxDiscountPercentage);

            long totalDiscount = (long)(totalAmount * totalDiscountPercent / 100);
            long finalAmount = totalAmount - totalDiscount;

            return (totalDiscount, finalAmount);
        }

        private long CalculateBaseDiscount(long totalAmount)
        {
            return totalAmount switch
            {
                < 20000 => 0,                    
                >= 20000 and <= 50000 => 5,     
                >= 50001 and <= 80000 => 7,     
                >= 80001 and <= 120000 => 10,   
                > 120000 => 15                   
            };
        }

        private long CalculateConditionalDiscount(long totalAmount)
        {
            long conditionalDiscount = 0;
            long ringgit = totalAmount / 100; 

            if (totalAmount > 50000 && IsPrime(ringgit))
            {
                conditionalDiscount += 8;
            }

            if (totalAmount > 90000 && EndsInFive(ringgit))
            {
                conditionalDiscount += 10;
            }

            return conditionalDiscount;
        }

        private bool IsPrime(long number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (long)Math.Floor(Math.Sqrt(number));

            for (long i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }

        private bool EndsInFive(long number)
        {
            return number % 10 == 5;
        }
    }
}
