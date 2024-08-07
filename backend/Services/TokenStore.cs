using HotDeskBookingSystem.Interfaces.ServiceInterfaces;
using System.Collections.Concurrent;

namespace HotDeskBookingSystem.Services
{
    public class TokenStore // store in memory
    {
        private readonly ConcurrentDictionary<string, DateTime> _tokenCache = new ConcurrentDictionary<string, DateTime>();

        public void AddToken(string jti, DateTime expirationTime)   // jti == unique identifier for the token
        {
            _tokenCache.TryAdd(jti, expirationTime);
        }
        
        public void RemoveToken(string jti)
        {
            _tokenCache.TryRemove(jti, out _);
        }

        public bool IsTokenActive(string jti)
        {
            if (_tokenCache.TryGetValue(jti, out DateTime expirationTime))
            {
                return expirationTime > DateTime.Now;
            }
            return false;
        }

        public void ClearExpiredTokens()
        {
            foreach (var token in _tokenCache)
            {
                if (token.Value < DateTime.Now)
                {
                    _tokenCache.TryRemove(token.Key, out _);
                }
            }
        }
    }
}
