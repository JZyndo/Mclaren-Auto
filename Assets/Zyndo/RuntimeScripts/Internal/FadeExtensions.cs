using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;

public static class FadeExtensions
{
    public static void SetAlphaGeneric(this Component component, float alpha)
    {
        Type type = component.GetType();

        if (type == typeof(Image))
            ((Image)component).SetAlpha(alpha);
        else if (type == typeof(MeshRenderer))
            ((MeshRenderer)component).SetAlpha(alpha);
        else if (type == typeof(CanvasGroup))
            ((CanvasGroup)component).SetAlpha(alpha);
        else if (type == typeof(Text))
            ((Text)component).SetAlpha(alpha);
		else if (type == typeof(TextMeshProUGUI))
			((TextMeshProUGUI)component).alpha = alpha;
    }

    public static float GetAlphaGeneric(this Component component)
    {
        float alpha = 0.0f;
        Type type = component.GetType();

        if (type == typeof(Image))
            alpha = ((Image)component).GetAlpha();
        else if (type == typeof(MeshRenderer))
           alpha = ((MeshRenderer)component).GetAlpha();
        else if (type == typeof(CanvasGroup))
            alpha = ((CanvasGroup)component).GetAlpha();
        else if (type == typeof(Text))
            alpha = ((Text)component).GetAlpha();
		else if (type == typeof(TextMeshProUGUI))
			alpha = ((TextMeshProUGUI)component).alpha;
        return alpha;
    }

    public static void SetAlpha(this Image image, float alpha)
    {
        var color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public static void SetAlpha(this CanvasGroup canvasGroup, float alpha)
    {
        canvasGroup.alpha = alpha;
    }

    public static float GetAlpha(this CanvasGroup canvasGroup)
    {
        return canvasGroup.alpha;
    }

    public static float GetAlpha(this Image image)
    {
        return image.color.a;
    }

    public static void SetAlpha(this MeshRenderer renderer, float alpha)
    {
        var mat = renderer.material;
        if (mat != null)
        {
            var color = mat.color;
            color.a = alpha;
            renderer.material.color = color;
        }
    }

    public static float GetAlpha(this MeshRenderer renderer)
    {
        float alpha = 0.0f;
        if(renderer.material != null)
        {
            alpha = renderer.material.color.a;
        }
        return alpha;
    }

    public static void SetAlpha(this Text text, float alpha)
    {
        var color = text.color;
        color.a = alpha;
        text.color = color;
    }

    public static float GetAlpha(this Text text)
    {
        return text.color.a;
    }
}

