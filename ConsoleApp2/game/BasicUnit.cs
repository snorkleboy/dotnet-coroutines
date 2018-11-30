using System;
using System.Collections;


namespace ConsoleApp2
{
    public class BasicUnit : GameObject
    {
        public Health health;
        public IWeapon[] weapons;

        public BasicUnit target;
        public BasicUnit(string name,Position position, Health health, IWeapon[] weapons):base(name , position)
        {
            this.health = health;
            this.weapons = weapons;
        }

        public override void update()
        {
            
        }
        public IEnumerator checkAlive()
        {
            while (true)
            {

                if (this.health.health <= 0)
                {
                    Console.WriteLine(this.name + " check Alive -- not alive");

                    break;
                }
                else
                {
                    Console.WriteLine(this.name + " check Alive -- alive");

                    yield return null;
                }
            }
        }

        private void die()
        {
            Console.WriteLine(this.name + " is dying");
            Game.destroyGameObject(this);
        }

        public override void Init()
        {
            Console.WriteLine(this.name + " init");
            startCoroutine(startState());
        }

        public IEnumerator startState()
        {
            Console.WriteLine(this.name + " startState");

            yield return CoRoutine.any(
                mainState(),
                checkAlive()
            );
            //breaks out of main state if checkAlive stops;
            die();
        }

        public IEnumerator mainState()
        {
            while (true)
            {
                Console.WriteLine(this.name + " MAIN STATE PATROL");

                yield return patrol();
                // breaks patrol if target found and set
                Console.WriteLine(this.name + " MAIN STATE FIGHT");

                yield return fight(target);
                //dead or other enemy dead
                Game.createGameObject(GameObjectFactory.makeDefault("another one"));
            }

        }
        public IEnumerator patrol()
        {
            Console.WriteLine(this.name + " patrol");

            yield return CoRoutine.any(
                checkNear(),
                moveAround()
            );
        }
        public IEnumerator moveAround()
        {
            var ran = new Random();
            while (true)
            {
                Console.WriteLine(this.name + " move around");

                var point = new Position(ran.Next(100), ran.Next(100));
                yield return CoRoutine.all(
                    moveX(position.x),
                    moveY(position.y)
                );
            }
        }
        public IEnumerator moveX(int x)
        {
            int thisSpeed;
            if (position.x > x)
            {
                thisSpeed = -speed;
            }

            while (position.x - x != 0)
            {
                Console.WriteLine(this.name + " moveX");
                position.x = position.x + speed;
                yield return null;
            }
        }
        public IEnumerator moveY(int y)
        {
            int thisSpeed;
            if (position.y > y)
            {
                thisSpeed = -speed;
            }

            while (position.y - y != 0)
            {
                Console.WriteLine(this.name + " moveY");

                position.y = position.y + speed;
                yield return null;
            }
        }
        public IEnumerator checkNear()
        {
            while (true)
            {
                Console.WriteLine("Checking nearby");
                yield return null;
                foreach (var goThing in Game.getAll())
                {
                    if (goThing is BasicUnit go)
                    {
                        var xdiff = go.position.x - position.x;
                        var ydiff = go.position.y - position.y;
                        var dis = Math.Sqrt(Math.Pow(xdiff, 2) - Math.Pow(ydiff, 2));

                        if (dis < 2)
                        {
                            Console.WriteLine("Checking nearby FOUND");
                            target = go;
                            yield break;
                        }
                        else
                        {
                            Console.WriteLine("Checking nearby NOTFOUND");
                        }
                    }
                }
            }

        }

        public IEnumerator fight(BasicUnit target)
        {
            while (target.health.health > 0)
            {
                
                yield return null;
                Console.WriteLine("fighting eachother");
                target.health.health -= new Random().Next(20);
            }
        }

        public override string render()
        {
            return name + " " + " position: x:(" + position.x + ")  y:(" + position.y +")   Health: (" + health.health + " )";
        }
    }
}