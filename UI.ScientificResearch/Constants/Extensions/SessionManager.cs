using System.Web;

namespace UI.ScientificResearch.Extensions
{
    /// <summary>
    /// Gets the System.Web.SessionState.HttpSessionState object for the current 
    /// HTTP request.
    /// </summary>
    public class SessionManager : ISession
    {
        public object this[SessionKeyEnum index]
        {
            get
            {
                object session;

                if (HttpContext.Current.Session[index.ToString()] == null ||
                   string.IsNullOrEmpty(HttpContext.Current.Session[index.ToString()].ToString()))
                {
                    session = null;
                }
                else
                {
                    session = HttpContext.Current.Session[index.ToString()];
                }

                return session;
            }

            set
            {
                HttpContext.Current.Session[index.ToString()] = value;
            }
        }

        /// <summary>
        /// Get the session by name
        /// </summary>
        /// <param name="name">key of session</param>
        /// <returns>the corresponding value of the name</returns>
        public static string GetSessionByName(string name)
        {
            string session = string.Empty;

            if (HttpContext.Current.Session[name] == null || HttpContext.Current.Session[name].ToString() == string.Empty)
            {
                session = string.Empty;
            }
            else
            {
                session = HttpContext.Current.Session[name].ToString();
            }

            return session;
        }

        /// <summary>
        /// Removes all keys and values from the session-state collection.
        /// </summary>
        public void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}