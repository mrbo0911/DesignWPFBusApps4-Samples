using System.Windows;
using System.Windows.Controls;
using WPF.Sample.ViewModelLayer;

namespace WPF.Sample.UserControls
{
  public partial class UserMaintenanceDetailControl : UserControl
  {
    public UserMaintenanceDetailControl()
    {
      InitializeComponent();
    }

    private UserMaintenanceViewModel _viewModel;

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      _viewModel = (UserMaintenanceViewModel)this.DataContext;
    }

    private void UndoButton_Click(object sender, RoutedEventArgs e)
    {
      _viewModel.CancelEdit();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      _viewModel.Save();
    }
  }
}
