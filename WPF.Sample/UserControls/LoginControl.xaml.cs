using System.Windows;
using System.Windows.Controls;
using WPF.Sample.ViewModelLayer;

namespace WPF.Sample.UserControls
{
  public partial class LoginControl : UserControl
  {
    public LoginControl()
    {
      InitializeComponent();

      // Connect to instance of the view model created by the XAML
      _viewModel = (LoginViewModel)this.Resources["viewModel"];
    }

    // Login view model class
    private LoginViewModel _viewModel = null;
    
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      _viewModel.Close();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
      // Add the Password manually because data binding does not work
      _viewModel.Entity.Password = txtPassword.Password;

      _viewModel.Login();
    }
  }
}
