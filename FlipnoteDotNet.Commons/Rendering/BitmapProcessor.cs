using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Commons.Rendering
{
    public class BitmapKey : object 
    { 
        public BitmapProcessor Processor { get; private set; }

        public BitmapKey(BitmapProcessor processor)
        {
            Processor = processor;
        }

        public void UseBitmap(Action<Bitmap> action) => Processor.UseBitmap(this, action);
        public void Refresh() => Processor.Refresh(this);

        internal void Deactivate() => Processor = null;
    }
    public class BitmapProcessor : IDisposable
    {        
        class Item
        {            
            public Bitmap Bitmap;
            public Func<Bitmap> UpdateFunc;
            public Action<BitmapKey> UpdateFinishedCallback;

            public bool NeedsRefreshing = true;
            public Item(Func<Bitmap> updateFunc, Action<BitmapKey> updateFinishedCallback)
            {                
                UpdateFunc = updateFunc;
                UpdateFinishedCallback = updateFinishedCallback;
            }            
        }
        
        private readonly ConcurrentDictionary<BitmapKey, Item> Items = new ConcurrentDictionary<BitmapKey, Item>();
        private readonly ConcurrentQueue<BitmapKey> PendingRemove = new ConcurrentQueue<BitmapKey>();
        private readonly ConcurrentQueue<(BitmapKey Key, Item Item)> PendingAdd = new ConcurrentQueue<(BitmapKey Key, Item Item)>();

        public BitmapKey AddBitmap(Func<Bitmap> updater, Action<BitmapKey> updateFinishedCallback)
        {
            var key = new BitmapKey(this);
            PendingAdd.Enqueue((key, new Item(updater, updateFinishedCallback)));            
            return key;
        }

        public void Remove(BitmapKey key) => PendingRemove.Enqueue(key);

        private object lIsWorking = new object();
        private bool pIsWorking = false;

        private bool IsWorking 
        { 
            get { lock (lIsWorking) return pIsWorking; } 
            set { lock (lIsWorking) pIsWorking = value; }
        }

        public void RunAsync() => Task.Run(Run);

        private void Run()
        {
            if (IsWorking)
                throw new InvalidOperationException("BitmapProcessor is already running");
            IsWorking = true;

            while (IsWorking)
            {
                while (PendingAdd.TryDequeue(out var kv))
                    Items.TryAdd(kv.Key, kv.Item);

                //foreach (var kv in Items)
                Parallel.ForEach(Items, kv =>
                {
                    var key = kv.Key;
                    var item = kv.Value;
                    if (item.NeedsRefreshing)
                    {
                        var bmp = item.UpdateFunc();
                        lock (key)
                        {
                            item.Bitmap?.Dispose();
                            item.Bitmap = bmp;
                        }
                        item.NeedsRefreshing = false;
                        item.UpdateFinishedCallback(key);
                    }
                });
                Thread.Sleep(20);
                while (PendingRemove.TryDequeue(out var key))
                {
                    Items.TryRemove(key, out _);
                    key.Deactivate();
                }
            }
        }

        public bool UseBitmap(BitmapKey key, Action<Bitmap> action)
        {
            if (!Items.TryGetValue(key, out var item))
                return false;
            lock (key) action(item.Bitmap);
            return true;
        }

        public void Refresh(BitmapKey key)
        {
            if (!Items.TryGetValue(key, out var item))
                return;
            item.NeedsRefreshing = true;
        }

        public void Dispose()
        {
            IsWorking = false;
        }
    }
}
