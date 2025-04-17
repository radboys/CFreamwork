using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CFramework.Core;

namespace CFramework.Managers
{
    /// <summary>
    /// 事件类型枚举：定义所有可用的全局事件
    /// </summary>
    public enum E_EventType
    {
        // 自定义额外事件可继续往后添加…
    }
    /// <summary>
    /// 事件消息基类，用于在同一个字典中存放不同泛型事件
    /// </summary>
    public abstract class EventInfoBase { }

    /// <summary>
    /// 封装带参数事件的订阅器
    /// </summary>
    /// <typeparam name="T">事件数据类型</typeparam>
    public class EventInfo<T> : EventInfoBase
    {
        public UnityAction<T> actions;

        public EventInfo(UnityAction<T> action)
        {
            actions += action;
        }
    }

    /// <summary>
    /// 封装无参事件的订阅器
    /// </summary>
    public class EventInfo : EventInfoBase
    {
        public UnityAction actions;

        public EventInfo(UnityAction action)
        {
            actions += action;
        }
    }

    /// <summary>
    /// 事件中心：提供基于枚举的类型化订阅/发布机制
    /// </summary>
    public class EventCenter : BaseManager<EventCenter>
    {
        // 事件字典：事件类型 → 事件订阅信息
        private readonly Dictionary<E_EventType, EventInfoBase> eventDic =
            new Dictionary<E_EventType, EventInfoBase>();

        // 私有构造，确保通过 BaseManager<Create>() 进行懒加载
        private EventCenter() { }

        public override void Initialize()
        {
            Debug.Log("[EventCenter] Initializing...");
            eventDic.Clear();
            Debug.Log("[EventCenter] Initialized.");
        }

        public override void Shutdown()
        {
            Debug.Log("[EventCenter] Shutting down...");
            eventDic.Clear();
            Debug.Log("[EventCenter] Shutdown.");
        }

        /// <summary>
        /// 添加一个带参数的事件监听
        /// </summary>
        public void AddEventListener<T>(E_EventType eventType, UnityAction<T> listener)
        {
            if (eventDic.TryGetValue(eventType, out var baseInfo))
            {
                var info = baseInfo as EventInfo<T>;
                if (info == null)
                {
                    Debug.LogError($"[EventCenter] Event type mismatch for {eventType}");
                    return;
                }
                info.actions += listener;
            }
            else
            {
                eventDic[eventType] = new EventInfo<T>(listener);
            }
        }

        /// <summary>
        /// 添加一个无参数的事件监听
        /// </summary>
        public void AddEventListener(E_EventType eventType, UnityAction listener)
        {
            if (eventDic.TryGetValue(eventType, out var baseInfo))
            {
                var info = baseInfo as EventInfo;
                if (info == null)
                {
                    Debug.LogError($"[EventCenter] Event type mismatch for {eventType}");
                    return;
                }
                info.actions += listener;
            }
            else
            {
                eventDic[eventType] = new EventInfo(listener);
            }
        }

        /// <summary>
        /// 移除一个带参数的事件监听
        /// </summary>
        public void RemoveEventListener<T>(E_EventType eventType, UnityAction<T> listener)
        {
            if (eventDic.TryGetValue(eventType, out var baseInfo))
            {
                var info = baseInfo as EventInfo<T>;
                if (info != null)
                {
                    info.actions -= listener;
                }
            }
        }

        /// <summary>
        /// 移除一个无参数的事件监听
        /// </summary>
        public void RemoveEventListener(E_EventType eventType, UnityAction listener)
        {
            if (eventDic.TryGetValue(eventType, out var baseInfo))
            {
                var info = baseInfo as EventInfo;
                if (info != null)
                {
                    info.actions -= listener;
                }
            }
        }

        /// <summary>
        /// 触发带参数事件
        /// </summary>
        public void EventTrigger<T>(E_EventType eventType, T eventData)
        {
            if (eventDic.TryGetValue(eventType, out var baseInfo))
            {
                var info = baseInfo as EventInfo<T>;
                info?.actions.Invoke(eventData);
            }
            else
            {
                Debug.LogError($"[EventCenter] Event doesn't exist");
            }
        }

        /// <summary>
        /// 触发无参数事件
        /// </summary>
        public void EventTrigger(E_EventType eventType)
        {
            if (eventDic.TryGetValue(eventType, out var baseInfo))
            {
                var info = baseInfo as EventInfo;
                info?.actions.Invoke();
            }
            else
            {
                Debug.LogError($"[EventCenter] Event doesn't exist");
            }
        }

        /// <summary>
        /// 清除指定事件的所有监听
        /// </summary>
        public void ClearEvent(E_EventType eventType)
        {
            if (eventDic.ContainsKey(eventType))
                eventDic.Remove(eventType);
        }

        /// <summary>
        /// 清除所有事件及其所有监听
        /// </summary>
        public void ClearAll()
        {
            eventDic.Clear();
        }
    }
}
