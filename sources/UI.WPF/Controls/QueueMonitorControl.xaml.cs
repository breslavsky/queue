using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;
using ClientRequest = Queue.Services.DTO.QueuePlan.ClientRequest;
using Color = System.Windows.Media.Color;
using Operator = Queue.Services.DTO.QueuePlan.Operator;
using Path = System.IO.Path;
using QIcons = Queue.UI.Common.Icons;
using Translation = Queue.Model.Common.Translation;
using WindowState = Xceed.Wpf.Toolkit.WindowState;

namespace Queue.UI.WPF
{
    public delegate void MonitorEventHandler(object sender, QueueMonitorEventArgs e);

    public partial class QueueMonitorControl : UserControl
    {
        private const byte BLOCK_BOX_WIDTH = 200;
        private const byte SMALL_BLOCK_BOX_WIDTH = 20;
        private const byte TOTAL_HOURS = 24;
        private const byte MINUTES_IN_HOUR = 60;
        private const byte SECONDS_IN_MINUTES = 60;
        private const int SECONDS_IN_HOUR = SECONDS_IN_MINUTES * MINUTES_IN_HOUR;
        private const int HEARTBIT_TIMEOUT = 30;

        private const int HORIZONTAL_MEDIUM_ZOOM = 250;
        private const int VERTICAL_MEDIUM_ZOOM = 100;

        private QueuePlan queuePlan;

        private Dictionary<int, object> indexes;

        public QueueMonitorControl()
        {
            InitializeComponent();

            indexes = new Dictionary<int, object>();
        }

        public event MonitorEventHandler OnClientRequestEdit
        {
            add { clientRequestEditHandler += value; }
            remove { clientRequestEditHandler -= value; }
        }

        public event MonitorEventHandler OnOperatorLogin
        {
            add { operatorLoginHandler += value; }
            remove { operatorLoginHandler -= value; }
        }

        private event MonitorEventHandler clientRequestEditHandler;

        private event MonitorEventHandler operatorLoginHandler;

        public QueueMonitorControlOptions Options { get; set; }

        public static Border createBlockBox(FrameworkElement child, double width, double height, double x, double y, Color backgroundColor)
        {
            var border = new Border()
            {
                BorderThickness = new Thickness(1, 1, 1, 1),
                BorderBrush = Brushes.Silver,
                Background = new SolidColorBrush(backgroundColor),

                Width = width,
                Height = height,
                Margin = new Thickness(x, y, 0, 0),
                Child = child
            };

            border.MouseEnter += (s, e) =>
            {
                border.Width = Math.Max(width, child.ActualWidth);
                border.Height = Math.Max(height, child.ActualHeight);
                Panel parent = (Panel)border.Parent;
                border.TopMost();
            };

            border.MouseLeave += (s, e) =>
            {
                border.Width = width;
                border.Height = height;
            };

            return border;
        }

        public void LoadQueuePlan(QueuePlan queuePlan)
        {
            this.queuePlan = queuePlan;
            reloadQueuePlan();
        }

        public void Search(int number)
        {
            if (IsLoaded && queuePlan != null)
            {
                if (indexes.ContainsKey(number))
                {
                    if (horizontalZoom.Value != HORIZONTAL_MEDIUM_ZOOM || verticalZoom.Value != VERTICAL_MEDIUM_ZOOM)
                    {
                        horizontalZoom.Value = HORIZONTAL_MEDIUM_ZOOM;
                        verticalZoom.Value = VERTICAL_MEDIUM_ZOOM;
                        reloadQueuePlan();
                    }

                    var block = (Border)indexes[number];
                    block.Background = Brushes.Red;
                    block.TopMost();

                    clientRequestsScrollViewer.ScrollToHorizontalOffset(block.Margin.Left);
                    clientRequestsScrollViewer.ScrollToVerticalOffset(block.Margin.Top);
                }
            }
        }

