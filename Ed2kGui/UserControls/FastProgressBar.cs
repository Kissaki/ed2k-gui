using System.ComponentModel;

namespace Ed2kGui.UserControls;

/// <summary>Replacement <see cref="ProgressBar"/> that immediately draws progress instead of smoothing it out (at the cost of timely accuracy).</summary>
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
