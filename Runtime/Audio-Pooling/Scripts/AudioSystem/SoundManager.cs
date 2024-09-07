using System;
using System.Collections.Generic;
using AudioSystem.Utils;
using UnityEngine;
using UnityEngine.Pool;

namespace AudioSystem {
    public class SoundManager : PersistentSingleton<SoundManager> {
        IObjectPool<SoundEmitter> soundEmitterPool;
        readonly List<SoundEmitter> activeSoundEmitters = new();
        public readonly LinkedList<SoundEmitter> FrequentSoundEmitters = new();

        [SerializeField] SoundLibrary soundLibrary;
        [SerializeField] SoundEmitter soundEmitterPrefab;
        [SerializeField] bool collectionCheck = true;
        [SerializeField] int defaultCapacity = 10;
        [SerializeField] int maxPoolSize = 100;
        [SerializeField] int maxSoundInstances = 30;

        public SoundBuilder CreateSoundBuilder() => new SoundBuilder(this);
        
        public void SetSoundLibrary(SoundLibrary newSoundLibrary) => soundLibrary = newSoundLibrary;

        protected override void Awake()
        {
            base.Awake();
            InitializePool();
        }

        public SoundLibrary CurrentSoundLibrary
        {
            get
            {
                if (soundLibrary == null)
                    AudioLogger.LogError("SoundLibrary reference is missing", this);
                return soundLibrary;
            }
            set => soundLibrary = value;
        }

        public SoundData GetSoundData(string nameTag)
        {
            return CurrentSoundLibrary ? soundLibrary.GetSoundData(nameTag) : null;
        }
        
        public SoundData GetSoundData(HashKey hashKey)
        {
            return CurrentSoundLibrary ? soundLibrary.GetSoundData(hashKey) : null;
        }
        
        public bool TryGetEntryHashString(string nameTag, out HashKey hashKey) {
            if (CurrentSoundLibrary)  return soundLibrary.TryGetHashString(nameTag, out hashKey);
            hashKey = default;
            return false;
        }

        public bool CanPlaySound(SoundData data) {
            if (!data.frequentSound) return true;
            
            if (FrequentSoundEmitters.Count >= maxSoundInstances) {
                try {
                    FrequentSoundEmitters.First.Value.Stop();
                    return true;
                } catch {
                    AudioLogger.Log("SoundEmitter is already released");
                }
                return false;
            }
            return true;
        }

        public SoundEmitter Get() {
            return soundEmitterPool.Get();
        }

        public void ReturnToPool(SoundEmitter soundEmitter) {
            soundEmitterPool.Release(soundEmitter);
        }

        public void StopAll() {
            foreach (var soundEmitter in activeSoundEmitters) {
                soundEmitter.Stop();
            }

            FrequentSoundEmitters.Clear();
        }

        void InitializePool() {
            soundEmitterPool = new ObjectPool<SoundEmitter>(
                CreateSoundEmitter,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                collectionCheck,
                defaultCapacity,
                maxPoolSize);
        }

        SoundEmitter CreateSoundEmitter() {
            var soundEmitter = Instantiate(soundEmitterPrefab);
            soundEmitter.gameObject.SetActive(false);
            return soundEmitter;
        }

        void OnTakeFromPool(SoundEmitter soundEmitter) {
            soundEmitter.gameObject.SetActive(true);
            activeSoundEmitters.Add(soundEmitter);
        }

        void OnReturnedToPool(SoundEmitter soundEmitter) {
            if (soundEmitter.Node != null) {
                FrequentSoundEmitters.Remove(soundEmitter.Node);
                soundEmitter.Node = null;
            }
            soundEmitter.gameObject.SetActive(false);
            activeSoundEmitters.Remove(soundEmitter);
        }

        void OnDestroyPoolObject(SoundEmitter soundEmitter) {
            Destroy(soundEmitter.gameObject);
        }
    }
}