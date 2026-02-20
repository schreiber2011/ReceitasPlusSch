namespace MyRecipeBook.Exceptions;

public class ErrorMessages
{
    private ErrorMessages() { }

    // Templates
    private static string EMPTY => ResourceMessagesException.ResourceManager.GetString("EMPTY")!;
    private static string INVALID => ResourceMessagesException.ResourceManager.GetString("INVALID")!;
    private static string INVALIDS => ResourceMessagesException.ResourceManager.GetString("INVALIDS")!;
    private static string NAME => ResourceMessagesException.ResourceManager.GetString("NAME")!;
    private static string EMAIL => ResourceMessagesException.ResourceManager.GetString("EMAIL")!;
    private static string PASSWORD => ResourceMessagesException.ResourceManager.GetString("PASSWORD")!;

    public static string PASSWORD_NOT6CHAR => ResourceMessagesException.ResourceManager.GetString("PASSWORD_NOT6CHAR")!;

    public static string PASSWORD_EMPTY => string.Format(EMPTY, PASSWORD);
    public static string NAME_EMPTY => string.Format(EMPTY, NAME);
    public static string EMAIL_EMPTY => string.Format(EMPTY, EMAIL);
    public static string EMAIL_INVALID => string.Format(INVALID, EMAIL);
    public static string EMAIL_OR_PASSWORD_INVALID => string.Format(INVALIDS, EMAIL, PASSWORD);

}
