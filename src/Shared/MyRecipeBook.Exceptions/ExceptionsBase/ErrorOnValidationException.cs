namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class ErrorOnValidationException(IList<string> errors) : MyRecipeBookException
{
    public IList<string> ErrorMessages { get; set; } = errors;
}
