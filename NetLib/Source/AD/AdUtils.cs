using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace NetLib.AD
{
    /// <summary>
    /// Работа с Active Directory - получение групп пользователя
    /// </summary>
    public class ADUtils : IDisposable
    {
        public PrincipalContext context;

        /// <summary>
        /// Группы текущего пользователя
        /// </summary>        
        public static List<string> GetCurrentUserADGroups(out string fio)
        {
            using (var adUtils = new ADUtils())
            {
                return adUtils.GetCurrentUserGroups(out fio);
            }
        }

	    public static string GetUserFIO(string login)
	    {
			using (var adUtils = new ADUtils())
			{
				return adUtils.GetFIO(login);
			}
		}

		public static bool IsLoginCorrect(string login)
		{
			return !string.IsNullOrEmpty(login) && !login.Any(char.IsWhiteSpace);
		}

	    /// <summary>
		/// Получить базовый основной контекст
		/// </summary>        
		private PrincipalContext GetPrincipalContext(string domain = null)
        {
            if (domain == null) return new PrincipalContext(ContextType.Domain);
            return new PrincipalContext(ContextType.Domain, domain);
        }

        /// <summary>
        /// Получить указанного пользователя Active Directory
        /// </summary>
        /// <param name="sUserName">Имя пользователя для извлечения</param>        
        private UserPrincipal GetUser(string sUserName, string domain)
        {
            context = GetPrincipalContext(domain);
            var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, sUserName);
	        return user;
        }

        /// <summary>
        /// Список групп пользователя
        /// </summary>        
        public List<string> GetCurrentUserGroups(out string fio)
        {
            var userGroups = new List<string>();
            using (var user = GetUser(Environment.UserName, Environment.UserDomainName))
            {
                using (var groups = user?.GetGroups())
                {
                    foreach (var group in groups)
                    {
                        userGroups.Add(group.Name);
                    }
                }
                fio = user?.DisplayName;
            }
            return userGroups;
        }

	    private string GetFIO(string login)
	    {
		    using (var user = GetUser(login, Environment.UserDomainName))
		    {
			    return user?.DisplayName;
		    }
		}

		/// <summary>
		/// Очистка контекста AD
		/// </summary>
		public void Dispose()
        {
            context?.Dispose();
        }
    }
}
