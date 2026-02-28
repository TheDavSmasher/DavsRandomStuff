using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.DavsRandomStuff.Entities
{
	[CustomEntity($"{ClassName}Up", $"{ClassName}Right", $"{ClassName}Left", $"{ClassName}Down")]
	public class DangerDashSpring : GeneralSpring
	{
		public bool ignoreRedBoosters;

		public Sprite spikes;

		private const string ClassName = "DavsRandomStuff/DangerDashSpring";

		public DangerDashSpring(Vector2 position, Orientations orientation, string spritePath, string spikesPath,
			bool playerCanUse = true, bool ignoreHoldables = false, bool ignoreRedBubble = false)
			: base(position, orientation, spritePath, playerCanUse, ignoreHoldables)
		{
			ignoreRedBoosters = ignoreRedBubble;
			AddSpikes(spikesPath);
		}

		public DangerDashSpring(EntityData data, Vector2 offset)
			: base(data, offset, ClassName)
		{
			ignoreRedBoosters = data.Bool("ignoreRedBoosters", false);
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