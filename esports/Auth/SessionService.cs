using Microsoft.CodeAnalysis.Elfie.Extensions;

namespace esports.Auth
{
    public class SessionService
    {
        public async Task CreateSession(Guid id, string userId, string refreshToken, DateTime expiresAt)
        {
            var session = new Session
            {
                Id = id,
                UserId = userId,
                LastRefreshToken = refreshToken.ToSHA256String(),
                ExpiresAt = expiresAt,
                InitiatedAt = DateTimeOffset.Now,
                isRevoked = false
            };

            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }
    }
}
