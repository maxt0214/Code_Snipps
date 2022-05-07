using System.Collections.Generic;
using UnityEngine;
using CustomUtilities;
using System;
using System.Reflection;

namespace Driver
{
    /// <summary>
    /// Do not fire SystemEvents unless you know what you are doing
    /// </summary>
    public enum SystemEvents
    {
        None = 0,
        //Network
        Connect_Server,
        On_Connection,
        //Audio
        Set_Sound_Config,
        Play_Music,
        Play_Sound,
        //Special Effect
        Play_Effect,
        Effect_Over,
    }

    public enum GameEvents
    {
        None = 0,
        //User Service
        Send_Login,
        On_Login,
        Send_Register,
        On_Register,
        Send_Create_Character,
        On_Create_Character,
        Send_Enter_Game,
        On_Enter_Game,
        //Scene

        //Game Object Managing
        Preview_Character,
    }

    public class EventManager
    {
        public delegate void EventWrapper();
        public delegate void EventWrapper<T>(T arg);
        public delegate void EventWrapper<T1,T2>(T1 arg1, T2 arg2);
        public delegate void EventWrapper<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
        public delegate void EventWrapper<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
        public delegate void EventWrapper<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

        private static Dictionary<MethodInfo, EventQueue.EventHandler> listenerMapping;
        private static EventQueue eventQueue;

        public EventManager()
        {
            listenerMapping = new Dictionary<MethodInfo, EventQueue.EventHandler>();
            eventQueue = new EventQueue();
        }

        public void Update()
        {
            eventQueue.Update();
        }

        #region System Events Subscribing
        /// <summary>
        /// Fire a System Event with any number of parameters in ragnge 1~5. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void FireSystemEvent(SystemEvents e, params object[] param)
        {
            PrintError(eventQueue.FireSystemEvent(e,param));
        }

        /// <summary>
        /// 0 Parameter event register. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void SubscribeSysEvent(SystemEvents e, EventWrapper callback)
        {
            EventQueue.EventHandler wrapper = (object[] param) =>
            {
                if (!ValidateEventParams(param, 0, 0, e, GameEvents.None)) return;
                callback?.Invoke();
            };
            PrintError(SubscribeSys(e, wrapper, callback.Method));
        }
        public static void UnsubscribeSysEvent(SystemEvents e, EventWrapper callback) { UnsubscribeSys(e, callback.Method); }

        /// <summary>
        /// 1 Parameter event subscribing. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void SubscribeSysEvent<T>(SystemEvents e, EventWrapper<T> callback)
        {
            EventQueue.EventHandler wrapper = (object[] param) =>
            {
                if (!ValidateEventParams(param, 1, 1, e, GameEvents.None, typeof(T))) return;
                callback?.Invoke((T)param[0]);
            };
            PrintError(SubscribeSys(e, wrapper, callback.Method));
        }
        public static void UnsubscribeSysEvent<T>(SystemEvents e, EventWrapper<T> callback) { UnsubscribeSys(e,callback.Method); }

        /// <summary>
        /// 2 Parameter event subscribing. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void SubscribeSysEvent<T1,T2>(SystemEvents e, EventWrapper<T1,T2> callback)
        {
            EventQueue.EventHandler wrapper = (object[] param) =>
            {
                if (!ValidateEventParams(param, 2, 2, e, GameEvents.None, typeof(T1), typeof(T2))) return;
                callback?.Invoke((T1)param[0], (T2)param[1]);
            };
            PrintError(SubscribeSys(e, wrapper, callback.Method));
        }
        public static void UnsubscribeSysEvent<T1,T2>(SystemEvents e, EventWrapper<T1,T2> callback) { UnsubscribeSys(e, callback.Method); }

