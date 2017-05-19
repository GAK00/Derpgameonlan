using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DerpGame.Model;
using DerpGame.View;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Threading;
namespace DerpGame.Controller
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class DerpGame : Game
	{
		#region Declaration Section
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		private Player player;
		private KeyboardState currentKeyboardState;
		private GamePadState currentGamePadState;
		private Texture2D mainBackground;
		// Parallaxing Layers
		private ParallaxingBackground bgLayer1;
		private ParallaxingBackground bgLayer2;
		private Texture2D enemyTexture;
		private List<Enemy> enemies;
		private float time;
		// The rate at which the enemies appear
		private TimeSpan enemySpawnTime;
		private TimeSpan previousSpawnTime;
		private TimeSpan dankTime;

		// A random number generator
		private Random random;
		private Texture2D projectileTexture;
		private List<Projectile> projectiles;

		// The rate of fire of the player laser
		private TimeSpan fireTime;
		private TimeSpan previousFireTime;
		private TimeSpan previousDankTime;
		private Texture2D explosionTexture;
		private List<Animation> explosions;
		private int score;
		private SpriteFont scoreFont;
		private List<DankLaser> dankLasers;
		private List<MegaLaser> megaLasers;
		private Texture2D dankBulletTexture;
		private Texture2D psychBulletTexture;
		private List<Animation> pops;
		private Texture2D cloudStrip;
		private Texture2D popStrip;
		// The sound that is played when a laser is fired
		private SoundEffect laserSound;
		// The sound used when the player or an enemy dies
		private SoundEffect explosionSound;
		private SoundEffect popSound;
		// The music played during gameplay
		private Song gameplayMusic;
        private int side;
        private Boolean hasBooted;
        private List<Player> players;
        private Network network;

		#endregion
		public DerpGame()
		{
            side = -1;
            hasBooted = false;

                graphics = new GraphicsDeviceManager(this);
                Content.RootDirectory = "Content";
		}
		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
            if (hasBooted)
            {
                Console.WriteLine("Start");
                pops = new List<Animation>();
                player = new Player();
                enemies = new List<Enemy>();
                megaLasers = new List<MegaLaser>();
                Console.WriteLine("Mid");
                // Set the time keepers to zero
                previousSpawnTime = TimeSpan.Zero;

                // Used to determine how fast enemy respawns
                enemySpawnTime = TimeSpan.FromSeconds(1f);
                // Initialize our random number generator
                random = new Random();
                projectiles = new List<Projectile>();
                dankLasers = new List<DankLaser>();
                Console.WriteLine("Almost");
                // Set the laser to fire every quarter second
                fireTime = TimeSpan.FromSeconds(.15f);
                dankTime = TimeSpan.FromSeconds(.4f);
                previousDankTime = TimeSpan.Zero;
                Console.WriteLine("End");
                explosions = new List<Animation>();
                score = 0;

                if (side == 0)
                {
                    bgLayer1 = new ParallaxingBackground();
                    bgLayer2 = new ParallaxingBackground();
                }
                else
                {
                    players = new List<Player>();
                }
                Console.WriteLine("OK");
                base.Initialize();
            }
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
            if (hasBooted)
            {
                    Console.WriteLine("Start");   
                    spriteBatch = new SpriteBatch(GraphicsDevice);
                Console.WriteLine("Start");
				Animation playerAnimation = new Animation();
                Console.WriteLine("Start");
                    Texture2D playerTexture = Content.Load<Texture2D>("Animation/shipAnimation");
                Console.WriteLine("Start");
                    dankBulletTexture = Content.Load<Texture2D>("Animation/rainbowBullet");
                Console.WriteLine("Start");
                    psychBulletTexture = Content.Load<Texture2D>("Animation/solidRainbow");
                Console.WriteLine("Start");
                    playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);
                Console.WriteLine("Star");
                    Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y
                    + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
                Console.WriteLine("Start");
                    player.Initialize(playerAnimation, playerPosition);
                Console.WriteLine("MID");
                    mainBackground = Content.Load<Texture2D>("Texture/mainbackground");
                    enemyTexture = Content.Load<Texture2D>("Animation/mineAnimation");
                    projectileTexture = Content.Load<Texture2D>("Texture/laser");
                    explosionTexture = Content.Load<Texture2D>("Animation/explosion");
                    scoreFont = Content.Load<SpriteFont>("Font/gameFont");
                    // Load the music

                    // Load the laser and explosion sound effect
                  
                    cloudStrip = Content.Load<Texture2D>("Animation/cloud");
                    popStrip = Content.Load<Texture2D>("Animation/popAnimation");
				// Start the music right away
				if (side == 0)
				{

                    bgLayer1.Initialize(Content, "Texture/bgLayer1", GraphicsDevice.Viewport.Width, -1);
                    bgLayer2.Initialize(Content, "Texture/bgLayer2", GraphicsDevice.Viewport.Width, -2);
                    gameplayMusic = Content.Load<Song>("sound/gameMusic");
					laserSound = Content.Load<SoundEffect>("Sound/laserFire");
					explosionSound = Content.Load<SoundEffect>("Sound/explosion");
					popSound = Content.Load<SoundEffect>("Sound/cloudPop");
                    PlayMusic(gameplayMusic);
                }
                Console.WriteLine("END");
            }
		}

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#endif

            //previousGamePadState = currentGamePadState;
            //previousKeyboardState = currentKeyboardState;

            // Read the current state of the keyboard and gamepad and store it
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);


            //Update the player
            if (!hasBooted)
            {
                if (currentKeyboardState.IsKeyDown(Keys.C))
                {
                    side = 0;
                    hasBooted = true;
                    Initialize();
                    LoadContent();
                    network = new Network(gameTime);
                }
                else if (currentKeyboardState.IsKeyDown(Keys.S))
                {
                    this.side = 1;
                    hasBooted = true;
                    Initialize();
                    LoadContent();
                    Console.WriteLine("Init");
                    network = new Network(gameTime);
                    Console.WriteLine("Live");
                }
            }
            if (hasBooted)
            {
                if (side == 0)
                {

                    bgLayer1.Update();
                    bgLayer2.Update();
                    network.StartClient(gameTime);
					if (network.data != null && network.data.Count > 0)
					{
                        enemies.Clear();
						List<Object> objs = network.data[1];
						foreach (Object obj in objs)
						{
							if (obj is SPoint)
							{
								if (obj != null)
								{
									SPoint point = (SPoint)obj;
									if (point.id == 0)
									{
                                        Console.WriteLine(point.x);
										Enemy enemy = new Enemy(false);
										Animation animate = new Animation();
										animate.Initialize(enemyTexture, new Vector2(point.x, point.y), 47, 61, 8, 30, Color.White, 1f, true);
                                        animate.Update(gameTime);
										enemy.Initialize(animate, new Vector2(point.x, point.y));
										enemies.Add(enemy);
									}
								}
							}
						}
					}
                    //UpdateClient();
                }
                else
                {
                    Console.WriteLine("ok");
                    UpdatePlayer(gameTime);
                    UpdateEnemies(gameTime);
                    UpdateCollision();
                    UpdateProjectiles(gameTime);
                    UpdateExplosions(gameTime);
                    UpdatePops(gameTime);
                    Console.WriteLine("ok");
                    List<Object> points = new List<Object>();
                    foreach (Enemy enemy in enemies)
                    {
                        int id = 0;
                        if (enemy.IsCloud)
                        {
                            id = 1;
                        }
                        points.Add(new SPoint(enemy.Position.X, enemy.Position.Y, 0, id));
                    }
                    foreach (Projectile bullet in projectiles)
                    {
                        points.Add(new SPoint(bullet.Position.X, bullet.Position.Y, 0, 2));
                    }
                    foreach (DankLaser laser in dankLasers)
                    {
                        points.Add(new SPoint(laser.Texture.Position.X, laser.Texture.Position.Y, laser.Theta, 3));
                    }
                    foreach (MegaLaser laser in megaLasers)
                    {
                        points.Add(new SPoint(laser.Texture.Position.X, laser.Texture.Position.Y,laser.Theta,4));
                    }
                    foreach(Animation pop in pops)
                    {
                        points.Add(new SPoint(pop.Position.X,pop.Position.Y,0,5));
                    }
                    foreach(Animation explosion in explosions)
                    {
                        points.Add(new SPoint(explosion.Position.X,explosion.Position.Y,0,6));
                    }
                    List<Object> playersP = new List<Object>();
                    foreach(Player player in players)
                    {
                        playersP.Add(new SPoint(player.Position.X,player.Position.Y, 0,player.Id));
                    }
                    Console.WriteLine("ok");
                    List<List<Object>> toSend = new List<List<Object>>();
                    List < Object > strings = new List<Object>();
                    toSend.Add(strings);
                    toSend.Add(points);
                    toSend.Add(playersP);
                    network.data = toSend;
					Console.WriteLine("ok");
                    network.StartSever(gameTime);
                    Console.WriteLine("update");
					

                }
                base.Update(gameTime);
            }
        }
		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
            
			graphics.GraphicsDevice.Clear(Color.Chartreuse);
            // Start drawing
            if(hasBooted){
                if (side == 0)
                {

                    spriteBatch.Begin();

                    spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);

                    // Draw the moving background
                    bgLayer1.Draw(spriteBatch);
                    bgLayer2.Draw(spriteBatch);
                    // Draw the Player
                    player.Draw(spriteBatch);
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        Console.WriteLine("Draw");
                        enemies[i].Draw(spriteBatch);
                    }

                    for (int i = 0; i < projectiles.Count; i++)
                    {
                        projectiles[i].Draw(spriteBatch);
                    }
                    for (int index = 0; index < dankLasers.Count; index++)
                    {
                        dankLasers[index].Draw(spriteBatch);
                    }
                    for (int index = 0; index < megaLasers.Count; index++)
                    {
                        megaLasers[index].Draw(spriteBatch);
                    }
                    for (int i = 0; i < explosions.Count; i++)
                    {
                        explosions[i].Draw(spriteBatch);
                    }
                    for (int i = 0; i < pops.Count; i++)
                    {
                        pops[i].Draw(spriteBatch);
                    }
                    spriteBatch.DrawString(scoreFont, "score: " + score, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.White);
                    // Draw the player health
                    spriteBatch.DrawString(scoreFont, "health: " + player.Health, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + 30), Color.White);
                    // Stop drawing
                    spriteBatch.End();
                }
            }

			base.Draw(gameTime);
		}
		private void UpdatePlayer(GameTime time)
		{
            foreach (Player player in players)
            {
                player.Update(time);
                // Get Thumbstick Controls
                player.Position.X += currentGamePadState.ThumbSticks.Left.X * player.MovementSpeed;
                player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * player.MovementSpeed;

                // Use the Keyboard / Dpad
                if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A) ||
                currentGamePadState.DPad.Left == ButtonState.Pressed)
                {
                    player.Position.X -= player.MovementSpeed;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D) ||
                currentGamePadState.DPad.Right == ButtonState.Pressed)
                {
                    player.Position.X += player.MovementSpeed;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W) ||
                currentGamePadState.DPad.Up == ButtonState.Pressed)
                {
                    player.Position.Y -= player.MovementSpeed;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S) ||
                currentGamePadState.DPad.Down == ButtonState.Pressed)
                {
                    player.Position.Y += player.MovementSpeed;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Space))
                {
                    if (megaLasers.Count == 0)
                    {
                        Animation dankBullet = new Animation();
                        dankBullet.Initialize(psychBulletTexture, player.Position, (psychBulletTexture.Width / 6), psychBulletTexture.Height, 6, 1, Color.White, 1f, true);
                        MegaLaser laser = new MegaLaser();
                        laser.Initialize(GraphicsDevice.Viewport, dankBullet, player.Position, 0, 1, 50, time.TotalGameTime.TotalSeconds);
                        megaLasers.Add(laser);
                    }
                }

                // Make sure that the player does not go out of bounds
                player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
                player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);
                // Fire only every interval we set as the fireTime
                if (time.TotalGameTime - (previousFireTime) > fireTime)
                {
                    Console.WriteLine("Pew");
                    // Reset our current time
                    previousFireTime = time.TotalGameTime;

                    // Add the projectile, but add it to the front and center of the player
                    AddProjectile(player.Position + new Vector2(player.Width / 2, 0));

                }
                if (time.TotalGameTime - previousDankTime > dankTime)
                {
                    Console.WriteLine("dank");
                    previousDankTime = time.TotalGameTime;
                    int detail = random.Next(4, 9);
                    int max = detail * 2;
                    int start = random.Next(0, max - ((max * 2) / 3));
                    addDank(player.Friend.Position, start, random.Next(start + 2, max), detail, 1);

                }

                if (player.Health <= 0)
                {
                    player.Health = 100;
                    score = 0;
                }
            }
		}
		private void AddEnemy()
		{
			// Create the animation object
			Animation enemyAnimation = new Animation();

			// Initialize the animation with the correct animation information
			enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);

			// Randomly generate the position of the enemy
			Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));

			// Create an enemy
			Enemy enemy = new Enemy(false);

			// Initialize the enemy
			enemy.Initialize(enemyAnimation, position);

			// Add the enemy to the active enemies list
			enemies.Add(enemy);
		}
		private void AddCloud()
		{
			// Create the animation object
			//this.popSound.Play();
			Animation cloudAnimation = new Animation();
			// Initialize the animation with the correct animation information
			cloudAnimation.Initialize(cloudStrip, Vector2.Zero, 64, 64, 8, 30, Color.White, 1f, true);
			// Randomly generate the position of the enemy
			Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + cloudStrip.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));
			// Create an enemy
			Enemy cloud = new Enemy(true);
			// Initialize the enemy
			cloud.Initialize(cloudAnimation, position);
			// Add the enemy to the active enemies list
			enemies.Add(cloud);
		}
		private void UpdateEnemies(GameTime gameTime)
		{
			// Spawn a new enemy enemy every 1.5 seconds
			if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
			{
				previousSpawnTime = gameTime.TotalGameTime;

				// Add an Enemy
				AddEnemy();
				AddCloud();
			}

			// Update the Enemies
			for (int i = enemies.Count - 1; i >= 0; i--)
			{
				enemies[i].Update(gameTime);
				if (!enemies[i].IsCloud)
				{
					if (enemies[i].Active == false)
					{
						// If not active and health <= 0
						if (enemies[i].Health <= 0)
						{
							// Add an explosion
							//explosionSound.Play();
							AddExplosion(enemies[i].Position);
							this.score += 10;
						}
						enemies.RemoveAt(i);
					}
				}
			}
			//update the clouds
			for (int i = enemies.Count - 1; i >= 0; i--)
			{
				enemies[i].Update(gameTime);

				if (enemies[i].IsCloud)
				{
					if (enemies[i].Active == false)
					{
						// If not active and health <= 0
						if (enemies[i].Health <= 0)
						{
							// Add an explosion
							//popSound.Play();
                            this.score += 5;
							AddPop(enemies[i].Position);
						}
						enemies.RemoveAt(i);
					}
				}
			}
		}

	
	private void UpdateCollision()
	{
		// Use the Rectangle's built-in intersect function to 
		// determine if two objects are overlapping
		Rectangle rectangle1;
		Rectangle rectangle2;

        List<Enemy> rainbowDudes = new List<Enemy>();
        List<List<DankLaser>> raindbowPewPews = new List<List<DankLaser>>();
            foreach(Player player in players){
		// Only create the rectangle once for the player
		rectangle1 = new Rectangle((int)player.Position.X,
		(int)player.Position.Y,
		player.Width,
		player.Height);

		// Do the collision between the player and the enemies
		for (int i = 0; i < enemies.Count; i++)
		{
			rectangle2 = new Rectangle((int)enemies[i].Position.X,
			(int)enemies[i].Position.Y,
			enemies[i].Width,
			enemies[i].Height);

			// Determine if the two objects collided with each
			// other
			if (rectangle1.Intersects(rectangle2))
			{
				// Subtract the health from the player based on
				// the enemy damage
				player.Health -= enemies[i].Damage;

				// Since the enemy collided with the player
				// destroy it
				enemies[i].Health = 0;

				// If the player health is less than zero we died
				if (player.Health <= 0)
					player.Active = false;
			}


		}
		rectangle1 = new Rectangle((int)player.Friend.Position.X,
		(int)player.Friend.Position.Y,
		player.Width,
		player.Height);

                // Do the collision between the player and the enemies
                for (int i = 0; i < enemies.Count; i++)
                {
                    rectangle2 = new Rectangle((int)enemies[i].Position.X,
                    (int)enemies[i].Position.Y,
                    enemies[i].Width,
                    enemies[i].Height);

                    // Determine if the two objects collided with each
                    // other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        // Subtract the health from the player based on
                        // the enemy damage

                        // Since the enemy collided with the player
                        // destroy it
                        enemies[i].Health = 0;

                    }

                }
		}
		for (int j = 0; j < enemies.Count; j++)
		{
			raindbowPewPews.Add(new List<DankLaser>());
		}           // Projectile vs Enemy Collision
		for (int i = 0; i < projectiles.Count; i++)
		{
			for (int j = 0; j < enemies.Count; j++)
			{
				// Create the rectangles we need to determine if we collided with each other
				rectangle1 = new Rectangle((int)projectiles[i].Position.X -
				projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
				projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);

				rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
				(int)enemies[j].Position.Y - enemies[j].Height / 2,
				enemies[j].Width, enemies[j].Height);

				// Determine if the two objects collided with each other
				if (rectangle1.Intersects(rectangle2))
				{
					enemies[j].Health -= projectiles[i].Damage;
					projectiles[i].Active = false;
				}
			}
		}
		for (int i = 0; i < dankLasers.Count; i++)
		{
			for (int j = 0; j < enemies.Count; j++)
			{
				// Create the rectangles we need to determine if we collided with each other

				rectangle1 = new Rectangle((int)dankLasers[i].Texture.Position.X -
				dankLasers[i].Width / 2, (int)dankLasers[i].Texture.Position.Y -
				dankLasers[i].Height / 2, dankLasers[i].Width, dankLasers[i].Height);
				rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
				(int)enemies[j].Position.Y - enemies[j].Height / 2,
				enemies[j].Width, enemies[j].Height);

				// Determine if the two objects collided with each other
				if (rectangle1.Intersects(rectangle2))
				{

					enemies[j].Health -= dankLasers[i].Damage;
					if (!rainbowDudes.Contains(enemies[j]))
					{
						rainbowDudes.Add(enemies[j]);
						raindbowPewPews[j].Add(dankLasers[i]);
					}
					dankLasers[i].Active = false;
				}
			}
		}
		for (int i = 0; i < megaLasers.Count; i++)
		{
			for (int j = 0; j < enemies.Count; j++)
			{
				// Create the rectangles we need to determine if we collided with each other
				rectangle1 = new Rectangle((int)megaLasers[i].Texture.Position.X -
				megaLasers[i].Width / 2, (int)megaLasers[i].Texture.Position.Y -
				megaLasers[i].Height / 2, megaLasers[i].Width, megaLasers[i].Height);

				rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
				(int)enemies[j].Position.Y - enemies[j].Height / 2,
				enemies[j].Width, enemies[j].Height);

				// Determine if the two objects collided with each other
				if (rectangle1.Intersects(rectangle2))
				{
					enemies[j].Health -= megaLasers[i].Damage;
				}
			}
		}
		for (int j = 0; j < rainbowDudes.Count; j++)
		{
			if (rainbowDudes[j].Health <= 0)
			{

				int gen = 1;
				for (int index = 0; index < raindbowPewPews[j].Count; index++)
				{
					if (raindbowPewPews[j][index].Generation > gen)
					{
						gen = raindbowPewPews[j][index].Generation;
					}
				}
				int detail = random.Next(8, 15) * gen;
				int max = detail * 2;
				int start = random.Next(0, max - ((max * 3) / 4));
				addDank(rainbowDudes[j].Position, start, random.Next(start + 3, max), detail, gen + 1);
			}
		}

	}
	private void AddProjectile(Vector2 position)
	{
		Projectile projectile = new Projectile();
		projectile.Initialize(GraphicsDevice.Viewport, projectileTexture, position);
		projectiles.Add(projectile);
	}
	private void addDank(Vector2 position, int start, int end, int detail, int gen)
	{

		for (int index = start; index <= end; index++)
		{
			Animation dankBullet = new Animation();
			dankBullet.Initialize(dankBulletTexture, position, (dankBulletTexture.Width / 20), dankBulletTexture.Height, 20, 1, Color.White, 1f, true);
			DankLaser dank = new DankLaser();
			dank.Initialize(GraphicsDevice.Viewport, dankBullet, position, (float)((Math.PI / detail) * index), gen);
			dankLasers.Add(dank);
		}

	}
	private void UpdateProjectiles(GameTime time)
	{
		// Update the Projectiles
		for (int i = projectiles.Count - 1; i >= 0; i--)
		{
			projectiles[i].Update();

			if (projectiles[i].Active == false)
			{
				projectiles.RemoveAt(i);
			}

		}
		for (int i = dankLasers.Count - 1; i >= 0; i--)
		{
			dankLasers[i].Update(time);
			if (dankLasers[i].Active == false)
			{
				dankLasers.RemoveAt(i);
			}
		}
		List<MegaLaser> splitLaser = new List<MegaLaser>();
		for (int i = megaLasers.Count - 1; i >= 0; i--)
		{
			List<MegaLaser> currentSplit = megaLasers[i].Update(time);
			for (int index = 0; index < currentSplit.Count; index++)
			{
				splitLaser.Add(currentSplit[index]);
			}
			if (megaLasers[i].Active == false)
			{
				megaLasers.RemoveAt(i);
			}
		}
		for (int index = 0; index < splitLaser.Count; index++)
		{
			megaLasers.Add(splitLaser[index]);
		}
	}
	private void AddExplosion(Vector2 position)
	{
	//		this.explosionSound.Play();
		Animation explosion = new Animation();
            explosion.Initialize(explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);
		explosions.Add(explosion);
	}
	private void UpdateExplosions(GameTime gameTime)
	{
		for (int i = explosions.Count - 1; i >= 0; i--)
		{
			explosions[i].Update(gameTime);
			if (explosions[i].Active == false)
			{
				explosions.RemoveAt(i);
			}
		}
	}
	private void UpdatePops(GameTime gameTime)
	{
		for (int i = pops.Count - 1; i >= 0; i--)
		{
			pops[i].Update(gameTime);
			if (pops[i].Active == false)
			{
				pops.RemoveAt(i);
			}
		}
	}
	private void UpdateSpawnTime()
	{
		if (time > 0f)
		{
			enemySpawnTime = TimeSpan.FromSeconds(time);
			time = time - 0.0001f;
			Console.WriteLine(enemySpawnTime.ToString());
		}
		//else if(!(enemySpawnTime = 0.8f))
		//{
		//	enemySpawnTime = TimeSpan.FromSeconds(0.8);
		//	Console.WriteLine(enemySpawnTime.ToString());
		//
	}
	private void AddPop(Vector2 position)
	{
		Animation pop = new Animation();
		pop.Initialize(popStrip, position, popStrip.Width / 6, popStrip.Height, 6, 60, Color.White, 1f, false);
		pops.Add(pop);
	}
	private void PlayMusic(Song song)
	{
		// Due to the way the MediaPlayer plays music,
		// we have to catch the exception. Music will play when the game is not tethered
		try
		{
			// Play the music
			MediaPlayer.Play(song);

			// Loop the currently playing song
			MediaPlayer.IsRepeating = true;
		}
		catch
		{
		}
	}
}

}
