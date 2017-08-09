using System;
using System.Windows.Input;

namespace SecurityPlusUI.Model
{
    public class CommandBase : ICommand
    {
        private Action<object> execute;

        private Predicate<object> canExecute;
        
        public CommandBase(Action<object> execute)
           : this(execute, o => true)
        {
        }

        public CommandBase(Action<object> execute, Predicate<object> canExecute)
        {
            if (null == execute)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            if (null == canExecute)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
