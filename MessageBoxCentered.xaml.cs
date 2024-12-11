using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MessageBoxCentered.xaml
    /// </summary>
    public partial class MessageBoxCentered : Window
    {
        /// <summary>
        /// Which buttons are displayed on the dialog
        /// </summary>
        public enum BoxType
        {
            YesNo,
            Ok,
            OkCancel
        }

        /// <summary>
        /// Defines each buttons and which button was clicked on.
        /// </summary>
        public enum DialogButtonType
        {
            Ok,
            Cancel, 
            Yes,
            No,
            Unknown
        }

        private DialogButtonType Result { get; set; }

        private MessageBoxCentered() { }
        /// <summary>
        /// constructor for message box
        /// </summary>
        /// <param name="caption">Title or caption, typically Error or Info</param>
        /// <param name="message">The message to dispaly</param>
        /// <param name="type">What buttons to display</param>
        private MessageBoxCentered(string caption, string message, BoxType type)
        {
            InitializeComponent();
            Caption.Text= caption;
            Message.Text = message;

            ConfigureButton(ref ButtonC, DialogButtonType.Unknown, Visibility.Collapsed);
            ConfigureButton(ref ButtonL, DialogButtonType.Unknown, Visibility.Collapsed);
            ConfigureButton(ref ButtonR, DialogButtonType.Unknown, Visibility.Collapsed);

            switch (type)
            {
                case BoxType.Ok:
                    ConfigureButton(ref ButtonC, DialogButtonType.Ok, Visibility.Visible);
                    break;

                case BoxType.OkCancel:
                    ConfigureButton(ref ButtonL, DialogButtonType.Ok, Visibility.Visible);
                    ConfigureButton(ref ButtonC, DialogButtonType.Cancel, Visibility.Visible);
                    break;
                case BoxType.YesNo:
                    ConfigureButton(ref ButtonL, DialogButtonType.Yes, Visibility.Visible);
                    ConfigureButton(ref ButtonC, DialogButtonType.No, Visibility.Visible);
                    break;
            }
        }

        /// <summary>
        /// Handels button clicks.  Extracts button from sender and uses the tag
        /// property on the button to determine which button was clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonHandler(object sender, RoutedEventArgs e)
        {
            DialogButtonType buttonType = DialogButtonType.Unknown;
            if ((sender is Button button) && button.Tag is DialogButtonType type)
                buttonType = type;

            Result = buttonType;
            Close();

            return;
        }

        /// <summary>
        /// configures a button.  Used to make a button visible and to set the 
        /// text and tag of the button per the type parameter
        /// </summary>
        /// <param name="button"></param>
        /// <param name="type"></param>
        /// <param name="vis"></param>
        private static void ConfigureButton(ref Button button, DialogButtonType type, Visibility vis)
        {
            button.Tag = type;
            button.Content = type.ToString();
            button.Visibility = vis;
        }

        /// <summary>
        /// Centers the dialog over the main window of the current application.
        /// TODO:  adjust the window size and location to prevent the window from being off screen.
        /// </summary>
        private void ShowAndPosition()
        {
            Window mainW = Application.Current.MainWindow;
            Left = mainW.Left + 0.5 * mainW.Width - 0.5 * Width;
            Top = mainW.Top + 0.5 * mainW.Height - 0.5 * Height;

            ShowDialog();
        }

        public static DialogButtonType ShowDialog(string Caption, string Title, BoxType boxType)
        {
            MessageBoxCentered box = new(Caption, Title, boxType);
            box.ShowAndPosition();
            return box.Result;
        }
    }
}
