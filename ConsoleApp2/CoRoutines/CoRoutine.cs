using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public interface IRoutineable
    {
        bool shouldUpdate();
        bool MoveNext();
    }
    
    public partial class CoRoutine: IRoutineable
    {
        public List<IEnumerator> routines;
        public TimeSpan waitSpan;
        public DateTime waitStartTime;
        private IRoutineable subRoutine;
    
        public CoRoutine(IEnumerator routine)
        {
            this.routines = new List<IEnumerator> {routine};
        }
    
        public CoRoutine(params IEnumerator[] routines)
        {
            this.routines = new List<IEnumerator>();
            this.routines.AddRange(routines);
        }
    
        public CoRoutine(IEnumerable<IEnumerator> routines)
        {
            this.routines = new List<IEnumerator>();
            this.routines.AddRange(routines);
        }
    
        public bool shouldUpdate()
        {
            if (this.waitSpan != TimeSpan.Zero)
            {
                return DateTime.Now > waitStartTime + waitSpan;
            }
            else
            {
                return true;
            }
        }
    
        public bool MoveNext()
        {
            if (subRoutine != null)
            {
                if (subRoutine.shouldUpdate() && !subRoutine.MoveNext())
                {
                    subRoutine = null;
                    return MoveNext();
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return moveThisNext();
            }
        }
    
        private bool moveThisNext()
        {
            var someRan = false;
            var toRemove = new List<IEnumerator>();
            foreach (var routine in routines)
            {
                if (runRoutine(routine))
                {
                    someRan = true;
                }
                else
                {
                    toRemove.Add(routine);
                }
            }
    
            foreach (var routine in toRemove)
            {
                routines.RemoveAt(routines.IndexOf(routine));
            }
    
            return someRan;
        }
    
        private bool runRoutine(IEnumerator routine)
        {
    
            bool moved = routine.MoveNext();
            if (moved)
            {
                var thing = routine.Current;
                if (thing == null)
                {
                }
                else if (thing is TimeSpan waitSpan)
                {
                    Console.WriteLine("coroutine will wait " + waitSpan);
                    this.waitSpan = waitSpan;
                    this.waitStartTime = DateTime.Now;
                }
                else if (thing is IEnumerator subRoutine)
                {
                    if (this.subRoutine != null)
                    {
                        Console.WriteLine("\n\n!!!!setting subcoroutine even though one was already set and not null!!!!!\n\n");
                    }
    
                    this.subRoutine = new CoRoutine(subRoutine);
                }
                else if (thing is IRoutineable subCoRoutine)
                {
                    if (this.subRoutine != null)
                    {
                        Console.WriteLine("\n\n!!!!setting subcoroutine even though one was already set and not null!!!!!\n\n");
                    }
    
                    this.subRoutine = subCoRoutine;
                }
            }
    
            return moved;
        }
    }
    
    public partial class CoRoutine
    {
        public static void startCoroutine(CoRoutine routine)
        {
            CoRoutineRunner.startCoroutine(routine);
        }
        public static void startCoroutine(IEnumerator enumerator)
        {
            CoRoutineRunner.startCoroutine(enumerator);
        }
        public static void startCoroutine(params IEnumerator[] routines)
        {
            CoRoutineRunner.startCoroutine(routines);
        }
        public static void startCoroutine(IEnumerable<IEnumerator> routines)
        {
            CoRoutineRunner.startCoroutine(routines);
        }
        public static IRoutineable all(params IEnumerator[] routines)
        {
            return new AllRoutiner(EnumeratorsToCoroutines(routines));
        }
        public static IRoutineable any(params IEnumerator[] routines)
        {
            return new AnyRoutiner(EnumeratorsToCoroutines(routines));
        }
        public static IRoutineable all(params CoRoutine[] routines)
        {
            return new AllRoutiner(routines);
        }
        public static IRoutineable any(params CoRoutine[] routines)
        {
            return new AnyRoutiner(routines);
        }
        private static CoRoutine[] EnumeratorsToCoroutines(IEnumerator[] routines)
        {
            var list = new CoRoutine[routines.Length];
            var count = 0;
            foreach (var r in routines)
            {
                list[count] = (new CoRoutine(r));
                count++;
            }
            return list;
        }
    }
}
