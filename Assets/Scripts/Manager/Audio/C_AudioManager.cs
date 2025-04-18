using UnityEngine;
using System.Collections.Generic;
using CFramework.Core;

namespace CFramework.Managers
{
    public class C_AudioManager : BaseManager<C_AudioManager>
    {
        [Tooltip("Reference to the audio configuration ScriptableObject.")]
        [SerializeField]
        private AudioConfig audioConfig;

        private Dictionary<string, AudioClip> musicDict;
        private Dictionary<string, AudioClip> sfxDict;

        private GameObject audioRoot;
        private AudioRunner runner;
        private AudioSource musicSource;
        private AudioSource sfxSource;

        /// <summary>
        /// Current volume for music (0 to 1).
        /// </summary>
        public float musicVolume { get; private set; } = 1f;

        /// <summary>
        /// Current volume for SFX (0 to 1).
        /// </summary>
        public float sfxVolume { get; private set; } = 1f;

        // 私有构造
        private C_AudioManager() { }

        public override void Initialize()
        {
            C_ResourceManager.Instance.LoadResourceAsync<AudioConfig>("AudioConfig", (config) =>
            {
                audioConfig = config;
            });

            // 创建持久化 GameObject 和协程 Runner
            audioRoot = new GameObject("C_MusicManager_Audio");
            Object.DontDestroyOnLoad(audioRoot);

            runner = audioRoot.AddComponent<AudioRunner>();

            // 添加并配置 AudioSource
            musicSource = audioRoot.AddComponent<AudioSource>();
            musicSource.loop = true;
            sfxSource = audioRoot.AddComponent<AudioSource>();

            // 构建音频字典
            musicDict = new Dictionary<string, AudioClip>();
            foreach (var info in audioConfig.musicInfos)
            {
                if (info.clip != null && !musicDict.ContainsKey(info.key))
                    musicDict.Add(info.key, info.clip);
            }

            sfxDict = new Dictionary<string, AudioClip>();
            foreach (var info in audioConfig.sfxInfos)
            {
                if (info.clip != null && !sfxDict.ContainsKey(info.key))
                    sfxDict.Add(info.key, info.clip);
            }

            Debug.Log($"[C_MusicManager] Initialized with {musicDict.Count} music and {sfxDict.Count} SFX clips.");
        }

        public override void Shutdown()
        {
            StopMusic();
            sfxSource.Stop();

            Object.Destroy(audioRoot);
            Debug.Log("[C_MusicManager] Shutdown complete.");
        }

        /// <summary>
        /// 播放背景音乐，可选循环和淡入。
        /// </summary>
        public void PlayMusic(string key, bool loop = true, float fadeTime = 0.5f)
        {
            if (!musicDict.TryGetValue(key, out var clip))
            {
                Debug.LogWarning($"[C_MusicManager] BGM key not found: {key}");
                return;
            }

            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
            musicSource.volume = musicVolume;
        }

        /// <summary>
        /// 停止当前背景音乐，支持淡出。
        /// </summary>
        public void StopMusic(float fadeTime = 0.5f)
        {
            musicSource.Stop();
        }

        /// <summary>
        /// 播放一次性音效。
        /// </summary>
        public void PlaySFX(string key, float volumeScale = 1f)
        {
            if (!sfxDict.TryGetValue(key, out var clip))
            {
                Debug.LogWarning($"[C_MusicManager] SFX key not found: {key}");
                return;
            }
            sfxSource.PlayOneShot(clip, sfxVolume * volumeScale);
        }

        /// <summary>
        /// 设置全局音乐和音效音量。
        /// </summary>
        public void SetVolume(float newMusicVolume, float newSfxVolume)
        {
            musicVolume = Mathf.Clamp01(newMusicVolume);
            sfxVolume = Mathf.Clamp01(newSfxVolume);
            musicSource.volume = musicVolume;
        }

        #region Helper MonoBehaviour
        /// <summary>
        /// 用于启动协程的内部 Runner。
        /// </summary>
        private class AudioRunner : MonoBehaviour { }
        #endregion
    }
}