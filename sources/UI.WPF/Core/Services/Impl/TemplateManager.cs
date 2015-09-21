using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace Queue.UI.WPF
{
    public class TemplateManager : ITemplateManager
    {
        private readonly string app;

        [Dependency]
        public ChannelManager<IServerTemplateTcpService> ChannelManager { get; set; }

        private List<TemplateInfo> cache = new List<TemplateInfo>();

        public TemplateManager(string app)
        {
            this.app = app;

            ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(this);
        }

        public DependencyObject GetTemplate(string template, string theme = "default")
        {
            var content = GetTemplateFromCache(template, theme);
            if (content == null)
            {
                using (var channel = ChannelManager.CreateChannel())
                {
                    content = channel.Service.GetTemplate(app, theme, template).GetAwaiter().GetResult();
                    cache.Add(new TemplateInfo()
                                {
                                    Content = content,
                                    Template = template,
                                    Theme = theme
                                });
                }
            }

            try
            {
                return XamlReader.Parse(content) as DependencyObject;
            }
            catch (Exception e)
            {
                throw new QueueException(String.Format("Невалидная разметка в шаблоне [template: {0}; theme: {1}]", template, theme), e);
            }
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