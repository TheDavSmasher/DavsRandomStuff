using Celeste.Mod.DavsRandomStuff.Entities;
using Microsoft.Xna.Framework;
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
		On.Celeste.CassetteBlock.UpdateVisualState += CassetteBlock_UpdateVisualState;
    }

	public override void Unload() {
        On.Celeste.CassetteBlock.UpdateVisualState -= CassetteBlock_UpdateVisualState;
    }

	private void CassetteBlock_UpdateVisualState(On.Celeste.CassetteBlock.orig_UpdateVisualState orig, CassetteBlock self)
	{
        orig(self);

		Vector2 scale = new Vector2(1f + self.wiggler.Value * 0.05f * self.wigglerScaler.X, 1f + self.wiggler.Value * 0.15f * self.wigglerScaler.Y);
		foreach (CassetteBlock item3 in self.group)
		{
			foreach (StaticMover staticMover2 in item3.staticMovers)
			{
				if (staticMover2.Entity is not DangerDashSpring spikes)
				{
					continue;
				}
				spikes.spikes?.Scale = scale;
			}
		}
	}
}