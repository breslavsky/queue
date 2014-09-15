using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Queue.UI.WPF.Controls.Sheduler
{
    public class SchedulerGrid : Grid
    {
        static SchedulerGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SchedulerGrid), new FrameworkPropertyMetadata(typeof(SchedulerGrid)));
        }

        #region Properties

        public bool ShowCustomGridLines
        {
            get { return (bool)GetValue(ShowCustomGridLinesProperty); }
            set { SetValue(ShowCustomGridLinesProperty, value); }
        }

        public static readonly DependencyProperty ShowCustomGridLinesProperty =
            DependencyProperty.Register("ShowCustomGridLines", typeof(bool), typeof(SchedulerGrid), new UIPropertyMetadata(false));

        public Brush GridLineBrush
        {
            get { return (Brush)GetValue(GridLineBrushProperty); }
            set { SetValue(GridLineBrushProperty, value); }
        }

        public static readonly DependencyProperty GridLineBrushProperty =
            DependencyProperty.Register("GridLineBrush", typeof(Brush), typeof(SchedulerGrid), new UIPropertyMetadata(Brushes.Black));

        public double GridLineThickness
        {
            get { return (double)GetValue(GridLineThicknessProperty); }
            set { SetValue(GridLineThicknessProperty, value); }
        }

        public static readonly DependencyProperty GridLineThicknessProperty =
            DependencyProperty.Register("GridLineThickness", typeof(double), typeof(SchedulerGrid), new UIPropertyMetadata(1.0));

        #endregion Properties

        protected override void OnRender(DrawingContext dc)
        {
            if (ShowCustomGridLines)
            {
                double width = 0;
                foreach (var columnDefinition in ColumnDefinitions)
                {
                    width += columnDefinition.ActualWidth;
                }

                foreach (var rowDefinition in RowDefinitions)
                {
                    dc.DrawLine(new Pen(GridLineBrush, GridLineThickness), new Point(0, rowDefinition.Offset), new Point(width, rowDefinition.Offset));
                }

                foreach (var columnDefinition in ColumnDefinitions)
                {
                    dc.DrawLine(new Pen(GridLineBrush, GridLineThickness), new Point(columnDefinition.Offset, 0), new Point(columnDefinition.Offset, ActualHeight));
                }

                dc.DrawRectangle(Brushes.Transparent, new Pen(GridLineBrush, GridLineThickness), new Rect(0, 0, width, ActualHeight));
            }
            base.OnRender(dc);
        }
    }
}