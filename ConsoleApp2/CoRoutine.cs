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

    public abstract class Routiner : IRoutineable
    {
        public List<CoRoutine> brotherRoutines;
        public Routiner(params IEnumerator[] routines)
        {
            brotherRoutines = new List<CoRoutine>();
            foreach (var enumerator in routines)
            {
                brotherRoutines.Add(new CoRoutine(enumerator));
            }
        }
        public Routiner(params CoRoutine[] routines)
        {
            brotherRoutines = new List<CoRoutine>();
            brotherRoutines.AddRange(routines);
        }
        public bool shouldUpdate()
        {
            foreach (var cr in brotherRoutines)
            {
                if (cr.shouldUpdate())
                {
                    return true;
                }
            }
            return false;
        }

        public abstract bool MoveNext();
    }
    public class AnyRoutiner : Routiner
    {
        public AnyRoutiner(params IEnumerator[] routines):base(routines)
        {
 
        }
        public AnyRoutiner(params CoRoutine[] routines):base(routines)
        {

        }
        public override bool MoveNext()
        {
            List<CoRoutine> toRemove = new List<CoRoutine>();
            foreach (var cr in brotherRoutines)
            {
                if (cr.shouldUpdate())
                {
                    if (!cr.MoveNext())
                    {
                        return false;
                    }
                }
            }

            foreach (var cr in toRemove)
            {
                brotherRoutines.RemoveAt(brotherRoutines.IndexOf(cr));
            }

            return true;
        }
    }

    public class AllRoutiner : Routiner
    {
        public AllRoutiner(params IEnumerator[] routines):base(routines)
        {
 
        }
        public AllRoutiner(params CoRoutine[] routines):base(routines)
        {

        }
        public override bool MoveNext()
        {
            bool moved = false;
            List<CoRoutine> toRemove = new List<CoRoutine>();
            foreach (var cr in brotherRoutines)
            {
                if (cr.shouldUpdate())
                {
                    if (cr.MoveNext())
                    {
                        moved = true;
                    }
                    else
                    {
                        toRemove.Add(cr);
                    }
                }
            }

            foreach (var cr in toRemove)
            {
                brotherRoutines.RemoveAt(brotherRoutines.IndexOf(cr));
            }

            return moved;
        }
    }

    public class CoRoutine: IRoutineable
    {
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
            return MoveNextHelper();
        }
    }

    private bool MoveNextHelper()
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
                    Console.WriteLine(
                        "\n\n!!!!setting subcoroutine even though one was already set and not null!!!!!\n\n");
                }

                this.subRoutine = new CoRoutine(subRoutine);
            }
            else if (thing is IRoutineable subCoRoutine)
            {
                if (this.subRoutine != null)
                {
                    Console.WriteLine(
                        "\n\n!!!!setting subcoroutine even though one was already set and not null!!!!!\n\n");
                }

                this.subRoutine = subCoRoutine;
            }
        }

        return moved;
    }
    }
}