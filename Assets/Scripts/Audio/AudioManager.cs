using System;
using System.Collections;
using Kodama.Pooling;
using Kodama.Scriptable.Channels;
using Kodama.Utility;
using UnityEngine;

namespace Kodama.Audio {
    public class AudioManager : Singleton<AudioManager> {
        [SerializeField] private AudioCueChannelSO _sfxChannel;
        [SerializeField] private AudioCueChannelSO _musicChannel;
        [SerializeField] private GameObject _soundEmitterPrefab;
        [SerializeField] private float _musicFadeDuration = 0.5f;
        private SoundEmitter _currentMusicTrack;
        private SoundEmitter _nextMusicTrack;
        private GameObjectPool _soundEmitterPool;
        

        protected override void Awake() {
            base.Awake();
            _soundEmitterPool = new GameObjectPool(transform, _soundEmitterPrefab, 12);
            var musicChild  = new GameObject("Music_Emitters");
            musicChild.transform.SetParent(transform);
            _currentMusicTrack = Instantiate(_soundEmitterPrefab, musicChild.transform).GetComponent<SoundEmitter>();
            _nextMusicTrack = Instantiate(_soundEmitterPrefab, musicChild.transform).GetComponent<SoundEmitter>();
            _currentMusicTrack.gameObject.SetActive(false);
            _nextMusicTrack.gameObject.SetActive(false);
        }

        private void OnEnable() {
            _sfxChannel.OnAudioCueRequested += PlayAudioCue;
            _musicChannel.OnAudioCueRequested += PlayMusic;
        }

        private void OnDisable() {
            _sfxChannel.OnAudioCueRequested -= PlayAudioCue;
            _musicChannel.OnAudioCueRequested -= PlayMusic;
        }
        
        public void PauseMusic() {
            if (!_currentMusicTrack) {
                return;
            }

            _currentMusicTrack.Pause();
        }

        public void ResumeMusic() {
            if (!_currentMusicTrack) {
                return;
            }

            _currentMusicTrack.Resume();
        }

        public void StopMusic() {
            if (!_currentMusicTrack) {
                return;
            }
            FadeOut(_currentMusicTrack, _musicFadeDuration, () => {
                _currentMusicTrack.Stop();
                _currentMusicTrack.gameObject.SetActive(false);
            });
        }

        public void StopMusicImmediate() {
            if (!_currentMusicTrack) {
                return;
            }
            _currentMusicTrack.Stop();
            _currentMusicTrack.gameObject.SetActive(false);
        }

        private void PlayAudioCue(AudioCueRequestData audioCueRequestData) {
            var clipsToPlay = audioCueRequestData.AudioCue.GetClips();
            int numberOfClips = clipsToPlay.Length;

            for (int i = 0; i < numberOfClips; i++) {
                var soundEmitter = _soundEmitterPool.Request().GetComponent<SoundEmitter>();

                if (soundEmitter == null) {
                    return;
                }

                if (!audioCueRequestData.AudioCue.Looping) {
                    soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                }
                
                soundEmitter.PlayAudioClip(clipsToPlay[i], audioCueRequestData.AudioConfig,
                    audioCueRequestData.AudioCue.Looping, audioCueRequestData.Position);

            }
        }

        private void PlayMusic(AudioCueRequestData audioCueRequestData) {
            var clipToPlay = audioCueRequestData.AudioCue.GetRandomClip();

            if (_currentMusicTrack == null || _nextMusicTrack == null) {
                return;
            }

            _nextMusicTrack.gameObject.SetActive(true);
            _nextMusicTrack.PlayAudioClip(
                clipToPlay, 
                audioCueRequestData.AudioConfig, 
                audioCueRequestData.AudioCue.Looping, 
                audioCueRequestData.Position);

            if (_currentMusicTrack.enabled && _currentMusicTrack.IsPlaying()) {
                FadeOut(_currentMusicTrack, _musicFadeDuration, () => {
                });
            }

            FadeIn(_nextMusicTrack, audioCueRequestData.AudioConfig.Volume, _musicFadeDuration, () => {
                    (_currentMusicTrack, _nextMusicTrack) = (_nextMusicTrack, _currentMusicTrack);
                    _nextMusicTrack.gameObject.SetActive(false);
            });

        }

        private void FadeOut(SoundEmitter soundEmitter, float durationInSeconds,
            Action fadeOutFinished = default) =>
            StartCoroutine(FadeOutEnumerator(soundEmitter, durationInSeconds, fadeOutFinished));

        private IEnumerator FadeOutEnumerator(SoundEmitter soundEmitter, float durationInSeconds,
            Action fadeOutFinished) {
            for (float volume = soundEmitter.GetVolume(); volume > 0; volume -= Time.deltaTime / durationInSeconds) {
                soundEmitter.SetVolume(volume);
                yield return null;
            }
            fadeOutFinished.Invoke();
        }

        private void FadeIn(SoundEmitter soundEmitter, float volume, float durationInSeconds,
            Action fadeInFinished = default) =>
            StartCoroutine(FadeInEnumerator(soundEmitter, volume, durationInSeconds, fadeInFinished));

        private IEnumerator FadeInEnumerator(SoundEmitter soundEmitter, float volume, float durationInSeconds,
            Action fadeInFinished) {
            for (float vol = 0; vol < volume; vol += Time.deltaTime / durationInSeconds) {
                soundEmitter.SetVolume(vol);
                yield return null;
            }

            fadeInFinished?.Invoke();
        }
        
        private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter) {
            soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
            soundEmitter.Stop();
            _soundEmitterPool.Return(soundEmitter.gameObject);
        }
    }
}