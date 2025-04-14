namespace LocalizeHelper;

public class Result
{
    public Result() { }

    public static Result Success(string? message=null) => new Result { Succeeded = true, Message=message??string.Empty };
    public static Result Failure(string message) => new Result { Succeeded = false, Message = message }; 
    public bool Succeeded { get; set; }
    public string Message { get; set; } = string.Empty;


}