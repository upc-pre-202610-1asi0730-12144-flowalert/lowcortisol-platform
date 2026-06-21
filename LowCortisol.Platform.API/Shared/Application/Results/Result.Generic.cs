namespace LowCortisol.Platform.API.Shared.Application.Results;

public sealed class Result<TValue> : Result
{
    private Result(bool isSuccess, TValue? value, string? error) : base(isSuccess, error)
    {
        Value = value;
    }

    public TValue? Value { get; }

    public static Result<TValue> Success(TValue value) => new(true, value, null);
    public new static Result<TValue> Failure(string error) => new(false, default, error);
}
