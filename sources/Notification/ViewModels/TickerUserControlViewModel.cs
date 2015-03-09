using Junte.UI.WPF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Queue.Notification.ViewModels
{
    public class TickerUserControlViewModel : ObservableObject, ITicker
    {
        private const double MillisecondsPerUnit = 15;
        private int DefaultSpeed = 5;

        private string ticker;
        private bool inited = false;
        public bool isRunning;

        private string newTicker;
        private double speed;

        private TranslateTransform translateTransform;

        private FrameworkElement container;
        private FrameworkElement tickerItem;

        public string Ticker
        {
            get { return ticker; }
            set { SetProperty(ref ticker, value); }
        }

        public TickerUserControlViewModel()
        {
            ServiceLocator.Current.GetInstance<IUnityContainer>().RegisterInstance<ITicker>(this);

            translateTransform = new TranslateTransform();
            SetSpeed(DefaultSpeed);
        }

        public void Initialize(FrameworkElement container, FrameworkElement tickerItem)
        {
            this.container = container;
            this.tickerItem = tickerItem;
            this.tickerItem.RenderTransform = translateTransform;

            inited = true;
        }

        public void SetSpeed(int speed)
        {
            this.speed = MillisecondsPerUnit / speed;
        }

        public void SetTicker(string ticker)
        {
            newTicker = ticker;
        }

        public void Stop()
        {
            isRunning = false;
        }

        public void Start()
        {
            if (!inited)
            {
                throw new ApplicationException("Бегущая строка не инициализирована");
            }

            if (isRunning)
            {
                return;
            }

            AnimateMove();
            isRunning = true;
        }

        private void AnimateMove()
        {
            if (!string.IsNullOrWhiteSpace(newTicker))
            {
                Ticker = newTicker;
                newTicker = string.Empty;
            }

            tickerItem.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            tickerItem.Arrange(new Rect(tickerItem.DesiredSize));

            double from = container.ActualWidth;
            double to = -tickerItem.ActualWidth;
            TimeSpan animationSpeed = TimeSpan.FromMilliseconds((from - to) * speed);

            DoubleAnimation ani = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = new Duration(animationSpeed)
            };

            Storyboard.SetTarget(ani, tickerItem);
            Timeline.SetDesiredFrameRate(ani, 180);

            ani.Completed += AnimationCompleted;

            translateTransform.BeginAnimation(TranslateTransform.XProperty, ani);
        }

        private void AnimationCompleted(object sender, EventArgs e)
        {
            if (isRunning)
            {
                AnimateMove();
            }
        }
    }
}