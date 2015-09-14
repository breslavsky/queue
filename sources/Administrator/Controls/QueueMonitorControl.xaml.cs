using Junte.Translation;
using Queue.Services.DTO;
using Queue.UI.WPF;
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
using Path = System.IO.Path;
using QIcons = Queue.UI.Common.Icons;
using QueueOperator = Queue.Services.DTO.QueuePlan.Operator;

namespace Queue.Administrator
{
    public partial class QueueMonitorControl : UserControl
    {
        #region fields

        private const byte BlockBoxWidth = 200;
        private const int HeartbeatTimeout = 30;
        private const int HorizontalMediumZoom = 250;
        private const byte HoursInDay = 24;
        private const byte MinutesInHour = 60;
        private const int SecondsInHour = SecondsInMinutes * MinutesInHour;
        private const byte SecondsInMinutes = 60;
        private const byte SmallBlockBoxWidth = 20;
        private const int VerticalMediumZoom = 100;

        private Dictionary<int, object> indexes;
        private QueuePlan queuePlan;

        #endregion fields

        #region properties

        public QueuePlan QueuePlan
        {
            get
            {
                return queuePlan;
            }
            set
            {
                queuePlan = value;
                if (value != null)
                {
                    ReloadQueuePlan();
                }
            }
        }

        #endregion properties

        public QueueMonitorControl()
        {
            InitializeComponent();

            indexes = new Dictionary<int, object>();
        }

        public event EventHandler<QueueMonitorEventArgs> OnClientRequestEdit;

        public event EventHandler<QueueMonitorEventArgs> OnOperatorLogin;

        public QueueMonitorControlOptions Options { get; set; }

        public static Border CreateBlockBox(FrameworkElement child, double width, double height, double x, double y, Color backgroundColor)
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

        public void Search(int number)
        {
            if (IsLoaded && queuePlan != null)
            {
                if (indexes.ContainsKey(number))
                {
                    if (horizontalZoom.Value != HorizontalMediumZoom || verticalZoom.Value != VerticalMediumZoom)
                    {
                        horizontalZoom.Value = HorizontalMediumZoom;
                        verticalZoom.Value = VerticalMediumZoom;
                        ReloadQueuePlan();
                    }

                    var block = (Border)indexes[number];
                    block.Background = Brushes.Red;
                    block.TopMost();

                    clientRequestsScrollViewer.ScrollToHorizontalOffset(block.Margin.Left);
                    clientRequestsScrollViewer.ScrollToVerticalOffset(block.Margin.Top);
                }
            }
        }

        private void monitorScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            operatorsScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
            timelineScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset);
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

        private void ReloadQueuePlan()
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

            operatorsCanvas.Width = BlockBoxWidth;
            operatorsCanvas.Height = height;

            clientRequestsCanvas.Width = (HoursInDay + 1) * SecondsInHour * SECOND_WIDTH;
            clientRequestsCanvas.Height = height;

            timelineCanvas.Width = clientRequestsCanvas.Width;

