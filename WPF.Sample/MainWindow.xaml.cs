﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Common.Library;
using WPF.Sample.DataLayer;
using WPF.Sample.ViewModelLayer;

namespace WPF.Sample
{
  public partial class MainWindow : Window
  {
    #region Constructor
    public MainWindow()
    {
      InitializeComponent();

      // Connect to instance of the view model created by the XAML
      _viewModel = (MainWindowViewModel)this.Resources["viewModel"];

      // Get the original status message
      _originalMessage = _viewModel.StatusMessage;

      // Initialize the Message Broker Events
      MessageBroker.Instance.MessageReceived += Instance_MessageReceived;
    }
    #endregion

    #region Private variables
    // Main window's view model class
    private MainWindowViewModel _viewModel = null;
    // Hold the main window's original status message
    private string _originalMessage = string.Empty;
    #endregion

    #region Window_Loaded Event
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      // Call method to load resources application
      await LoadApplication();

      // Turn off informational message area
      _viewModel.ClearInfoMessages();
    }
    #endregion

    #region Instance_MessageReceived Event
    private void Instance_MessageReceived(object sender, MessageBrokerEventArgs e)
    {
      switch (e.MessageName) {
        case MessageBrokerMessages.DISPLAY_TIMEOUT_INFO_MESSAGE_TITLE:
          _viewModel.InfoMessageTitle = e.MessagePayload.ToString();
          _viewModel.CreateInfoMessageTimer();
          break;

        case MessageBrokerMessages.DISPLAY_TIMEOUT_INFO_MESSAGE:
          _viewModel.InfoMessage = e.MessagePayload.ToString();
          _viewModel.CreateInfoMessageTimer();
          break;

        case MessageBrokerMessages.DISPLAY_STATUS_MESSAGE:
          // Set new status message
          _viewModel.StatusMessage = e.MessagePayload.ToString();
          break;

        case MessageBrokerMessages.LOGIN_SUCCESS:
          _viewModel.UserEntity = (User)e.MessagePayload;
          _viewModel.LoginMenuHeader = "Logout " +
              _viewModel.UserEntity.UserName;
          break;

        case MessageBrokerMessages.LOGOUT:
          _viewModel.UserEntity.IsLoggedIn = false;
          _viewModel.LoginMenuHeader = "Login";
          break;

        case MessageBrokerMessages.CLOSE_USER_CONTROL:
          CloseUserControl();
          break;
      }
    }
    #endregion

    #region MenuItem_Click Event
    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
      MenuItem mnu = (MenuItem)sender;
      string cmd = string.Empty;

      // The Tag property contains a command 
      // or the name of a user control to load
      if (mnu.Tag != null) {
        cmd = mnu.Tag.ToString();
        if (cmd.Contains(".")) {
          // Display a user control
          LoadUserControl(cmd);
        }
        else {
          // Process special commands
          ProcessMenuCommands(cmd);
        }
      }
    }
    #endregion

    #region LoadUserControl Method
    private void LoadUserControl(string controlName)
    {
      Type ucType = null;
      UserControl uc = null;

      if (ShouldLoadUserControl(controlName)) {
        // Create a Type from controlName parameter
        ucType = Type.GetType(controlName);
        if (ucType == null) {
          MessageBox.Show("The Control: " + controlName
                           + " does not exist.");
        }
        else {
          // Close current user control in content area
          // NOTE: Optionally add current user control to a list 
          //       so you can restore it when you close the newly added one
          CloseUserControl();

          // Create an instance of this control
          uc = (UserControl)Activator.CreateInstance(ucType);
          if (uc != null) {
            // Display control in content area
            DisplayUserControl(uc);
          }
        }
      }
    }
    #endregion

    #region DisplayUserControl Method
    public void DisplayUserControl(UserControl uc)
    {
      // Add new user control to content area
      contentArea.Children.Add(uc);
    }
    #endregion   

    #region ProcessMenuCommands Method
    private void ProcessMenuCommands(string command)
    {
      switch (command.ToLower()) {
        case "exit":
          this.Close();
          break;

        case "login":
          if (_viewModel.UserEntity.IsLoggedIn) {
            // Logging out, so close any open windows
            CloseUserControl();
            // Reset the user object
            _viewModel.UserEntity = new User();
            // Make menu display Login
            _viewModel.LoginMenuHeader = "Login";
          }
          else {
            // Display the login screen
            LoadUserControl(
               "WPF.Sample.UserControls.LoginControl");
          }
          break;

        default:
          break;
      }
    }
    #endregion

    #region ShouldLoadUserControl Method
    private bool ShouldLoadUserControl(string controlName)
    {
      bool ret = true;

      // Make sure you don't reload a control already loaded.
      if (contentArea.Children.Count > 0) {
        if (((UserControl)contentArea.Children[0]).GetType().FullName == controlName) {
          ret = false;
        }
      }

      return ret;
    }
    #endregion

    #region CloseUserControl Method
    private void CloseUserControl()
    {
      // Remove current user control
      contentArea.Children.Clear();

      // Restore the original status message
      _viewModel.StatusMessage = _originalMessage;
    }
    #endregion

    #region LoadApplication Method
    public async Task LoadApplication()
    {
      _viewModel.InfoMessage = "Loading State Codes...";
      await Dispatcher.BeginInvoke(new Action(() => {
        _viewModel.LoadStateCodes();
      }), DispatcherPriority.Background);

      _viewModel.InfoMessage = "Loading Country Codes...";
      await Dispatcher.BeginInvoke(new Action(() => {
        _viewModel.LoadCountryCodes();
      }), DispatcherPriority.Background);

      _viewModel.InfoMessage = "Loading Employee Types...";
      await Dispatcher.BeginInvoke(new Action(() => {
        _viewModel.LoadEmployeeTypes();
      }), DispatcherPriority.Background);
    }
    #endregion
  }
}
