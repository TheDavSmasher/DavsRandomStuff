using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.DavsRandomStuff.Entities
{
	[CustomEntity("DavsRandomStuff/DangerDashSpringUp", "DavsRandomStuff/DangerDashSpringRight", "DavsRandomStuff/DangerDashSpringLeft", "DavsRandomStuff/DangerDashSpringDown")]
	public class DangerDashSpring : GeneralSpring
	{
		private readonly bool ignoreRedBoosters;

		public DangerDashSpring(Vector2 position, Orientations orientation, string spritePath, bool playerCanUse, bool ignoreHoldables, bool ignoreRedBubble)
			: base(position, orientation, spritePath, playerCanUse, ignoreHoldables)
		{
			ignoreRedBoosters = ignoreRedBubble;
		}

		public DangerDashSpring(EntityData data, Vector2 offset)
			: base(data, offset, "DavsRandomStuff/DangerDashSpring")
		{
			ignoreRedBoosters = data.Bool("ignoreRedBoosters", false);
		}

		protected override void OnCollide(Player player)
		{
			if (ignoreRedBoosters && player.StateMachine.State == Player.StRedDash || !player.DashAttacking)
			{
				player.Die((player.Position - Position).SafeNormalize());
				return;
			}
			base.OnCollide(player);
		}
	}
}