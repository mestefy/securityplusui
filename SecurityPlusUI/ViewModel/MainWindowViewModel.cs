using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Data;
using SecurityPlusCore;

using SecurityPlusUI.Model;

namespace SecurityPlusUI.ViewModel
{
    public class MainWindowViewModel
    {
        private Client client;

        private readonly object Locker = new object();

        private ExternalCommandsManager commandsManager;

        public ObservableCollection<IProcessValidationCommand> ValidationCommands { get; set; }

        public ObservableQueue<ProcessEvent> ProcessEvents { get; set; }

        public ICommand StartCommand => new CommandBase(o => this.Start());

        public ICommand RefreshRulesCommand => new CommandBase(o => this.RefreshRules());

        public ICommand ExitCommand => new CommandBase(o => this.Exit());

        public MainWindowViewModel()
        {
            this.client = new Client(new IPEndPoint(IPAddress.Parse(@"127.0.0.1"), 40007));
            this.commandsManager = new ExternalCommandsManager();
            this.ValidationCommands = new ObservableCollection<IProcessValidationCommand>();
            this.ProcessEvents = new ObservableQueue<ProcessEvent>(100);

            this.InitializeCommands();

            // For collection synchronization WPF
            BindingOperations.EnableCollectionSynchronization(this.ProcessEvents, this.Locker);
            this.client.RegisterPostProcessValidationCallback((sender, args) => this.AddToQueue(args.Item1, args.Item2));
        }

        public void Start()
        {
            this.client.Start();
        }

        public void RefreshRules()
        {
            /*
            BindingOperations.EnableCollectionSynchronization(this.ValidationCommands, this.Locker);

            Task.Run(() =>
            {

                for (var index = 0; index < int.MaxValue / 2000; index++)
                {
                    lock (this.Locker)
                    {
                        this.ValidationCommands.Add(new UserValidationCommand() { Name = "UUUU" });
                    }

                    // Task.Delay(10);
                }
            });

            Task.Run(() =>
            {
                for (var index = 0; index < int.MaxValue / 2000; index++)
                {
                    lock (this.Locker)
                    {
                        this.ValidationCommands.Add(new UserValidationCommand() { Name = "VVVV" });
                    }

                    // Task.Delay(10);
                }
            });*/

            // this.InitializeCommands();            
        }

        public void Exit()
        {
            this.client.Stop();
            Environment.Exit(0);
        }

        private void InitializeCommands()
        {
            this.client.ClearRegisteredCommands();
            this.ValidationCommands.Clear();
            // this.ValidationCommands.Add(new UserValidationCommand());
            // this.client.RegisterValidationCommand(this.ValidationCommands.First());

            var commands = this.commandsManager.GetExternalValidationCommands(Environment.CurrentDirectory);
            foreach (var command in commands)
            {
                this.ValidationCommands.Add(command);
                this.client.RegisterValidationCommand(command);
            }
        }

        private void AddToQueue(ProcessValidationRequest request, ProcessValidationResponse response)
        {
            var processEvent = new ProcessEvent(request, response);
            lock(this.Locker)
            {
                this.ProcessEvents.Enqueue(processEvent);
            }
        }
    }
}
