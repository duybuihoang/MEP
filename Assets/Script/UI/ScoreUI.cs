using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{

    private static ScoreUI instance;
    public static ScoreUI Instance { get => instance; }

    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI windows;
    [SerializeField] private TextMeshProUGUI combo;
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

    public void SetScoreUI(float value)
    {
        score.text = value.ToString();
    }

    public void SetWindowsUI(string type, Color color)
    {
        windows.text = type;
        windows.color = color;
    }

    public void SetComboUI(int value)
    {
        if(value <= 0)
        {
            combo.text = "";
        }
        combo.text = "X" + value.ToString();
    }
}
