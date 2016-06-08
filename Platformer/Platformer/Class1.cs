using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Player
    {
        Sprite sprite = new Sprite();

        public Player()
        {

        }
        public void Update (float deltaTime)
        {
            sprite.Update(deltaTime);
        }

        public void Load(ContentManager content)
        {
            sprite.Load(content, "hero");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
