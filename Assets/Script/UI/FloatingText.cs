using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float duration;
    private Vector3 targetOffset;
    private Vector3 startPos;
    private TextMesh text;

    public void Setup(float dur, Vector3 offset)
    {
        duration = dur;
        targetOffset = offset;
        startPos = transform.position;
        text = GetComponent<TextMesh>();

        Destroy(gameObject, duration);
    }

    private void Update()
    {
        float progress = 1f - (duration / 1f);

        // Move upward
        //transform.position = Vector3.Lerp(startPos, startPos + targetOffset, progress);

        // Fade out
        if (text != null)
        {
            Color color = text.color;
            color.a = Mathf.Lerp(1f, 0f, progress);
            text.color = color;
        }

        duration -= Time.deltaTime;
    }
}
