using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTest;

public class Arcitecturetests
{
    private const string DomainNamespace = "Domain";
    private const string ApplicationNamespace = "Application";
    private const string InfrastructureNamespace = "Infrastructure";
    private const string PersistenceNamespace = "Persistence";
    private const string PresentationNamespace = "Presentation";
    private const string WebApiNamespace = "WebApi";

    [Fact]
    public void Domain_Should_Not_Have_DependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Domain.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            ApplicationNamespace,
            InfrastructureNamespace,
            PersistenceNamespace,
            WebApiNamespace,
            PresentationNamespace
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
            InfrastructureNamespace,
            PersistenceNamespace,
            WebApiNamespace,
            PresentationNamespace
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
            PersistenceNamespace,
            WebApiNamespace,
            PresentationNamespace
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
            InfrastructureNamespace,
            WebApiNamespace,
            PresentationNamespace
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
            InfrastructureNamespace,
            WebApiNamespace,
            PersistenceNamespace
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
}