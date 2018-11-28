using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class CoRoutine
    {
        public IEnumerator routine;
        public TimeSpan waitSpan;
        public DateTime waitStartTime;
        private CoRoutine subRoutine;
        public CoRoutine(IEnumerator routine)
        {
            this.routine = routine;
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
                return MoveNextHelper();
            }
        }

        private bool MoveNextHelper()
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
            }
            return moved;
        }
    }
}