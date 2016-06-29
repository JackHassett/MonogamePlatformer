using AwesomePlatformer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended.Maps.Tiled;

namespace Platformer
{
    class Player
    {
        Sprite sprite = new Sprite();

        Game1 game = null;
        bool hasGravity = true;
        bool isJumping = false;

        Vector2 velocity = Vector2.Zero;
        Vector2 position = Vector2.Zero;

         
        SoundEffect jumpSound;
        SoundEffectInstance jumpSoundInsance;


        bool autoJump = true;

        public Vector2 Velocity
        {
            get { return velocity; }
        }

        public Rectangle Bounds
        {
            get { return sprite.Bounds; }
        }

        public bool IsJumping
        {
            get { return isJumping; }
        }

        public void JumpOnCollision()
        {
            autoJump = true;
        }

        public Vector2 Position
        {
            get { return sprite.position; }
        }
        private void UpdateInput(float deltaTime)
        {
            bool wasMovingLeft = velocity.X < 0;
            bool wasMovingRight = velocity.X > 0;
            Vector2 acceleration = new Vector2(0, Game1.gravity);


           

            if (Keyboard.GetState().IsKeyDown(Keys.Left) == true)
            { acceleration.X -= Game1.acceleration;
                sprite.Play();
                sprite.SetFlipped(true);
                
            }
            else if (wasMovingLeft == true)
            { acceleration.X += Game1.friction;
                
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) == true)
            {
                acceleration.X += Game1.acceleration;
                sprite.Play();
                sprite.SetFlipped(false);
            }
            else if (wasMovingRight == true)
            {
                acceleration.X -= Game1.friction;
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) == true && this.isJumping == false && hasGravity == false || autoJump == true)
            {
                autoJump = false;
                acceleration.Y -= (9999999999999 * 99999);
                this.isJumping = true;
                jumpSoundInsance.Play();
            }
            velocity += acceleration * deltaTime;
            velocity.X = MathHelper.Clamp(velocity.X, -Game1.maxVelocity.X, Game1.maxVelocity.X);
            velocity.Y = MathHelper.Clamp(velocity.Y, -Game1.maxVelocity.Y, Game1.maxVelocity.Y);
            sprite.position += velocity * deltaTime;

            if ((wasMovingLeft && (velocity.X > 0)) || (wasMovingRight && (velocity.X < 0)))
            {
                velocity.X = 0;
                sprite.Pause();
            }
            int tx = game.PixelToTile(sprite.position.X);
            int ty = game.PixelToTile(sprite.position.Y);
            bool nx = (sprite.position.X) % Game1.tile != 0;
            bool ny = (sprite.position.Y) % Game1.tile != 0;
            bool cell = game.CellAtTileCoord(tx, ty) != 0;
            bool cellright = game.CellAtTileCoord(tx + 1, ty) != 0;
            bool celldown = game.CellAtTileCoord(tx, ty + 1) != 0;
            bool celldiag = game.CellAtTileCoord(tx + 1, ty + 1) != 0;

            //if (celldown == false)
            //{
            //    acceleration = new Vector2(0, Game1.gravity);
            //}

            if (this.velocity.X == 0)
            {
                sprite.Pause();
            }

            if (this.velocity.Y > 0)
            {
                if ((celldown && !cell) || (celldiag && !cellright && nx))
                {
                    sprite.position.Y = game.TileToPixel(ty);
                    this.velocity.Y = 0;
                    hasGravity = false;
                    this.isJumping = false;
                    ny = false;
                    this.hasGravity = false;
                }
            }
            else if (this.velocity.Y < 0)
            {
                if ((cell && !celldown) || (cellright && !celldiag && nx))
                {
                    sprite.position.Y = game.TileToPixel(ty + 1);
                    this.velocity.Y = 0;
                    cell = celldown;
                    cellright = celldiag;
                    ny = false;
                    this.hasGravity = false;
                }
            }
            if (this.velocity.X > 0)
            {
                if ((cellright && !cell) || (celldiag && !celldown && ny))
                {
                    sprite.position.X = game.TileToPixel(tx);
                    this.velocity.X = 0;
                    sprite.Pause();
                }
            }
            else if (this.velocity.X < 0)
            {
                if ((cell && !cellright) || (celldown && !celldiag && ny))
                {
                    sprite.position.X = game.TileToPixel(tx + 1);
                    this.velocity.X = 0;
                    sprite.Pause();
                }
            }
        if (celldown || (nx && celldiag))
            {
                this.hasGravity = false;
                stopGravity();
                
            }
            else
            {
                this.hasGravity = true;
            }

        

            
        }

        private void stopGravity ()
        {
            velocity.Y = 0;
            hasGravity = false;

        }
        public Player(Game1 game)
        {
            this.game = game;
            
            isJumping = false;
            velocity = Vector2.Zero;
            position = Vector2.Zero;   
        }
        public void Update (float deltaTime)
        {
            UpdateInput(deltaTime);
            sprite.Update(deltaTime);
            this.position = sprite.position;
        }

        public void Load(ContentManager content)
        {
            AnimatedTexture animation = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            animation.Load(content, "walk", 12, 20);

            jumpSound = content.Load<SoundEffect>("Jump");
            jumpSoundInsance = jumpSound.CreateInstance();

            sprite.Add(animation, 0, -5);
            sprite.Pause();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

    }
}
