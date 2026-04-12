using Avalonia.Controls;
using Avalonia.Metadata;
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
    public string Title { get; }

    public MaterialIconKind Icon { get;}

    public ICommand CreateObject { get; }

    public BlockModel(string title, MaterialIconKind icon, Action createAction)
    {
        Title = title;
        Icon = icon;
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

    public string CachedMarkdown { get; set; } = "";
}

public partial class HeaderEditorBlock : EditorBlock
{
    [ObservableProperty]
    private string _text = "";

    [ObservableProperty]
    private int _headerLevel = 0;
};

public partial class QuoteEditorBlock : EditorBlock
{
    [ObservableProperty]
    private string _text = "";
}

public partial class ImageEditorBlock : EditorBlock
{
    [ObservableProperty]
    private string _url = "";
    [ObservableProperty]
    private string _text = "";
}

public partial class CodeEditorBlock : EditorBlock
{
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
    [ObservableProperty]
    private string _title = "";
    [ObservableProperty]
    private string _text = "";
}

public partial class CustomEditorBlock : EditorBlock
{
    [ObservableProperty]
    private string _text = "";
}

public partial class AlertEditorBlock : EditorBlock
{
    [ObservableProperty]
    private string _text = "";

    [ObservableProperty]
    private int _alertLevel = 0;
}

public partial class Main : UserControl
{
    public ObservableCollection<BlockModel> BlockCatalog { get; set; }
    public ObservableCollection<EditorBlock> ActiveBlocks { get; set; } = new();

    private BlockModel CreateMenuOption<T>(string title, MaterialIconKind icon) where T : EditorBlock, new() 
    {
        return new BlockModel(title, icon, () =>
        {
            var newBlock = new T();
            newBlock.BlockTitle = title.ToUpper();
            AddNewBlock(newBlock);
        });
    }

    private void AddNewBlock(EditorBlock newBlock)
    {

        newBlock.RemoveAction = (block) =>
        {
            block.PropertyChanged -= Block_PropertyChanged;
            ActiveBlocks.Remove(block);

            UpdateMarkdownPreview();
        };
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

                UpdateMarkdownPreview();
            }
        };
        newBlock.PropertyChanged += Block_PropertyChanged;

        ActiveBlocks.Add(newBlock);

        UpdateMarkdownPreview();
    }

    private string GenerateMarkdownForBlock(EditorBlock block)
    {
        string[] alertTypes = { "NOTE", "TIP", "IMPORTANT", "WARNING", "CAUTION" };

        switch (block)
        {
            //System.Diagnostics.Debug.WriteLine($"POSITION: {index}, Block: {block.BlockTitle}, has: {headerBlock.HeaderLevel} and {headerBlock.Text}");
            case HeaderEditorBlock headerBlock:
                return $"{new string('#', headerBlock.HeaderLevel + 1)} {headerBlock.Text}\n";
            case QuoteEditorBlock quoteBlock:
                return $"> {quoteBlock.Text}\n";
            case CodeEditorBlock codeBlock:
                return $"```{(string.IsNullOrWhiteSpace(codeBlock.Language) ? "text" : codeBlock.Language)}\n{codeBlock.Text}\n```\n";
            case CollapseEditorBlock collapseBlock:
                return $"<details>\n<summary>{collapseBlock.Title}</summary>\n{collapseBlock.Text}\n</details>\n";
            case AlertEditorBlock alertBlock:
                return $"> [!{alertTypes[alertBlock.AlertLevel]}]\n> {alertBlock.Text}\n";
            case ImageEditorBlock imageBlock:
                return $"![{imageBlock.Text}]({imageBlock.Url})\n";
            case CustomEditorBlock customBlock:
                return $"customBlock.Text\n";
            default:
                return "";
        }
    }

    private void UpdateMarkdownPreview()
    {
        var sb = new System.Text.StringBuilder();

        foreach (var block in ActiveBlocks)
        {
            sb.AppendLine(block.CachedMarkdown);
        }

        MarkdownViewer.Markdown = sb.ToString();
    }

    private void Block_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if(sender is EditorBlock changedBlock && e.PropertyName != null)
        {
            changedBlock.CachedMarkdown = GenerateMarkdownForBlock(changedBlock);

            UpdateMarkdownPreview();
        }
    }
    public Main()
    {
        InitializeComponent();

        BlockCatalog = new ObservableCollection<BlockModel>
        {
            CreateMenuOption<HeaderEditorBlock>("Header", MaterialIconKind.FormatSize),
            CreateMenuOption<QuoteEditorBlock>("Quote", MaterialIconKind.FormatQuoteClose),
            CreateMenuOption<CodeEditorBlock>("Code", MaterialIconKind.Console),
            CreateMenuOption<CollapseEditorBlock>("Collapse", MaterialIconKind.CollapseAll),
            CreateMenuOption<AlertEditorBlock>("Alert", MaterialIconKind.Alert),
            CreateMenuOption<ImageEditorBlock>("Image", MaterialIconKind.ImageOutline),
            CreateMenuOption<CustomEditorBlock>("Custom Code", MaterialIconKind.CodeBraces),

            //new BlockModel(() => { })
            //{
            //    Title = "Text",
            //    Icon = MaterialIconKind.FormatAlignLeft
            //},

            //new BlockModel(() => { })
            //{
            //    Title = "Color",
            //    Icon = MaterialIconKind.Color
            //},

            //new BlockModel(() => { })
            //{
            //    Title = "List",
            //    Icon = MaterialIconKind.FormatListNumbered
            //},

            //new BlockModel(() => { })
            //{
            //    Title = "Table",
            //    Icon = MaterialIconKind.Table
            //},
        };
        this.DataContext = this;
    }
}