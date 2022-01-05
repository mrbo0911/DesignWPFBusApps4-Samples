using System.Windows.Controls;
using WPF.Sample.ViewModelLayer;

namespace WPF.Sample.UserControls
{
  public partial class UserMaintenanceControl : UserControl
  {
    public UserMaintenanceControl()
    {
      InitializeComponent();

      // Connect to instance of the view model created by the XAML
      _viewModel = (UserMaintenanceViewModel)this.Resources["viewModel"];
    }

    private UserMaintenanceViewModel _viewModel = null;

    private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      _viewModel.Close();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
      _viewModel.LoadUsers();
    }

    private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      _viewModel.BeginEdit(true);
    }

    private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      _viewModel.BeginEdit(false);
    }

    private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      listControl.DeleteUser();
    }

    private void UndoButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      _viewModel.CancelEdit();
    }

    private void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      _viewModel.Save();
    }
  }
}