        /// <summary>
        /// 3 Parameter event subscribing. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void SubscribeSysEvent<T1, T2, T3>(SystemEvents e, EventWrapper<T1, T2, T3> callback)
        {
            EventQueue.EventHandler wrapper = (object[] param) =>
            {
                if (!ValidateEventParams(param, 3, 3, e, GameEvents.None, typeof(T1), typeof(T2), typeof(T3))) return;
                callback?.Invoke((T1)param[0], (T2)param[1], (T3)param[2]);
            };
            PrintError(SubscribeSys(e, wrapper, callback.Method));
        }
        public static void UnsubscribeSysEvent<T1, T2, T3>(SystemEvents e, EventWrapper<T1, T2, T3> callback) { UnsubscribeSys(e, callback.Method); }

        /// <summary>
        /// 4 Parameter event subscribing. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void SubscribeSysEvent<T1, T2, T3, T4>(SystemEvents e, EventWrapper<T1, T2, T3, T4> callback)
        {
            EventQueue.EventHandler wrapper = (object[] param) =>
            {
                if (!ValidateEventParams(param, 4, 4, e, GameEvents.None, typeof(T1), typeof(T2), typeof(T3), typeof(T4))) return;
                callback?.Invoke((T1)param[0], (T2)param[1], (T3)param[2], (T4)param[3]);
            };
            PrintError(SubscribeSys(e, wrapper, callback.Method));
        }
        public static void UnsubscribeSysEvent<T1, T2, T3, T4>(SystemEvents e, EventWrapper<T1, T2, T3, T4> callback) { UnsubscribeSys(e, callback.Method); }

        /// <summary>
        /// 5 Parameter event subscribing. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void SubscribeSysEvent<T1, T2, T3, T4, T5>(SystemEvents e, EventWrapper<T1, T2, T3, T4, T5> callback)
        {
            EventQueue.EventHandler wrapper = (object[] param) =>
            {
                if (!ValidateEventParams(param, 5, 5, e, GameEvents.None, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5))) return;
                callback?.Invoke((T1)param[0], (T2)param[1], (T3)param[2], (T4)param[3], (T5)param[4]);
            };
            PrintError(SubscribeSys(e, wrapper, callback.Method));
        }
        public static void UnsubscribeSysEvent<T1, T2, T3, T4, T5>(SystemEvents e, EventWrapper<T1, T2, T3, T4, T5> callback) { UnsubscribeSys(e, callback.Method); }
        #endregion

        #region Game Events Subscribing
        /// <summary>
        /// Fire a Game Event with any number of parameters in ragnge 1~5. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void FireGameEvent(GameEvents e, params object[] param)
        {
            PrintError(eventQueue.FireGameEvent(e, param));
        }

        /// <summary>
        /// 0 Parameter event register. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void SubscribeGameEvent(GameEvents e, EventWrapper callback)
        {
            EventQueue.EventHandler wrapper = (object[] param) =>
            {
                if (!ValidateEventParams(param, 0, 0, SystemEvents.None, e)) return;
                callback?.Invoke();
            };
            PrintError(SubscribeGame(e, wrapper, callback.Method));
        }
        public static void UnsubscribeGameEvent(GameEvents e, EventWrapper callback) { UnsubscribeGame(e, callback.Method); }

        /// <summary>
        /// 1 Parameter event subscribing. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void SubscribeGameEvent<T>(GameEvents e, EventWrapper<T> callback)
        {
            EventQueue.EventHandler wrapper = (object[] param) =>
            {
                if (!ValidateEventParams(param, 1, 1, SystemEvents.None, e, typeof(T))) return;
                callback?.Invoke((T)param[0]);
            };
            PrintError(SubscribeGame(e, wrapper, callback.Method));
        }
        public static void UnsubscribeGameEvent<T>(GameEvents e, EventWrapper<T> callback) { UnsubscribeGame(e, callback.Method); }

        /// <summary>
        /// 2 Parameter event subscribing. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void SubscribeGameEvent<T1, T2>(GameEvents e, EventWrapper<T1, T2> callback)
        {
            EventQueue.EventHandler wrapper = (object[] param) =>
            {
                if (!ValidateEventParams(param, 2, 2, SystemEvents.None, e, typeof(T1), typeof(T2))) return;
                callback?.Invoke((T1)param[0], (T2)param[1]);
            };
            PrintError(SubscribeGame(e, wrapper, callback.Method));
        }
        public static void UnsubscribeGameEvent<T1, T2>(GameEvents e, EventWrapper<T1, T2> callback) { UnsubscribeGame(e, callback.Method); }

