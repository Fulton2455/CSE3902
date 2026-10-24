using _3902sprint0;

namespace _3902sprint0
{
    /// <summaraaay>
    /// Represents the player's sprite, handling its initialization, updating, and drawing. This class implements the ISprite interface, allowing it to be used in a consistent manner with other sprite objects in the game. The PlayerSprite class manages the sprite's texture, animation states, and rendering logic, ensuring that the correct animation is displayed based on the player's actions and state.
    /// </summary>
    public class PlayerSprite : ISprite
    {

        /// <summary>
        /// The texture used for the player's sprite, which contains the various frames of animation for different actions such as walking, attacking, and dying. This texture is loaded from the game's content and is used to render the sprite on the screen.
        /// </summary>
        private Texture2D texture;
        /// <summary>
        /// The source rectangle defines the portion of the texture to be drawn, while the destination rectangle specifies where on the screen the sprite will be rendered
        /// </summary>
        private Rectangle sourceRectangle;
        private Rectangle destinationRectangle;
        /// <summary>
        /// The location of the player's sprite in the game world, represented as a Vector2. This property is used to determine where the sprite should be drawn on the screen and is updated based on the player's movement and actions. The location is also used to calculate the aim direction for actions such as shooting or interacting with objects in the game.
        /// </summary>
        private Vector2 location;
        /// Indicates whether the current animation has finished playing. This property is used to determine when to transition to a different animation state, such as returning to idle after an attack or stopping movement after a death animation. It is updated based on the current frame of the animation and the total number of frames for that animation.
        private bool animationFinished = false;

        /// The direction in which the sprite is facing, represented as a SpriteEffects value. This property is used to flip the sprite horizontally when the player changes direction, ensuring that the sprite's orientation matches the player's movement and aim direction. It is updated based on the player's input and aim direction.
        private SpriteEffects spriteDirection = SpriteEffects.None;

        /// The current frame of the animation being played, represented as an integer. This property is used to determine which portion of the texture to draw for the current animation state.
        private int currentFrame = 0;
        // The timer used to track the elapsed time since the last frame change in the animation.
        private float animationTimer = 0f;
        // The speed at which the animation frames change, represented as a float.
        private float animationSpeed = 0.15f;

        /// The width and height of each frame in the sprite's texture, used to calculate the source rectangle for rendering the correct portion of the texture.
        private int frameWidth = 32;
        private int frameHeight = 32;

        /// <summary>
        /// Defines the different animation states for the player's sprite, including idle, walking, attacking, and dying. This enumeration is used to manage the current animation state and determine which frames of the texture to display based on the player's actions and state in the game.
        /// </summary>
        public enum AnimationState
        {
            Idle,
            Walk,
            Attack,
            Death
        }

        /// <summary>
        /// The current animation state of the player's sprite, represented as an AnimationState value. This property is used to determine which frames of the texture to display and how to update the animation based on the player's actions and state in the game. It is updated when the player performs actions such as moving, attacking, or dying, and it controls the flow of the animation logic in the UpdateSprite method.
        /// </summary>
        private AnimationState currentAnimation = AnimationState.Idle;

        /// <summary>
        /// Initializes the player's sprite with the specified texture, setting up the source and destination rectangles, and initializing the location to the origin. This method is called when the sprite is first created and prepares it for rendering in the game.
        /// </summary>
        /// <param name="texture"></param>
        public void Initialize(Texture2D texture)
        {
            this.texture = texture;

            sourceRectangle = new Rectangle(0, 0, 32, 32);
            destinationRectangle = new Rectangle(0, 0, 128, 128);
            location = Vector2.Zero;
        }

        /// <summary>
        /// Updates the player's sprite based on the elapsed game time, managing the animation state and frame changes. This method is called once per frame and handles the logic for transitioning between different animation states, updating the source rectangle to display the correct portion of the texture, and tracking whether the current animation has finished playing. It ensures that the sprite's appearance reflects the player's actions and state in the game.
        /// </summary>
        /// <param name="gameTime"></param>

        public void UpdateSprite(GameTime gameTime)
        {
            animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Check if it's time to advance to the next frame in the animation based on the animation speed. If the timer exceeds the animation speed, reset the timer and update the current frame based on the current animation state. This logic ensures that the animation progresses at a consistent rate, providing smooth visual feedback for the player's actions.
            if (animationTimer >= animationSpeed)
            {
                // Reset the animation timer to zero, preparing for the next frame update in the animation cycle.
                animationTimer = 0f;




                // Update the current frame based on the current animation state, ensuring that the correct portion of the texture is displayed for each action. The logic handles looping for walking and idle animations, while attack and death animations stop at their final frames, indicating that the action has completed. This approach allows for a clear visual representation of the player's actions and state in the game.
                switch (currentAnimation)
                {

                    case AnimationState.Walk:
                        currentFrame++;
                        // Loop the walking animation by resetting the current frame to zero when it exceeds the total number of frames for the walking animation. This ensures that the walking animation continues to play smoothly as long as the player is moving.
                        if (currentFrame >= 4)
                        {
                            currentFrame = 0;
                        }
                        break;

                    case AnimationState.Idle:
                        currentFrame++;
                        // Loop the idle animation by resetting the current frame to zero when it exceeds the total number of frames for the idle animation. This ensures that the idle animation continues to play smoothly when the player is not moving.
                        if (currentFrame >= 3)
                        {
                            currentFrame = 0;
                        }
                        break;
                        

                    case AnimationState.Attack:
                        currentFrame++;
                        // Stop the attack animation at the last frame, indicating that the attack action has completed. This prevents the attack animation from looping and allows for a clear visual representation of the player's attack action.
                        if (currentFrame >= 3)
                        {
                            currentFrame = 2;
                            animationFinished = true;
                        }
                        break;

                    case AnimationState.Death:

                        currentFrame++;
                        // Stop the death animation at the last frame, indicating that the death action has completed. This prevents the death animation from looping and allows for a clear visual representation of the player's death state.
                        if (currentFrame >= 4)
                        {
                            currentFrame = 3;
                            animationFinished = true;
                        }
                        break;
                }
            }
            // Update the source rectangle based on the current animation state and frame, ensuring that the correct portion of the texture is drawn for each action. The source rectangle is calculated using the current frame and the dimensions of each frame in the texture, allowing for accurate rendering of the sprite's appearance in the game.
            switch (currentAnimation)
            {

                case AnimationState.Walk:
                    sourceRectangle = new Rectangle(
                     currentFrame * frameWidth,
                     32,
                     frameWidth,
                     frameHeight
                );
                    break;

                case AnimationState.Idle:
                    sourceRectangle = new Rectangle(
                        currentFrame * frameWidth,
                        0,
                        frameWidth,
                        frameHeight
                    );

                    break;

                case AnimationState.Attack:
                    sourceRectangle = new Rectangle(
                     currentFrame * frameWidth,
                     128,
                    frameWidth,
                    frameHeight
                  );
                    break;

                case AnimationState.Death:
                    sourceRectangle = new Rectangle(
                      currentFrame * frameWidth,
                      192,
                      frameWidth,
                      frameHeight
                );
                    break;
            }


        }

