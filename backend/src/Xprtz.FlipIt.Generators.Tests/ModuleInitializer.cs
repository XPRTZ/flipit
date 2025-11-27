using System.Runtime.CompilerServices;

namespace Xprtz.FlipIt.Generators.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize() => VerifySourceGenerators.Initialize();
}
