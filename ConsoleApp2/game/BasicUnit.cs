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
        public IEnumerator checkAlive()
        {
            while (true)
            {

                if (this.health.health <= 0)
                {
//                    Console.WriteLine(this.name + " check Alive -- not alive");

                    yield break;
                }
                else
                {
//                    Console.WriteLine(this.name + " check Alive -- alive");

                    yield return null;
                }
            }
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
                if (health.health > 0)
                {
                    Game.createGameObject(GameObjectFactory.makeDefault("made from " + name));
                }
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
                    moveX(point.x),
                    moveY(point.y)
                );
            }
        }
        public IEnumerator moveX(int x)
        {

            int thisSpeed = speed;
            if (position.x > x)
            {
                thisSpeed = -speed;
            }

            while (position.x - x != 0)
            {
                position.x = position.x + thisSpeed;

                yield return null;
            }
        }
        public IEnumerator moveY(int y)
        {
            int thisSpeed = speed;
            if (position.y > y)
            {
                thisSpeed = -speed;
            }

            while (position.y - y != 0)
            {
                position.y = position.y + thisSpeed;
                yield return null;
            }
        }
        public IEnumerator checkNear()
        {
            while (true)
            {
//                Console.WriteLine("Checking nearby");
                foreach (var goThing in Game.getAll())
                {
                    if (this != goThing && goThing is BasicUnit go)
                    {
                        var xdiff = go.position.x - this.position.x;
                        var ydiff = go.position.y - this.position.y;
                        var diff = (Math.Pow(xdiff, 2) + Math.Pow(ydiff, 2));
                        var dis = Math.Sqrt(diff);
                        if (dis < 10)
                        {
                            Console.WriteLine("FOUND ENEMY ");
                            target = go;
                            yield break;
                        }
                        else
                        {
//                            Console.WriteLine("Checking nearby NOTFOUND");
                        }
                    }
                }
                yield return null;
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