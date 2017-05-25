using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DerpGame.View;
namespace DerpGame.Model
{
public class DankLaser
{
	// Image representing the Projectile
		private Animation texture;
		public Animation Texture 
		{
			get { return texture;}
		}
        public Player player;
		private float theta;
        public float Theta
        {
            get { return theta; }
        }
	public Vector2 Direction;
	// State of the Projectile
	private bool active;
	public bool Active
	{
		get { return active; }
		set { active = value; }
	}

		private int generation;
		public int Generation 
		{
			get { return generation;}
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
			get { return texture.FrameWidth; }
	}

	// Get the height of the projectile ship
	public int Height
		{
			get { return texture.FrameHeight; }
		}
	// Determines how fast the projectile moves
	private float projectileMoveSpeed;

	public void Initialize(Viewport viewport, Animation texture, Vector2 position, float theta , int generation, Player player)
	{
            this.player = player;
			this.generation = generation;
			this.theta = theta;
		this.texture = texture;
			this.texture.Position = position;
			Direction = new Vector2((float)Math.Cos(theta),(float)Math.Sin(theta)*-1);
		this.viewport = viewport;

		active = true;

			damage = 7 + generation * 2;

			projectileMoveSpeed = 8f *((3f / generation));
	}
		public void Update(GameTime time)
	{
		// Projectiles always move to the right
			texture.Update(time, 0f);
			this.texture.Position.X += projectileMoveSpeed* Direction.X;
			this.texture.Position.Y += projectileMoveSpeed* Direction.Y;
		// Deactivate the bullet if it goes out of screen
			if (this.texture.Position.X + texture.FrameWidth / 2 > viewport.Width || this.texture.Position.Y + texture.FrameHeight / 2 > viewport.Height || this.texture.Position.X + texture.FrameWidth / 2 < 0 || this.texture.Position.Y + texture.FrameHeight / 2 < 0)
			active = false;
			
	}
	public void Draw(SpriteBatch spriteBatch)
	{
			texture.Draw(spriteBatch, theta);
	}

}


}
