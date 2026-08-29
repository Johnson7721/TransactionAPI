using System.Globalization;
using FluentValidation;
using TransactionAPI.Models;

namespace TransactionAPI.Helpers.Validator
{
    public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
    {
        public TransactionRequestValidator()
        {
            RuleFor(x => x.PartnerKey)
                .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.PartnerKeyRequired)
                .Must(x => ValidatorText.TrimText(x).Length <= ValidationConstants.StringLength.PartnerKey)
                .WithMessage(ValidationMessages.PartnerKeyTooLong);

            RuleFor(x => x.PartnerRefNo)
                .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.PartnerRefNoRequired)
                .Must(x => ValidatorText.TrimText(x).Length <= ValidationConstants.StringLength.PartnerRefNo)
                .WithMessage(ValidationMessages.PartnerRefNoTooLong);

            RuleFor(x => x.PartnerPassword)
                .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.PartnerPasswordRequired)
                .Must(x => ValidatorText.TrimText(x).Length <= ValidationConstants.StringLength.PartnerPassword)
                .WithMessage(ValidationMessages.PartnerPasswordTooLong);

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage(ValidationMessages.TotalAmountPositive);

            RuleFor(x => x.Timestamp)
                .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.TimestampRequired)
                .Must(BeWithinTimeWindow).WithMessage(x => GetTimestampErrorMessage(x.Timestamp));

            RuleFor(x => x.Sig)
                .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.SigRequired);

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.PartnerItemRef)
                    .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.PartnerItemRefRequired)
                    .Must(x => ValidatorText.TrimText(x).Length <= ValidationConstants.StringLength.PartnerItemRef)
                    .WithMessage(ValidationMessages.PartnerItemRefTooLong);

                item.RuleFor(x => x.Name)
                    .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.NameRequired)
                    .Must(x => ValidatorText.TrimText(x).Length <= ValidationConstants.StringLength.Name)
                    .WithMessage(ValidationMessages.NameTooLong);

                item.RuleFor(x => x.Qty)
                    .GreaterThan(0).WithMessage(ValidationMessages.QtyPositive)
                    .LessThanOrEqualTo(ValidationConstants.Numeric.MaxQty)
                    .WithMessage(ValidationMessages.QtyMaxExceeded);

                item.RuleFor(x => x.UnitPrice)
                    .GreaterThan(0).WithMessage(ValidationMessages.UnitPricePositive);
            });

            RuleFor(x => x)
                .Must(HaveValidItemTotal).WithMessage(ValidationMessages.InvalidTotalAmount)
                .When(x => x.Items != null);
        }

        private bool BeWithinTimeWindow(string timestamp)
        {
            var trimmed = ValidatorText.TrimText(timestamp);
            if (!DateTimeOffset.TryParse(
                    trimmed,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind,
                    out var requestTime))
            {
                return false;
            }

            var serverTime = DateTimeOffset.UtcNow;
            var difference = Math.Abs((serverTime - requestTime).TotalMinutes);
            return difference <= ValidationConstants.Time.MaxTimestampDifferenceMinutes;
        }

        private string GetTimestampErrorMessage(string timestamp)
        {
            var trimmed = ValidatorText.TrimText(timestamp);
            if (!DateTimeOffset.TryParse(
                    trimmed,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind,
                    out _))
            {
                return ValidationMessages.TimestampInvalidFormat;
            }

            return ValidationMessages.Expired;
        }

        private bool HaveValidItemTotal(TransactionRequest request)
        {
            if (request.Items == null) return true;

            var itemTotal = request.Items.Sum(i => i.Qty * i.UnitPrice);
            return itemTotal == request.TotalAmount;
        }
    }
}
