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
            MainProgram.main();
        }
    }

    class MainProgram
    {
        public static void main()

        {
            CoRoutineRunner.startCoroutine(Work.testStart());
;//            CoRoutineRunner.startCoroutine(Work.doThing());
//            CoRoutineRunner.startCoroutine(Work.doTimedThing());
//            CoRoutineRunner.addCoroutine(Work.doOdaThing());
//            CoRoutineRunner.addCoroutine(Work.doTimedThing());
            while (CoRoutineRunner.hasWork())
            {
                
            }
        }
    }
}




