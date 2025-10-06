using System;

namespace Celeste.Mod.DavsRandomStuff;

public class DavsRandomStuffModule : EverestModule {
    public static DavsRandomStuffModule Instance { get; private set; }

    public override Type SessionType => typeof(DavsRandomStuffModuleSession);
    public static DavsRandomStuffModuleSession Session => (DavsRandomStuffModuleSession) Instance._Session;

    public DavsRandomStuffModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(DavsRandomStuffModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(DavsRandomStuffModule), LogLevel.Info);
#endif
    }

    public override void Load() {
        // TODO: apply any hooks that should always be active
    }

    public override void Unload() {
        // TODO: unapply any hooks applied in Load()
    }
}