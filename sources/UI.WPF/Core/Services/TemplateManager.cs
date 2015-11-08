﻿using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;

namespace Queue.UI.WPF
{
    public class TemplateManager : ITemplateManager
    {
        private readonly string app;
        private readonly string theme;

        [Dependency]
        public ChannelManager<ITemplateTcpService> ChannelManager { get; set; }

        private Dictionary<string, string> cache = new Dictionary<string, string>();

        public TemplateManager(string app, string theme = "default")
        {
            this.app = app;
            this.theme = theme;

            ServiceLocator.Current.GetInstance<IUnityContainer>()
                .BuildUp(this);
        }

        public FrameworkElement GetTemplate(string template)
        {
            string content = cache.ContainsKey(template) ?
                cache[template] :
                DownloadTemplate(template);

            try
            {
                return XamlReader.Parse(content) as FrameworkElement;
            }
            catch (Exception e)
            {
                throw new QueueException(String.Format("Невалидная разметка в шаблоне [template: {0}; theme: {1}]", template, theme), e);
            }
        }

        private string DownloadTemplate(string template)
        {
            try
            {
                using (var channel = ChannelManager.CreateChannel())
                {
                    var content = channel.Service.GetTemplate(app, theme, template).GetAwaiter().GetResult();
                    cache.Add(template, content);
                    return content;
                }
            }
            catch (Exception e)
            {
                throw new QueueException("Не удалось получить шаблон с сервера: " + e.Message);
            }
        }
    }
}