        /// <summary>
        /// Draws the player's sprite on the screen using the provided SpriteBatch, rendering the correct portion of the texture based on the current animation state and frame. This method is called once per frame and handles the rendering logic for the sprite, ensuring that it appears in the correct location and orientation in the game world. The destination rectangle is updated based on the sprite's location, and the sprite is drawn with any necessary effects, such as flipping for direction changes.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            destinationRectangle.X = (int)location.X;
            destinationRectangle.Y = (int)location.Y;
            spriteBatch.Draw(
                texture,
                destinationRectangle,
                sourceRectangle,
                Color.White,
                 0f,
                Vector2.Zero,
                spriteDirection,
                0f

            );
        }

        /// <summary>
        /// Gets the source rectangle of the sprite, which defines the portion of the texture to be drawn. This method is used by the rendering logic to determine which part of the texture corresponds to the current animation state and frame, allowing for accurate rendering of the sprite's appearance in the game.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetSourceRectangle()
        {
            return sourceRectangle;
        }

        /// <summary>
        /// Sets the current animation state of the player's sprite, updating the animation logic to reflect the new state. This method is called when the player performs actions such as moving, attacking, or dying, and it resets the current frame and animation timer to ensure that the new animation starts from the beginning. It also marks the animation as not finished, allowing for proper tracking of the animation's progress.
        /// </summary>
        /// <param name="animation"></param>
        public void SetAnimation(AnimationState animation)
        {
            // If the new animation state is different from the current one, update the current animation, reset the current frame and animation timer, and mark the animation as not finished. This ensures that the new animation starts from the beginning and plays correctly based on the player's actions and state in the game.
            if (currentAnimation != animation)
            {
                currentAnimation = animation;
                currentFrame = 0;
                animationTimer = 0f;
                animationFinished = false;
            }
        }
        /// <summary>
        /// Checks if the current animation has finished playing, returning a boolean value that indicates whether the animation has completed. This method is used to determine when to transition to a different animation state, such as returning to idle after an attack or stopping movement after a death animation. It allows for proper management of the sprite's appearance based on the player's actions and state in the game.
        /// </summary>
        /// <returns></returns>
        public bool IsAnimationFinished()
        {
            return animationFinished;
        }
        /// <summary>
        /// Sets the location of the player's sprite in the game world, updating the destination rectangle to reflect the new position. This method is called when the player moves or teleports, ensuring that the sprite is drawn in the correct location on the screen. The location is represented as a Vector2, allowing for precise positioning in the game world.
        /// </summary>
        /// <param name="newLocation"></param>
        public void SetLocation(Vector2 newLocation)
        {
            location = newLocation;
            destinationRectangle.X = (int)location.X;
            destinationRectangle.Y = (int)location.Y;
        }

        /// <summary>
        /// Checks if the current animation state matches the specified animation, returning a boolean value that indicates whether the two states are the same. This method is used to determine if the sprite is currently performing a specific action, such as walking, attacking, or dying, allowing for proper management of the sprite's appearance based on the player's actions and state in the game.
        /// </summary>
        /// <param name="animation"></param>
        /// <returns></returns>
        public bool IsAnimation(AnimationState animation)
        {
            return currentAnimation == animation;
        }

        /// <summary>
        /// Sets the direction of the sprite based on the provided aim direction, updating the sprite's orientation to face left or right. This method is called when the player changes direction, ensuring that the sprite's appearance matches the player's movement and aim direction. It uses the X component of the aim direction to determine whether to flip the sprite horizontally or keep it facing its default orientation.
        /// </summary>
        /// <param name="aimDirection"></param>
        public void SetDirection(Vector2 aimDirection)
        {
            // Update the sprite's direction based on the X component of the aim direction. If the aim direction is positive, the sprite faces right; if negative, it faces left. This ensures that the sprite's orientation matches the player's movement and aim direction in the game.
            if (aimDirection.X > 0)
            {
                spriteDirection = SpriteEffects.None;
            }
            else if (aimDirection.X < 0)
            {
                spriteDirection = SpriteEffects.FlipHorizontally;
            }
        }
    }
}


