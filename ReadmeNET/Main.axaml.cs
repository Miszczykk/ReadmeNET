using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Material.Icons;

namespace ReadmeNET;

public class BlockModel
{
    public required string Title { get; set; }

    public MaterialIconKind Icon { get; set; }

    //public ICommand RunSt
}

public partial class Main : UserControl
{
    public ObservableCollection<BlockModel> BlockCatalog { get; set; }
    public Main()
    {
        InitializeComponent();

        BlockCatalog = new ObservableCollection<BlockModel>
        {
            new BlockModel
            {
                Title = "Header",
                Icon = MaterialIconKind.FormatSize
            },
            new BlockModel
            {
                Title = "Text",
                Icon = MaterialIconKind.FormatAlignLeft
            },
            new BlockModel
            {
                Title = "Quote",
                Icon = MaterialIconKind.FormatQuoteClose
            },
            new BlockModel
            {
                Title = "Alert",
                Icon = MaterialIconKind.Alert
            },
            new BlockModel
            {
                Title = "Color",
                Icon = MaterialIconKind.Color
            },
            new BlockModel
            {
                Title = "List",
                Icon = MaterialIconKind.FormatListNumbered
            },
            new BlockModel
            {
                Title = "Table",
                Icon = MaterialIconKind.Table
            },
            new BlockModel
            {
                Title = "Collapse",
                Icon = MaterialIconKind.CollapseAll
            },
            new BlockModel
            {
                Title = "Image",
                Icon = MaterialIconKind.ImageOutline
            },
            new BlockModel
            {
                Title = "Code",
                Icon = MaterialIconKind.Console
            },
            new BlockModel
            {
                Title = "Custom Code",
                Icon = MaterialIconKind.CodeBraces
            },
        };
        this.DataContext = this;
    }
}