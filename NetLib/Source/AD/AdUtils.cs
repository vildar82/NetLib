namespace NetLib.AD
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;
    using JetBrains.Annotations;

    /// <summary>
    ///     Работа с Active Directory - получение групп пользователя
    /// </summary>
    [PublicAPI]
    public class ADUtils : IDisposable
    {
        public PrincipalContext context;

        /// <summary>
        ///     Очистка контекста AD
        /// </summary>
        public void Dispose()
        {
            context?.Dispose();
        }

        /// <summary>
        ///     Группы текущего пользователя
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
        ///     Список групп пользователя
        /// </summary>
        [NotNull]
        public List<string> GetCurrentUserGroups([CanBeNull] out string fio)
        {
            using (var user = GetUser(Environment.UserName, Environment.UserDomainName))
            {
                fio = user?.DisplayName;
                using (var groups = user?.GetGroups())
                {
                    return groups?.Select(s => s.Name).ToList() ?? new List<string>();
                }
            }
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
        ///     Получить базовый основной контекст
        /// </summary>
        [NotNull]
        private PrincipalContext GetPrincipalContext([CanBeNull] string domain = null)
        {
            return domain == null
                ? new PrincipalContext(ContextType.Domain)
                : new PrincipalContext(ContextType.Domain, domain);
        }

        /// <summary>
        ///     Получить указанного пользователя Active Directory
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

        public static UserData GetUserData(string login, string domain)
        {
            using (var utils = new ADUtils())
            using (var user = utils.GetUser(login, domain))
            {
                var de = (DirectoryEntry)user.GetUnderlyingObject();
                var department = de.Properties["department"];
                var position = de.Properties["title"];
                return new UserData
                {
                    Department = department.Value?.ToString(),
                    Position = position.Value?.ToString(),
                    Fio = user.DisplayName
                };
            }
        }
    }
}