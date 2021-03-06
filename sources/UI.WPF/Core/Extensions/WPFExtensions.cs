﻿using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Bitmap = System.Drawing.Bitmap;
using Drawing = System.Drawing;

namespace Queue.UI.WPF
{
    public static class WPFExtensions
    {
        public static List<TChild> FindChildren<TChild>(this DependencyObject d) where TChild : DependencyObject
        {
            var children = new List<TChild>();
            var childCount = VisualTreeHelper.GetChildrenCount(d);

            for (int i = 0; i < childCount; i++)
            {
                var dependencyObject = VisualTreeHelper.GetChild(d, i);

                if (dependencyObject is TChild)
                {
                    children.Add(dependencyObject as TChild);
                }

                foreach (TChild child in dependencyObject.FindChildren<TChild>())
                {
                    children.Add(child);
                }
            }

            return children;
        }

        public static SolidColorBrush GetBrushForColor(this string color)
        {
            var c = Drawing.ColorTranslator.FromHtml(color);
            return new SolidColorBrush(Color.FromRgb(c.R, c.G, c.B));
        }

        public static Image ToWpfImage(this Bitmap bitmap)
        {
            var memoryStream = new MemoryStream();

            bitmap.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();

            return new Image() { Source = bitmapImage };
        }

        public static void FadeIn(this UIElement targetControl)
        {
            var fadeInAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(1)));
            Storyboard.SetTarget(fadeInAnimation, targetControl);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeInAnimation);
            storyboard.Begin();
        }

        public static void FadeOut(this UIElement targetControl)
        {
            var fadeInAnimation = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(1)));
            Storyboard.SetTarget(fadeInAnimation, targetControl);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeInAnimation);
            storyboard.Begin();
        }

        public static void Blur(this UIElement targetControl)
        {
            Blur(targetControl, 5);
        }

        public static void Blur(this UIElement targetControl, int radius)
        {
            var blurEffect = new BlurEffect();
            blurEffect.Radius = radius;
            targetControl.Effect = blurEffect;
        }

        public static void UnBlur(this UIElement targetControl)
        {
            targetControl.Effect = null;
        }

        public static void TopMost(this FrameworkElement targetControl)
        {
            var parent = targetControl.Parent as Panel;
            parent.Children.Remove(targetControl);
            parent.Children.Add(targetControl);
        }

        public static void SetXamlLanguage(CultureInfo culture)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                           new FrameworkPropertyMetadata(
                               XmlLanguage.GetLanguage(culture.IetfLanguageTag)
                           )
            );
        }
    }
}