using Celeste;
using Celeste.Mod.MoonlightSonataHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Monocle;
using System;

public class RainRenderer : Renderer {
    public override void BeforeRender(Scene scene) {
        Camera camera = (scene as Level).Camera;
        Engine.Graphics.GraphicsDevice.SetRenderTarget(MoonlightSonataHelperModule.RainDisplacement);
        Engine.Instance.GraphicsDevice.Clear(new Color(0.5f,0.5f,0,1));

        //Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, camera.Matrix);
        foreach (RainRenderHook component in scene.Tracker.GetComponents<RainRenderHook>()) {
            if (component.Visible && component.RenderRain != null) {
                component.RenderRain(camera.Matrix);
            }
        }

        //Draw.SpriteBatch.End();
    }

    public static bool HasRain(Scene scene) {
        return scene.Tracker.GetComponent<RainRenderHook>() != null;
    }
    public static void RenderRainDisplacement() {
        Engine.Instance.GraphicsDevice.SetRenderTarget(GameplayBuffers.TempA);
        Engine.Instance.GraphicsDevice.Clear(Color.Transparent);
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null);
        Draw.SpriteBatch.Draw(GameplayBuffers.Level, Vector2.Zero, Color.White);
        Draw.SpriteBatch.End();

        Engine.Instance.GraphicsDevice.SetRenderTarget(GameplayBuffers.Level);
        Engine.Instance.GraphicsDevice.Clear(Color.Transparent);
        Engine.Graphics.GraphicsDevice.Textures[1] = MoonlightSonataHelperModule.RainDisplacement;
        GFX.FxDistort.CurrentTechnique = GFX.FxDistort.Techniques["Displace"];
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, GFX.FxDistort);
        Draw.SpriteBatch.Draw(GameplayBuffers.TempA, Vector2.Zero, Color.White);
        Draw.SpriteBatch.End();
    }
}