        private void reloadQueuePlan()
        {
            double SECOND_WIDTH = horizontalZoom.Value / 1000;
            double BLOCK_BOX_HEIGHT = verticalZoom.Value;

            indexes.Clear();

            if (timelineCanvas.Children.Count > 0)
            {
                timelineCanvas.Children.Clear();
            }

            if (operatorsCanvas.Children.Count > 0)
            {
                operatorsCanvas.Children.Clear();
            }

            if (clientRequestsCanvas.Children.Count > 0)
            {
                clientRequestsCanvas.Children.Clear();
            }

            var operatorsPlans = queuePlan.OperatorsPlans;

            double height = BLOCK_BOX_HEIGHT * (operatorsPlans.Length + 1) + timelineCanvas.Height;

            operatorsCanvas.Width = BLOCK_BOX_WIDTH;
            operatorsCanvas.Height = height;

            clientRequestsCanvas.Width = (TOTAL_HOURS + 1) * SECONDS_IN_HOUR * SECOND_WIDTH;
            clientRequestsCanvas.Height = height;

            timelineCanvas.Width = clientRequestsCanvas.Width;

            for (byte Hour = 1; Hour <= TOTAL_HOURS; Hour++)
            {
                double x = Hour * SECONDS_IN_HOUR * SECOND_WIDTH;

                timelineCanvas.Children.Add(new Line()
                {
                    X1 = x,
                    X2 = x,
                    Y1 = 20,
                    Y2 = 35,
                    StrokeThickness = 0.5,
                    Stroke = Brushes.Black
                });

                timelineCanvas.Children.Add(new TextBlock()
                {
                    Text = string.Format("{0}:00", Hour),
                    Margin = new Thickness(x - 10, 5, 0, 0)
                });

                clientRequestsCanvas.Children.Add(new Line()
                {
                    X1 = x,
                    X2 = x,
                    Y1 = 0,
                    Y2 = operatorsPlans.Length * BLOCK_BOX_HEIGHT,
                    StrokeThickness = 1,
                    Stroke = Brushes.LightGray,
                    StrokeDashArray = new DoubleCollection(new double[] { 2, 4 })
                });
            }

            var queuePlanTime = queuePlan.PlanTime;

            double currentBlockY;
            int index = 0;

            TextBlock textBlock;

            foreach (var operatorPlan in operatorsPlans)
            {
                var queueOperator = operatorPlan.Operator;

                var dockPanel = new DockPanel();

                var button = new Button()
                {
                    Height = 22,
                    Width = 25,
                    ToolTip = "Список запросов",
                    Content = QIcons.clientRequests16x16.ToWpfImage()
                };

                var clientsRequestPlans = operatorPlan.ClientRequestPlans;

                button.Click += (s, e) =>
                {
                    clientRequestsChildWindow.WindowState = WindowState.Open;
                    clientRequestsDataGrid.ItemsSource = clientsRequestPlans;
                };

                dockPanel.Children.Add(button);

                if (Options.HasFlag(QueueMonitorControlOptions.OperatorLogin))
                {
                    button = new Button()
                    {
                        Height = 22,
                        Width = 25,
                        Margin = new Thickness(5, 0, 0, 0),
                        ToolTip = "Управление очередью",
                        Content = QIcons.operator16x16.ToWpfImage()
                    };

                    button.Click += (s, e) =>
                    {
                        if (operatorLoginHandler != null)
                        {
                            operatorLoginHandler(this, new QueueMonitorEventArgs()
                            {
                                Operator = queueOperator
                            });
                        }
                    };

                    dockPanel.Children.Add(button);
                }

                var border = new Border()
                {
                    Height = 16,
                    Width = 16,
                    Child = (queueOperator.Online ? QIcons.online16x16 : QIcons.offline16x16).ToWpfImage()
                };
                dockPanel.Children.Add(border);

                textBlock = new TextBlock()
                {
                    Margin = new Thickness(0, 5, 0, 0)
                };
                textBlock.Inlines.Add(new Run(queueOperator.ToString()) { FontWeight = FontWeights.Bold });
                dockPanel.Children.Add(textBlock);

                var stackPanel = new StackPanel()
                {
                    Margin = new Thickness(5, 5, 5, 5)
                };
                stackPanel.Children.Add(dockPanel);

                if (!string.IsNullOrWhiteSpace(queueOperator.Workplace))
                {
                    textBlock = new TextBlock()
                    {
                        Margin = new Thickness(0, 5, 0, 0),
                        FontSize = 14,
                        FontWeight = FontWeights.Bold
                    };

                    textBlock.Inlines.Add(new Run(queueOperator.Workplace));
                    stackPanel.Children.Add(textBlock);
                }

                currentBlockY = index * BLOCK_BOX_HEIGHT;

                operatorsCanvas.Children.Add(createBlockBox(stackPanel, BLOCK_BOX_WIDTH, BLOCK_BOX_HEIGHT, 0, currentBlockY, Colors.WhiteSmoke));

                clientRequestsCanvas.Children.Add(new Line()
                {
                    X1 = 0,
                    X2 = TOTAL_HOURS * SECONDS_IN_HOUR,
                    Y1 = currentBlockY,
                    Y2 = currentBlockY,
                    StrokeThickness = 1,
                    Stroke = Brushes.Silver,
                    StrokeDashArray = new DoubleCollection(new double[] { 2, 4 })
                });

                foreach (var clientRequestPlan in clientsRequestPlans)
                {
                    var clientRequest = clientRequestPlan.ClientRequest;

                    textBlock = new TextBlock();
                    textBlock.Padding = new Thickness(5, 5, 5, 5);

                    var translation = Translation.ClientRequestType.ResourceManager;

                    textBlock.Inlines.Add(new Run(string.Format("[{0}] {1}", clientRequest.Number, translation.GetString(clientRequest.Type.ToString()))) { FontWeight = FontWeights.Bold });
                    textBlock.Inlines.Add(new LineBreak());

                    var client = clientRequest.Client;
                    if (!string.IsNullOrWhiteSpace(client))
                    {
                        textBlock.Inlines.Add(client);
                        textBlock.Inlines.Add(new LineBreak());
                    }
                    textBlock.Inlines.Add(string.Format("{0:hh\\:mm\\:ss} - {1:hh\\:mm\\:ss}", clientRequestPlan.StartTime, clientRequestPlan.FinishTime));
                    textBlock.Inlines.Add(new LineBreak());

                    translation = Translation.ClientRequestState.ResourceManager;
                    textBlock.Inlines.Add(translation.GetString(clientRequest.State.ToString()));

                    if (queuePlanTime > TimeSpan.Zero)
                    {
                        var renderStartTime = clientRequestPlan.StartTime;

                        var waitingTime = renderStartTime.Subtract(queuePlanTime);
                        if (waitingTime > TimeSpan.Zero)
                        {
                            textBlock.Inlines.Add(new LineBreak());
                            textBlock.Inlines.Add(new Run(string.Format("Время ожидания: {0} ч. {1} мин.", waitingTime.Hours, waitingTime.Minutes)));
                        }
                    }

                    var RenderStartTime = clientRequestPlan.StartTime;
                    double x1 = RenderStartTime.TotalSeconds * SECOND_WIDTH;
                    var RenderFinishTime = clientRequestPlan.FinishTime;
                    double x2 = RenderFinishTime.TotalSeconds * SECOND_WIDTH;
                    int width = Math.Max(20, (int)(x2 - x1));

                    var color = ColorTranslator.FromHtml(clientRequest.Color);
                    var block = createBlockBox(textBlock, width, BLOCK_BOX_HEIGHT, x1, currentBlockY, Color.FromRgb(color.R, color.G, color.B));
                    color = ColorTranslator.FromHtml(clientRequest.Color);
                    block.BorderBrush = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));

                    if (Options.HasFlag(QueueMonitorControlOptions.ClientRequestEdit))
                    {
                        block.Cursor = Cursors.Hand;

                        block.MouseDown += (s, e) =>
                        {
                            if (clientRequestEditHandler != null)
                            {
                                clientRequestEditHandler(this, new QueueMonitorEventArgs()
                                {
                                    ClientRequest = clientRequest
                                });
                            }
                        };
                    }

                    clientRequestsCanvas.Children.Add(block);

                    if (!indexes.ContainsKey(clientRequest.Number))
                    {
                        indexes.Add(clientRequest.Number, block);
                    }
                }

