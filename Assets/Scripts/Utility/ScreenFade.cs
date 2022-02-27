using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFade : Singleton<ScreenFade>
{
    [SerializeField]
    private GameObject image;
    protected override void Awake()
    {
        base.Awake();
    }

    public IEnumerator Require(float fadeTime)
    {
        Debug.Log("require fade");
        Image image = GetComponentInChildren<Image>();

        for (float alpha = 0; alpha < 1; alpha += Time.deltaTime / fadeTime)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
    }

    public IEnumerator Release(float fadeTime)
    {
        Debug.Log("Release Fade");
        Image image = GetComponentInChildren<Image>();

        for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime / fadeTime)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
    }
}