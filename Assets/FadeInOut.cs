using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{

    public float alpha = 0;

    List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    List<Image> images = new List<Image>();
    List<Text> texts = new List<Text>();
    Text infoText;
    bool fadingOut = false;
    bool fadingIn = false;

    private float timeToFade = 0.3f;

    private void Awake()
    {
        spriteRenderers.AddRange(GetComponents<SpriteRenderer>());
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());

        images.AddRange(GetComponents<Image>());
        images.AddRange(GetComponentsInChildren<Image>());

        texts.AddRange(GetComponents<Text>());
        texts.AddRange(GetComponentsInChildren<Text>());

        infoText = gameObject.GetComponent<Text>();

        SetAlpha(0);
    }

    public void StartFadeIn()
    {
        fadingIn = true;
        FadeIn();
    }
    public void StartFadeOut()
    {
        fadingOut = true;
        FadeOut();

    }

    private void FadeIn()
    {
        if (alpha < 1 && fadingIn)
        {
            SetAlpha(alpha + 0.01f / timeToFade);
            Invoke("FadeIn", 0.01f);
        }
        else
            fadingIn = false;
    }

    private void FadeOut()
    {
        if (alpha > 0 && fadingOut)
        {
            SetAlpha(alpha - 0.01f / timeToFade);
            Invoke("FadeOut", 0.01f);
        }
        else
            fadingOut = false;
    }

    internal void ShowText(String text)
    {
        infoText.text = text;
        StartFadeIn();
        Invoke("StartFadeOut", 5f);
    }

    private void SetAlpha(float alpha)
    {
        this.alpha = alpha;
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
        foreach (Image sr in images)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
        foreach (Text sr in texts)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}
