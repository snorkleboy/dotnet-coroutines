using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {

            new CoRoutineRunner(TimeSpan.FromSeconds(.5f)).run();
            new Game(new GameObject[] {GameObjectFactory.makeDefault("plane"), GameObjectFactory.makeDefault("tank")})
                .start();
            MainProgram.main();
        }
    }

    class MainProgram
    {
        public static void main()
        {
            while (true)
            {
                
            }
        }
    }
}




