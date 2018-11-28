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

            new CoRoutineRunner(TimeSpan.FromSeconds(1)).run();
            MainProgram.main();
        }
    }

    class MainProgram
    {
        public static void main()
        {
            CoRoutineRunner.addCoroutine(Work.doThing());
            CoRoutineRunner.addCoroutine(Work.doOdaThing());
            CoRoutineRunner.addCoroutine(Work.doTimedThing());
            while (true)
            {
            
            }
        }
    }
}




