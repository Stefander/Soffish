using System;
using System.Windows.Input;

namespace Common.Framework
{
    public class DelegateCommand<T> : ICommand
    {
        public Action<T> _execute { get; private set; }
        public Predicate<T> _canExecute { get; private set; }
        public string Label { get; private set; }

        public DelegateCommand(Action<T> execute, Predicate<T> canExecute, string label)
        {
            _execute = execute;
            _canExecute = canExecute;
            Label = label;
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }
        
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }
    }
}
