using System;
using UnityEngine;

namespace CFramework.Core
{
    public static class FrameworkBootstrap
    {
        private static bool isInitialized = false;

        public static void Initialize()
        {
            if (isInitialized)
            {
                return;
            }

            isInitialized = true;

            InitializeManager();
        }

        private static void InitializeManager()
        {
            //Initialize managers
            Debug.Log("[FrameworkBootstrap] Initializing managers...");

            //todo


            Debug.Log("[FrameworkBootstrap] Managers initialized.");
        }
    }
}
