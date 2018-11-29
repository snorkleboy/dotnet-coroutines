using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp2
{

    public class Game
    {
        public List<GO> gameObjects = new List<GO>();

        public void createGameObject(GO gameObj)
        {
            gameObjects.Add(gameObj);
        }

        public void destroyGameObject(GO gameObj)
        {
            gameObjects.RemoveAt(gameObjects.IndexOf(gameObj));
        }

        public async Task start()
        {
            while (true)
            {
                foreach (var go in gameObjects)
                {
                    await Task.Delay(TimeSpan.FromSeconds(.5));
                    go.update();
                } 
            }
            

        }
    }
}