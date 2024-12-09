using FluentValidation;

namespace Projects.Application.Features.CreateOrUpdateProperty
{
    public class CreateOrUpdatePropertyValidator : AbstractValidator<CreateOrUpdatePropertyRequestDto>
    {
        public CreateOrUpdatePropertyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.PropertyType).NotEmpty().WithMessage("Property type is required");
            RuleFor(x => x.Address).NotNull().WithMessage("Address is required");
            RuleFor(x => x.Address.City).NotEmpty().WithMessage("City is required");
            RuleFor(x => x.Address.Street).NotEmpty().WithMessage("Street is required");
            RuleFor(x => x.Address.BuildingNumber).NotEmpty().WithMessage("Building number is required");
            RuleFor(x => x.Floors).NotNull().WithMessage("Floors are required");
            RuleForEach(x => x.Floors).Custom(ValidateFloor());
        }

        private static Action<FloorDto, ValidationContext<CreateOrUpdatePropertyRequestDto>> ValidateFloor()
        {
            return (floor, context) =>
            {
                if (floor.FloorNumber <= 0)
                {
                    context.AddFailure("Floor number must be greater than 0");
                }
                if (floor.Area <= 0)
                {
                    context.AddFailure("Area must be greater than 0");
                }
            };
        }
    }

    
}
