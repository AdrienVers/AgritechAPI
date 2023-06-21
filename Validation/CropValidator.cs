using agricultureAPI.Models;
using FluentValidation;

namespace agricultureAPI.Validation
{
    public class CropValidator : AbstractValidator<CropInputModel>
    {
        public CropValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty()
                .MaximumLength(1024);
        }
    }
}
