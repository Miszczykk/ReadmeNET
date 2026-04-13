using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace ReadmeNET;

public partial class TextBlockView : UserControl
{
    public TextBlockView()
    {
        InitializeComponent();
    }
    private void Bold_Click(object? sender, RoutedEventArgs e) => ApplyFormatting("**", "**");
    private void Italic_Click(object? sender, RoutedEventArgs e) => ApplyFormatting("*", "*");
    private void Underline_Click(object? sender, RoutedEventArgs e) => ApplyFormatting("<ins>", "</ins>");
    private void Subscript_Click(object? sender, RoutedEventArgs e) => ApplyFormatting("<sub>", "</sub>");
    private void Superscript_Click(object? sender, RoutedEventArgs e) => ApplyFormatting("<sup>", "</sup>");

    private void ApplyFormatting(string prefix, string suffix)
    {
        if (EditorTextBox == null) return;

        string currentText = EditorTextBox.Text ?? string.Empty;

        int start = EditorTextBox.SelectionStart;
        int end = EditorTextBox.SelectionEnd;

        int realStart = Math.Min(start, end);
        int realEnd = Math.Max(start, end);

        string selectedText = currentText.Substring(realStart, realEnd - realStart);

        string newText = currentText.Substring(0, realStart)
                       + prefix
                       + selectedText
                       + suffix
                       + currentText.Substring(realEnd);

        EditorTextBox.Text = newText;
        EditorTextBox.Focus();

        if (string.IsNullOrEmpty(selectedText))
        {
            EditorTextBox.CaretIndex = realStart + prefix.Length;
        }
        else
        {
            EditorTextBox.SelectionStart = realStart;
            EditorTextBox.SelectionEnd = realEnd + prefix.Length + suffix.Length;
        }
    }
}