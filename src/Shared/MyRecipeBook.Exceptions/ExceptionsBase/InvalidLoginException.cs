namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class InvalidLoginException() : MyRecipeBookException(ErrorMessages.EMAIL_OR_PASSWORD_INVALID)
{
}
