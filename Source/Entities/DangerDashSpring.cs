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

		protected override bool IgnorePlayerCollision(Player player)
		{
			if (ignoreRedBoosters && player.StateMachine.State == Player.StRedDash ||
				player.StateMachine.State == Player.StDreamDash || !player.DashAttacking)
			{
				player.Die((player.Position - Position).SafeNormalize());
				return true;
			}
			return false;
		}

		protected override void OnPlayerCollide(Player player)
		{
			switch (Orientation)
			{
				case Orientations.Floor when player.Speed.Y >= 0f:
					BounceAnimate();
					player.SuperBounce(Top);
					break;
				case Orientations.WallLeft when player.SideBounce(1, Right, CenterY):
					BounceAnimate();
					break;
				case Orientations.WallRight when player.SideBounce(-1, Left, CenterY):
					BounceAnimate();
					break;
				case Ceiling when player.Speed.Y <= 0f:
					BounceAnimate();
					player.SuperBounce(Bottom + player.Height);
					player.varJumpSpeed = player.Speed.Y = 185f;
					SceneAs<Level>().DirectionalShake(Vector2.UnitY, 0.1f);
					break;
			}
		}
	}
}