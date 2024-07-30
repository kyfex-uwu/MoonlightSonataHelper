using Celeste;
using Celeste.Mod;
using Celeste.Mod.Helpers;
using Microsoft.Xna.Framework;
using Monocle;
using System;

class SkewCollider : ColliderList{
    public SkewCollider(float skew, float width, float height, float x = 0, float y = 0) : base() {
        this.Skew = skew;//how many units to skew per 1 height unit; skew of 2 means 2 pixels to the right for every pixel down
        this.realWidth = width;
        this.height = height;
        this.realX = x;

        this.realX = x;
        Position.Y = y;
    }

    private float skew;
    private float realWidth;
    private float height;
    private float realX;
    public float Skew { get => this.skew; set => this.skew = value; }
    public override float Height { get => this.height; set { this.height = value; this.RecalcApproximation(); } }
    public override float Top { get => Position.Y; set { Position.Y = value; this.RecalcApproximation(); } }
    public override float Bottom { get => Position.Y + this.height; set { Position.Y = value - this.height; this.RecalcApproximation(); } }

    public override float Width { get => this.Right - this.Left; 
        set => throw new NotSupportedException("Set RealWidth instead when using a SkewCollider"); }
    public override float Left { get => this.realX + (this.skew > 0 ? 0 : this.skew * this.height); 
        set => throw new NotSupportedException("Set RealLeft instead when using a SkewCollider"); }
    public override float Right { get => this.realX + this.realWidth + (this.skew < 0 ? 0 : this.skew * this.height); 
        set => throw new NotSupportedException("Set RealRight instead when using a SkewCollider");
    }
    public float RealWidth { get => this.realWidth; set { this.realWidth = value; this.RecalcApproximation(); } }
    public float RealLeft { get => this.realX; set { this.realX = value; this.RecalcApproximation(); } }
    public float RealRight { get => this.realX + this.realWidth; set { this.realX = value - this.realWidth; this.RecalcApproximation(); } }

    public override Collider Clone() { return new SkewCollider(this.skew, this.realWidth, this.height, this.realX, this.Position.Y); }

    public override bool Collide(Vector2 point) {
        return this.IsPointIn(point.X, point.Y);
    }

    private bool Collide(FRect rect) {
        if (rect.Bottom < this.AbsoluteTop || rect.Top > this.AbsoluteBottom ||
               rect.Right < this.AbsoluteLeft || rect.Left > this.AbsoluteRight)
            return false;

        if (this.IsPointInH(rect.Left, rect.Top, rect.Width) || this.IsPointInH(rect.Left, rect.Bottom, rect.Width)) return true;

        Vector2 p1 = new Vector2(this.AbsoluteLeft - rect.Width, this.Height * -Math.Sign(this.skew));
        Vector2 p2 = new Vector2(this.Width + rect.Width, this.Height * Math.Sign(this.skew));
        var m = (this.AbsoluteRight - p1.X) / p2.X;
        var collLineY = p1.Y + p2.Y * m;
        return rect.Top < collLineY && collLineY < rect.Bottom;
    }
    public override bool Collide(Rectangle rect) {
        return this.Collide(FRect.from(rect));
    }

    public override bool Collide(Vector2 from, Vector2 to) {
        if (this.IsPointIn(from.X, from.Y) || this.IsPointIn(to.X, to.Y)) return true;

        var points = new Vector2[]{
            new Vector2(this.realX, this.Top),
            new Vector2(this.skew>0?(this.realX+this.skew*this.height):this.Left, this.Bottom),
            new Vector2(this.skew>0?this.Right:(this.realX+this.skew*this.height+this.realWidth), this.Bottom),
            new Vector2(this.realX+this.realWidth, this.Top),
        };
        for (var i = 0; i < 3; i++)
            if (LinesIntersect(points[i], points[i + 1], from, to)) return true;
        if (LinesIntersect(points[3], points[0], from, to)) return true;

        return false;
    }

    public override bool Collide(Hitbox hitbox) {
        return this.Collide(FRect.from(hitbox));
    }

    public override bool Collide(Grid grid) {//todo
        return false;
    }

    public override bool Collide(Circle circle) {//todo
        return false;
    }

    private ColliderList approximation;
    public override bool Collide(ColliderList list) {//todo
        if (list is SkewCollider) {
            return false;
        }

        //return list.Collide(approximation);//TODO: approximate with rects
        return false;
    }

    public override void Render(Camera camera, Color color) {
        var points = new Vector2[]{
            new Vector2(this.realX, this.Top),
            new Vector2(this.realX+this.skew*this.height, this.Bottom),
            new Vector2(this.realX+this.skew*this.height+this.realWidth, this.Bottom),
            new Vector2(this.realX+this.realWidth, this.Top)
        };
        for (var i = 0; i < points.Length - 1; i++)
            Draw.Line(points[i] + this.AbsolutePosition, points[i + 1] + this.AbsolutePosition, color);
        Draw.Line(points[points.Length - 1] + this.AbsolutePosition, points[0] + this.AbsolutePosition, color);
    }

    //--

    private bool IsPointIn(float x, float y, float extendLeft = 0) {
        return (y >= this.AbsoluteTop && y <= this.AbsoluteBottom) && this.IsPointInH(x, y, extendLeft);
    }
    private bool IsPointInH(float x, float y, float extendLeft = 0) {
        var adjX = x + this.skew * (this.AbsoluteTop - y);
        if (this.Entity != null) adjX -= Entity.Position.X;
        return this.realX - extendLeft < adjX && adjX < this.realX + this.realWidth;
    }
    private static bool LinesIntersect(Vector2 p1from, Vector2 p1to, Vector2 p2from, Vector2 p2to) {
        // https://stackoverflow.com/a/1968345/14000178
        var s1_x = p1to.X - p1from.X; 
        var s1_y = p1to.Y - p1from.Y;
        var s2_x = p2to.X - p2from.X; 
        var s2_y = p2to.Y - p2from.Y;

        float s, t;
        s = (-s1_y * (p1from.X - p2from.X) + s1_x * (p1from.Y - p2from.Y)) / (-s2_x * s1_y + s1_x * s2_y);
        t = (s2_x * (p1from.Y - p2from.Y) - s2_y * (p1from.X - p2from.X)) / (-s2_x * s1_y + s1_x * s2_y);

        return s >= 0 && s <= 1 && t >= 0 && t <= 1;
    }
    private void RecalcApproximation() {
        //todo
    }
}
