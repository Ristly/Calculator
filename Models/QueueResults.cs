using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calculator.Models;

public class QueueResults<T>
{
    private readonly Queue<T> queue = new Queue<T>();


    public void Enqueue(T item)
    {
        lock (queue)
        {
            //while (queue.Count >= maxSize)
            //{
            //    Monitor.Wait(queue);
            //}
            queue.Enqueue(item);
            if (queue.Count == 1)
            {
                // wake up any blocked dequeue
                Monitor.PulseAll(queue);
            }
        }
    }
    public T Dequeue()
    {
        lock (queue)
        {
            while (queue.Count == 0)
            {
                Monitor.Wait(queue);
            }
            T item = queue.Dequeue();
            //if (queue.Count == maxSize - 1)
            //{
            //    // wake up any blocked enqueue
            //    Monitor.PulseAll(queue);
            //}
            return item;
        }
    }
    public IEnumerable<T> ReadAll()
    {
        foreach (T item in queue)
        {
            yield return item;
        }
    }
}
