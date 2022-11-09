using ds.core.common;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace ds.portal.ui.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        /// <summary>
        /// The user identifier
        /// </summary>
        private const string USER_ID = "UserId";
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentUserService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            var claim = httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "id");
            if (claim != null)
            {
                UserId = int.Parse(claim.Value);
            }
            UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string UserName { get; }

        public int UserId { get; }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <returns></returns>
        public int? GetUserIdFromSession()
        {
            var userId = _httpContextAccessor.HttpContext.Session.Keys.FirstOrDefault(x => x == USER_ID);
            return (userId != null) ? _httpContextAccessor.HttpContext.Session.GetInt32(USER_ID) : 0;
        }
        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <returns></returns>
        public string GetUserNameFromSession()
        {
            var userName = _httpContextAccessor.HttpContext.Session.Keys.FirstOrDefault(x => x == "UserName");
            return (userName != null) ? _httpContextAccessor.HttpContext.Session.GetString("UserName") : string.Empty;
        }
    }
}
