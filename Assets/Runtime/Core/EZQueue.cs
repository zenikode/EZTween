using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZTween.Runtime.Core
{
    public class EZQueue
    {
        private readonly Queue<EZStep> _queue = new(13);
        private float _stepOrigin;
        private bool _isLooped;
        private bool _run;

        private async void Run()
        {
            try
            {
                if (_run) return;
                _run = true;
                _stepOrigin = Time.realtimeSinceStartup;
                while (_run)
                {
                        await Awaitable.EndOfFrameAsync();
                        Tick();
                }
            }
            catch (Exception e)
            {
                Clear();
                Debug.LogWarning($"EZ interrupted. {e}");
            }
        }

        private void Tick()
        {
            while (_queue.TryPeek(out var current))
            {
                var stepTime = Time.realtimeSinceStartup - _stepOrigin;
                
                if (stepTime < current.Duration)
                {
                    var progress = stepTime / current.Duration;
                    current.Action?.Invoke(new EZData(this, progress, stepTime));
                    return;
                }
                
                current.Action?.Invoke(new EZData(this, 1, current.Duration));

                _stepOrigin += current.Duration;
                GoNext();
            }
            _run = false;
        }

        private void GoNext()
        {
            if(_queue.TryDequeue(out var last))
                if (_isLooped)
                    _queue.Enqueue(last);
                else
                    EZStepPool.Remove(last);
        }

        public void Clear()
        {
            _queue.Clear();
            _isLooped = false;
            _run = false;
        }

        public void Forward()
        {
            while (_queue.TryDequeue(out var task)) 
                task.Action?.Invoke(new EZData(this, 1, task.Duration));
            Clear();
        }
        
        public EZQueue Loop() => Call(_ => _isLooped = true);

        public void Unloop() => _isLooped = false;

        public bool IsRunning() => _queue.Count > 0;

        public EZQueue Tween(float duration, Action<EZData> action)
        {
            var step = EZStepPool.Get(action, duration);
            _queue.Enqueue(step);
            Run();
            return this;
        }
        
        public EZQueue Tween(Action<EZData> action) => Tween(0.3f, action);
        
        public EZQueue Call(Action<EZData> action) => Tween(0, action);
        
        public EZQueue Delay(float duration = 0.1f) => Tween(duration, null);
        
        public EZQueue Yield() => Tween(0.001f, null);
        
        public EZQueue Wait(Func<bool> func) =>
            Tween(float.MaxValue, _ =>
            {
                if (!func.Invoke()) return;
                _stepOrigin = Time.realtimeSinceStartup;
                GoNext();
            });
    }
}