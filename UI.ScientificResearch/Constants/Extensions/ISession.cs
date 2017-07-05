
namespace UI.ScientificResearch.Extensions
{
    /// <summary>
    /// Gets the System.Web.SessionState.HttpSessionState object for the current 
    /// HTTP request, contructed by Jim Wang.
    /// Defines the action of sessions, for the moke up of session in test.
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// Gets or sets a session value by numerical index.
        /// </summary>
        /// <param name="index">The enum index of the session value.</param>
        /// <returns>The session-state value stored at the specified index, or null if the item does not exist.</returns>
        object this[SessionKeyEnum index] { get; set; }

        /// <summary>
        /// Removes all keys and values from the session-state collection.
        /// </summary>
        void ClearSession();
    }
}
