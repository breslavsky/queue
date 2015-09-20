namespace Queue.UI.WPF
{
    public interface ITemplateManager
    {
        string GetTemplate(string template, string theme = "default");
    }
}