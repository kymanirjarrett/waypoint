using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;

namespace Waypoint.Data;

/// <summary>Recognizes the errors Azure SQL serverless returns while a paused database is starting up.</summary>
public static class DatabaseWake
{
    /// <summary>Typical time for the free-offer database to resume after an auto-pause.</summary>
    public const int EstimatedSeconds = 60;

    /// <summary>How often the waking page reloads itself.</summary>
    public const int RetryAfterSeconds = 30;

    // -2: timeout. Seen in production: while the database resumes, login stalls in the post-login phase until the
    //     connect timeout expires (EF does not retry this, so it must be recognized here).
    // 40613: database not currently available (resuming). 40197/40501: service busy or reconfiguring.
    // 49918-49920: not enough resources to process the request yet.
    private static readonly HashSet<int> WakingErrorNumbers = [-2, 40613, 40197, 40501, 49918, 49919, 49920];

    /// <summary>True if a SQL error number means the database is (probably) still waking up.</summary>
    public static bool IsWakingErrorNumber(int number) => WakingErrorNumbers.Contains(number);

    /// <summary>True if the exception (or anything it wraps) means the database is still waking up.</summary>
    public static bool IsWaking(Exception? exception)
    {
        for (var e = exception; e is not null; e = e.InnerException)
        {
            // EF Core throws this once EnableRetryOnFailure gives up on transient errors.
            if (e is RetryLimitExceededException)
            {
                return true;
            }

            if (e is SqlException sql && sql.Errors.Cast<SqlError>().Any(err => IsWakingErrorNumber(err.Number)))
            {
                return true;
            }
        }

        return false;
    }
}
