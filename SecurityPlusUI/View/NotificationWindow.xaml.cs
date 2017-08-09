using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using SecurityPlusUI.Model;

namespace SecurityPlusUI.View
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        private DoubleAnimation animation;

        private Storyboard storyboard;

        public NotificationWindow(IContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException(nameof(context));
            }

            InitializeComponent();

            this.Left = SystemParameters.WorkArea.Right - this.Width - 10;
            this.Top = SystemParameters.WorkArea.Bottom - this.Height - 10;
            this.DataContext = context;

            var interval = TimeSpan.FromSeconds(30);
            this.InitializeStoryboard(interval);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.storyboard.Pause();
            this.storyboard.Seek(TimeSpan.FromSeconds(0));
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.storyboard.Begin();
        }
        
        private void InitializeStoryboard(TimeSpan interval)
        {
            this.animation = new DoubleAnimation(0, interval);

            this.storyboard = new Storyboard();
            this.storyboard.Children.Add(this.animation);
            this.storyboard.Duration = interval;
            this.storyboard.Completed += (s, e) => this.Close();

            Storyboard.SetTarget(this.animation, this);

            Storyboard.SetTargetProperty(this.animation, new PropertyPath("Opacity"));

            this.storyboard.Begin();
        }

        #region Code for timer initialization. If it will be needed someday
        /*private void InitializeTimer(TimeSpan interval)
        {
            this.timer = new DispatcherTimer();
            this.timer.Interval = interval;
            this.timer.Tick += this.TimerTick;
            this.timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            var timer = sender as DispatcherTimer;
            timer.Stop();
            timer.Tick -= TimerTick;
            this.Close();
        }*/
        #endregion

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
