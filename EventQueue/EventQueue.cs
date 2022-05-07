using CustomUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Driver
{

    public class EventQueue : MonoBehaviour
    {
        public delegate void EventHandler(params object[] param);

        private readonly Dictionary<SystemEvents, EventHandler> systemEvents = new Dictionary<SystemEvents, EventHandler>();
        private readonly Dictionary<GameEvents, EventHandler> gameEvents = new Dictionary<GameEvents, EventHandler>();

        #region EventQueue 
        struct EventRegistry
        {
            internal SystemEvents sysEvent;
            internal GameEvents gameEvent;
            internal object[] param;

            internal EventRegistry(SystemEvents sys, GameEvents game, params object[] param)
            {
                sysEvent = sys;
                gameEvent = game;
                this.param = param;
            }
        }
        private static readonly Queue<EventRegistry> eventQueue = new Queue<EventRegistry>();
        #endregion

        public string SubscribeSysEvent(SystemEvents e, EventHandler callback)
        {
            if (e == SystemEvents.None)
                return "EventManager: Trying to register an empty system event!";

            if (systemEvents.TryGetValue(e, out EventHandler handler))
                systemEvents[e] += callback;
            else
                systemEvents[e] = callback;
            return null;
        }

        public string UnsubscribeSysEvent(SystemEvents e, EventHandler callback)
        {
            if (e == SystemEvents.None)
                return "EventManager: Trying to register an empty system event!";

            if(!systemEvents.ContainsKey(e))
                return string.Format("System Event[{0}] is unsubscirbed when it is not registered!", EnumUtil.GetEnumDescription(e));

            systemEvents[e] -= callback;
            return null;
        }

        public string FireSystemEvent(SystemEvents e, params object[] param)
        {
            if (e == SystemEvents.None)
                return"EventManager: Trying to register an empty system event!";

            eventQueue.Enqueue(new EventRegistry(e, GameEvents.None, param));
            return null;
        }

        public string SubscribeGameEvent(GameEvents e, EventHandler callback)
        {
            if (e == GameEvents.None) return "EventManager: Trying to fire an empty game event. Event None is invalid!";

            if (gameEvents.TryGetValue(e, out EventHandler handler))
                gameEvents[e] += callback;
            else
                gameEvents[e] = callback;
            return null;
        }

        public string UnsubscribeGameEvent(GameEvents e, EventHandler callback)
        {
            if (e == GameEvents.None)
                return "EventManager: Trying to register an empty game event!";

            if (!gameEvents.ContainsKey(e))
                return string.Format("EventManager: System Event[{0}] is unsubscirbed when it is not registered!", EnumUtil.GetEnumDescription(e));

            //No Error, unsubscribe
            gameEvents[e] -= callback;
            return null;
        }

        public string FireGameEvent(GameEvents e, params object[] param)
        {
            if (e == GameEvents.None)
                return "EventManager: Trying to fire an empty game event! Event None is invalid!";

            eventQueue.Enqueue(new EventRegistry(SystemEvents.None, e, param));
            return null;
        }

        public void Update()
        {
            //Handle registered system and game events
            for (int i = 0; i < ObjectPoolCons.EventHandleRate; i++)
            {
                if (eventQueue.Count == 0) break;
                var registry = eventQueue.Dequeue();
                if (registry.sysEvent > 0) //System Event exists, send
                {
                    if (systemEvents.TryGetValue(registry.sysEvent, out EventHandler handler))
                    {
                        Debug.LogFormat("EventManager::Sending SystemEvent[{0}]", EnumUtil.GetEnumDescription(registry.sysEvent));
                        handler?.Invoke(registry.param);
                    }
                }
                else if (registry.gameEvent > 0) //game event exists, send
                {
                    if (gameEvents.TryGetValue(registry.gameEvent, out EventHandler handler))
                    {
                        Debug.LogFormat("EventManager::Sending GameEvent[{0}]", EnumUtil.GetEnumDescription(registry.gameEvent));
                        handler?.Invoke(registry.param);
                    }
                }
                else //neither exists, warn
                {
                    Debug.LogWarning("EventManager: a event has neither SysEvent nor GameEvent!");
                }
            }
        }
    }
}
