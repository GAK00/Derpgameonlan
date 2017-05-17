using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace DerpGame
{

	public class Projectile
	{
		// Image representing the Projectile
		private Texture2D texture;

		// Position of the Projectile relative to the upper left side of the screen
		public Vector2 Position;

		// State of the Projectile
		private bool active;
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		// The amount of damage the projectile can inflict to an enemy
		private int damage;
		public int Damage 
		{
			get { return damage; }
		}
		// Represents the viewable boundary of the game
		private Viewport viewport;

		// Get the width of the projectile ship
		public int Width
		{
			get { return texture.Width; }
		}

		// Get the height of the projectile ship
		public int Height
		{
			get { return texture.Height; }
		}

		// Determines how fast the projectile moves
		private float projectileMoveSpeed;

		public Projectile()
		{
		}
		public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
		{
			this.texture = texture;
			Position = position;
			this.viewport = viewport;

			active = true;

			damage = 2;

			projectileMoveSpeed = 20f;
		}
		public void Update()
		{
			// Projectiles always move to the right
			Position.X += projectileMoveSpeed;

			// Deactivate the bullet if it goes out of screen
			if (Position.X + texture.Width / 2 > viewport.Width)
				active = false;
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, Position, null, Color.White, 0f,
			new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
		}

	}
}
