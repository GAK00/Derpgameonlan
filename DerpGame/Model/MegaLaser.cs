using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DerpGame.View;
using System.Collections.Generic;
namespace DerpGame
{
	public class MegaLaser
	{
		// Image representing the Projectile
		private Animation texture;
		public Animation Texture
		{
			get { return texture; }
		}
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
			get { return generation; }
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
		private TimeSpan timeToLive;
		private TimeSpan startTime;


		public void Initialize(Viewport viewport, Animation texture, Vector2 position, float theta, int generation, double ttl, double startTime)
		{
			//System.Console.Out.WriteLine((theta * (180 / Math.PI)) % 360 + "      " + generation);
			this.generation = generation;
			this.theta = theta;
			this.texture = texture;
			this.texture.Position = position;
			Direction = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta) * -1);
			this.viewport = viewport;
            this.active = true;
			timeToLive = TimeSpan.FromSeconds(ttl);
			this.startTime = TimeSpan.FromSeconds(startTime);
			damage = 7 + (int)((float)generation * 1.2f);

			projectileMoveSpeed = 5f + (int)(0.8f * (float)generation);
		}
		public List<MegaLaser> Update(GameTime time)
		{
			// Projectiles always move to the right
			List<MegaLaser> newLasers = new List<MegaLaser>();
            if (time.TotalGameTime.TotalSeconds > startTime.TotalSeconds + timeToLive.TotalSeconds)
            {
                this.active = false;
            }
            else
            {
                double dem = 2.25 + (generation * 4);
                texture.Update(time, 0f);
                this.texture.Position.X += projectileMoveSpeed * Direction.X;
                this.texture.Position.Y += projectileMoveSpeed * Direction.Y;
                // Deactivate the bullet if it goes out of screen
                if ((((this.texture.Position.X > viewport.Width && this.Direction.X > 0) || (this.texture.Position.X < 0 && this.Direction.X < 0)) || ((this.texture.Position.Y > viewport.Height && this.Direction.Y > 0) || (this.texture.Position.Y < 0 && this.Direction.Y < 0))) && generation >= 10)
                {
					this.active = false;
					this.texture.Position.X += projectileMoveSpeed * Direction.X * -2;
					this.texture.Position.Y += projectileMoveSpeed * Direction.Y * -2;
                    Animation dankBullet = new Animation();
                    Texture2D dankBulletTexture = texture.Strip;
                    dankBullet.Initialize(dankBulletTexture, texture.Position, (dankBulletTexture.Width / 6), dankBulletTexture.Height, 6, 1, Color.White, 1f, true);
                    MegaLaser laser = new MegaLaser();
                    laser.Initialize(viewport, dankBullet, dankBullet.Position, (float)(theta + Math.PI +(Math.PI/dem)), generation , timeToLive.TotalSeconds, startTime.TotalSeconds);
                    newLasers.Add(laser);

                }
                else if (((this.texture.Position.X > viewport.Width && this.Direction.X > 0) || (this.texture.Position.X < 0 && this.Direction.X < 0)) && active)
                {
                    this.active = false;
                    this.texture.Position.X += projectileMoveSpeed * Direction.X * -2;
                    this.texture.Position.Y += projectileMoveSpeed * Direction.Y * -2;
                    Animation dankBullet = new Animation();
                    Texture2D dankBulletTexture = texture.Strip;
                    dankBullet.Initialize(dankBulletTexture, texture.Position, (dankBulletTexture.Width / 6), dankBulletTexture.Height, 6, 1, Color.White, 1f, true);
                    MegaLaser laser = new MegaLaser();
                    laser.Initialize(viewport, dankBullet, dankBullet.Position, (float)(theta + Math.PI + (Math.PI / dem)), generation + 1, timeToLive.TotalSeconds, startTime.TotalSeconds);
                    newLasers.Add(laser);

                    Animation dankBullet2 = new Animation();
                    Texture2D dankBulletTexture2 = texture.Strip;
                    dankBullet2.Initialize(dankBulletTexture2, texture.Position, (dankBulletTexture2.Width / 6), dankBulletTexture2.Height, 6, 1, Color.White, 1f, true);
                    MegaLaser laser2 = new MegaLaser();
                    laser2.Initialize(viewport, dankBullet2, dankBullet2.Position, (float)(theta + Math.PI - (Math.PI / dem)), generation + 1, timeToLive.TotalSeconds, startTime.TotalSeconds);
                    newLasers.Add(laser2);
                }

                else if (((this.texture.Position.Y > viewport.Height && this.Direction.Y > 0) || (this.texture.Position.Y < 0 && this.Direction.Y < 0)) && active)
                {

                    this.active = false;
                    this.texture.Position.X += projectileMoveSpeed * Direction.X * -2;
                    this.texture.Position.Y += projectileMoveSpeed * Direction.Y * -2;
                    Animation dankBullet = new Animation();
                    Texture2D dankBulletTexture = texture.Strip;
                    dankBullet.Initialize(dankBulletTexture, texture.Position, (dankBulletTexture.Width / 6), dankBulletTexture.Height, 6, 1, Color.White, 1f, true);
                    MegaLaser laser = new MegaLaser();
                    laser.Initialize(viewport, dankBullet, dankBullet.Position, (float)(theta + Math.PI + (Math.PI / dem)), generation + 1, timeToLive.TotalSeconds, startTime.TotalSeconds);
                    newLasers.Add(laser);
                    Animation dankBullet2 = new Animation();
                    Texture2D dankBulletTexture2 = texture.Strip;
                    dankBullet2.Initialize(dankBulletTexture2, texture.Position, (dankBulletTexture2.Width / 6), dankBulletTexture2.Height, 6, 1, Color.White, 1f, true);
                    MegaLaser laser2 = new MegaLaser();
                    laser2.Initialize(viewport, dankBullet2, dankBullet2.Position, (float)(theta + Math.PI - (Math.PI / dem)), generation + 1, timeToLive.TotalSeconds, startTime.TotalSeconds);
                    newLasers.Add(laser2);
                }
            }
			//System.Console.Out.WriteLine(newLasers.Count);
			return newLasers;
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			texture.Draw(spriteBatch, theta);
		}

	}
}
