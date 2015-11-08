using System.Windows;

namespace Queue.UI.WPF
{
    public interface ITemplateManager
    {
        FrameworkElement GetTemplate(string template);
    }
}