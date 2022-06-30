using System.Collections;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    /// <summary>
    ///     This class gives functionality for screen transitions and should be used in the Initialize scene
    /// </summary>
    public class Transitions : MonoBehaviour
    {
        [SerializeField] Image _transitionImage;

        [SerializeField] TransitionEventChannelSO _transitionEventChannel;

        void Awake()
        {
            //_transitionImage = GetComponentInChildren<Image>();
            _transitionImage.gameObject.SetActive(false);
        }

        void OnEnable()
        {
            _transitionEventChannel.OnTransitionRequested += HandleTransitionRequest;
        }

        void OnDisable()
        {
            _transitionEventChannel.OnTransitionRequested -= HandleTransitionRequest;
        }

        void HandleTransitionRequest(TransitionType transitionType, float duration)
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

        IEnumerator FadeIn(float duration)
        {
            for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime / duration)
            {
                _transitionImage.color = new Color(_transitionImage.color.r, _transitionImage.color.g,
                    _transitionImage.color.b, alpha);
                yield return null;
            }

            _transitionImage.gameObject.SetActive(false);
        }

        IEnumerator FadeOut(float duration)
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