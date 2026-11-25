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
}
