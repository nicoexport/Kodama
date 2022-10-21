using System.Collections;
using Scriptable;
using Scriptable.Channels;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    /// <summary>
    ///     This class gives functionality for screen transitions and should be used in the Initialize scene
    /// </summary>
    public class Transitions : MonoBehaviour
    {
        [SerializeField] private Image _transitionImage;

        [SerializeField] private TransitionEventChannelSO _transitionEventChannel;

        private void Awake()
        {
            //_transitionImage = GetComponentInChildren<Image>();
            _transitionImage.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _transitionEventChannel.OnTransitionRequested += HandleTransitionRequest;
        }

        private void OnDisable()
        {
            _transitionEventChannel.OnTransitionRequested -= HandleTransitionRequest;
        }

        private void HandleTransitionRequest(TransitionType transitionType, float duration)
        {
            switch (transitionType)
            {
                case TransitionType.FadeIn:
                    StartCoroutine(FadeIn(duration));
                    break;
                case TransitionType.FadeOut:
                    StartCoroutine(FadeOut(duration));
                    break;
            }
        }

        private IEnumerator FadeIn(float duration)
        {
            for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime / duration)
            {
                _transitionImage.color = new Color(_transitionImage.color.r, _transitionImage.color.g,
                    _transitionImage.color.b, alpha);
                yield return null;
            }

            _transitionImage.gameObject.SetActive(false);
        }

        private IEnumerator FadeOut(float duration)
        {
            _transitionImage.gameObject.SetActive(true);
            for (float alpha = 0; alpha < 1; alpha += Time.deltaTime / duration)
            {
                _transitionImage.color = new Color(_transitionImage.color.r, _transitionImage.color.g,
                    _transitionImage.color.b, alpha);
                yield return null;
            }
        }
    }
}