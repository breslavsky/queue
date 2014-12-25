using System;
using System.Windows.Input;

namespace Queue.UI.WPF
{
    public class RelayCommand<T> : ICommand
    {
        private static bool CanExecute(T parameter)
        {
            return true;
        }

        private readonly Action<T> execute;
        private readonly Func<T, bool> canExecute;

        public RelayCommand(Action<T> execute,
            Func<T, bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute ?? CanExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute(TranslateParameter(parameter));
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public void Execute(object parameter)
        {
            execute(TranslateParameter(parameter));
        }

        private T TranslateParameter(object parameter)
        {
            return parameter != null && typeof(T).IsEnum ? (T)Enum.Parse(typeof(T), (string)parameter) : (T)parameter;
        }
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute, Func<bool> canExecute = null)
            : base(obj => execute(), (canExecute == null ? null : new Func<object, bool>(obj => canExecute())))
        {
        }
    }
}