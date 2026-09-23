using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using _3902sprint0.Environment;

namespace _3902sprint0
{
    /// <summary>
    /// Represents the player character in the game, handling movement, aiming, and actions based on keyboard and mouse input. The Player class manages the player's location, velocity, speed, and acceleration, as well as the associated sprite for rendering. It utilizes keyboardController and mousecontroller classes to process input and update the player's state accordingly.
    /// </summary>
    public class Player : IDamageable
    {
        // Assumed from the existing screen-clamp values (1820-128, 1280-128): Location is
        // the player's top-left corner, and the player occupies a 128x128 box.
        public const int Width = 128;
        public const int Height = 128;

        // The player's current location in the game world.
        public Vector2 Location;
        public terrain Terrain;
        // The player's current velocity, which is updated based on input and acceleration.
        public Vector2 Velocity;
        // The player's maximum movement speed, which determines how fast the player can move.
        public float Speed = 200f;

        // The player's acceleration, which affects how quickly the player can change their velocity. This value may be adjusted in the future to accommodate different terrains or gameplay mechanics.
        public float Acceleration = 500f;

        // New: basic health so hazard tiles (fire) have something to affect. Wire this
        // up to a HUD/health-bar whenever that exists.
        public int Health { get; private set; } = 6;

        private KeyboardController Keyboard;
        private Inventory inventory;
        private Mousecontroller Mouse;
        private Room currentRoom;
        public Vector2 AimDirection;

        // The player's sprite, which is responsible for rendering the player character on the screen and managing animations.
        private PlayerSprite PlayerSprite1;

        public Rectangle Bounds => new Rectangle((int)Location.X, (int)Location.Y, Width, Height);

        /// <summary>
        /// Initializes a new instance of the Player class with the specified starting location. The constructor sets up the player's initial position, creates a new PlayerSprite for rendering, and initializes the keyboard and mouse controllers for handling input.
        /// </summary>
        /// <param name="startingLocation"></param>
        public Player(Vector2 startingLocation, Inventory inventory)
        {
            Location = startingLocation;
            PlayerSprite1 = new PlayerSprite();
            Keyboard = new KeyboardController();
            Mouse = new Mousecontroller();
            this.inventory = inventory;

            Terrain = new terrain();
            Terrain.currentState = terrain.terrainState.Stone;

        }

        /// <summary>
        /// Assigns the room the player is currently in, so movement can be resolved
        /// against that room's tiles (walls, blocks, doors, hazards, stairs).
        /// </summary>
        public void SetRoom(Room room)
        {
            currentRoom = room;
        }

        /// <summary>
        /// Applies damage from a hazard tile (e.g. Fire). Ignored while already dying.
        /// </summary>
        public void TakeDamage(int amount)
        {
            if (PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Death))
                return;

            Health -= amount;
            if (Health <= 0)
            {
                Health = 0;
                Die();
            }
        }

        /// <summary>
        /// Initializes the player's sprite with the provided texture. This method sets the sprite's location to the player's current position and prepares it for rendering with the specified texture.
        /// </summary>
        /// <param name="texture"></param>
        public void InitializeSprite(Texture2D texture)
        {
            PlayerSprite1.SetLocation(Location);
            PlayerSprite1.Initialize(texture);
        }

