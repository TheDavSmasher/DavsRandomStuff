using Microsoft.Xna.Framework;
using Monocle;
using System;

namespace Celeste.Mod.DavsRandomStuff.Entities
{
	public abstract class GeneralSpring : Spring
	{
		public const Orientations Ceiling = (Orientations)3;

		public bool ignoreHoldables;

		public GeneralSpring(Vector2 position, Orientations orientation, string spritePath, bool playerCanUse = true, bool ignoreHoldables = false)
			: base(position, orientation == Ceiling ? Orientations.Floor : orientation, playerCanUse)
		{
			Orientation = orientation;
			this.ignoreHoldables = ignoreHoldables;
			
			Get<PlayerCollider>().OnCollide = OnCollide;
			Get<PufferCollider>().OnCollide = OnPuffer;
			Get<HoldableCollider>().OnCollide = OnHoldable;

			sprite.Reset(GFX.Game, spritePath);
			sprite.Add("idle", "", 0f, default(int));
			sprite.Add("bounce", "", 0.07f, "idle", 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 4, 5);
			sprite.Add("disabled", "white", 0.07f);
			sprite.Play("idle");
			if (orientation == Ceiling)
			{
				staticMover.SolidChecker = (s) => CollideCheck(s, Position - Vector2.UnitY);
				staticMover.JumpThruChecker = (jt) => CollideCheck(jt, Position - Vector2.UnitY);
				Collider = new Hitbox(16f, 6f, -8f, 0f);
				Get<PufferCollider>().Collider = new Hitbox(16f, 10f, -8f, 0f);
				sprite.Rotation = (float)Math.PI;
			}
			staticMover.OnEnable = OnEnable;
			staticMover.OnDisable = OnDisable;
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

			if (CanBounceAnimate(player))
			{
				BounceAnimate();
				switch (Orientation)
				{
					case Orientations.Floor:
						player.SuperBounce(Top);
						break;
					case Orientations.WallLeft:
						player.SideBounce(1, Right, CenterY);
						break;
					case Orientations.WallRight:
						player.SideBounce(-1, Left, CenterY);
						break;
					case Ceiling:
						player.SuperBounce(Bottom + player.Height);
						player.varJumpSpeed = player.Speed.Y = 185f;
						SceneAs<Level>().DirectionalShake(Vector2.UnitY, 0.1f);
						break;
				}
			}
		}

		protected bool CanBounceAnimate(Player player)
		{
			return Orientation switch
			{
				Orientations.Floor => player.Speed.Y >= 0f,
				Orientations.WallLeft => player.Speed.X <= 240f,
				Orientations.WallRight => player.Speed.X >= -240f,
				Ceiling => player.Speed.Y <= 0f,
				_ => false
			};
		}

		protected new virtual void OnEnable()
		{
			base.OnEnable();
		}

		protected new virtual void OnDisable()
		{
			base.OnDisable();
		}

		protected new virtual void OnHoldable(Holdable h)
		{
			if (ignoreHoldables)
				return;
			base.OnHoldable(h);
		}

		protected new virtual void OnPuffer(Puffer p) => base.OnPuffer(p);
	}
}