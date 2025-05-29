using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactiveBG : MonoBehaviour
{

    private static ReactiveBG instance;
    public static ReactiveBG Instance { get => instance; }

    [SerializeField] private Image image;
    [SerializeField] private float minTransparent = 0.5f;
    [SerializeField] private float ResetTime = 0.5f;
    private float currentTime;
    private float currentAlpha;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        currentTime = Time.time;
    }
    private void Update()
    {
        if(Time.time >= currentTime + ResetTime)
        {
            SetTransparency(minTransparent);
        }
    }

    public void SetTransparency(float value)
    {
        if (value != minTransparent) currentTime = Time.time;

        Color color = image.color;
        color.a = value;
        image.color = color;
    }
}
