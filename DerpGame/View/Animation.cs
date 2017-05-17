using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace DerpGame.View
{
	public class Animation
	{// The image representing the collection of images used for animation
		private Texture2D spriteStrip;
		public Texture2D Strip
		{
			get { return spriteStrip; }
		}
		// The scale used to display the sprite strip
		private float scale;

		// The time since we last updated the frame
		private int elapsedTime;

		// The time we display a frame until the next one
		private int frameTime;

		// The number of frames that the animation contains
		private int frameCount;

		// The index of the current frame we are displaying
		private int currentFrame;

		// The color of the frame we will be displaying
		private Color color;

		// The area of the image strip we want to display
		private Rectangle sourceRect = new Rectangle();

		// The area where we want to display the image strip in the game
		private Rectangle destinationRect = new Rectangle();

		// Width of a given frame
		private int frameWidth;
		public int FrameWidth
		{
			get { return frameWidth; }
			set { frameWidth = value; }
		}

		// Height of a given frame
		private int frameHeight;
		public int FrameHeight
		{
			get { return frameHeight; }
			set { frameHeight = value; }
		}

		// The state of the Animation
		private bool active;
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		// Determines if the animation will keep playing or deactivate after one run
		private bool looping;
		public bool Looping
		{
			get { return looping; }
			set { looping = value; }
		}
		// Width of a given frame
		public Vector2 Position;

		public void Initialize(Texture2D texture, Vector2 position,
		int frameWidth, int frameHeight, int frameCount,
		int frametime, Color color, float scale, bool looping)
		{
			// Keep a local copy of the values passed in
			this.color = color;
			this.FrameWidth = frameWidth;
			this.FrameHeight = frameHeight;
			this.frameCount = frameCount;
			this.frameTime = frametime;
			this.scale = scale;

			this.looping = looping;
			Position = position;
			spriteStrip = texture;

			// Set the time to zero
			elapsedTime = 0;
			currentFrame = 0;

			// Set the Animation to active by default
			active = true;
			sourceRect = new Rectangle(currentFrame* FrameWidth, 0, FrameWidth, FrameHeight);
		}
		public void Update(GameTime gameTime)
		{
			// Do not update the game if we are not active
			if (active == false)
				return;

			// Update the elapsed time
			elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

			// If the elapsed time is larger than the frame time
			// we need to switch frames
			if (elapsedTime > frameTime)
			{
				// Move to the next frame
				currentFrame++;

				// If the currentFrame is equal to frameCount reset currentFrame to zero
				if (currentFrame == frameCount)
				{
					currentFrame = 0;
					// If we are not looping deactivate the animation
					if (this.looping == false)
						active = false;
				}

				// Reset the elapsed time to zero
				elapsedTime = 0;
			}

			// Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
			sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

			// Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
			destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale) / 2,
			(int)Position.Y - (int)(FrameHeight * scale) / 2,
			(int)(FrameWidth * scale),
			(int)(FrameHeight * scale));
		}
		public void Update(GameTime gameTime, float theta)
		{
			// Do not update the game if we are not active
			if (active == false)
				return;

			// Update the elapsed time
			elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

			// If the elapsed time is larger than the frame time
			// we need to switch frames
			if (elapsedTime > frameTime)
			{
				// Move to the next frame
				currentFrame++;

				// If the currentFrame is equal to frameCount reset currentFrame to zero
				if (currentFrame == frameCount)
				{
					currentFrame = 0;
					// If we are not looping deactivate the animation
					if (this.looping == false)
						active = false;
				}

				// Reset the elapsed time to zero
				elapsedTime = 0;
			}

			// Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
			sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			if (active)
			{
				spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
			}
		}
		public void Draw(SpriteBatch spriteBatch, float theta)
		{
			spriteBatch.Draw(spriteStrip, Position, sourceRect, Color.White, -1 * theta,
			                 new Vector2((frameWidth / 2), (frameHeight / 2)), 1f, SpriteEffects.None, 0f);
		}



	}
}