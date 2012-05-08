﻿using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Coral.Engine.Logger;

namespace Coral.Engine.Tender
{
    public class ProcessTender : AbstractService, IProcessTender
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IScheduler _scheduler;
        readonly ConcurrentDictionary<string, Context> _contexts = new ConcurrentDictionary<string, Context>();

        public ProcessTender(ILoggerFactory loggerFactory, IScheduler scheduler)
            : base(loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _scheduler = scheduler;
        }


        Task PostTask(string label, Action work)
        {
            var tcs = new TaskCompletionSource<object>();
            _scheduler.Post(label, () =>
            {
                try
                {
                    work();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }

        public Task AddProcess(ProcessDefinition definition)
        {
            return PostTask(
                "Add process",
                () =>
                {
                    if (!_contexts.TryAdd(definition.Name, new Context { Definition = definition }))
                    {
                        throw new Exception("Process name already present");
                    }
                    StartProcess(definition.Name);
                });
        }

        public Task RemoveProcess(string processName)
        {
            return PostTask(
                "Remove process", 
                () => { });
        }

        public Task StartProcess(string processName)
        {
            return PostTask(
                "Start process",
                () =>
                {                   
                    Context context;
                    if (!_contexts.TryGetValue(processName, out context))
                    {
                        throw new Exception("Process name not present");
                    }
                    context.Instance = new ProcessInstance(_loggerFactory, context.Definition);
                    context.Instance.Execute();
                });
        }

        class Context
        {
            public ProcessDefinition Definition { get; set; }
            public ProcessInstance Instance { get; set; }
        }
    }
}
