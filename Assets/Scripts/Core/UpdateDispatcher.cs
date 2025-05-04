using System;
using System.Collections.Generic;
using UnityEngine;

namespace CFramework.Core
{
    public class UpdateDispatcher : MonoBehaviour
    {
        public static UpdateDispatcher Instance { get; private set; }
        private Dictionary<UpdatePhase, List<IUpdatable>> updatables = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Debug.Log("<color=orange>[UpdateDispatcher]</color> Created.");

            foreach (UpdatePhase phase in Enum.GetValues(typeof(UpdatePhase)))
            {
                updatables[phase] = new List<IUpdatable>();
            }
        }

        public void Register(IUpdatable target)
        {
            if (!updatables.ContainsKey(target.Phase))
            {
                updatables[target.Phase] = new List<IUpdatable>();
            }

            updatables[target.Phase].Add(target);
        }

        public void Unregister(IUpdatable target)
        {
            if (updatables.TryGetValue(target.Phase, out var list))
            {
                list.Remove(target);
            }
        }

        private void Update()
        {
            foreach (UpdatePhase phase in Enum.GetValues(typeof(UpdatePhase)))
            {
                if (updatables.TryGetValue(phase, out var list))
                {
                    list.ForEach(updatable => updatable.OnUpdate());
                }
            }
        }

        private void OnDestroy()
        {
            Debug.Log("<color=orange>[UpdateDispatcher]</color> Destroyed.");
        }
    }
}
