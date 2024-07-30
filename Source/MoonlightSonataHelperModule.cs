using System;

namespace Celeste.Mod.MoonlightSonataHelper;

public class MoonlightSonataHelperModule : EverestModule {
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

    public override void Load() {

    }

    public override void Unload() {

    }
}