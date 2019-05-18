using System;
using System.Collections;
using System.Collections.Generic;

namespace Giant.Share
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
        private static readonly List<CoroutineItem> removeList = new List<CoroutineItem>();
        private static readonly List<CoroutineItem> looping = new List<CoroutineItem>();
        private static readonly List<CoroutineItem> addWaiting = new List<CoroutineItem>();

        public static void StartCoroutine(IEnumerator enumerable)
        {
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

            if (looping.Count > 0)
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
