using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
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

            ServiceLocator.Current.GetInstance<UnityContainer>().BuildUp(this);
        }

        public DependencyObject GetTemplate(string template, string theme = "default")
        {
            var data = GetTemplateFromCache(template, theme);
            if (data == null)
            {
                //data = File.ReadAllText(@"D:\templates\main_page.xml");

                using (var channel = ChannelManager.CreateChannel())
                {
                    data = new StreamReader(channel.Service.GetTemplate(app, theme, template)).ReadToEnd();
                }
            }

            try
            {
                return XamlReader.Parse(data) as DependencyObject;
            }
            catch (Exception e)
            {
                throw new QueueException("Невалидная разметка в шаблоне", e);
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