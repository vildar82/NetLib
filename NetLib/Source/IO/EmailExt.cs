namespace NetLib.IO
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Mail;

    public static class EmailExt
    {
        public static void MailTo(this MailMessage message)
        {
            var mailTo = message.ToUrl();
            Process.Start(mailTo);
        }
        
        public static string ToUrl(this MailMessage message) =>
            "mailto:?" + string.Join("&", Parameters(message));

        static IEnumerable<string> Parameters(MailMessage message)
        {
            if (message.To.Any())
                yield return "to=" + Recipients(message.To);

            if (message.CC.Any())
                yield return "cc=" + Recipients(message.CC);

            if (message.Bcc.Any())
                yield return "bcc=" + Recipients(message.Bcc);

            if (!string.IsNullOrWhiteSpace(message.Subject))
                yield return "subject=" + EscapeDataString(message.Subject);

            if (!string.IsNullOrWhiteSpace(message.Body))
                yield return "body=" + EscapeDataString(message.Body);
        }

        static object EscapeDataString(string messageBody) => messageBody;

        static string Recipients(MailAddressCollection addresses) =>
            string.Join(",", from r in addresses select EscapeDataString(r.Address));
    }
}