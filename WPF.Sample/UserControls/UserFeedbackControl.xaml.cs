using System.Windows.Controls;
using WPF.Sample.ViewModelLayer;

namespace WPF.Sample.UserControls
{
  public partial class UserFeedbackControl : UserControl
  {
    public UserFeedbackControl()
    {
      InitializeComponent();

      // Connect to instance of the view model created by the XAML
      _viewModel = (UserFeedbackViewModel)this.Resources["viewModel"];
    }

    private UserFeedbackViewModel _viewModel = null;

    private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      _viewModel.Close();
    }

    private void SendFeedbackButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      _viewModel.SendFeedback();
    }
  }
}
