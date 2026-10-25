using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using _3902sprint0.Environment;

namespace _3902sprint0
{
    public class SwitchTerrain
    {
        private readonly Player p;

        public SwitchTerrain(Player player)
        {
            p = player;
        }

        public void UpdateTerrain(GameTime gameTime)
        {

            string terrainInput = p.Keyboard.terrain();

            if (terrainInput == "Ice")
            {
                if (p.Terrain.currentState != terrain.terrainState.Ice)
                {
                    p.Terrain.currentState = terrain.terrainState.Ice;
                    p.Terrain.newTerrainAccessed(p);
                    p.Terrain.getTerainEffect(p);
                    p.inventory.ResetItems();
                    if (p.chestManager.chests.Count > 0)
                    {
                        p.chestManager.deleteChest(p.chestManager.chests[0]);
                    }
                    p.enemyManager.RespawnEnemies(
                            EnemyType.Basic,
                            EnemyType.Basic,
                            EnemyType.Chaser,
                            EnemyType.Fleeing,
                            EnemyType.Erratic);
                    p.damage = 1f;
                    p.fireballManager.clearFireballs();

                }
            }
            else if (terrainInput == "Swamp")
            {
                if (p.Terrain.currentState != terrain.terrainState.Swamp)
                {
                    p.Terrain.currentState = terrain.terrainState.Swamp;
                    p.Terrain.newTerrainAccessed(p);
                    p.Terrain.getTerainEffect(p);
                    p.inventory.ResetItems();
                    if (p.chestManager.chests.Count > 0)
                    {
                        p.chestManager.deleteChest(p.chestManager.chests[0]);
                    }
                    p.enemyManager.RespawnEnemies(
                            EnemyType.Basic,
                            EnemyType.Basic,
                            EnemyType.Chaser,
                            EnemyType.Fleeing,
                            EnemyType.Erratic,
                            EnemyType.ToadBoss);
                    p.damage = 1f;
                    p.fireballManager.clearFireballs();

                }
            }
            else if (terrainInput == "Stone")
            {
                if (p.Terrain.currentState != terrain.terrainState.Stone)
                {
                    p.Terrain.currentState = terrain.terrainState.Stone;
                    p.Terrain.newTerrainAccessed(p);
                    p.Terrain.getTerainEffect(p);
                    p.inventory.ResetItems();
                    if (p.chestManager.chests.Count > 0)
                    {
                        p.chestManager.deleteChest(p.chestManager.chests[0]);
                    }
                    p.chestManager.CreateRandomChest();
                    p.enemyManager.RespawnEnemies(
                            EnemyType.Basic,
                            EnemyType.Basic,
                            EnemyType.Chaser,
                            EnemyType.Fleeing,
                            EnemyType.Erratic);
                    p.damage = 1f;
                    p.fireballManager.clearFireballs();

                }
            }

        }

    }
}