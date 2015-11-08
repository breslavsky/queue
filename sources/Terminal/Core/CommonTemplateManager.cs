using Queue.Common;
using Queue.UI.WPF;

namespace Queue.Terminal.Core
{
    public class CommonTemplateManager : TemplateManager, ICommonTemplateManager
    {
        public CommonTemplateManager(string theme)
            : base(Templates.Apps.Common, theme)
        {
        }
    }
}