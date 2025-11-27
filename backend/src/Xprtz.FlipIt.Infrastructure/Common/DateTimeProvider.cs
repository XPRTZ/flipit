using Xprtz.FlipIt.Domain.Common;

namespace Xprtz.FlipIt.Infrastructure.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}
