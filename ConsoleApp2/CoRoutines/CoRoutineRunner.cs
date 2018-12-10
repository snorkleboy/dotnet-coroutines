using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public partial class CoRoutineRunner
    {
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
        public static void startCoroutine(CoRoutine routine)
        {
            coroutines.Add(routine);
        }
    }
    public partial class CoRoutineRunner
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
        
        private async Task update()
        {
//            Console.WriteLine("tick ");
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