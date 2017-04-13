using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib.AD
{
    /// <summary>
    /// Работа с Active Directory - получение групп пользователя
    /// </summary>
    public class ADUtils : IDisposable
    {
        private PrincipalContext context;

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
            if (user != null)
            {
                return user;
            }
            return null;
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

        /// <summary>
        /// Очистка контекста AD
        /// </summary>
        public void Dispose()
        {
            context?.Dispose();
        }
    }
}
