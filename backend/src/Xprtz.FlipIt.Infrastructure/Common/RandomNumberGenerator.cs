using Xprtz.FlipIt.Domain.Common;

namespace Xprtz.FlipIt.Infrastructure.Common;

public class RandomNumberGenerator(int seed) : IRandomNumberGenerator
{
    private readonly Random _random = new(seed);

    public double GetNoise(double min, double max)
    {
        var mean = (min + max) / 2;
        var standardDeviation = (max - mean) / 2;
        
        return Math.Clamp(NextGaussian(mean, standardDeviation), min, max);
    }

    private double NextGaussian(double mean, double standardDeviation)
    {
        var u1 = _random.NextDouble();
        var u2 = _random.NextDouble();

        // Box-Muller transform
        var randStdNormal = 
            Math.Sqrt(-2.0 * Math.Log(u1)) *
            Math.Cos(2.0 * Math.PI * u2);

        return mean + standardDeviation * randStdNormal;
    }
}
