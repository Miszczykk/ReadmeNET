using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace ReadmeNET;

public partial class BlockWrapperView : UserControl
{
    public BlockWrapperView()
    {
        InitializeComponent();

        DragDrop.SetAllowDrop(this, true);
        AddHandler(DragDrop.DragOverEvent, OnDragOver);
        AddHandler(DragDrop.DropEvent, OnDrop);
    }


    private async void DragHandle_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var block = DataContext as EditorBlock;
        if (block == null) return;

        var dragData = new DataObject();
        dragData.Set("DraggedBlock", block);

        await DragDrop.DoDragDrop(e, dragData, DragDropEffects.Move);
    }

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains("DraggedBlock"))
            e.DragEffects = DragDropEffects.Move;
        else
            e.DragEffects = DragDropEffects.None;
    }

    private void OnDrop(object? sender, DragEventArgs e)
    {
        var sourceBlock = e.Data.Get("DraggedBlock") as EditorBlock;
        var targetBlock = DataContext as EditorBlock;

        if (sourceBlock != null && targetBlock != null && sourceBlock != targetBlock)
        {
            var position = e.GetPosition(this);
            bool isBottomHalf = position.Y > (this.Bounds.Height / 2);
            targetBlock.MoveAction?.Invoke(sourceBlock, targetBlock, isBottomHalf);
        }
    }
}