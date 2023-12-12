using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calculator.Models;

public class SyncQueue
{

    private readonly Queue<string> queue = new Queue<string>();


    public void Enqueue(string item)
    {
        lock (queue)
        {

            queue.Enqueue(item);
        }
    }
    public string Dequeue()
    {
        lock (queue)
        {
            if (queue.Count == 0)
                return null;
       
            return queue.Dequeue();
        }
    }
    public IEnumerable<string> ReadAll()
    {
        lock(queue)
        {
            foreach (var item in queue)
            {
                yield return item;
            }

        }
        
    }
}
