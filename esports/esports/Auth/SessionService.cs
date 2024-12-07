using Microsoft.CodeAnalysis.Elfie.Extensions;
using esports.Data;

namespace esports.Auth
{
    public class SessionService
    {
        private readonly EsportsContext _context;

        public SessionService(EsportsContext context)
        {
            _context = context;
        }

        public async Task CreateSession(Guid sessionId, string userId, string refreshToken, DateTime expiresAt)
        {
            _context.Sessions.Add(new Session
            {
                Id = sessionId,
                UserId = userId,
                InitiatedAt = DateTimeOffset.UtcNow,                
                ExpiresAt = expiresAt,
                LastRefreshToken = refreshToken.ToSHA256String()
            });
            
            await _context.SaveChangesAsync();
        }

        public async Task ExtendSession(Guid sessionId, string refreshToken, DateTime expiresAt)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            session.ExpiresAt = expiresAt;
            session.LastRefreshToken = refreshToken.ToSHA256String();

            await _context.SaveChangesAsync();
        }

        public async Task InvalidateSession(Guid sessionId)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            if (session == null)
            {
                return;
            }

            session.isRevoked = true;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsSessionValid(Guid sessionId, string refreshToken)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            return session != null && session.LastRefreshToken == refreshToken.ToSHA256String()
                && session.ExpiresAt > DateTimeOffset.UtcNow
                && !session.isRevoked;
        }
    }
}
