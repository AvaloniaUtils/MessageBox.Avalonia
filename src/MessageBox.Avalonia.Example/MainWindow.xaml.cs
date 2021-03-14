using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using MessageBoxAvaloniaEnums = MessageBox.Avalonia.Enums;

namespace MessageBox.Avalonia.Example
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        public async void MsBoxStandard_Click(object sender, RoutedEventArgs e)
        {
           
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("title", "Are you sure you want clear all the 1 profiles?", MessageBoxAvaloniaEnums.ButtonEnum.Ok, MessageBoxAvaloniaEnums.Icon.Battery);
            var r = await messageBoxStandardWindow.ShowDialog(this);
        }

        public async void MsBoxCustom_Click(object sender, RoutedEventArgs e)
        {

            var messageBoxCustomWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    Style = Style.UbuntuLinux,
                    ContentMessage = "支持FontFamily",
                    FontFamily = "Microsoft YaHei,Simsun",
                    ButtonDefinitions = new[] {
                        new ButtonDefinition {Name = "My"},
                        new ButtonDefinition {Name = "Buttons", Type = ButtonType.Colored}
                    },
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                });
            var r = await messageBoxCustomWindow.ShowDialog(this);
        }

        public async void MsBoxHyperlink_Click(object sender, RoutedEventArgs e)
        {

            var messageBoxHyperlinkWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxHyperlinkWindow(new MessageBoxHyperlinkParams()
                {
                    CanResize = true,
                    Style = MessageBoxAvaloniaEnums.Style.MacOs,
                    HyperlinkContentProvider = new[]{
                        new HyperlinkContent { Alias = "dedede         ", Url = "https://avaloniaui.net/docs/styles/styles" },
                        new HyperlinkContent { Alias="edvyydebbvydebvyed         "},
                        new HyperlinkContent { Url= "https://avaloniaui.net/docs/styles/styles" }
                    },
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ButtonDefinitions = MessageBoxAvaloniaEnums.ButtonEnum.Ok,
                    });
            var r = await messageBoxHyperlinkWindow.ShowDialog(this);
        }

        public async void MsBoxInput_Click(object sender, RoutedEventArgs e)
        {

            var messageBoxInputWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxInputWindow(new MessageBoxInputParams
                {
                    Style = Style.UbuntuLinux,
                    ContentMessage = "Password",
                    IsPassword = true,
                    ButtonDefinitions = new[] {
                        new ButtonDefinition {Name = "Cancel"},
                        new ButtonDefinition {Name = "Confirm", Type = ButtonType.Colored}
                    },
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                });
            var r = await messageBoxInputWindow.ShowDialog(this);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