        /// <summary>
        /// 3 Parameter event subscribing. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void SubscribeGameEvent<T1, T2, T3>(GameEvents e, EventWrapper<T1, T2, T3> callback)
        {
            EventQueue.EventHandler wrapper = (object[] param) =>
            {
                if (!ValidateEventParams(param, 3, 3, SystemEvents.None, e, typeof(T1), typeof(T2), typeof(T3))) return;
                callback?.Invoke((T1)param[0], (T2)param[1], (T3)param[2]);
            };
            PrintError(SubscribeGame(e, wrapper, callback.Method));
        }
        public static void UnsubscribeGameEvent<T1, T2, T3>(GameEvents e, EventWrapper<T1, T2, T3> callback) { UnsubscribeGame(e, callback.Method); }

        /// <summary>
        /// 4 Parameter event subscribing. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void SubscribeGameEvent<T1, T2, T3, T4>(GameEvents e, EventWrapper<T1, T2, T3, T4> callback)
        {
            EventQueue.EventHandler wrapper = (object[] param) =>
            {
                if (!ValidateEventParams(param, 4, 4, SystemEvents.None, e, typeof(T1), typeof(T2), typeof(T3), typeof(T4))) return;
                callback?.Invoke((T1)param[0], (T2)param[1], (T3)param[2], (T4)param[3]);
            };
            PrintError(SubscribeGame(e, wrapper, callback.Method));
        }
        public static void UnsubscribeGameEvent<T1, T2, T3, T4>(GameEvents e, EventWrapper<T1, T2, T3, T4> callback) { UnsubscribeGame(e, callback.Method); }

