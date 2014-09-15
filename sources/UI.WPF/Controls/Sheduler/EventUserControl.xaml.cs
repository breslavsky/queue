using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Queue.UI.WPF.Controls.Sheduler
{
    public partial class EventUserControl : UserControl
    {
        private Event _e;

        public EventUserControl(Event e, bool showTime)
        {
            InitializeComponent();

            _e = e;

            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.Subject = e.Subject;

            SetBackgroundColor(e.Color);

            if (!showTime || e.AllDay)
            {
                this.DisplayDateText.Visibility = System.Windows.Visibility.Hidden;
                this.DisplayDateText.Height = 0;
                this.DisplayDateText.Text = String.Format("{0} - {1}", e.Start.ToShortDateString(), e.End.ToShortDateString());
            }
            else
            {
                this.DisplayDateText.Text = String.Format("{0} - {1}", e.Start.ToString("HH:mm"), e.End.ToString("HH:mm"));
            }
            //this.BorderElement.ToolTip = this.DisplayDateText.Text + System.Environment.NewLine + this.DisplayText.Text;
        }

        public Event Event
        {
            get { return _e; }
        }

        #region Subject

        public static readonly DependencyProperty SubjectProperty =
            DependencyProperty.Register("Subject", typeof(string), typeof(EventUserControl),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(AdjustSubject)));

        public string Subject
        {
            get { return (string)GetValue(SubjectProperty); }
            set { SetValue(SubjectProperty, value); }
        }

        private static void AdjustSubject(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as EventUserControl).DisplayText.Text = (string)e.NewValue;
        }

        #endregion Subject

        public void SetBackgroundColor(Brush color)
        {
            BorderElement.Background = color;
        }
    }
}