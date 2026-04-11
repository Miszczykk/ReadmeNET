using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
};

public partial class QuoteEditorBlock : EditorBlock
{
    public QuoteEditorBlock()
    {
        BlockTitle = "QUOTE";
    }
    [ObservableProperty]
    private string _text = "";

}

public partial class ImageEditorBlock : EditorBlock
{
    public ImageEditorBlock()
    {
        BlockTitle = "IMAGE";
    }
    [ObservableProperty]
    private string _url = "";
    [ObservableProperty]
    private string _text = "";
}

public partial class CodeEditorBlock : EditorBlock
{
    public CodeEditorBlock()
    {
        BlockTitle = "CODE";
    }
    [ObservableProperty]
    private string _language = "";
    [ObservableProperty]
    private string _text = "";

    public List<string> ProgrammingLanguages { get; } = new()
    {
        "Cucumber", "abap", "ada", "ahk", "apacheconf", "applescript", "as", "as3", "asy", "bash",
        "bat", "befunge", "blitzmax", "boo", "brainfuck", "c", "cfm", "cheetah", "cl", "clojure",
        "cmake", "coffeescript", "console", "control", "cpp", "csharp", "css", "cython", "d", "delphi",
        "diff", "dpatch", "duel", "dylan", "erb", "erl", "erlang", "evoque", "factor", "felix",
        "fortran", "gas", "genshi", "gitignore", "glsl", "gnuplot", "go", "groff", "haml", "haskell",
        "html", "hx", "hybris", "ini", "io", "ioke", "irc", "jade", "java", "js", "jsp",
        "lhs", "llvm", "logtalk", "lua", "make", "mako", "maql", "mason", "markdown", "modelica",
        "modula2", "moocode", "mupad", "mxml", "myghty", "nasm", "newspeak", "objdump", "objectivec", "objectivej",
        "ocaml", "ooc", "perl", "php", "postscript", "pot", "pov", "prolog", "properties", "protobuf",
        "py3tb", "pytb", "python", "r", "rb", "rconsole", "rebol", "redcode", "rhtml", "rst",
        "sass", "scala", "scaml", "scheme", "scss", "smalltalk", "smarty", "sourceslist", "splus", "sql",
        "sqlite3", "squidconf", "ssp", "tcl", "tcsh", "tex", "text", "v", "vala", "vbnet",
        "velocity", "vim", "xml", "xquery", "xslt", "yaml"
    };
}

public partial class CollapseEditorBlock : EditorBlock
{
    public CollapseEditorBlock()
    {
        BlockTitle = "COLLAPSE";
    }
    [ObservableProperty]
    private string _title = "";
    [ObservableProperty]
    private string _text = "";
}

public partial class CustomEditorBlock : EditorBlock
{
    public CustomEditorBlock()
    {
        BlockTitle = "CUSTOM";
    }
    [ObservableProperty]
    private string _text = "";
}

public partial class AlertEditorBlock : EditorBlock
{
    public AlertEditorBlock()
    {
        BlockTitle = "ALERT";
    }
    [ObservableProperty]
    private string _text = "";

    [ObservableProperty]
    private int _alertLevel = 0;
}

public partial class Main : UserControl
{
    public ObservableCollection<BlockModel> BlockCatalog { get; set; }
    public ObservableCollection<EditorBlock> ActiveBlocks { get; set; } = new();

    private void AddNewBlock(EditorBlock newBlock)
    {
        newBlock.RemoveAction = (block) => ActiveBlocks.Remove(block);
        newBlock.MoveAction = (source, target, isBottomHalf) =>
        {
            int oldIndex = ActiveBlocks.IndexOf(source);
            int targetIndex = ActiveBlocks.IndexOf(target);

            if (oldIndex != -1 && targetIndex != -1 && oldIndex != targetIndex)
            {
                ActiveBlocks.RemoveAt(oldIndex);
                if (oldIndex < targetIndex)
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
    }
    public Main()
    {
        InitializeComponent();

        BlockCatalog = new ObservableCollection<BlockModel>
        {
            new BlockModel(() => AddNewBlock(new HeaderEditorBlock()))
            {
                Title = "Header",
                Icon = MaterialIconKind.FormatSize
            },

            //new BlockModel(() => { })
            //{
            //    Title = "Text",
            //    Icon = MaterialIconKind.FormatAlignLeft
            //},

            new BlockModel(() => AddNewBlock(new QuoteEditorBlock()))
            {
                Title = "Quote",
                Icon = MaterialIconKind.FormatQuoteClose
            },

            new BlockModel(() => AddNewBlock(new CodeEditorBlock()))
            {
                Title = "Code",
                Icon = MaterialIconKind.Console
            },

            //new BlockModel(() => { })
            //{
            //    Title = "Color",
            //    Icon = MaterialIconKind.Color
            //},

            new BlockModel(() => AddNewBlock(new CollapseEditorBlock()))
            {
                Title = "Collapse",
                Icon = MaterialIconKind.CollapseAll
            },

            new BlockModel(() => AddNewBlock(new AlertEditorBlock()))
            {
                Title = "Alert",
                Icon = MaterialIconKind.Alert
            },

            //new BlockModel(() => { })
            //{
            //    Title = "List",
            //    Icon = MaterialIconKind.FormatListNumbered
            //},

            new BlockModel(() => AddNewBlock(new ImageEditorBlock()))
            {
                Title = "Image",
                Icon = MaterialIconKind.ImageOutline
            },

            //new BlockModel(() => { })
            //{
            //    Title = "Table",
            //    Icon = MaterialIconKind.Table
            //},

            new BlockModel(() => AddNewBlock(new CustomEditorBlock()))
            {
                Title = "Custom Code",
                Icon = MaterialIconKind.CodeBraces
            },
        };
        this.DataContext = this;
    }
}