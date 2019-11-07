using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Force.Common.LightMessager.Pool
{
    internal interface IPooledWapper : IDisposable
    {
        DateTime LastGetTime { set; get; }
    }

    internal class ObjectPool<T> : IDisposable where T : IPooledWapper
    {
        private static int _id = 0;
        private int _minRetained;
        private int _maxRetained;
        private ConcurrentBag<T> _objects;
        private Func<ObjectPool<T>, T> _objectGenerator;
        private SemaphoreSlim sync;
        private bool _disposed;
        public bool IsDisposed { get { return _disposed; } }
        public int Id { get { return _id; } }

        public ObjectPool(Func<ObjectPool<T>, T> objectGenerator, int minRetained = 0, int maxRetained = 50)
        {
            if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
            Interlocked.Increment(ref _id);
            _objects = new ConcurrentBag<T>();
            _objectGenerator = objectGenerator;
            _disposed = false;
            _minRetained = minRetained;
            _maxRetained = maxRetained;
            sync = new SemaphoreSlim(_maxRetained, _maxRetained);

            // 预先初始化
            if (minRetained > 0)
            {
                Parallel.For(0, minRetained, i => _objects.Add(_objectGenerator(this)));
            }
        }

        public T Get()
        {
            sync.Wait();
            T item;
            if (!_objects.TryTake(out item))
            {
                item = _objectGenerator(this);
            }
            item.LastGetTime = DateTime.Now;
            return item;
        }

        public void Put(T item)
        {
            _objects.Add(item);
            sync.Release();
        }

        public void Dispose()
        {
            // 必须为true
            Dispose(true);
            // 通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源
                foreach (var item in _objects)
                {
                    item.Dispose();
                }
            }

            // 清理非托管资源

            // 让类型知道自己已经被释放
            _disposed = true;
            sync.Dispose();
        }
    }
}
