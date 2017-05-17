using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DerpGame.View;
using System.Collections.Generic;
namespace DerpGame.Model
{
	public class Friend
	{
		private Animation texture;
		public Vector2 Position
		{
			get { return texture.Position; }
		}
		private float theta;
		private float radius;
		private float increment;
		private Player player;
		public void Initalize(Texture2D texture, Player player)
		{
			this.texture = new Animation();
			radius = 125;
			this.texture.Initialize(texture,new Vector2(player.Position.X, player.Position.Y+ radius),115, 69, 8, 30, Color.White, .75f, true);
			increment = (float)Math.PI/64;
			theta = 0;
			this.player = player;
		}
		public void Update(GameTime time)
		{
			if (theta < Math.PI * 2)
			{
				theta += increment;
			}
			else 
			{
				theta = 0;
			}
			float x = (float)Math.Cos(theta) * radius;
			float y = (float)Math.Sin(theta) * radius;
			texture.Position.X = player.Position.X + x;
			texture.Position.Y = player.Position.Y + y;
			texture.Update(time);
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			texture.Draw(spriteBatch);
		}
	}
}
