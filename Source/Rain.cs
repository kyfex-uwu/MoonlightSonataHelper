using Celeste;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using System;
using System.IO;

[CustomEntity("MoonlightSonataHelper/Rain")]
class Rain : Entity {
    public static readonly Texture2D rainTexture = VirtualContent.CreateTexture(Path.Combine("Graphics", "Atlases", "Gameplay",
        "kyfexuwu", "MoonlightSonataHelper","rain")).Texture_Safe;

    public Rain(EntityData data, Vector2 offset)
            : this(data.Position + offset, data.Width) {
    }
    private readonly SkewCollider skewCollider;
    public Rain(Vector2 position, float width)
        : base(position) {
        this.skewCollider = new SkewCollider(0, width, 50);
        base.Collider = this.skewCollider;
        base.Depth = -8500;

        this.Add(new RainRenderHook(RenderRain));
        this.Add(new PlayerCollider(OnCollide));
    }

    private void OnCollide(Player player) {
        player.Die(new Vector2(0,0));
    }

    private Matrix createSkew(Matrix origMatrix) {
        Matrix translate1 = Matrix.CreateTranslation(-(this.Position.X + this.skewCollider.RealLeft), -this.Position.Y, 0);

        Matrix skew = Matrix.Identity;
        skew.M21 = this.skewCollider.Skew;

        Matrix translate2 = Matrix.CreateTranslation(this.Position.X + this.skewCollider.RealLeft, this.Position.Y, 0);
        return origMatrix * translate1 * skew * translate2;
    }
    public void RenderRain(Matrix origMatrix) {
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
            SamplerState.PointWrap, DepthStencilState.Default,
            RasterizerState.CullNone, null, this.createSkew(origMatrix));
        Draw.SpriteBatch.Draw(rainTexture, this.Position + new Vector2(this.skewCollider.RealLeft, 0), 
            new Rectangle(-(int)this.Position.X, -(int)(this.Scene.TimeActive*60*3+this.Position.Y), 
                (int)Math.Round(this.skewCollider.RealWidth), (int)Math.Round(this.Height)), 
            Color.White);
        Draw.SpriteBatch.End();
    }
    private static readonly Color rainColor = new Color(28/255f, 36 / 255f, 41 / 255f, 0.07f);// /255f
    public override void Render() {
        for (int y = 0; y < this.Height; y++) {
            Draw.Rect((int)Math.Ceiling(this.Position.X+this.skewCollider.RealLeft + this.skewCollider.Skew * y), 
                    this.Top + y, this.skewCollider.RealWidth, 1, rainColor);
        }
    }
}
