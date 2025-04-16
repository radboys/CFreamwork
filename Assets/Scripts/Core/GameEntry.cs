using UnityEngine;

namespace CFramework.Core
{
    public class GameEntry : MonoBehaviour
    {
        private void Awake()
        {
            var existsEntry = FindObjectsByType<GameEntry>(FindObjectsSortMode.None);
            if (existsEntry.Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            InitializeFramework();
        }

        private void InitializeFramework()
        {
            Debug.Log("[GameEntry] Initializing Framework...");

            FrameworkBootstrap.Initialize();

            Debug.Log("[GameEntry] Framework Initialized.");
        }
    }
}
