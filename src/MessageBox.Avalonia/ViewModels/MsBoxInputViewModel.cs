using System;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using MessageBox.Avalonia.Views;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using MessageBox.Avalonia.Enums;

namespace MessageBox.Avalonia.ViewModels
{
    public class MsBoxInputViewModel : AbstractMsBoxViewModel
    {
        private readonly ToggleButton _passwordRevealBtn;
        private readonly MsBoxInputWindow _window;
        private string _inputText;
        private char? _passChar;
        public char? InitialPassChar { get; }

        public char? PassChar
        {
            get => _passChar;
            private set
            {
                _passChar = value;
                OnPropertyChanged();
            }
        }

        public MessageBoxInputParams.PasswordRevealModes PasswordRevealMode { get; }

        public bool IsPasswordRevealButtonVisible => InitialPassChar == '*' && PasswordRevealMode != MessageBoxInputParams.PasswordRevealModes.None;

        public string WatermarkText { get; }

        public bool Multiline { get; }
        // public ReactiveCommand<string, Unit> ButtonClickCommand { get; private set; }

        public IEnumerable<ButtonDefinition> ButtonDefinitions { get; }

        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged();
            }
        }

        public MsBoxInputViewModel(MessageBoxInputParams @params, MsBoxInputWindow msBoxInputWindow) : base(@params,@params.Icon)
        {
            _window = msBoxInputWindow;
            ButtonDefinitions = @params.ButtonDefinitions;
            InitialPassChar = PassChar = @params.IsPassword ? '*' : null;
            PasswordRevealMode = @params.PasswordRevealMode;
            WatermarkText = @params.WatermarkText;
            Multiline = @params.Multiline;

            // Make sure there are default buttons on dialog
            if (ButtonDefinitions is null)
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition {Name = "Confirm", IsDefault = true, Type = ButtonType.Colored},
                    new ButtonDefinition {Name = "Cancel", IsCancel = true}
                };
            }

            if (Multiline) // Fill if multi-line
            {
                var grid = _window.FindControl<Grid>("ContentGrid");
                grid.RowDefinitions[0].Height = GridLength.Parse("*");
            }
            
            _passwordRevealBtn = _window.FindControl<ToggleButton>("PasswordRevealBtn");

            //PointerPressedEvent
            _passwordRevealBtn.AddHandler(InputElement.PointerPressedEvent, (sender, e) =>
            {
                if (!IsPasswordRevealButtonVisible || _passChar is null) return;

                var pointer = e.GetCurrentPoint(_passwordRevealBtn);

                if ((pointer.Properties.IsLeftButtonPressed || pointer.Properties.IsRightButtonPressed) && PasswordRevealMode == MessageBoxInputParams.PasswordRevealModes.Hold 
                    || pointer.Properties.IsRightButtonPressed && PasswordRevealMode == MessageBoxInputParams.PasswordRevealModes.Both)
                {
                    PassChar = null;
                    _passwordRevealBtn.IsChecked = true;
                    e.Handled = true;
                }
            }, RoutingStrategies.Tunnel);

            // PointerReleasedEvent
            _passwordRevealBtn.AddHandler(InputElement.PointerReleasedEvent, (sender, e) =>
            {
                if (_passChar == '*' || 
                    !IsPasswordRevealButtonVisible ||
                    (PasswordRevealMode != MessageBoxInputParams.PasswordRevealModes.Hold &&
                     PasswordRevealMode != MessageBoxInputParams.PasswordRevealModes.Both)) return;
                if (PasswordRevealMode == MessageBoxInputParams.PasswordRevealModes.Both && e.InitialPressMouseButton != MouseButton.Right) return;

                PassChar = InitialPassChar;
                _passwordRevealBtn.IsChecked = false;
                e.Handled = true;
            }, RoutingStrategies.Tunnel);
        }
        
        public void ButtonClick(string parameter)
        {
            foreach (var bd in ButtonDefinitions)
            {
                if (parameter.Equals(bd.Name))
                {
                    _window.ButtonResult = bd.Name;
                    _window.MessageResult = InputText;
                    break;
                }
            }

            _window.Close();
            // Code for executing the command here.
        }

        public void PasswordRevealClick()
        {
            if (!IsPasswordRevealButtonVisible) return;
            switch (PasswordRevealMode)
            {
                case MessageBoxInputParams.PasswordRevealModes.Toggle:
                case MessageBoxInputParams.PasswordRevealModes.Both:
                    PassChar = _passwordRevealBtn.IsChecked.Value ? null : InitialPassChar;
                    break;
                case MessageBoxInputParams.PasswordRevealModes.Hold:
                    _passwordRevealBtn.IsChecked = false;
                    PassChar = InitialPassChar;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }
}