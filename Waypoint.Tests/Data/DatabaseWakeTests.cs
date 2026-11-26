using Microsoft.EntityFrameworkCore.Storage;
using Waypoint.Data;

namespace Waypoint.Tests.Data;

public class DatabaseWakeTests
{
    [Fact]
    public void RetryLimitExceeded_IsWaking() =>
        Assert.True(DatabaseWake.IsWaking(new RetryLimitExceededException("gave up")));

    [Fact]
    public void WrappedRetryLimitExceeded_IsWaking() =>
        Assert.True(DatabaseWake.IsWaking(new InvalidOperationException("outer", new RetryLimitExceededException("gave up"))));

    [Fact]
    public void UnrelatedException_IsNotWaking() =>
        Assert.False(DatabaseWake.IsWaking(new InvalidOperationException("bug")));

    [Fact]
    public void NoException_IsNotWaking() =>
        Assert.False(DatabaseWake.IsWaking(null));

    [Theory]
    [InlineData(-2)]     // connect/login timeout while resuming (seen in production)
    [InlineData(40613)]  // database not currently available
    [InlineData(49918)]
    public void ResumeErrorNumbers_AreWaking(int number) => Assert.True(DatabaseWake.IsWakingErrorNumber(number));

    [Theory]
    [InlineData(208)]    // invalid object name: a real bug, not a wake-up
    [InlineData(18456)]  // login failed
    public void OtherErrorNumbers_AreNotWaking(int number) => Assert.False(DatabaseWake.IsWakingErrorNumber(number));
}