        /// <summary>
        /// Draws the player's sprite using the provided SpriteBatch. This method is responsible for rendering the player character on the screen, ensuring that the sprite is drawn at the correct location and with the appropriate animation state.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerSprite1.Draw(spriteBatch);
        }
        /// <summary>
        /// Updates the player's state based on input and game logic. This method processes keyboard and mouse input to determine movement direction, aim direction, and actions such as attacking or dying. It also updates the player's location, velocity, and sprite animation state accordingly.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            foreach (Iitem item in inventory.items)
            {
                item.Update(gameTime);
            }



            // Update the keyboard and mouse controllers to process input
            Keyboard.update();
            Vector2 movementDirection = Keyboard.direction;
            useItem(gameTime);
            // Update the mouse controller to determine the aim direction based on the player's current location
            Mouse.update(Location);
            AimDirection = Mouse.AimDirection;
            // If the player is not in the death animation, update the sprite's direction based on the aim direction
            if (!PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Death))
                PlayerSprite1.SetDirection(AimDirection);
            // If the player is in the death animation, stop movement and update the sprite without changing its state (helped made by Chatgpt free)
            if (PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Death))
            {
                Velocity = Vector2.Zero;
                PlayerSprite1.UpdateSprite(gameTime);
                return;
            }
            // If the player presses the die key, stop movement and set the sprite to the death animation (helped made by Chatgpt free)
            if (Keyboard.die())
            {
                Velocity = Vector2.Zero;
                PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Death);
                PlayerSprite1.UpdateSprite(gameTime);
                return;
            }
            // If the player is in the attack animation, continue moving and updating the sprite until the animation finishes, then return to the appropriate state (walk or idle) (helped made by Chatgpt free)
            if (PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Attack))
            {
                Move(gameTime, movementDirection);
                Location.X = MathHelper.Clamp(Location.X, 0, 1820 - 128);
                Location.Y = MathHelper.Clamp(Location.Y, 0, 1280 - 128);
                PlayerSprite1.SetLocation(Location);

                PlayerSprite1.UpdateSprite(gameTime);
                // If the attack animation has finished, return to the appropriate state (walk or idle) based on movement direction
                if (PlayerSprite1.IsAnimationFinished())
                {
                    // If the player is moving, set the sprite to the walk animation; otherwise, set it to idle
                    if (movementDirection != Vector2.Zero)
                    {
                        PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Walk);
                    }
                    else
                    {
                        PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Idle);
                    }
                }
                // Update the sprite's location and return to avoid further processing in this frame
                return;
            }
            // If the player presses the left mouse button, initiate an attack by setting the sprite to the attack animation and updating it for this frame
            if (Mouse.IsLeftClickPressed())
            {
                Attack();
                PlayerSprite1.UpdateSprite(gameTime);
                return;
            }

            // Move the player based on the current movement direction and update the player's location, ensuring it stays within the game boundaries
            Move(gameTime, movementDirection);
            Location.X = MathHelper.Clamp(Location.X, 0, 1820 - 128);
            Location.Y = MathHelper.Clamp(Location.Y, 0, 1280 - 128);
           
            newTerrain(gameTime);
        
                // Update the sprite's location to match the player's current position
                PlayerSprite1.SetLocation(Location);
            // Set the sprite's animation state based on whether the player is moving or idle
            if (movementDirection != Vector2.Zero)
            {
                PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Walk);
            }
            else
            {
                PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Idle);
            }
            // Update the sprite for this frame, ensuring that the correct animation is displayed based on the player's current state and actions
            PlayerSprite1.UpdateSprite(gameTime);
        }
        /// <summary>
        /// Calculates the terrain acceleration for the player, which affects how quickly the player can change their velocity. This method is designed to accommodate different terrains in the future, allowing for varied movement mechanics based on the environment. Currently, it returns a fixed acceleration value of 500f.
        /// </summary>
        /// <returns></returns>
        private  float GetTerrainAcceleration()
        {

            //will build in a way to have differnt terrains later
         //   if (newroom)
           // {
           //     Speed = 200f;
           //     Acceleration = 500f;
            //}
            Terrain.getTerainEffect(this);
            return Terrain.Acceleration;
        }
        /// <summary>
        /// Moves the player based on the provided movement direction and updates the player's location and velocity accordingly. This method calculates the change in position based on the elapsed time since the last frame, the player's acceleration, and the current movement direction. It also ensures that the player's velocity does not exceed the maximum speed and applies deceleration when no movement input is detected. Movement is resolved one axis at a time against the current Room so the player can slide along walls, gets stopped by solid tiles, pushes PushableBlockTile out of the way, and auto-unlocks locked doors when carrying the right key.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="movementDirection"></param>
        private void Move(GameTime gameTime, Vector2 movementDirection)
        {
            // Calculate the elapsed time since the last frame in seconds, which is used to determine how much the player's position and velocity should change during this update cycle.
            float deltaTime =
                (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Get the terrain acceleration, which may be adjusted in the future to accommodate different terrains or gameplay mechanics.
            float acceleration = GetTerrainAcceleration();
            // Update the player's velocity based on the movement direction and acceleration. If the player is moving, increase the velocity in the direction of movement, ensuring that it does not exceed the maximum speed. If the player is not moving, apply deceleration to gradually reduce the velocity to zero.
            if (movementDirection != Vector2.Zero)
            {
                // Normalize the movement direction to ensure consistent movement speed in all directions.
                Velocity += movementDirection * acceleration * deltaTime;
                // If the player's velocity exceeds the maximum speed, normalize it and scale it to the maximum speed to maintain consistent movement behavior.
                if (Velocity.Length() > Speed)
                {
                    Velocity.Normalize();
                    Velocity *= Speed;
                }

               
            }
            else
            {
                // Apply deceleration to gradually reduce the player's velocity when no movement input is detected. This ensures that the player comes to a stop smoothly rather than abruptly.
                if (Velocity.Length() > 0)
                {
                    // Normalize the velocity vector and scale it by the acceleration and elapsed time to determine the amount of deceleration to apply.
                    Velocity -= Vector2.Normalize(Velocity) * acceleration * deltaTime;
                    if(Velocity.Length() < acceleration * deltaTime)
            
                        Velocity = Vector2.Zero;
                    }
                }

            Vector2 delta = Velocity * deltaTime;
            MoveAxis(new Vector2(delta.X, 0));
            MoveAxis(new Vector2(0, delta.Y));

            if (currentRoom != null)
            {
                currentRoom.ApplyHazards(Bounds, this);
                currentRoom.CheckStairs(Bounds);
            }
        }

        /// <summary>
        /// Attempts to move along a single axis (delta has only X or only Y set).
        /// Checked against the current Room so walls/blocks/locked doors can stop it;
        /// zeroes velocity on that axis when blocked instead of leaving it to keep
        /// pushing into the obstacle every frame.
        /// </summary>
        /// <param name="delta"></param>
        private void MoveAxis(Vector2 delta)
        {
            if (delta == Vector2.Zero)
                return;

            Vector2 attemptedLocation = Location + delta;
            Rectangle attemptedBounds = new Rectangle((int)attemptedLocation.X, (int)attemptedLocation.Y, Width, Height);

            if (currentRoom == null || currentRoom.CanEnter(attemptedBounds, Vector2.Normalize(delta), inventory))
            {
                Location = attemptedLocation;
            }
            else
            {
                if (delta.X != 0) Velocity.X = 0;
                if (delta.Y != 0) Velocity.Y = 0;
            }
        }
        /// <summary>
        /// Checks if the player has requested to quit the game by pressing the escape key. This method delegates the check to the keyboard controller, which handles input processing and determines whether the quit action has been triggered.
        /// </summary>
        /// <returns></returns>
        public bool IsQuit()
        {
            return Keyboard.isQuit();
        }

        /// <summary>
        /// Initiates an attack action for the player by setting the player's sprite to the attack animation. This method is called when the player presses the left mouse button, and it updates the sprite's animation state to reflect the attack action. The actual attack logic, such as dealing damage or interacting with enemies, would be handled elsewhere in the game code.
        /// </summary>
        private void Attack()
        {
            
                PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Attack);
            

        }
        private void useItem(GameTime gameTime)
        {
            int itemNumber = Keyboard.items();
            if (itemNumber >= 0 && itemNumber < inventory.items.Count)
            {
                Iitem item = inventory.items[itemNumber];

                item.Use(this, gameTime);
            }

        }
        /// <summary>
        /// Initiates the player's death sequence by setting the player's sprite to the death animation and stopping all movement. This method is called when the player presses the die key, and it updates the sprite's animation state to reflect the death action. The actual game over logic, such as ending the game or restarting, would be handled elsewhere in the game code.
        /// </summary>
        private void Die()
        {
            
                PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Death);
                Velocity = Vector2.Zero;


        }
        private void newTerrain(GameTime gameTime)
        {
            


            string terrainInput = Keyboard.terrain();

                if (terrainInput == "Ice")
                {
                    if (Terrain.currentState != terrain.terrainState.Ice)
                    {
                        Terrain.currentState = terrain.terrainState.Ice;
                        Terrain.newTerrainAccessed(this);
                        Terrain.getTerainEffect(this);
                        inventory.ResetItems();
                    

                }
                }
                else if (terrainInput == "Swamp")
                {
                    if (Terrain.currentState != terrain.terrainState.Swamp)
                    {
                        Terrain.currentState = terrain.terrainState.Swamp;
                        Terrain.newTerrainAccessed(this);
                        Terrain.getTerainEffect(this);
                        inventory.ResetItems();
                        
                   
                }
                }
                else if (terrainInput == "Stone")
                {
                    if (Terrain.currentState != terrain.terrainState.Stone)
                    {
                        Terrain.currentState = terrain.terrainState.Stone;
                        Terrain.newTerrainAccessed(this);
                        Terrain.getTerainEffect(this);
                        inventory.ResetItems();
                   
                }
                }
            
        }
    }
}