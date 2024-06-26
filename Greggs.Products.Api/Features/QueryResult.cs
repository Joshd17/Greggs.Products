using System.Collections.Generic;

namespace Greggs.Products.Api.Features;

public class QueryResult<T>
{
    public T Value { get; private set; }
    public IEnumerable<string> Errors { get; private set; } = new List<string>();

    private QueryResult(T value)
    {
        Value = value;
    }

    private QueryResult(IEnumerable<string> errors = null)
    {
        Errors = errors ?? new List<string>();
    }

    public static QueryResult<T> Success(T value)
    {
        return new QueryResult<T>(value);
    }

    public static QueryResult<T> Fail(IEnumerable<string> errors)
    {
        return new QueryResult<T>(errors);
    }
}
