using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTest;

public class Arcitecturetests
{
    private const string _applicationNamespace = "Application";
    private const string _infrastructureNamespace = "Infrastructure";
    private const string _persistenceNamespace = "Persistence";
    private const string _presentationNamespace = "Presentation";
    private const string _webApiNamespace = "WebApi";

    [Fact]
    public void Domain_Should_Not_Have_DependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Domain.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            _applicationNamespace,
            _infrastructureNamespace,
            _persistenceNamespace,
            _webApiNamespace,
            _presentationNamespace
        };

        //Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_Have_DependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Application.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            _infrastructureNamespace,
            _persistenceNamespace,
            _webApiNamespace,
            _presentationNamespace
        };

        //Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Not_Have_DependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Infrastructure.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            _persistenceNamespace,
            _webApiNamespace,
            _presentationNamespace
        };

        //Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Persistence_Should_Not_Have_DependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Persistence.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            _infrastructureNamespace,
            _webApiNamespace,
            _presentationNamespace
        };

        //Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Presentation_Should_Not_Have_DependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Presentation.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            _infrastructureNamespace,
            _webApiNamespace,
            _persistenceNamespace
        };

        //Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_ShouldHaveDependencyOn_MediatR()
    {
        //Arrange
        var assembly = typeof(Presentation.AssemblyReference).Assembly;

        //Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .Should()
            .HaveDependencyOn("MediatR")
            .GetResult();

        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
}