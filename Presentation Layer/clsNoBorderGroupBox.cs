using System;
using System.Drawing;
using System.Windows.Forms;

public class NoBorderGroupBox : GroupBox
{
    protected override void OnPaint(PaintEventArgs e)
    {
        // Draw the background and text without the border
        e.Graphics.Clear(this.BackColor);
        TextRenderer.DrawText(e.Graphics, this.Text, this.Font, new Point(0, 0), this.ForeColor);
    }
}
