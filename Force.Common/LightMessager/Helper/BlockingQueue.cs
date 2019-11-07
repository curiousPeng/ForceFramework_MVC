using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Force.Common.LightMessager.Helper
{
    /// <summary>
    /// 阻塞队列 from https://stackoverflow.com/questions/530211/creating-a-blocking-queuet-in-net
    ///实现队列为空时出队阻塞，队列已满时入队阻塞
    /// </summary>
    public class BlockingQueue<T>
    {

        //队列最大长度
        private readonly int _maxSize;
        //FIFO队列
        private Queue<T> _queue;
        public BlockingQueue(int maxSize)
        {
            _maxSize = maxSize;
            _queue = new Queue<T>(maxSize);
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {

            lock (_queue)
            {
                while (_queue.Count >= _maxSize)
                {
                    Monitor.Wait(_queue);
                }
                _queue.Enqueue(item);
                if (_queue.Count == 1)
                {
                    // wake up any blocked dequeue
                    Monitor.PulseAll(_queue);
                }
            }

        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public T Dequeue()
        {
            lock (_queue)
            {
                while (_queue.Count == 0)
                {
                    Monitor.Wait(_queue);
                }
                T item = _queue.Dequeue();
                if (_queue.Count == _maxSize - 1)
                {
                    // wake up any blocked enqueue
                    Monitor.PulseAll(_queue);
                }
                return item;
            }
        }
    }
}
