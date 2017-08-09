using System;
using System.Threading;

using SecurityPlusCore;
using SecurityPlusUI.Services;

namespace SecurityPlusUI.Model
{
    public class UserValidationCommand : IProcessValidationCommand
    {
        private Thread staThread;

        private ManualResetEventSlim synchronizationEvent;

        public UserValidationCommand()
        {
            this.Enabled = true;
            this.staThread = new Thread(this.RunOnThread);
            this.staThread.SetApartmentState(ApartmentState.STA);
            this.synchronizationEvent = new ManualResetEventSlim();
        }

        public bool Enabled { get; set; }

        public string Name { get; set; } = @"User validation";

        public ProcessOperationResultType Validate(ProcessOperationType operation, string processPath)
        {
            if (!this.Enabled)
            {
                return ProcessOperationResultType.Allow;
            }

            var context = new UserValidationContext(operation, processPath);

            var result = ProcessOperationResultType.Allow;

            try
            {
                if (ApartmentState.STA == Thread.CurrentThread.GetApartmentState())
                {
                    this.RunOnThread(context);
                }
                else
                {
                    this.staThread.Start(context);
                    this.synchronizationEvent.Wait();
                }
            }
            finally
            {
                result = context.Result;
            }

            return result;
        }

        private void RunOnThread(object userValidationContext)
        {
            if (null == userValidationContext)
            {
                throw new ArgumentNullException(nameof(userValidationContext));
            }

            var context = userValidationContext as UserValidationContext;

            NotificationService.DisplayWindows(context);

            this.synchronizationEvent.Set();
            this.synchronizationEvent.Reset();
        }
    }
}
