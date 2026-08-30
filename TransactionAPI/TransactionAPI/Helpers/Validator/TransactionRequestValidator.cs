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
                .Cascade(CascadeMode.Stop)
                .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.PartnerKeyRequired)
                .Must(x => ValidatorText.TrimText(x).Length <= ValidationConstants.StringLength.PartnerKey)
                .WithMessage(ValidationMessages.PartnerKeyTooLong);

            RuleFor(x => x.PartnerRefNo)
                .Cascade(CascadeMode.Stop)
                .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.PartnerRefNoRequired)
                .Must(x => ValidatorText.TrimText(x).Length <= ValidationConstants.StringLength.PartnerRefNo)
                .WithMessage(ValidationMessages.PartnerRefNoTooLong);

            RuleFor(x => x.PartnerPassword)
                .Cascade(CascadeMode.Stop)
                .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.PartnerPasswordRequired)
                .Must(x => ValidatorText.TrimText(x).Length <= ValidationConstants.StringLength.PartnerPassword)
                .WithMessage(ValidationMessages.PartnerPasswordTooLong);

            RuleFor(x => x.TotalAmount)
                .Cascade(CascadeMode.Stop)
                 .NotNull().WithMessage(ValidationMessages.TotalAmountRequired)
                .GreaterThan(0).WithMessage(ValidationMessages.TotalAmountPositive);

            RuleFor(x => x.Timestamp)
                 .Cascade(CascadeMode.Stop)
                .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.TimestampRequired)
                .Must(BeValidIso8601).WithMessage(ValidationMessages.TimestampInvalidFormat);
           
            RuleFor(x => x.Sig)
                .Cascade(CascadeMode.Stop)
                .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.SigRequired);

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.PartnerItemRef)
                    .Cascade(CascadeMode.Stop)
                    .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.PartnerItemRefRequired)
                    .Must(x => ValidatorText.TrimText(x).Length <= ValidationConstants.StringLength.PartnerItemRef)
                    .WithMessage(ValidationMessages.PartnerItemRefTooLong);

                item.RuleFor(x => x.Name)
                    .Cascade(CascadeMode.Stop)
                    .Must(ValidatorText.HasValue).WithMessage(ValidationMessages.NameRequired)
                    .Must(x => ValidatorText.TrimText(x).Length <= ValidationConstants.StringLength.Name)
                    .WithMessage(ValidationMessages.NameTooLong);

                item.RuleFor(x => x.Qty)
                .Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage(ValidationMessages.ItemQtyRequired)
                    .GreaterThan(0).WithMessage(ValidationMessages.QtyPositive)
                    .LessThanOrEqualTo(ValidationConstants.Numeric.MaxQty)
                    .WithMessage(ValidationMessages.QtyMaxExceeded);

                item.RuleFor(x => x.UnitPrice)
                .Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage(ValidationMessages.ItemUnitPriceRequired)
                    .GreaterThan(0).WithMessage(ValidationMessages.UnitPricePositive);
            });
        }
        private bool BeValidIso8601(string timestamp)
        {
            return DateTimeOffset.TryParse(
                ValidatorText.TrimText(timestamp),
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal,
                out _);
        }
    }
}
