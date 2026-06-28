using FluentAssertions;
using FS.Core.ValueObjects;

namespace FS.Tests.ValueObjects;

public class MediaOwnerTests
{
    [Theory]
    [InlineData("lesson")]
    [InlineData("module")]
    [InlineData("user")]
    public void Create_WithAllowedContext_ShouldSucceed(string context)
    {
        var result = MediaOwner.Create(context, Guid.NewGuid());

        result.IsSuccess.Should().BeTrue();
        result.Value.Context.Should().Be(context);
    }

    [Fact]
    public void Create_WithEmptyContext_ShouldFail()
    {
        var result = MediaOwner.Create("", Guid.NewGuid());

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("empty.context");
    }

    [Fact]
    public void Create_WithDisallowedContext_ShouldFail()
    {
        var result = MediaOwner.Create("course", Guid.NewGuid());

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("unnormalized.context");
    }

    [Fact]
    public void Create_WithEmptyGuid_ShouldFail()
    {
        var result = MediaOwner.Create("lesson", Guid.Empty);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("empty.guid");
    }

    [Fact]
    public void Create_WithContextLongerThan50_ShouldFail()
    {
        var result = MediaOwner.Create(new string('a', 51), Guid.NewGuid());

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Length.context");
    }

    [Fact]
    public void Create_ShouldNormalizeContextToLowerCase()
    {
        var result = MediaOwner.Create("LESSON", Guid.NewGuid());

        result.IsSuccess.Should().BeTrue();
        result.Value.Context.Should().Be("lesson");
    }
}