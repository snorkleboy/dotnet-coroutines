using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class CoRoutineRunner
    {
        private TimeSpan intervalTime;
        public CoRoutineRunner(TimeSpan intervalTime)
        {
            this.intervalTime = intervalTime;
        }
        private static List<IRoutineable> coroutines = new List<IRoutineable>();

        public static bool hasWork()
        {
            return coroutines.Count > 0;
        }
        public async Task run()
        {
            Console.WriteLine("run " + DateTime.Now);

            while (true)
            {
                await Task.Delay(intervalTime);
                await update();
            }
        }
        public static void startCoroutine(params CoRoutine[] routines)
        {
            coroutines.Add(new MultiRoutine(routines));
        }
        
        
        public static void startCoroutine(IEnumerator enumerator)
        {
            coroutines.Add(new CoRoutine(enumerator));
        }
        public static void startCoroutine(params IEnumerator[] routines)
        {
            coroutines.Add(new CoRoutine(routines));
        }
        public static void startCoroutine(IEnumerable<IEnumerator> routines)
        {
            coroutines.Add(new CoRoutine(routines));
        }

        private async Task update()
        {
            Console.WriteLine("update ");
            List<IRoutineable> toRemove = new List<IRoutineable>();
            for(var i = 0;i< coroutines.Count;i++)
            {
                var coRoutine = coroutines[i];
                if (coRoutine.shouldUpdate() && !coRoutine.MoveNext())
                {
                    toRemove.Add(coRoutine);
                }
            }

            foreach (var cr in toRemove)
            {
                coroutines.RemoveAt(
                    coroutines.FindIndex((i)=>i==cr)
                );
            }
        }
    }
}