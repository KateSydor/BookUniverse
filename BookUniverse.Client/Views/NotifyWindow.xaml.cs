namespace BookUniverse.Client
{
    using System.Windows;
    /// <summary>
    /// Interaction logic for NotifyWindow.xaml.
    /// </summary>
    public partial class NotifyWindow : Window
    {
        public NotifyWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        public void ShowNotification(string message)
        {
            NotificationText.Text = message;
            Show();
        }
    }
}
