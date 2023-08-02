namespace Ytsoob.Services.Posts.Poll.ValueObjects;

public class Fiction
{
    public Fiction(long value)
    {
        Value = value;
    }

    public long Value { get; private set; }

    public static Fiction Of(long value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Negative number");
        }

        return new Fiction(value);
    }

    public static implicit operator long(Fiction value) => value.Value;
}
