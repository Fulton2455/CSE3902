using _3902sprint0.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    public class fireballManager
{
        public List<Fireball> fireballs;
        public List<Fireball2> fireballs2;

        public IReadOnlyList<Fireball> Fireballs => fireballs;
        public IReadOnlyList<Fireball2> Fireballs2 => fireballs2;        private Texture2D fireballTexture;
        private Texture2D detonateTexture;
        private Room room;

        public fireballManager(Texture2D fireballTexture, Texture2D detonateTexture)
        {
            fireballs = new List<Fireball>();
            fireballs2 = new List<Fireball2>();

            this.fireballTexture = fireballTexture;
            this.detonateTexture = detonateTexture;
          

        }
        public void SetRoom(Room room)
        {
            this.room = room;
        }


        public void createFireball(int fireballtype, Vector2 position, Vector2 direction, bool isHostile)
        {
          
            
            if (fireballtype == 1)
            {
                FireballSprite fireballSprite = new FireballSprite();
                detonateSprite detonateSprite = new detonateSprite();

                fireballSprite.Initialize(fireballTexture);
                detonateSprite.Initialize(detonateTexture);
                Fireball newFireball = new Fireball(fireballTexture, fireballSprite, detonateSprite, room);
                newFireball.generateEntity(position, direction, isHostile);
                fireballs.Add(newFireball);
            }
            else if (fireballtype == 2)
            {
                FireballSprite fireballSprite = new FireballSprite();
                fireballSprite.Initialize(fireballTexture);
                Fireball2 newFireball2 = new Fireball2(fireballTexture, fireballSprite, room);
                newFireball2.generateEntity(position, direction, isHostile);
                fireballs2.Add(newFireball2);
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach (Fireball fireball in fireballs)
            {
                if (fireball.finishedDetonation)
                {
                    fireballs.Remove(fireball);
                    break;
                }
                fireball.Update(gameTime);
            }
            foreach (Fireball2 fireball2 in fireballs2)
            {
                if (fireball2.finishedDetonation) { 
                    fireballs2.Remove(fireball2);
                    break;}
                fireball2.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Fireball fireball in fireballs)
            {
                fireball.Draw(spriteBatch);
            }
            foreach (Fireball2 fireball2 in fireballs2)
            {
                fireball2.Draw(spriteBatch);
            }
        }
    }
}
