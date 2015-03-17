using Junte.UI.WPF;
using Queue.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;

namespace Queue.UI.WPF.ViewModels
{
    public class KeyboardControlViewModel : ObservableObject
    {
        private const string Numbers = "1234567890";

        private string[] RusLetters =
        {
            "йцукенгшщзхъ",
            "фывапролджэ",
            "ячсмитьбю.,"
        };

        private string[] EngLetters =
        {
            "qwertyuiop",
            "asdfghjkl",
            "zxcvbnm.,"
        };

        private Language language;
        private bool isUpper = false;
        private string currentLanguage;
        private List<LetterKeyboardButton> letters = new List<LetterKeyboardButton>();

        public IList<KeyboardRow> Rows { get; set; }

        public ICommand KeyClickCommand { get; set; }

        public ICommand BackspaceCommand { get; set; }

        public ICommand KeyboardButtonClick { get; set; }

        public ICommand ToogleLanguageCommand { get; set; }

        public ICommand ToogleCapsCommand { get; set; }

        public string CurrentLanguage
        {
            get { return currentLanguage; }
            set { SetProperty(ref currentLanguage, value); }
        }

        public bool IsUpper
        {
            get { return isUpper; }
            set { SetProperty(ref isUpper, value); }
        }

        public event EventHandler<string> OnLetter = delegate { };

        public event EventHandler OnBackspace = delegate { };

        public KeyboardControlViewModel()
        {
            Rows = new List<KeyboardRow>();
            KeyClickCommand = new RelayCommand<KeyboardButton>(KeyClick);
            BackspaceCommand = new RelayCommand(Backspace);
            ToogleCapsCommand = new RelayCommand(ToogleCaps);
            ToogleLanguageCommand = new RelayCommand(ToogleLanguage);
            KeyboardButtonClick = new RelayCommand<string>(KeyboardButtonClicked);

            AddLetterKeyboardRow(RusLetters[0], Language.ru_RU);
            AddLetterKeyboardRow(RusLetters[1], Language.ru_RU);
            AddLetterKeyboardRow(RusLetters[2], Language.ru_RU);

            AddLetterKeyboardRow(EngLetters[0], Language.en_EN);
            AddLetterKeyboardRow(EngLetters[1], Language.en_EN);
            AddLetterKeyboardRow(EngLetters[2], Language.en_EN);

            AddKeyboardRow(Numbers);

            SetLanguage(CultureInfo.DefaultThreadCurrentUICulture.GetLanguage());
        }

        private void Backspace()
        {
            OnBackspace(null, null);
        }

        private void KeyboardButtonClicked(string val)
        {
            OnLetter(null, val);
        }

        private void KeyClick(KeyboardButton btn)
        {
            OnLetter(null, btn.Title);
        }

        private void ToogleLanguage()
        {
            SetLanguage(language == Language.ru_RU ? Language.en_EN : Language.ru_RU);
        }

        private void AddKeyboardRow(string line)
        {
            KeyboardRow row = new KeyboardRow();

            foreach (char ch in line)
            {
                row.Buttons.Add(new KeyboardButton() { Title = ch.ToString() });
            }

            Rows.Add(row);
        }

        private void AddLetterKeyboardRow(string line, Language lang)
        {
            KeyboardRow row = new KeyboardRow();

            foreach (char ch in line)
            {
                LetterKeyboardButton letter = new LetterKeyboardButton()
                {
                    Title = ch.ToString(),
                    Language = lang
                };

                letters.Add(letter);
                row.Buttons.Add(letter);
            }

            Rows.Add(row);
        }

        private void SetLanguage(Language lang)
        {
            if (lang == Language.zh_CN)
            {
                lang = Language.ru_RU;
            }

            language = lang;
            foreach (LetterKeyboardButton btn in letters)
            {
                btn.Visible = btn.Language == lang;
            }

            AdjustCurrentLanguage();
        }

        private void ToogleCaps()
        {
            SetUppercase(!IsUpper);
        }

        private void SetUppercase(bool val)
        {
            IsUpper = val;

            foreach (LetterKeyboardButton btn in letters)
            {
                btn.SetUppercase(IsUpper);
            }

            AdjustCurrentLanguage();
        }

        private void AdjustCurrentLanguage()
        {
            string str = Translater.Enum(language, "LanguageShort");
            if (IsUpper)
            {
                str = str.ToUpperInvariant();
            }

            CurrentLanguage = str;
        }
    }

    public class KeyboardRow
    {
        public IList<KeyboardButton> Buttons { get; set; }

        public KeyboardRow()
        {
            Buttons = new List<KeyboardButton>();
        }
    }

    public class KeyboardButton : ObservableObject
    {
        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
    }

    public class LetterKeyboardButton : KeyboardButton
    {
        private bool visible;

        public Language Language { get; set; }

        public bool Visible
        {
            get { return visible; }
            set { SetProperty(ref visible, value); }
        }

        public void SetUppercase(bool val)
        {
            Title = val ?
                Title.ToUpperInvariant() :
                Title.ToLowerInvariant();
        }
    }
}