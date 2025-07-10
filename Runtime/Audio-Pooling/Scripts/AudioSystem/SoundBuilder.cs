using AudioSystem.Utils;
using UnityEngine;

namespace AudioSystem {
    public class SoundBuilder {
        readonly SoundManager _soundManager;
        Vector3 _position = Vector3.zero;
        private Transform _parent = null;
        bool _randomPitch;
        private string _cachedName;
        private HashKey _cachedHashKey;
        private SoundEmitter _cachedEmitter;
        private SoundData _previousLoopingSoundData;
        
        public SoundEmitter GetCachedEmitter => _cachedEmitter;

        public SoundBuilder(SoundManager soundManager) {
            this._soundManager = soundManager;
        }
        
        public SoundBuilder WithParent(Transform parent) {
            this._parent = parent;
            return this;
        }

        public SoundBuilder WithPosition(Vector3 position) {
            this._position = position;
            return this;
        }

        public SoundBuilder WithRandomPitch() {
            this._randomPitch = true;
            return this;
        }
        
        public void Play(string nameTag) {
            if (string.IsNullOrWhiteSpace(nameTag)) {
                Debug.LogError("nameTag is null");
                return;
            }
            
            if(nameTag == _cachedName) {
                Play(_soundManager.GetSoundData(_cachedHashKey));
                return;
            }

            if (!_soundManager.TryGetEntryHashString(nameTag, out _cachedHashKey)) return;
            
            _cachedName = nameTag;
            Play(_soundManager.GetSoundData(_cachedHashKey));
        }
        
        public void StopCachedEmitter()
        {
            if(!_cachedEmitter) return;
            if(_cachedEmitter.IsActive) _cachedEmitter.Stop();
            _previousLoopingSoundData = null;
            _cachedEmitter = null;
        }
        
        public void SetCachedEmitterVolume(float volume) 
        {
            if(!_cachedEmitter) return;
            if(_cachedEmitter.IsActive) _cachedEmitter.SetVolume(volume);
        }

        public void Play(SoundData soundData) {
            if (soundData == null) {
                Debug.LogError("SoundData is null");
                return;
            }
            
            if (!_soundManager.CanPlaySound(soundData)) return;
            SoundEmitter soundEmitter = _soundManager.Get();
            soundEmitter.Initialize(soundData);
            soundEmitter.transform.position = _position;
            soundEmitter.transform.parent = _soundManager.transform;

            if (_randomPitch) {
                soundEmitter.WithRandomPitch();
            }

            if (soundData.frequentSound) {
                soundEmitter.Node = _soundManager.FrequentSoundEmitters.AddLast(soundEmitter);
            }
            
            soundEmitter.Play();
        }
        
        private void SetCachedEmitter()
        {
            if(_cachedEmitter) ResetCachedEmitter();
            _cachedEmitter = _soundManager.Get();
            _cachedEmitter.OnStopped += ResetCachedEmitter;
        }

        private void ResetCachedEmitter()
        {
            if(!_cachedEmitter) return;
            _cachedEmitter.OnStopped -= ResetCachedEmitter;
            _cachedEmitter = null;
        }

        private void ClearPreviousLooping()
        {
            if (_previousLoopingSoundData == null) return;
            if(_previousLoopingSoundData.loop && _cachedEmitter)
                StopCachedEmitter();
        }
    }
}