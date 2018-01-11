using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace NetLib.AD
{
    /// <summary>
    /// Работа с Active Directory - получение групп пользователя
    /// </summary>
    [PublicAPI]
    public class ADUtils : IDisposable
    {
        public PrincipalContext context;

        /// <summary>
        /// Группы текущего пользователя
        /// </summary>
        [NotNull]
        public static List<string> GetCurrentUserADGroups([CanBeNull] out string fio)
        {
            using (var adUtils = new ADUtils())
            {
                return adUtils.GetCurrentUserGroups(out fio);
            }
        }

        [CanBeNull]
        public static string GetUserFIO([NotNull] string login)
        {
            using (var adUtils = new ADUtils())
            {
                return adUtils.GetFIO(login);
            }
        }

        public static bool IsLoginCorrect([CanBeNull] string login)
        {
            return !string.IsNullOrEmpty(login) && !login.Any(char.IsWhiteSpace);
        }

        /// <summary>
        /// Очистка контекста AD
        /// </summary>
        public void Dispose()
        {
            context?.Dispose();
        }

        /// <summary>
        /// Список групп пользователя
        /// </summary>
        [NotNull]
        public List<string> GetCurrentUserGroups([CanBeNull] out string fio)
        {
            var userGroups = new List<string>();
            using (var user = GetUser(Environment.UserName, Environment.UserDomainName))
            {
                using (var groups = user?.GetGroups())
                {
                    if (groups != null)
                    {
                        foreach (var group in groups)
                        {
                            userGroups.Add(@group.Name);
                        }
                    }
                }
                fio = user?.DisplayName;
            }
            return userGroups;
        }

        [CanBeNull]
        private string GetFIO([NotNull] string login)
        {
            using (var user = GetUser(login, Environment.UserDomainName))
            {
                return user?.DisplayName;
            }
        }

        /// <summary>
        /// Получить базовый основной контекст
        /// </summary>
        [NotNull]
        private PrincipalContext GetPrincipalContext([CanBeNull] string domain = null)
        {
            return domain == null ? new PrincipalContext(ContextType.Domain) : new PrincipalContext(ContextType.Domain, domain);
        }

        /// <summary>
        /// Получить указанного пользователя Active Directory
        /// </summary>
        /// <param name="sUserName">Имя пользователя для извлечения</param>
        /// <param name="domain">Домен</param>
        [CanBeNull]
        private UserPrincipal GetUser([NotNull] string sUserName, string domain)
        {
            context = GetPrincipalContext(domain);
            var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, sUserName);
            return user;
        }
    }
}