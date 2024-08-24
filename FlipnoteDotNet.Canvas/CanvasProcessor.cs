using FlipnoteDotNet.Canvas.Components;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FlipnoteDotNet.Canvas
{
    internal class CanvasProcessor
    {
        private readonly CanvasModel CanvasModel;

        private readonly Atomic<bool> Running = new Atomic<bool>(false);


        public class DirtyLock
        {
            private readonly CanvasModel Model;
            private int PulseCount = 0;

            public DirtyLock(CanvasModel model)
            {
                Model = model;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void NotifyDirty()
            {
                Model.IsDirty = true;
                lock (this)
                {
                    PulseCount++;
                    Monitor.Pulse(this);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool ConsumePulse()
            {
                lock (this)
                {
                    if (PulseCount > 0)
                    {
                        PulseCount = 0;
                        return true;
                    }
                    return false;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Wait()
            {
                lock (this)
                {
                    Monitor.Wait(this, 2000);
                }
            }
        }

        private readonly DirtyLock Lock;


        public CanvasProcessor(CanvasModel canvasModel)
        {
            CanvasModel = canvasModel;
            Lock = new DirtyLock(CanvasModel);
            CanvasModel.DirtyLock = Lock;
        }

        internal void AttachComponent(CanvasComponent component)
        {
            component.DirtyLock = Lock;
        }

        internal void DetachComponent(CanvasComponent component) 
        {
            component.DirtyLock = null;
        }

        public void PushMessage()
        {
            Lock.NotifyDirty();            
        }        

        public void Execute()
        {
            Running.Value = true;
            try
            {
                var stopwatch = new Stopwatch();
                int k = 0;
                do
                {
                    if (!Lock.ConsumePulse()) 
                    {
                        Debug.WriteLine($"Wait Lock {k++}");
                        Lock.Wait();                        
                    }                    
                    stopwatch.Restart();
                    CanvasModel.Update();
                    stopwatch.Stop();
                    Debug.WriteLine($"Rendered {stopwatch.ElapsedMilliseconds}ms");
                    FrameUpdated?.Invoke(this, EventArgs.Empty);
                }
                while (Running.Value);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        private Task CurrentTask = null;

        public void Start()
        {
            if (Running.Value)
                throw new InvalidOperationException("CancasQueueProcessor instance already running");
            CurrentTask = Task.Run(Execute);
        }

        public void Stop(bool wait = false)
        {
            if (CurrentTask == null) return;
            Running.Value = false;
            if (wait)
                CurrentTask.Wait();
            CurrentTask = null;
        }

        public delegate void OnFrameUpdated(object sender, EventArgs e);
        public event OnFrameUpdated FrameUpdated;

    }
}
