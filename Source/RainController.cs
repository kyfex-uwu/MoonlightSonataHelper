using Celeste;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

[Tracked(true)]
public class RainController : Entity {

    public static readonly Texture2D rainTexture = VirtualContent.CreateTexture(Path.Combine("Graphics", "Atlases", "Gameplay",
        "kyfexuwu", "MoonlightSonataHelper", "rain")).Texture_Safe;

    private List<Entity> rains;
    private float skew;

    public RainController() : base() {
        this.skew = 0f;
        this.Add(new RainRenderHook(RenderRain));
    }
    public override void Awake(Scene scene) {
        base.Awake(scene);

        this.rains = this.SceneAs<Level>().Tracker.GetEntities<Rain>();

        this.updateSkew(0.5f);
    }
    private void updateSkew(float newSkew) {
        this.skew = newSkew;
        foreach (var rain in rains) ((Rain)rain).updateSkew(newSkew);
    }

    private Matrix createSkew(Matrix origMatrix) {
        Matrix skew = Matrix.Identity;
        skew.M21 = this.skew;
        return origMatrix * skew;
    }
    public void RenderRain(Matrix origMatrix) {
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
            SamplerState.PointWrap, DepthStencilState.Default,
            RasterizerState.CullNone, null, this.createSkew(origMatrix));
        foreach(var e in this.rains) {
            var rain = (Rain)e;
            Draw.SpriteBatch.Draw(rainTexture, new Vector2(rain.origPos.X - this.skew * rain.Position.Y, rain.Position.Y),
                new Rectangle(-(int)rain.Position.X, -(int)(this.Scene.TimeActive * 60 * 3 + rain.Position.Y),
                    (int)Math.Round(rain.origWidth), (int)Math.Round(rain.Height)),
            Color.White);
        }
        Draw.SpriteBatch.End();
    }
    private static readonly Color rainColor = new Color(28 / 255f, 36 / 255f, 41 / 255f, 0.07f);// /255f
    public override void Render() {
        foreach (var e in this.rains) {
            var rain = (Rain)e;
            foreach(var section in rain.sections) {
                for (int y = 0; y < this.Height; y++) {
                    //TODO
                    Draw.Rect((int)Math.Ceiling(section.Left + this.skew * y),
                            section.Top + y, section.Width, 1, rainColor);
                }
            }
        }
    }
}
