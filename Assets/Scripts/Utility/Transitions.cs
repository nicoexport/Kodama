using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
///  This class gives functionality for screen transitions and should be used in the Initialize scene
/// </summary>

public class Transitions : MonoBehaviour
{
    [SerializeField]
    private Image _transitionImage;
    [SerializeField]
    private TransitionEventChannelSO _transitionEventChannel = default;

    private void Awake()
    {
        //_transitionImage = GetComponentInChildren<Image>();
        _transitionImage.gameObject.SetActive(false);
    }

    private void OnEnable() => _transitionEventChannel.OnTransitionRequested += HandleTransitionRequest;
    private void OnDisable() => _transitionEventChannel.OnTransitionRequested -= HandleTransitionRequest;

    private void HandleTransitionRequest(TransitionType transitionType, float duration)
    {
        Debug.Log("HandleTransitionRequest");
        switch (transitionType)
        {
            case TransitionType.FadeIn:
                Debug.Log("Case: FadeIn");
                StartCoroutine(FadeIn(duration));
                break;
            case TransitionType.FadeOut:
                Debug.Log("Case: FadeOut");
                StartCoroutine(FadeOut(duration));
                break;

            default:
                Debug.Log("Case: Default");
                break;
        }
    }

    private IEnumerator FadeIn(float duration)
    {
        Debug.Log("Fade In");
        for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime / duration)
        {
            _transitionImage.color = new Color(_transitionImage.color.r, _transitionImage.color.g, _transitionImage.color.b, alpha);
            yield return null;
        }
        _transitionImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(float duration)
    {
        Debug.Log("Fade Out");
        _transitionImage.gameObject.SetActive(true);
        for (float alpha = 0; alpha < 1; alpha += Time.deltaTime / duration)
        {
            _transitionImage.color = new Color(_transitionImage.color.r, _transitionImage.color.g, _transitionImage.color.b, alpha);
            yield return null;
        }
    }
}