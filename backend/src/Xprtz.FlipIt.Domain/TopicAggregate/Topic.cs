using ErrorOr;
using Xprtz.FlipIt.Domain.SeedWork;

namespace Xprtz.FlipIt.Domain.TopicAggregate;

public class Topic : AggregateRoot
{
    public string Name { get; private set; }
    public string FrontLabel { get; private set; }
    public string BackLabel { get; private set; }

    private Topic(Guid id, string name, string frontLabel, string backLabel)
        : base(id)
    {
        Name = name;
        FrontLabel = frontLabel;
        BackLabel = backLabel;
    }

    public ErrorOr<Topic> Update(string name, string frontLabel, string backLabel)
    {
        var errors = Validate(name, frontLabel, backLabel);

        if (errors.Count != 0)
        {
            return errors;
        }

        Name = name;
        FrontLabel = frontLabel;
        BackLabel = backLabel;

        return this;
    }

    public ErrorOr<Success> Delete()
    {
        // nothing for now, raise domain event later
        return Result.Success;
    }

    public static ErrorOr<Topic> Create(string name, string frontLabel, string backLabel)
    {
        var errors = Validate(name, frontLabel, backLabel);

        if (errors.Count != 0)
        {
            return errors;
        }

        return new Topic(Guid.NewGuid(), name, frontLabel, backLabel);
    }

    private static List<Error> Validate(string name, string frontLabel, string backLabel)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add(Error.Validation("Name is required"));
        }

        if (string.IsNullOrWhiteSpace(frontLabel))
        {
            errors.Add(Error.Validation("Front label is required"));
        }

        if (string.IsNullOrWhiteSpace(backLabel))
        {
            errors.Add(Error.Validation("Back label is required"));
        }

        return errors;
    }
}
