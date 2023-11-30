namespace BookUniverse.BLL.DTOValidators.BookValidators
{
    using BookUniverse.BLL.DTOs.BookDTOs;
    using BookUniverse.DAL.Constants.ValidationConstants;
    using FluentValidation;

    public class AddBookDtoValidator : AbstractValidator<AddBookDto>
    {
        public AddBookDtoValidator()
        {
            RuleFor(book => book.Title)
                .NotEmpty()
                .MinimumLength(BookValidationConstants.BOOKTITLE_MIN_LENGTH)
                .MaximumLength(BookValidationConstants.BOOKTITLE_MAX_LENGTH);

            RuleFor(book => book.Description)
                .NotEmpty()
                .MinimumLength(BookValidationConstants.DESCRIPTION_MIN_LENGTH)
                .MaximumLength(BookValidationConstants.DESCRIPTION_MAX_LENGTH);

            RuleFor(book => book.Author)
                .NotEmpty()
                .MinimumLength(BookValidationConstants.AUTHOR_MIN_LENGTH)
                .MaximumLength(BookValidationConstants.AUTHOR_MAX_LENGTH);

            RuleFor(book => book.NumberOfPages)
                .InclusiveBetween(BookValidationConstants.PAGES_MIN, BookValidationConstants.PAGES_MAX);

            RuleFor(book => book.Path)
                .NotEmpty();

            RuleFor(book => book.CategoryName)
                .NotEmpty();
        }
    }
}
