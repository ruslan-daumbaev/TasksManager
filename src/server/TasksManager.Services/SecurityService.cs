using Ganss.Xss;
using System;
using TasksManager.Services.Interfaces;

namespace TasksManager.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly HtmlSanitizer htmlSanitizer = new();

        public string SanitizeText(string text)
        {
            ArgumentException.ThrowIfNullOrEmpty(text, nameof(text));

            return htmlSanitizer.Sanitize(text);
        }
    }
}