            for (byte Hour = 1; Hour <= HoursInDay; Hour++)
            {
                double x = Hour * SecondsInHour * SECOND_WIDTH;

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
                if (Options.HasFlag(QueueMonitorControlOptions.OperatorLogin))
                {
                    var button = new Button()
                    {
                        Height = 22,
                        Width = 25,
                        Margin = new Thickness(5, 0, 0, 0),
                        ToolTip = "Управление очередью",
                        Content = QIcons.operator16x16.ToWpfImage()
                    };

                    button.Click += (s, e) =>
                    {
                        if (OnOperatorLogin != null)
                        {
                            OnOperatorLogin(this, new QueueMonitorEventArgs()
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

                operatorsCanvas.Children.Add(CreateBlockBox(stackPanel, BlockBoxWidth, BLOCK_BOX_HEIGHT, 0, currentBlockY, Colors.WhiteSmoke));

                clientRequestsCanvas.Children.Add(new Line()
                {
                    X1 = 0,
                    X2 = HoursInDay * SecondsInHour,
                    Y1 = currentBlockY,
                    Y2 = currentBlockY,
                    StrokeThickness = 1,
                    Stroke = Brushes.Silver,
                    StrokeDashArray = new DoubleCollection(new double[] { 2, 4 })
                });

                var clientsRequestPlans = operatorPlan.ClientRequestPlans;

                foreach (var clientRequestPlan in clientsRequestPlans)
                {
                    var clientRequest = clientRequestPlan.ClientRequest;

                    textBlock = new TextBlock();
                    textBlock.Padding = new Thickness(5, 5, 5, 5);

                    textBlock.Inlines.Add(new Run(string.Format("[{0}] {1}", clientRequest.Number, Translater.Enum(clientRequest.Type)))
                    {
                        FontWeight = FontWeights.Bold
                    });
                    textBlock.Inlines.Add(new LineBreak());

                    var client = clientRequest.Client;
                    if (!string.IsNullOrWhiteSpace(client))
                    {
                        textBlock.Inlines.Add(client);
                        textBlock.Inlines.Add(new LineBreak());
                    }
                    textBlock.Inlines.Add(string.Format("{0:hh\\:mm\\:ss} - {1:hh\\:mm\\:ss}", clientRequestPlan.StartTime, clientRequestPlan.FinishTime));
                    textBlock.Inlines.Add(new LineBreak());

                    textBlock.Inlines.Add(Translater.Enum(clientRequest.State));

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
                    var block = CreateBlockBox(textBlock, width, BLOCK_BOX_HEIGHT, x1, currentBlockY, Color.FromRgb(color.R, color.G, color.B));
                    color = ColorTranslator.FromHtml(clientRequest.Color);
                    block.BorderBrush = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));

                    if (Options.HasFlag(QueueMonitorControlOptions.ClientRequestEdit))
                    {
                        block.Cursor = Cursors.Hand;

                        block.MouseDown += (s, e) =>
                        {
                            if (OnClientRequestEdit != null)
                            {
                                OnClientRequestEdit(this, new QueueMonitorEventArgs()
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
            textBlock.Inlines.Add(new Run("Нераспределенные") { FontWeight = FontWeights.Bold });

            currentBlockY = index * BLOCK_BOX_HEIGHT;
            operatorsCanvas.Children.Add(CreateBlockBox(textBlock, BlockBoxWidth, BLOCK_BOX_HEIGHT, 0, currentBlockY, Colors.LightPink));

            clientRequestsCanvas.Children.Add(new Line()
            {
                X1 = 0,
                X2 = HoursInDay * SecondsInHour,
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

                textBlock.Inlines.Add(new Run(string.Format("[{0}] {1}", clientRequest.Number, Translater.Enum(clientRequest.Type)))
                {
                    FontWeight = FontWeights.Bold
                });
                textBlock.Inlines.Add(new LineBreak());
                textBlock.Inlines.Add(new Run(clientRequest.RequestTime.ToString("hh\\:mm\\:ss")) { FontWeight = FontWeights.Bold });
                textBlock.Inlines.Add(new LineBreak());
                textBlock.Inlines.Add(new Run(clientRequest.Client));
                textBlock.Inlines.Add(new LineBreak());
                textBlock.Inlines.Add(new Run(notDistributedClientRequest.Reason));

                var block = CreateBlockBox(textBlock, SmallBlockBoxWidth, BLOCK_BOX_HEIGHT, index++ * SmallBlockBoxWidth + CurrentTimeLineX, currentBlockY, Colors.LightPink);
                var color = ColorTranslator.FromHtml(clientRequest.Color);
                block.BorderBrush = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));

                if (Options.HasFlag(QueueMonitorControlOptions.ClientRequestEdit))
                {
                    block.Cursor = Cursors.Hand;

                    block.MouseDown += (s, e) =>
                    {
                        if (OnClientRequestEdit != null)
                        {
                            OnClientRequestEdit(this, new QueueMonitorEventArgs()
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

        private void zoom_Updated(object sender, MouseButtonEventArgs e)
        {
            if (IsLoaded && queuePlan != null)
            {
                ReloadQueuePlan();
            }
        }
    }

    public class QueueMonitorEventArgs : EventArgs
    {
        public ClientRequest ClientRequest { get; set; }

        public bool IsChecked { get; set; }

        public QueueOperator Operator { get; set; }
    }
}