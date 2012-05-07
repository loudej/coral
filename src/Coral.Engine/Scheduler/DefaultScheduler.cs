using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coral.Engine.Logger;

namespace Coral.Engine.Scheduler
{
    public class DefaultScheduler : AbstractService, IScheduler
    {
        private enum State
        {
            Initial,
            Starting,
            Started,
            Stopping,
            Stopped,
        }

        private readonly object _sync = new object();
        private State _state = State.Initial;
        private readonly Queue<Action> _queue = new Queue<Action>();
        private Thread _thread;

        public DefaultScheduler(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    

        public void Start()
        {
            Log.Info("Start");
            lock (_sync)
            {
                if (_state == State.Starting || _state == State.Stopping)
                {
                    throw new InvalidOperationException("Cannot start scheduler when state is " + _state);
                }
                _state = State.Starting;
                _thread = new Thread(ThreadProc);
                _thread.Start();
            }
        }

        public Task Stop()
        {
            Log.Info("Stop");
            lock (_sync)
            {
                var tcs = new TaskCompletionSource<object>();

                if (_state != State.Starting)
                {
                    throw new InvalidOperationException("Cannot post work when scheduler state is " + _state);
                }
                _state = State.Stopping;
                _queue.Enqueue(
                    () =>
                        {
                            try
                            {
                                lock (_sync)
                                {
                                    if (_state != State.Stopping)
                                    {
                                        throw new InvalidOperationException("Cannot post work when scheduler state is " + _state);
                                    }
                                    _thread = null;
                                    _state = State.Stopped;
                                    tcs.SetResult(null);
                                }
                            }
                            catch (Exception ex)
                            {
                                tcs.SetException(ex);
                            }
                        });
                Monitor.Pulse(_sync);
                return tcs.Task;
            }
        }

        public void Post(Action work)
        {
            Log.Info("Post");
            lock (_sync)
            {
                if (_state != State.Starting)
                {
                    throw new InvalidOperationException("Cannot post work when scheduler state is " + _state);
                }
                _queue.Enqueue(work);
                Monitor.Pulse(_sync);
            }
        }

        void ThreadProc()
        {
            Log.Info("Enter ThreadProc");
            var loop = true;
            while (loop)
            {
                Action work;
                lock (_sync)
                {
                    while (_queue.Count == 0)
                    {
                        Monitor.Wait(_sync);
                    }
                    work = _queue.Dequeue();

                    if (_state == State.Stopping && _queue.Count == 0)
                    {
                        loop = false;
                    }
                }

                try
                {
                    work.Invoke();
                }
                catch (Exception ex)
                {
                    //todo: log
                }
            }
            Log.Info("Exit ThreadProc");
        }
    }
}