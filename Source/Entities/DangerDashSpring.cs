using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.DavsRandomStuff.Entities
{
	[CustomEntity($"{ClassName}Up", $"{ClassName}Right", $"{ClassName}Left", $"{ClassName}Down")]
	public class DangerDashSpring : GeneralSpring
	{
		public bool ignoreRedBoosters;

		public bool spikesOutline;

		public bool killAsSpikes;

		public Sprite spikes;

		private Vector2 spikesOffset;

		private const string ClassName = "DavsRandomStuff/DangerDashSpring";

		public DangerDashSpring(Vector2 position, Orientations orientation, string spritePath, string spikesPath,
			bool spikesOutline, bool playerCanUse = true, bool killAsSpikes = false, bool ignoreHoldables = false, bool ignoreRedBubble = false)
			: base(position, orientation, spritePath, playerCanUse, ignoreHoldables)
		{
			ignoreRedBoosters = ignoreRedBubble;
			this.spikesOutline = spikesOutline;
			this.killAsSpikes = killAsSpikes;
			AddSpikes(spikesPath);
		}

		public DangerDashSpring(EntityData data, Vector2 offset)
			: base(data, offset, ClassName)
		{
			ignoreRedBoosters = data.Bool("ignoreRedBoosters", false);
			this.spikesOutline = data.Bool("spikesOutline", false);
			this.killAsSpikes = data.Bool("killAsSpikes", false);
			AddSpikes(data.String("spikesPath"));
		}

		public void AddSpikes(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				return;
			Add(spikes = sprite.CreateClone());
			spikes.Reset(GFX.Game, path);
			spikes.Add("idle", "spikes");
			spikes.Play("idle");
			spikes.Rotation = sprite.Rotation;
			spikes.Origin -= Vector2.UnitY;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			spikes?.Color = Color.White;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (VisibleWhenDisabled)
			{
				spikes?.Color = DisabledColor;
			}
		}

		protected override void OnCollide(Player player)
		{
			if (player.StateMachine.State == Player.StDreamDash || !playerCanUse)
				return;

			if (ignoreRedBoosters && player.StateMachine.State == Player.StRedDash ||
				!player.DashAttacking && (!killAsSpikes || CanBounceAnimate(player)))
			{
				player.Die((player.Position - Position).SafeNormalize());
				return;
			}
			base.OnCollide(player);
		}

		protected override void OnShake(Vector2 amount)
		{
			spikesOffset += amount;
		}

		public override void Render()
		{
			base.Render();
			if (spikesOutline && spikes != null)
			{
				Vector2 position = spikes.Position;
				spikes.Position += spikesOffset;
				spikes.DrawSimpleOutline();
				spikes.Render();
				spikes.Position = position;
			}
		}
	}
}