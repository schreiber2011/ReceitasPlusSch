namespace MyRecipeBook.Communication.Responses;

public class ResponseErrorJson
{
    public IList<string> Errors { get; set; }

    public ResponseErrorJson(IList<string> errors) => Errors = errors;

    public bool TokenIsExpire { get; set; }

    public ResponseErrorJson(string error) => Errors = [error];

}
