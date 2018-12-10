using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class GameObjectList
    {
        public List<GO> gameObjects = new List<GO>();

        public List<GO> getAll()
        {
            return gameObjects;
        }

        public GameObjectList(GO[] startObjects)
        {
            gameObjects.AddRange(startObjects);
        }
        public void createGameObject(GO gameObj)
        {
            gameObjects.Add(gameObj);
            gameObj.Init();
        }

        public void destroyGameObject(GO gameObj)
        {
            gameObjects.RemoveAt(gameObjects.IndexOf(gameObj));
        }
    }

    public class Game
    {
        private static GameObjectList list;
        public static void createGameObject(GO gameObj)
        {
            list.createGameObject(gameObj);
        }

        public static void destroyGameObject(GO gameObj)
        {
            list.destroyGameObject(gameObj);
        }
        public static List<GO> getAll()
        {
            return list.getAll();
        }

        public Game(GO[] startObjects)
        {
            list = new GameObjectList(startObjects);
        }
        public void start()
        {
            CoRoutine.startCoroutine(run());
        }

        public IEnumerator run()
        {
            foreach (var go in list.getAll())
            {
                go.Init();
            }
            while (true)
            {
//                Console.WriteLine("game update " + list.getAll().Count);

                foreach (var go in list.getAll())
                {
                    go.update();
                    Console.WriteLine(go.render());
                }
                yield return null;
            }
        }

    }
}