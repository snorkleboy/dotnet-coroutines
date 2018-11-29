using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ConsoleApp2
{
    public static class Work
    {
        public static IEnumerator testStart()
        {
            while (true)
            {
                Console.WriteLine("\n\nstarting multi routine -- ANY");
                yield return CoRoutine.any(doTimedThingShort(),doThing());
                
                Console.WriteLine("\n\nstarting multi routine -- ALL");
                yield return CoRoutine.all(doThing(),doTimedThingShort());
                
                Console.WriteLine("\n\nstarting multi routine -- ANY RANDOMS");
                yield return CoRoutine.any(doTimedThingRandom(),doTimedThingRandom(),doTimedThingRandom());
                
                Console.WriteLine("\n\nstarting multi routine -- ALL RANDOMS");
                yield return CoRoutine.all(doTimedThingRandom(),doTimedThingRandom(),doTimedThingRandom(),doTimedThingRandom());

            }
        }
        public static IEnumerator doThing()
        {
            for(var i=0;i<4;i++)
            {
                Console.WriteLine("doThing      work doThing");
                yield return null;
            }
            Console.WriteLine("doThing     starting sub routine");
            yield return doSubThing();
            Console.WriteLine("doThing    returned from sub routine");

            Console.WriteLine("doThing      work doThing");
            yield return null;
            Console.WriteLine("doThing      END");
        }
        public static IEnumerator doSubThing()
        {
            for(var i=0;i<4;i++)
            {
                Console.WriteLine("subRoutine do work      subRoutine");
                yield return TimeSpan.FromSeconds(2);
            }
            Console.WriteLine("subRoutine      starting double nested sub routine");
            yield return doSubSubThing();
            Console.WriteLine("subRoutine      returned from double nested sub routine");
        
            Console.WriteLine("subRoutine do work      subRoutine");
            yield return TimeSpan.FromSeconds(3);

            Console.WriteLine("subRoutine      END");


        }
        public static IEnumerator doSubSubThing()
        {
            for(var i=0;i<4;i++)
            {
                Console.WriteLine("doubleNested SubRoutine do work      doubleNested SubRoutine");
                yield return TimeSpan.FromSeconds(2);
            }
            Console.WriteLine("doubleNested SubRoutine      END");

        }
        public static IEnumerator doOdaThing()
        {
            for(var i=10;i<20;i++)
            {
                Console.WriteLine("doOdaThing do work     doOdaThing");

                yield return i;
            }
            Console.WriteLine("doOdaThing      END");

        }
        public static IEnumerator doTimedThingLong()
        {
            for(var i=0;i<15;i++)
            {
                Console.WriteLine("doTimedThingLong do work     doTimedThingLong");

                yield return TimeSpan.FromSeconds(2);
            }
            Console.WriteLine("doTimedThingLong      END");

        }
        public static IEnumerator doTimedThingShort()
        {
            for(var i=0;i<2;i++)
            {
                Console.WriteLine("doTimedThingShort do work     doTimedThingShort");

                yield return TimeSpan.FromSeconds(3);
            }
            Console.WriteLine("doTimedThingShort      END");

        }
        public static IEnumerator doTimedThingRandom()
        {
            var num = new Random().Next(10);
            for(var i=0;i<num;i++)
            {
                Console.WriteLine("doTimedThingRandom do work     doTimedThingRandom. randomNum = " + num);

                yield return TimeSpan.FromSeconds(4);
            }
            Console.WriteLine("doTimedThingRandom      END");

        }
    }
}