using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mod.events;
using Event = Mod.events.Event;

namespace Mod.manager
{
    public class EventManager : Dictionary<Event, IEnumerable<IListener>>
    {
        private List<IListener> _listeners;

        public EventManager()
        {
            InstanceAllListeners();
            Add<EventRespawn>();
        }

        private void InstanceAllListeners()
        {
            _listeners = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.GetInterfaces().Contains(typeof(IListener)) && t.GetConstructor(Type.EmptyTypes) != null)
                    .Select(t => Activator.CreateInstance(t) as IListener)
                    .ToList();
        }

        public void Fire(Type eventType)
        {
            foreach (IListener listener in _listeners)
                if (listener.GetEvent() == eventType)
                    listener.OnEvent();
        }

        public void Add<TEvent>() where TEvent: Event, new()
        {
            Event evt = new TEvent();
            Add(evt, GetListeners(evt.GetType()));
        }

        private IEnumerable<IListener> GetListeners(Type evt) => _listeners.Where(listener => listener.GetEvent() == evt).ToArray();

        //public TListener Add<TListener>(Type eventType) where TListener: IListener, new()
        //{
        //    Event @event = Keys.FirstOrDefault(evt => evt.GetType() == eventType);
        //    if (@event == null) return new TListener();
        //    this[@event] = this[@event].Concat(new[] { typeof(TListener) });
        //    return new TListener();
        //}

        //public void Remove<TListener>(Type eventType) where TListener : Listener, new()
        //{
        //    if (ContainsKey(eventType))
        //    {
        //        var asd = this[eventType].ToList();
        //        for (var i = 0; i < asd.Count; i++)
        //        {
        //            if (asd[i].GetType() == TListener)
        //            {
                        
        //            }
        //        }
        //    }
        //}
    }
}
