using System;
using System.Collections;
using Pooling;
using Scriptable;
using UnityEngine;
using Utility;

namespace Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] AudioCueChannelSo _sfxChannel;
        [SerializeField] AudioCueChannelSo _musicChannel;
        [SerializeField] GameObject _soundEmitterPrefab;
        [SerializeField] float _musicFadeDuration = 1f;
        SoundEmitter _currentMusicTrack;
        GameObjectPool _soundEmitterPool;

        protected override void Awake()
        {
            base.Awake();
            _soundEmitterPool = new GameObjectPool(transform, _soundEmitterPrefab, 12);
        }

        void OnEnable()
        {
            _sfxChannel.OnAudioCueRequested += PlayAudioCue;
            _musicChannel.OnAudioCueRequested += PlayMusic;
        }

        void OnDisable()
        {
            _sfxChannel.OnAudioCueRequested -= PlayAudioCue;
            _musicChannel.OnAudioCueRequested -= PlayMusic;
        }

        void PlayAudioCue(AudioCueRequestData audioCueRequestData)
        {
            var clipsToPlay = audioCueRequestData.AudioCue.GetClips();
            var numberOfClips = clipsToPlay.Length;

            for (var i = 0; i < numberOfClips; i++)
            {
                var soundEmitter = _soundEmitterPool.Request().GetComponent<SoundEmitter>();

                if (soundEmitter == null) return;
                soundEmitter.PlayAudioClip(clipsToPlay[i], audioCueRequestData.AudioConfig,
                    audioCueRequestData.AudioCue.Looping, audioCueRequestData.Position);

                if (!audioCueRequestData.AudioCue.Looping)
                    soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
            }
        }

        void PlayMusic(AudioCueRequestData audioCueRequestData)
        {
            var clipsToPlay = audioCueRequestData.AudioCue.GetClips();
            var numberOfClips = clipsToPlay.Length;

            for (var i = 0; i < numberOfClips; i++)
            {
                var soundEmitter = _soundEmitterPool.Request().GetComponent<SoundEmitter>();
                if (!soundEmitter) return;

                if (_currentMusicTrack != null && _currentMusicTrack.IsPlaying())
                    FadeOut(_currentMusicTrack, _musicFadeDuration, obj => { _soundEmitterPool.Return(obj); });

                _currentMusicTrack = soundEmitter;
                _currentMusicTrack.PlayAudioClip(clipsToPlay[i], audioCueRequestData.AudioConfig,
                    audioCueRequestData.AudioCue.Looping, audioCueRequestData.Position);
                FadeIn(_currentMusicTrack, audioCueRequestData.AudioConfig.Volume, _musicFadeDuration);

                if (!audioCueRequestData.AudioCue.Looping)
                    soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
            }
        }

        void FadeOut(SoundEmitter soundEmitter, float durationInSeconds, Action<GameObject> fadeOutFinished = default)
        {
            StartCoroutine(FadeOutEnumerator(soundEmitter, durationInSeconds, fadeOutFinished));
        }

        IEnumerator FadeOutEnumerator(SoundEmitter soundEmitter, float durationInSeconds,
            Action<GameObject> fadeOutFinished)
        {
            for (var volume = soundEmitter.GetVolume(); volume > 0; volume -= Time.deltaTime / durationInSeconds)
            {
                soundEmitter.SetVolume(volume);
                yield return null;
            }

            fadeOutFinished?.Invoke(soundEmitter.gameObject);
        }

        void FadeIn(SoundEmitter soundEmitter, float volume, float durationInSeconds,
            Action<GameObject> fadeInFinished = default)
        {
            StartCoroutine(FadeInEnumerator(soundEmitter, volume, durationInSeconds, fadeInFinished));
        }

        IEnumerator FadeInEnumerator(SoundEmitter soundEmitter, float volume, float durationInSeconds,
            Action<GameObject> fadeInFinished)
        {
            for (float vol = 0; vol < volume; vol += Time.deltaTime / durationInSeconds)
            {
                soundEmitter.SetVolume(volume);
                yield return null;
            }

            fadeInFinished?.Invoke(soundEmitter.gameObject);
        }

        public void PauseMusic()
        {
            if (!_currentMusicTrack) return;
            _currentMusicTrack.Pause();
        }

        public void ResumeMusic()
        {
            if (!_currentMusicTrack) return;
            _currentMusicTrack.Resume();
        }

        public void StopMusic()
        {
            if (!_currentMusicTrack) return;
            FadeOut(_currentMusicTrack, 0.5f, o => { _soundEmitterPool.Return(o); });
        }

        void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
        {
            soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
            soundEmitter.Stop();
            _soundEmitterPool.Return(soundEmitter.gameObject);
        }
    }
}