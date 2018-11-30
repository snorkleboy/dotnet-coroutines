using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp2
{
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
        public AnyRoutiner(params IEnumerator[] routines) : base(routines)
        {

        }

        public AnyRoutiner(params CoRoutine[] routines) : base(routines)
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
        public AllRoutiner(params IEnumerator[] routines) : base(routines)
        {

        }

        public AllRoutiner(params CoRoutine[] routines) : base(routines)
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
}
