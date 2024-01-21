using System.ComponentModel;

namespace Ed2kGui.UserControls;

internal class FastProgressBar : UserControl
{
    [Category("Appearance")]
    [DefaultValue(0)]
    public int ChunkMargin { get; } = 0;

    private int _value;
    public int Value
    {
        get => _value;
        set => SetValue(value);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        ProgressBarRenderer.DrawHorizontalBar(e.Graphics, ClientRectangle);
        ProgressBarRenderer.DrawHorizontalChunks(e.Graphics, bounds: GetBounds());
    }

    private void SetValue(int value)
    {
        Guard.IsInRange(value, 0, 101);
        _value = value;
        Invalidate();
    }

    private Rectangle GetBounds()
    {
        var x = ClientRectangle.X + ChunkMargin;
        var y = ClientRectangle.Y + ChunkMargin;
        var w = ClientRectangle.Width * _value / 100 - ChunkMargin * 2;
        var h = ClientRectangle.Height - ChunkMargin * 2;
        return new Rectangle(x, y, w, h);
    }
}
