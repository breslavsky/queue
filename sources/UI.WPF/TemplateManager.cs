using Junte.WCF;
using Queue.Services.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Queue.UI.WPF
{
    public class TemplateManager : ITemplateManager
    {
        private readonly string app;

        public ChannelManager<IServerTemplateTcpService> ChannelManager { get; set; }

        private List<TemplateInfo> cache = new List<TemplateInfo>();

        public TemplateManager(string app)
        {
            this.app = app;
        }

        public string GetTemplate(string template, string theme = "default")
        {
            var data = GetTemplateFromCache(template, theme);
            if (data == null)
            {
                using (var channel = ChannelManager.CreateChannel())
                {
                    data = new StreamReader(channel.Service.GetTemplate(app, theme, template)).ReadToEnd();
                }
            }

            return data;
        }

        private string GetTemplateFromCache(string template, string theme)
        {
            var result = cache.FirstOrDefault(i => i.Template == template && i.Theme == theme);
            return result == null ? null : result.Template;
        }

        private class TemplateInfo
        {
            public string Theme;
            public string Template;
            public string Content;
        }
    }
}