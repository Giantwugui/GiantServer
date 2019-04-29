using System;
using System.Collections;
using System.Collections.Generic;

namespace Giant.Frame
{
    public class CoroutineItem
    {
        private IEnumerator enumerator;

        public CoroutineItem(IEnumerator enumerator)
        {
            this.enumerator = enumerator;
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }
    }

    public static class Coroutine
    {
        private static int Sleeptime = 1;
        private static List<CoroutineItem> removeList = new List<CoroutineItem>();
        private static List<CoroutineItem> looping = new List<CoroutineItem>();
        private static List<CoroutineItem> addWaiting = new List<CoroutineItem>();


        public static void StartCoroutine(IEnumerator enumerable)
        {
            Sleeptime = 1;

            CoroutineItem item = new CoroutineItem(enumerable);

            addWaiting.Add(item);
        }

        internal static void StartCoroutine(Func<IEnumerator> doSomething)
        {
            CoroutineItem item = new CoroutineItem(doSomething.Invoke());

            addWaiting.Add(item);
        }

        public static void Update()
        {
            if (addWaiting.Count > 0)
            {
                looping.AddRange(addWaiting);
                addWaiting.Clear();
            }

            removeList.Clear();

            if (looping.Count <= 0)
            {
                Sleeptime = 500;
            }
            else
            {
                looping.ForEach(item =>
                {
                    try
                    {
                        if (item.MoveNext())
                        {
                        }
                        else
                        {
                            removeList.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
            }

            if (removeList.Count > 0)
            {
                removeList.ForEach(x => looping.Remove(x));
            }
        }
    }
}
