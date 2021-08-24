namespace Librarian.Domain.Settings
{
    /*
     * Shared altındaki EmailService, mail gönderme işini üstleniyor.
     * Mail gönderimi sırasında kimden gittiği, Smtp ayarları gibi bilgileri appSettings.json içeriğinden alsak hiç fena olmaz.
     * Bu nedenle Domain altına appSettings altındaki MailSettings sekmesini işaret eden aşağıdaki sınıf eklendi.
     */
    public class MailSettings
    {
        public string From { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string DisplayName { get; set; }
    }
}
