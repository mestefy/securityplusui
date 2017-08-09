using System;
using System.Windows.Input;

using SecurityPlusCore;

namespace SecurityPlusUI.Model
{
    public class UserValidationContext : ContextBase
    {
        private string processPath;

        private ProcessOperationType operation;

        private ICommand allowCommand;

        private ICommand denyCommand;

        public UserValidationContext(ProcessOperationType operation, string processPath)
        {
            if (null == processPath)
            {
                throw new ArgumentNullException(nameof(processPath));
            }

            this.processPath = processPath;

            this.operation = operation;

            this.allowCommand = new CommandBase(o => this.Allow());
            this.denyCommand = new CommandBase(o => this.Deny());
        }

        public ProcessOperationResultType Result { get; private set; } = ProcessOperationResultType.Allow;

        public override ICommand AllowCommand
        {
            get { return this.allowCommand; }
        }

        public override ICommand DenyCommand
        {
            get { return this.denyCommand; }
        }

        public override string Message
        {
            get
            {
                return string.Format("Process {0} is executing operation: {1}.", this.processPath, this.operation.ToString().ToUpper());
            }
        }

        private void Allow()
        {
            this.Result = ProcessOperationResultType.Allow;
        }

        private void Deny()
        {
            this.Result = ProcessOperationResultType.Deny;
        }
    }
}
