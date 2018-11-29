namespace ConsoleApp2
{
    public class Position
    {
        public int x;
        public int y;
    }

    public class Health
    {
        public int health;
    }

    public interface IWeapon
    {
        bool attack(GO target);
    }
    
    public abstract class GO
    {
        public string name;
        public abstract void update();
        public abstract string render();
        public GO(string name)
        {
            this.name = name;
        }

    }

    public class GameObject : GO
    {
        public Position position;
        public Health health;
        public IWeapon[] weapons;
        public GameObject(string name,Position position, Health health, IWeapon[] weapons):base(name)
        {
            this.position = position;
            this.health = health;
            this.weapons = weapons;
        }
        public override void update()
        {
            
        }

        public override string render()
        {
            return "x";
        }
    }
}