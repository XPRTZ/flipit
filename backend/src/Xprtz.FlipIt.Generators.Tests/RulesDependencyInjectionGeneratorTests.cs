using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Xprtz.FlipIt.Generators.Tests;

public class RulesDependencyInjectionGeneratorTests
{
    [Fact]
    public async Task Should_Create_An_Empty_Extension_Method_If_There_Are_No_Rules()
    {
        var source =
            """
            public interface IRule { }
            
            public interface IIncludeInQuizRule : IRule { }
            """;
        
        await Verify(source);
    }
    
    [Fact]
    public async Task Should_Create_A_Scoped_Service_In_The_Extension_Method_If_There_Is_A_Rule()
    {
        var source =
            """
            public interface IRule { }
            
            public interface IIncludeInQuizRule : IRule { }
            
            public class MyRule : IIncludeInQuizRule { }
            """;
        
        await Verify(source);
    }
    
    private async Task Verify(string source)
    {
        // Arrange
        var syntaxTree = CSharpSyntaxTree.ParseText(source);

        var compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: [syntaxTree]);
        
        var generator = new RulesDependencyInjectionGenerator();
        
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        
        // Act
        driver = driver.RunGenerators(compilation);
        
        // Assert
        await Verifier.Verify(driver);
    }
}
