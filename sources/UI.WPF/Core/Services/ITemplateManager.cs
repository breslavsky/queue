using System.Windows;

namespace Queue.UI.WPF
{
    public interface ITemplateManager
    {
        DependencyObject GetTemplate(string template);
    }
}