using System.Windows.Input;

namespace SecurityPlusUI.Model
{
    public interface IContext
    {
        string Title { get; }

        string Message { get; }

        NotificationType Type { get; }

        ICommand AllowCommand { get; }

        ICommand DenyCommand { get; }
    } 
}