                index++;
            }

            textBlock = new TextBlock()
            {
                Padding = new Thickness(5, 5, 5, 5)
            };
            textBlock.Inlines.Add(new Run("Не распределенные") { FontWeight = FontWeights.Bold });

            currentBlockY = index * BLOCK_BOX_HEIGHT;
            operatorsCanvas.Children.Add(createBlockBox(textBlock, BLOCK_BOX_WIDTH, BLOCK_BOX_HEIGHT, 0, currentBlockY, Colors.LightPink));

            clientRequestsCanvas.Children.Add(new Line()
            {
                X1 = 0,
                X2 = TOTAL_HOURS * SECONDS_IN_HOUR,
                Y1 = currentBlockY,
                Y2 = currentBlockY,
                StrokeThickness = 1,
                Stroke = Brushes.Silver
            });

            var notDistributedQueueClientsRequests = queuePlan.NotDistributedClientRequests;

            double CurrentTimeLineX = queuePlanTime.TotalSeconds * SECOND_WIDTH;
            index = 0;

            foreach (var notDistributedClientRequest in notDistributedQueueClientsRequests)
            {
                var clientRequest = notDistributedClientRequest.ClientRequest;

                textBlock = new TextBlock();
                textBlock.Padding = new Thickness(5, 5, 5, 5);

                var translation = Translation.ClientRequestType.ResourceManager;

                textBlock.Inlines.Add(new Run(string.Format("[{0}] {1}", clientRequest.Number, translation.GetString(clientRequest.Type.ToString()))) { FontWeight = FontWeights.Bold });
                textBlock.Inlines.Add(new LineBreak());
                textBlock.Inlines.Add(new Run(clientRequest.RequestTime.ToString("hh\\:mm\\:ss")) { FontWeight = FontWeights.Bold });
                textBlock.Inlines.Add(new LineBreak());
                textBlock.Inlines.Add(new Run(clientRequest.Client));
                textBlock.Inlines.Add(new LineBreak());
                textBlock.Inlines.Add(new Run(notDistributedClientRequest.Reason));

                var block = createBlockBox(textBlock, SMALL_BLOCK_BOX_WIDTH, BLOCK_BOX_HEIGHT, index++ * SMALL_BLOCK_BOX_WIDTH + CurrentTimeLineX, currentBlockY, Colors.LightPink);
                var color = ColorTranslator.FromHtml(clientRequest.Color);
                block.BorderBrush = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));

                if (Options.HasFlag(QueueMonitorControlOptions.ClientRequestEdit))
                {
                    block.Cursor = Cursors.Hand;

                    block.MouseDown += (s, e) =>
                    {
                        if (clientRequestEditHandler != null)
                        {
                            clientRequestEditHandler(this, new QueueMonitorEventArgs()
                            {
                                ClientRequest = clientRequest
                            });
                        }
                    };
                }

                clientRequestsCanvas.Children.Add(block);

                if (!indexes.ContainsKey(clientRequest.Number))
                {
                    indexes.Add(clientRequest.Number, block);
                }
            }

            if (CurrentTimeLineX > 0)
            {
                clientRequestsCanvas.Children.Add(new Line()
                {
                    X1 = CurrentTimeLineX,
                    X2 = CurrentTimeLineX,
                    Y1 = 0,
                    Y2 = operatorsPlans.Length * BLOCK_BOX_HEIGHT + BLOCK_BOX_HEIGHT,
                    StrokeThickness = 1,
                    Stroke = Brushes.Red
                });

                timelineCanvas.Children.Add(new TextBlock()
                {
                    Text = queuePlanTime.ToString("hh\\:mm\\:ss"),
                    Foreground = Brushes.Red,
                    Margin = new Thickness(CurrentTimeLineX - 20, 35, 0, 0)
                });
            }
        }

        private void monitorScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            operatorsScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
            timelineScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        private void zoomUpdated(object sender, MouseButtonEventArgs e)
        {
            if (IsLoaded && queuePlan != null)
            {
                reloadQueuePlan();
            }
        }

        private void queueReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (queuePlan != null && queuePlan.Report != null)
            {
                string file = Path.GetTempFileName() + ".txt";
                File.WriteAllLines(file, queuePlan.Report);
                Process.Start(file);
            }
        }
    }

    public class QueueMonitorEventArgs : EventArgs
    {
        public Operator Operator { get; set; }

        public ClientRequest ClientRequest { get; set; }

        public bool IsChecked { get; set; }
    }
}