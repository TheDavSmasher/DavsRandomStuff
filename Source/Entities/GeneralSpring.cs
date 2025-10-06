using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste.Mod.DavsRandomStuff.Entities
{
	public abstract class GeneralSpring : Spring
	{
		public const Orientations Ceiling = (Orientations)3;

		public GeneralSpring(Vector2 position, Orientations orientation, string spritePath, bool playerCanUse, bool ignoreHoldables)
			: base(position, orientation == Ceiling ? Orientations.Floor : orientation, playerCanUse)
		{
			Orientation = orientation;

			if (ignoreHoldables)
			{
				Remove(Get<HoldableCollider>());
			}

			Get<PlayerCollider>().OnCollide = OnCollide;
			Get<PufferCollider>().OnCollide = OnPuffer;

			HoldableCollider holdable = Get<HoldableCollider>();
			if (ignoreHoldables)
			{
				Remove(holdable);
			}
			else
			{
				holdable.OnCollide = OnHoldable;
			}

				sprite.Reset(GFX.Game, spritePath);
			sprite.Add("idle", "", 0f, default(int));
			sprite.Add("bounce", "", 0.07f, "idle", 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 4, 5);
			sprite.Add("disabled", "white", 0.07f);
			sprite.Play("idle");
			sprite.Origin.X = sprite.Width / 2f;
			sprite.Origin.Y = sprite.Height;
			if (orientation == Ceiling)
			{
				staticMover.SolidChecker = (s) => CollideCheck(s, Position - Vector2.UnitY);
				staticMover.JumpThruChecker = (jt) => CollideCheck(jt, Position - Vector2.UnitY);
				Collider = new Hitbox(16f, 6f, -8f, 0f);
				Get<PufferCollider>().Collider = new Hitbox(16f, 10f, -8f, 0f);
				sprite.Rotation = (float)Math.PI;
			}
		}

		public GeneralSpring(EntityData data, Vector2 offset, string nameFormat)
			: this(data.Position + offset, GetOrientationFromName(data.Name, nameFormat), data.Attr("spritePath", $"objects/{nameFormat}/"), data.Bool("playerCanUse", true), data.Bool("ignoreHoldables", false))
		{
		}

		public static Orientations GetOrientationFromName(string name, string nameFormat)
		{
			return name.Replace(nameFormat, string.Empty) switch
			{
				"Up" => Orientations.Floor,
				"Right" => Orientations.WallRight,
				"Left" => Orientations.WallLeft,
				"Down" => Ceiling,
				_ => throw new Exception("Spring name doesn't correlate to a valid Orientation!"),
			};
		}

		protected new virtual void OnCollide(Player player)
		{
			if (player.StateMachine.State == Player.StDreamDash || !playerCanUse)
				return;

			if (!Enum.IsDefined(Orientation) && Orientation != Ceiling)
				throw new Exception("Orientation not supported!");

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

		protected new virtual void OnHoldable(Holdable h) => base.OnHoldable(h);

		protected new virtual void OnPuffer(Puffer p) => base.OnPuffer(p);
	}
}