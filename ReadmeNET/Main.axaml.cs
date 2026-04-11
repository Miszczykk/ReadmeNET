using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Material.Icons;
using Markdown.Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace ReadmeNET;

public class BlockModel
{
    public required string Title { get; set; }

    public MaterialIconKind Icon { get; set; }

    public ICommand CreateObject { get; }

    public BlockModel(Action createAction)
    {
        CreateObject = new RelayCommand(createAction);
    }
}

public abstract partial class EditorBlock : ObservableObject
{
    public string BlockTitle { get; set; } = "BLOCK";
    public Action<EditorBlock>? RemoveAction { get; set; }
    public Action<EditorBlock, EditorBlock, bool>? MoveAction { get; set; }

    [RelayCommand]
    private void Remove()
    {
        RemoveAction?.Invoke(this);
    }
}

public partial class HeaderEditorBlock : EditorBlock
{
    public HeaderEditorBlock()
    {
        BlockTitle = "HEADER";
    }
    [ObservableProperty]
    private string _text = "";

    [ObservableProperty]
    private int _headerLevel = 0;
}

public partial class Main : UserControl
{
    public ObservableCollection<BlockModel> BlockCatalog { get; set; }
    public ObservableCollection<EditorBlock> ActiveBlocks { get; set; } = new();
    public Main()
    {
        InitializeComponent();

        BlockCatalog = new ObservableCollection<BlockModel>
        {
            new BlockModel(() =>
            {
                var newBlock = new HeaderEditorBlock();
                newBlock.RemoveAction = (block) => ActiveBlocks.Remove(block);

                newBlock.MoveAction = (source, target, isBottomHalf) =>
                {
                    int oldIndex = ActiveBlocks.IndexOf(source);
                    int targetIndex = ActiveBlocks.IndexOf(target);

                    if(oldIndex != -1 && targetIndex != -1 && oldIndex != targetIndex)
                    {
                        ActiveBlocks.RemoveAt(oldIndex);
                        if(oldIndex < targetIndex)
                        {
                            targetIndex--;
                        }
                        if (isBottomHalf)
                        {
                            targetIndex++;
                        }
                        ActiveBlocks.Insert(targetIndex, (EditorBlock)source);
                    }
                };

                ActiveBlocks.Add(newBlock);
            })
    {
        Title = "Header",
        Icon = MaterialIconKind.FormatSize
    },

    // Reszta ma na razie pustą funkcję () => { }, czeka na swoje klasy
    new BlockModel(() => { })
    {
        Title = "Text",
        Icon = MaterialIconKind.FormatAlignLeft
    },
    new BlockModel(() => { })
    {
        Title = "Quote",
        Icon = MaterialIconKind.FormatQuoteClose
    },
    new BlockModel(() => { })
    {
        Title = "Alert",
        Icon = MaterialIconKind.Alert
    },
    new BlockModel(() => { })
    {
        Title = "Color",
        Icon = MaterialIconKind.Color
    },
    new BlockModel(() => { })
    {
        Title = "List",
        Icon = MaterialIconKind.FormatListNumbered
    },
    new BlockModel(() => { })
    {
        Title = "Table",
        Icon = MaterialIconKind.Table
    },
    new BlockModel(() => { })
    {
        Title = "Collapse",
        Icon = MaterialIconKind.CollapseAll
    },
    new BlockModel(() => { })
    {
        Title = "Image",
        Icon = MaterialIconKind.ImageOutline
    },
    new BlockModel(() => { })
    {
        Title = "Code",
        Icon = MaterialIconKind.Console
    },
    new BlockModel(() => { })
    {
        Title = "Custom Code",
        Icon = MaterialIconKind.CodeBraces
    }
        };
        this.DataContext = this;
    }
}