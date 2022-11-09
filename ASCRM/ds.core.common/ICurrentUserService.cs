namespace ds.core.common
{
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <returns></returns>
        int? GetUserIdFromSession();
        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <returns></returns>
        string GetUserNameFromSession();
        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        string UserName { get; }
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        int UserId { get; }
    }
}
