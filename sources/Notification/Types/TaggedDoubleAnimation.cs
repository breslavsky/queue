using System.Windows;
using System.Windows.Media.Animation;

namespace Queue.Notification.Types
{
    public class TaggedDoubleAnimation : DoubleAnimation
    {
        public FrameworkElement TargetElement { get; set; }

        protected override Freezable CreateInstanceCore()
        {
            return new TaggedDoubleAnimation { TargetElement = TargetElement };
        }
    }
}