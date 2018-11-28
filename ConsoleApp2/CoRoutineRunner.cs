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
        private static List<CoRoutine> coroutines = new List<CoRoutine>();
        public async Task run()
        {
            Console.WriteLine("run " + DateTime.Now);

            while (true)
            {
                await Task.Delay(intervalTime);
                await update();
            }
        }

        public static void addCoroutine(IEnumerator enumerator)
        {
            coroutines.Add(new CoRoutine(enumerator));
        }

        private async Task update()
        {
            Console.WriteLine("update ");
            List<CoRoutine> toRemove = new List<CoRoutine>();
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