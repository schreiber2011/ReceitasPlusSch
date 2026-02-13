namespace MyRecipeBook.Exceptions;

public class ErrorMessages
{
    // Templates
    private static string EMPTY => ResourceMessagesException.EMPTY;
    private static string INVALID => ResourceMessagesException.INVALID;
    private static string NAME => ResourceMessagesException.NAME;
    private static string EMAIL => ResourceMessagesException.EMAIL;
    private static string PASSWORD => ResourceMessagesException.PASSWORD;

    public static string PASSWORD_NOT6CHAR => ResourceMessagesException.PASSWORD_NOT6CHAR;

    public static string PASSWORD_EMPTY => string.Format(EMPTY, PASSWORD);
    public static string NAME_EMPTY => string.Format(EMPTY, NAME);
    public static string EMAIL_EMPTY => string.Format(EMPTY, EMAIL);
    public static string EMAIL_INVALID => string.Format(INVALID, EMAIL);

}
