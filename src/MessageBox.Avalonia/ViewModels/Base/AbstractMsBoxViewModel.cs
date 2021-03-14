using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using JetBrains.Annotations;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;

namespace MessageBox.Avalonia.ViewModels
{
    public abstract class AbstractMsBoxViewModel:INotifyPropertyChanged

    {
    public bool CanResize { get; }
    public bool HasHeader => !(string.IsNullOrEmpty(ContentHeader));
    public bool HasIcon => !(ImagePath is null);
    public FontFamily FontFamily { get; }
    public string ContentTitle { get; }
    public string ContentHeader { get; }
    public string ContentMessage { get; }
    public WindowIcon WindowIconPath { get; } = null;
    public Bitmap ImagePath { get; } = null;
    public int? MaxWidth { get; }

    public WindowStartupLocation LocationOfMyWindow { get; }

    public AbstractMsBoxViewModel(AbstractMessageBoxParams @params)
    {
        if (@params.Icon != Icon.None)
        {
            ImagePath = new Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>()
                .Open(new Uri(
                    $" avares://MessageBox.Avalonia/Assets/{@params.Icon.ToString().ToLowerInvariant()}.png")));
        }

        MaxWidth = @params.MaxWidth;
        CanResize = @params.CanResize;
        FontFamily = @params.FontFamily;
        ContentTitle = @params.ContentTitle;
        ContentHeader = @params.ContentHeader;
        ContentMessage = @params.ContentMessage;
        WindowIconPath = @params.WindowIcon;
        LocationOfMyWindow = @params.WindowStartupLocation;
    }

    public async Task Copy()
    {
        await AvaloniaLocator.Current.GetService<IClipboard>().SetTextAsync(ContentMessage);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    }
}