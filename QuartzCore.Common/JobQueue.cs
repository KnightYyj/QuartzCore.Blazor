using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Common
{
  public class JobQueue<T> where T: new()
    {
        private static ConcurrentQueue<T> JobExecuteQueue { get; set; }

        public JobQueue()
        {
            JobExecuteQueue = new ConcurrentQueue<T>();
        }

        public void Enqueue(T value)
        {
            JobExecuteQueue.Enqueue(value);
        }

       
    }
}
