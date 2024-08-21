using Microsoft.Xna.Framework.Graphics;
using Monocle;
using MonoMod.Cil;
using System;
using System.IO;

namespace Celeste.Mod.MoonlightSonataHelper;

public class MoonlightSonataHelperModule : EverestModule {

    public static VirtualRenderTarget RainDisplacement;
    private static RainRenderer rainRenderer = new RainRenderer();

    //--
    public static MoonlightSonataHelperModule Instance { get; private set; }

    public override Type SettingsType => typeof(MoonlightSonataHelperModuleSettings);
    public static MoonlightSonataHelperModuleSettings Settings => (MoonlightSonataHelperModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(MoonlightSonataHelperModuleSession);
    public static MoonlightSonataHelperModuleSession Session => (MoonlightSonataHelperModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(MoonlightSonataHelperModuleSaveData);
    public static MoonlightSonataHelperModuleSaveData SaveData => (MoonlightSonataHelperModuleSaveData) Instance._SaveData;

    public MoonlightSonataHelperModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(MoonlightSonataHelperModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(MoonlightSonataHelperModule), LogLevel.Info);
#endif
    }

    //--

    private static void CreateRainTarget(On.Celeste.GameplayBuffers.orig_Create orig) {
        orig.Invoke();
        RainDisplacement = GameplayBuffers.Create(320, 180);
    }

    private static void InsertRainRenderCall(ILContext il) {
        var cursor = new ILCursor(il);

        //IL_00b6: ldfld class Celeste.BackdropRenderer Celeste.Level::Foreground
        //IL_00bb: ldarg.0
        //IL_00bc: callvirt instance void Monocle.Renderer::Render(class Monocle.Scene)
        cursor.GotoNext(MoveType.After, instr => 
            instr.Previous!=null&&instr.Previous.Previous!=null&&
            instr.Previous.Previous.MatchLdfld<Level>("Foreground") &&
            instr.Previous.MatchLdarg0() &&
            instr.MatchCallvirt<Renderer>("Render"));

        cursor.EmitLdarg0();
        cursor.EmitDelegate(RenderRain);
    }
    private static void RenderRain(Level level) {
        RainRenderer.RenderRainDisplacement();
    }
    private static void AddRainRenderer(On.Celeste.Level.orig_ctor orig, Level self) {
        orig.Invoke(self);
        self.Add(rainRenderer);
    }

    public override void Load() {
        On.Celeste.GameplayBuffers.Create += CreateRainTarget;
        On.Celeste.Level.ctor += AddRainRenderer;

        IL.Celeste.Level.Render += InsertRainRenderCall;
    }

    public override void Unload() {
        On.Celeste.GameplayBuffers.Create -= CreateRainTarget;
        On.Celeste.Level.ctor -= AddRainRenderer;

        IL.Celeste.Level.Render -= InsertRainRenderCall;
    }
}