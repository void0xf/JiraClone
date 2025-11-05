using System.Collections.Concurrent;

namespace UserService.Infrastructure;

public interface IVerificationCodeService
{
    void StoreCode(string email, string code, DateTime expiry);
    (string? code, DateTime? expiry) GetCode(string email);
    void RemoveCode(string email);
    void CleanupExpiredCodes();
}

public class InMemoryVerificationCodeService : IVerificationCodeService
{
    private readonly ConcurrentDictionary<string, (string Code, DateTime Expiry)> _verificationCodes = new();
    private readonly Timer _cleanupTimer;

    public InMemoryVerificationCodeService()
    {
        // Cleanup expired codes every 5 minutes
        _cleanupTimer = new Timer(_ => CleanupExpiredCodes(), null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
    }

    public void StoreCode(string email, string code, DateTime expiry)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        _verificationCodes[normalizedEmail] = (code, expiry);
    }

    public (string? code, DateTime? expiry) GetCode(string email)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        if (_verificationCodes.TryGetValue(normalizedEmail, out var codeData))
        {
            return (codeData.Code, codeData.Expiry);
        }
        return (null, null);
    }

    public void RemoveCode(string email)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        _verificationCodes.TryRemove(normalizedEmail, out _);
    }

    public void CleanupExpiredCodes()
    {
        var now = DateTime.UtcNow;
        var expiredKeys = _verificationCodes
            .Where(kvp => kvp.Value.Expiry < now)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var key in expiredKeys)
        {
            _verificationCodes.TryRemove(key, out _);
        }
    }
}
