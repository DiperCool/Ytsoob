using Ardalis.GuardClauses;

namespace Ytsoob.Services.Posts.Polls.ValueObjects;

public class OptionCount
{
    public OptionCount(long value)
    {
        Value = value;
    }

    public long Value { get; private set; }

    public static OptionCount Of(long value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Negative number");
        }

        return new OptionCount(value);
    }

    public static implicit operator long(OptionCount value) => value.Value;
}
