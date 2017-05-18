using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DerpGame.View;
namespace DerpGame.Model
{
	public class Player
	{
        // Animation representing the player
        private int id;
        public int Id
        {
            get { return id; }
        }
		private Texture2D playerTexture;
		private Friend friend;
		public Friend Friend
		{
			get { return friend;}
		}
		public Texture2D PlayerTexture
		{
			get { return playerTexture; }
			set { playerTexture = value; }
		}
		// Position of the Player relative to the upper left side of the screen
		public Vector2 Position;
		// State of the player
		private bool active;
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}
		// Amount of hit points that player has
		private int health;
		public int Health
		{
			get { return health; }
			set { health = value; }
		}

		// Get the width of the player ship
		public int Width
		{
			get { return playerAnimation.FrameWidth; }
		}

		// Get the height of the player ship
		public int Height
		{
			get { return playerAnimation.FrameHeight; }
		}
		private float movementSpeed;
		public float MovementSpeed
		{
			get { return movementSpeed; }
			set { movementSpeed = value; }
		}
		private Animation playerAnimation;
		public Player()
		{
			movementSpeed = 2.5f;
			friend = new Friend();
		}
		public void Initialize(Animation animation, Vector2 position)
		{
			playerAnimation = animation;

			// Set the starting position of the player around the middle of the screen and to the back
			Position = position;

			// Set the player to be active
			Active = true;

			// Set the player health
			Health = 100;

			friend.Initalize(animation.Strip, this);
		}
		public void Initialize(Animation animation, Vector2 position, int id)
		{
            this.id = id;
			playerAnimation = animation;

			// Set the starting position of the player around the middle of the screen and to the back
			Position = position;

			// Set the player to be active
			Active = true;

			// Set the player health
			Health = 100;

			friend.Initalize(animation.Strip, this);
		}
		public void Update(GameTime gameTime)
		{
			playerAnimation.Position = Position;
			playerAnimation.Update(gameTime);
			friend.Update(gameTime);
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			playerAnimation.Draw(spriteBatch);
			friend.Draw(spriteBatch);
		}
	}
}