        /// <summary>
        /// 5 Parameter event subscribing. Make sure to match the number of parameters between firers and subscribers
        /// </summary>
        public static void SubscribeGameEvent<T1, T2, T3, T4, T5>(GameEvents e, EventWrapper<T1, T2, T3, T4, T5> callback)
        {
            EventQueue.EventHandler wrapper = (object[] param) =>
            {
                if (!ValidateEventParams(param, 5, 5, SystemEvents.None, e, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5))) return;
                callback?.Invoke((T1)param[0], (T2)param[1], (T3)param[2], (T4)param[3], (T5)param[4]);
            };
            PrintError(SubscribeGame(e, wrapper, callback.Method));
        }
        public static void UnsubscribeGameEvent<T1, T2, T3, T4, T5>(GameEvents e, EventWrapper<T1, T2, T3, T4, T5> callback) { UnsubscribeGame(e, callback.Method); }
        #endregion

        #region Subscribe Helpers
        private static string SubscribeSys(SystemEvents e, EventQueue.EventHandler wrapper, MethodInfo callback)
        {
            if (listenerMapping.ContainsKey(callback))
                return "EventManager: A caller trying to register for the same event twice!";

            listenerMapping.Add(callback, wrapper);
            return eventQueue.SubscribeSysEvent(e, wrapper);
        }

        private static string UnsubscribeSys(SystemEvents e, MethodInfo callback)
        {
            if (!listenerMapping.TryGetValue(callback, out var wrapper))
                return string.Format("EventManager: a listener trying to unsubscribe event[{0}] when it was never subscribed!", GetEventDescription(e, GameEvents.None));

            listenerMapping.Remove(callback);
            return eventQueue.UnsubscribeSysEvent(e, wrapper);
        }

        private static string SubscribeGame(GameEvents e, EventQueue.EventHandler wrapper, MethodInfo callback)
        {
            if (listenerMapping.ContainsKey(callback))
                return "EventManager: A caller trying to register for the same event twice!";

            listenerMapping.Add(callback, wrapper);
            return eventQueue.SubscribeGameEvent(e, wrapper);
        }

        private static string UnsubscribeGame(GameEvents e, MethodInfo callback)
        {
            if (!listenerMapping.TryGetValue(callback, out var wrapper))
                return string.Format("EventManager: a listener trying to unsubscribe event[{0}] when it was never subscribed!", GetEventDescription(SystemEvents.None, e));

            listenerMapping.Remove(callback);
            return eventQueue.UnsubscribeGameEvent(e, wrapper);
        }

        private static bool PrintError(string errmsg)
        {
            if(errmsg != null)
            {
                Debug.LogError(errmsg);
                return true;
            }
            return false;
        }
        #endregion

        #region Param Validation
        public static readonly int paramCap = 5;

        /// <summary>
        /// Validate param wih the size in range [min,max]. will compare against types passed in. 
        /// Note: Will compare param against types based on param size. Any extra type in types will be ignored
        /// </summary>
        /// <param name="types">Must be Type object</param>
        public static bool ValidateEventParams(object[] param, int minSize, int maxSize, SystemEvents sysE, GameEvents gameE, params object[] types)
        {
            if (minSize < 0 || maxSize > paramCap || minSize > maxSize) {
                Debug.LogErrorFormat("EventManager::ValidateEventParams - Event[{0}] has invalid minSize or maxSize passed in! Constrains: 0 < minSize < maxSize. maxSize < EventManager.paramCap!", GetEventDescription(sysE,gameE));
                return false;
            }

            if(param.Length < minSize || param.Length > maxSize)
            {
                Debug.LogErrorFormat("EventManager::ValidateEventParams - Event[{0}] has either invalid param size or invalid min/max size! Constrains: minSize < param.Length < maxSize", GetEventDescription(sysE, gameE));
                return false;
            }

            if (types.Length < param.Length)
            {
                Debug.LogErrorFormat("EventManager::ValidateEventParams - Event[{0}] less types passed in than param! Constrains: types.Length >= param.Length", GetEventDescription(sysE, gameE));
                return false;
            }

            for (int i = 0; i < param.Length; i++)
            {
                if(types[i] is Type type)
                {
                    if (param[i].GetType() != type)
                    {
                        Debug.LogErrorFormat("EventManager::ValidateEventParams - Event[{0}] has type mismatch! Expected[{1}] Actual Type:[{2}]", GetEventDescription(sysE, gameE), type, param[i].GetType());
                        return false;
                    }
                } else
                {
                    throw new Exception(string.Format("EventManager::ValidateEventParams - Event[{0}] detected invalid types parameter. One parameter with type[{1}] instead of Type", GetEventDescription(sysE, gameE), types[i].GetType()));
                }
            }

            return true;
        }

        private static string GetEventDescription(SystemEvents sysE, GameEvents gameE)
        {
            if (sysE != SystemEvents.None) return EnumUtil.GetEnumDescription(sysE);
            else return EnumUtil.GetEnumDescription(gameE);
        }
        #endregion
    }

    /// <summary>
    /// Event Constants. Mostly Type objects for event validation
    /// </summary>
    public static class ECons
    {
        #region Platform Generic
        public static readonly Type tInt = typeof(int);
        /// <summary>
        /// Type of Float
        /// </summary>
        public static readonly Type tFlt = typeof(float);
        public static readonly Type tStr = typeof(string);
        /// <summary>
        /// Type of gameobject
        /// </summary>
        public static readonly Type tGO = typeof(GameObject);
        /// <summary>
        /// Type of Transform
        /// </summary>
        public static readonly Type tTrans = typeof(Transform);
        /// <summary>
        /// Type of Vector3
        /// </summary>
        public static readonly Type tVec = typeof(Vector3);
        #endregion

        #region Gameplay specific
        /// <summary>
        /// Type of effect type
        /// </summary>
        public static readonly Type tFXT = typeof(EffectType);
        /// <summary>
        /// Type of Network Result
        /// </summary>
        public static readonly Type tResult = typeof(SkillBridge.Message.Result);
        #endregion
    }
}