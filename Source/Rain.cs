using Celeste;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using System;

[CustomEntity("MoonlightSonataHelper/Rain")]
class Rain : Entity {
    public Rain(EntityData data, Vector2 offset)
            : this(data.Position + offset, data.Width, data.Height, data.Float("skew")) {
    }
    private readonly SkewCollider skewCollider;
    public Rain(Vector2 position, float width, float height, float skew)
        : base(position) {
        this.skewCollider = new SkewCollider(skew, width, height);
        base.Collider = this.skewCollider;
        base.Depth = -8500;

        this.Add(new DisplacementRenderHook(RenderDisplacement));
        this.Add(new PlayerCollider(OnCollide));
    }

    private void OnCollide(Player player) {
        player.Die(new Vector2(0,0));
    }

    private static readonly Random rainRandom = new Random();
    public void RenderDisplacement() {
        for(int x= (int)Math.Min(0, this.skewCollider.Skew*this.Height);x<this.Width;x++) {
            for(int y=0;y<this.Height;y++) {
                var pos = this.Position + new Vector2(x, y);
                if (!this.Collider.Collide(pos)) continue;
                Draw.SpriteBatch.Draw(Draw.Pixel.Texture.Texture_Safe, pos, Draw.Pixel.ClipRect, 
                    new Color(0.5f, rainRandom.Next()%50*0.01f+0.01f, 0f, 1f));
            }
        }
    }
}
