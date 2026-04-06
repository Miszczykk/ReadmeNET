using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReadmeNET.Views;
using System;

namespace ReadmeNET;

public partial class Home : UserControl
{
    public Home()
    {
        InitializeComponent();
    }

    private void NavigateToMain(object? sender, RoutedEventArgs e)
    {
        var window = TopLevel.GetTopLevel(this) as Window;
        if(window != null)
        {
            window.Content = new Main();
        }
    }
}