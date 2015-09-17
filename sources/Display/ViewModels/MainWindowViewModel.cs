using Junte.UI.WPF;
using System;
using System.ComponentModel;
using System.Reflection;
using WPFLocalizeExtension.Engine;

namespace Queue.Display.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public MainWindowViewModel()
        {
            LocalizeDictionary.Instance.PropertyChanged += Instance_PropertyChanged;

            UpdateTitle();
        }

        private void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Culture")
            {
                UpdateTitle();
            }
        }

        private void UpdateTitle()
        {
            string appName = (string)LocalizeDictionary.Instance.GetLocalizedObject(typeof(MainWindowViewModel).Assembly.FullName,
                                                                 "Strings",
                                                                 "AppName",
                                                                 LocalizeDictionary.Instance.Culture);

            Version version = Assembly.GetEntryAssembly().GetName().Version;

            Title = String.Format("{0} ({1})", appName, version);
        }
    }
}