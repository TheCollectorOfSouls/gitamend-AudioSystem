using System;
using System.Collections.Generic;
using AudioSystem.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioSystem {
    public class MusicManager : PersistentSingleton<MusicManager> {
        const float crossFadeTime = 1.0f;
        float _fading;
        AudioSource _current;
        AudioSource _previous;
        readonly Queue<AudioClip> _playlist = new();
        private AudioClip[] _loopPlayList;
        private bool _crossFadeEnabled = true;
        
        [SerializeField] MusicLibrary musicLibrary;
        [SerializeField] List<AudioClip> initialPlaylist;
        [SerializeField] AudioMixerGroup musicMixerGroup;

        public MusicLibrary CurrentMusicLibrary
        {
            get
            {
                if(!musicLibrary)
                    AudioLogger.LogError("MusicLibrary reference is missing", this);
                return musicLibrary;
            }
            set => musicLibrary = value;
        }

        protected void Start()
        {
            foreach (var clip in initialPlaylist) {
                AddToPlaylist(clip);
            }
        }
        
        public void EnableCrossFade(bool value) => _crossFadeEnabled = value;
        
        public void CreateLoopingPlaylist(AudioClip[] audioClips)
        {
            _loopPlayList = new AudioClip[audioClips.Length];
            Array.Copy(sourceArray: audioClips, destinationArray: _loopPlayList, audioClips.Length);
        }
        
        public void AddToPlaylist(string nameTag) {
            if(!CurrentMusicLibrary) return;

            var audioClip = musicLibrary.GetMusicClip(nameTag);
            if(!audioClip) return;
            
            AddToPlaylist(musicLibrary.GetMusicClip(nameTag));
        }

        public void AddToPlaylist(AudioClip clip) {
            _playlist.Enqueue(clip);
            if (_current == null && _previous == null) {
                PlayNextTrack();
            }
        }

        public void Clear() => _playlist.Clear();

        public void PlayNextTrack() {
            if (_playlist.TryDequeue(out AudioClip nextTrack)) {
                Play(nextTrack, loop: false, _crossFadeEnabled);
            }
        }

        public void Play(string nameTag, bool loop = false, bool crossFade = false)
        {
            if(!CurrentMusicLibrary) return;
            Play(musicLibrary.GetMusicClip(nameTag), loop, crossFade);
        }

        public void Play(AudioClip clip, bool loop = false, bool crossFade = false) {
            if (_current && _current.clip == clip && _current.isPlaying) return;

            if (_previous) {
                Destroy(_previous);
                _previous = null;
            }
            _previous = _current;

            _current = gameObject.AddComponent<AudioSource>();
            _current.clip = clip;
            _current.outputAudioMixerGroup = musicMixerGroup; // Set mixer group
            _current.loop = loop; // For playlist functionality, we want tracks to play once
            _current.volume = crossFade? 0 : 1;
            _current.bypassListenerEffects = true;
            _current.Play();

            if(crossFade)
            {
                _fading = 0.001f;
            }
            else
            {
                _fading = 0.0f;
                DestroyPrevious();
            }
        }

        void Update() {
            HandleCrossFade();

            if (_current && !_current.isPlaying) 
            {
                if(_playlist.Count > 0) 
                {
                    PlayNextTrack();
                }

                else if(_playlist.Count <= 0 && _loopPlayList.Length > 0) 
                {
                    foreach (var audioClip in _loopPlayList)
                    {
                        AddToPlaylist(audioClip);
                    }
                    PlayNextTrack();
                }
            }
        }

        void HandleCrossFade() {
            if (_fading <= 0f) return;
            
            _fading += Time.deltaTime;

            float fraction = Mathf.Clamp01(_fading / crossFadeTime);

            // Logarithmic fade
            float logFraction = fraction.ToLogarithmicFraction();

            if (_previous) _previous.volume = 1.0f - logFraction;
            if (_current) _current.volume = logFraction;

            if (fraction >= 1) {
                _fading = 0.0f;
                DestroyPrevious();
            }
        }
        
        void DestroyPrevious()
        {
            if (!_previous) return;
            Destroy(_previous);
            _previous = null;
        }
    }
}