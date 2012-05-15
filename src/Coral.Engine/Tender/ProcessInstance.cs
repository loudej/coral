using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Coral.Engine.Logger;

namespace Coral.Engine.Tender
{
    public class ProcessInstance
    {
        private readonly TaskCompletionSource<object> _tcs = new TaskCompletionSource<object>();

        public ProcessInstance(ILoggerFactory loggerFactory, ProcessDefinition definition)
        {
            Definition = definition;
            Log = loggerFactory.Create(GetType());
            Output = loggerFactory.Create("Process." + definition.Name);
            Exited = _tcs.Task;
        }

        public ILogger Log { get; set; }
        public ILogger Output { get; set; }
        public ProcessDefinition Definition { get; set; }
        public Process Process { get; set; }
        public Task Exited { get; set; }


        public void Execute()
        {
            var info = new ProcessStartInfo
            {
                WorkingDirectory = Definition.WorkingDirectory,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                ErrorDialog = false,
                CreateNoWindow = false,
            };
            
            if (IsLinux)
            {
                info.FileName = "sh";
                info.Arguments = "-c \"" + Definition.Command.Replace("\"", "\\\"") + "\"";
            }
            else
            {
                info.FileName = "cmd.exe";
                info.Arguments = "/D /C " + Definition.Command;
            }
            
            Log.Debug("Executing {0}", Definition.Command);
            Process = Process.Start(info);            

            Process.OutputDataReceived += OutputDataReceived;
            Process.ErrorDataReceived += OutputDataReceived;
            Process.Exited += (a, b) => _tcs.TrySetResult(null);
            Process.EnableRaisingEvents = true;

            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
        }

        bool IsLinux
        {
            get 
            {
                var platform = (int) Environment.OSVersion.Platform;
                return (platform == 4) || (platform == 6) || (platform == 128);
            }
        }
        
        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output.Data(e.Data);
        }

        public Task Terminate()
        {
            var tcs = new TaskCompletionSource<object>();

            try
            {
                Exited.ContinueWith(
                    _ =>
                    {
                        Log.Info("**{0}** terminated", Definition.Name);
                        tcs.TrySetResult(null);
                    });

                if (!Process.HasExited)
                {
                    // TODO: send graceful shutdown signal? 
                    // what's the windows mechanism for that?
                    Log.Info("**{0}** exit", Definition.Name);
                    //entry.Process.Kill();
                    Process.StandardInput.WriteLine();
                }
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }
    }
}
