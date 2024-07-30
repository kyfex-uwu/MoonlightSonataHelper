using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class FRect {
    public float Top;
    public float Left;
    public float Width;
    public float Height;
    public FRect(float x, float y, float width, float height) {
        this.Top = y;
        this.Left = x;
        this.Width = width;
        this.Height = height;
    }

    public float Bottom { get => this.Top + this.Height; }
    public float Right { get => this.Left + this.Width; }

    public static FRect from(Rectangle rect) {
        return new FRect(rect.X, rect.Y, rect.Width, rect.Height);
    }
    public static FRect from(Hitbox rect) {
        return new FRect(rect.AbsoluteLeft, rect.AbsoluteTop, rect.Width, rect.Height);
    }
}
