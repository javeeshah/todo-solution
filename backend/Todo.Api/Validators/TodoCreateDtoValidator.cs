using FluentValidation;
using Todo.Api.Dtos;

namespace Todo.Api.Validators
{
    public class TodoCreateDtoValidator: AbstractValidator<TodoItemCreateDto>
    {
        public TodoCreateDtoValidator() 
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
        }
    }
}
