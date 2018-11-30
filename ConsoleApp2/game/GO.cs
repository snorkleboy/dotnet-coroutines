using System;
using System.Collections;

namespace ConsoleApp2
{
    public class Position
    {
        public Position(int x , int y)
        {
            this.x = x;
            this.y = y;
        }
        public Position()
        {
            x = 0;
            y = 0;
        }
        public int x;
        public int y;
    }

    public class Health
    {
        public int health;

        public Health()
        {
            health = 100;
        }
    }

    public interface IWeapon
    {
        bool attack(GO target);
    }
    
    public abstract class GO
    {
        public string name;
        public Position position;

        public abstract void update();
        public abstract string render();
        public abstract void Init();
        public GO(string name)
        {
            this.name = name;
            this.position = new Position();
        }

        public void startCoroutine(IEnumerator routine)
        {
            CoRoutine.startCoroutine(routine);
        }

        public void startCoroutine(CoRoutine routine)
        {
            CoRoutine.startCoroutine(routine);
        }
    }

    public static class GameObjectFactory
    {
        public static BasicUnit makeDefault(string name)
        {
            var ran = new Random();
            var position = new Position(ran.Next(100), ran.Next(100));
            var health = new Health();
            var weapons = new IWeapon[0];
            return new BasicUnit(name,position,health,weapons);
        } 
    }
    public abstract class GameObject : GO
    {
        public int speed = 1;
        public GameObject(string name,Position position):base(name)
        {
            this.position = position;
        }
    }

    
}