using System.Windows;
using System.Windows.Input;

namespace SecurityPlusUI.Model
{
    public class ContextBase : IContext
    {
        public virtual ICommand AllowCommand { get; } = new CommandBase(o => MessageBox.Show("Allow"));
        
        public virtual ICommand DenyCommand { get; } = new CommandBase(o => MessageBox.Show("Deny"));

        public virtual string Message { get; } = "Default message";

        public virtual string Title { get; } = "Default title";

        public virtual NotificationType Type { get; } = NotificationType.Action;
    }
}
