namespace NEventStore.Persistence.Sql
{
    using System;
    using System.Threading;
#if FRAMEWORK
    using System.Web;
#endif
    using NEventStore.Logging;

    public class ThreadScope<T> : IDisposable where T : class
    {
#if FRAMEWORK
        private readonly HttpContext _context = HttpContext.Current;
#endif
        private readonly T _current;
        private readonly ILog _logger = LogFactory.BuildLogger(typeof (ThreadScope<T>));
        private readonly bool _rootScope;
        private readonly string _threadKey;
        private bool _disposed;
        private readonly ThreadLocal<T> _data = new ThreadLocal<T>();

        public ThreadScope(string key, Func<T> factory)
        {
            _threadKey = typeof (ThreadScope<T>).Name + ":[{0}]".FormatWith(key ?? string.Empty);

            T parent = Load();
            _rootScope = parent == null;
            _logger.Debug(Messages.OpeningThreadScope, _threadKey, _rootScope);

            _current = parent ?? factory();

            if (_current == null)
            {
                throw new ArgumentException(Messages.BadFactoryResult, "factory");
            }

            if (_rootScope)
            {
                Store(_current);
            }
        }

        public T Current
        {
            get { return _current; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _disposed)
            {
                return;
            }

            _logger.Debug(Messages.DisposingThreadScope, _rootScope);
            _disposed = true;
            if (!_rootScope)
            {
                return;
            }

            _logger.Verbose(Messages.CleaningRootThreadScope);
            Store(null);

            var resource = _current as IDisposable;
            if (resource == null)
            {
                return;
            }

            _logger.Verbose(Messages.DisposingRootThreadScopeResources);
            resource.Dispose();

            _data.Dispose();
        }

        private T Load()
        {
#if FRAMEWORK
            if (_context != null)
            {
                return _context.Items[_threadKey] as T;
            }
#endif
            return _data.Value;
        }

        private void Store(T value)
        {
#if FRAMEWORK
            if (_context != null)
            {
                _context.Items[_threadKey] = value;
                return;
            }
#endif
            _data.Value = value;
        }
    }
}