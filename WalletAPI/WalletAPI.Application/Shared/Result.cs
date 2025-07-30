namespace WalletAPI.Application.Shared;
public class Result
{
    public bool Succeeded { get; protected set; }
    public string Message { get; protected set; }
    public List<string> Errors { get; protected set; } = new List<string>();

    protected Result(bool succeeded, string message, IEnumerable<string> errors = null)
    {
        Succeeded = succeeded;
        Message = message;
        if (errors != null)
        {
            Errors.AddRange(errors);
        }
    }

    public static Result Success(string message = "Operación Exitosa.")
    {
        return new Result(true, message);
    }

    public static Result Failure(string message = "Operación Fallida.", IEnumerable<string> errors = null)
    {
        return new Result(false, message, errors);
    }

    public static Result<T> Success<T>(T value, string message = "Operación Exitosa.")
    {
        return new Result<T>(value, true, message);
    }

    public static Result<T> Failure<T>(string message = "Operación Fallida.", IEnumerable<string> errors = null)
    {
        return new Result<T>(default(T), false, message, errors);
    }
}

public class Result<T> : Result
{
    public T Value { get; private set; }

    protected internal Result(T value, bool succeeded, string message, IEnumerable<string> errors = null)
        : base(succeeded, message, errors)
    {
        Value = value;
    }
}
