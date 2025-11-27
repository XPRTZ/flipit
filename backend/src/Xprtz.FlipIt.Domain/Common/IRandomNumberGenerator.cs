namespace Xprtz.FlipIt.Domain.Common;

public interface IRandomNumberGenerator
{
    double GetNoise(double min, double max);
}
