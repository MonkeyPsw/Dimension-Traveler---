using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressShiftKey : MonoBehaviour
{
    public Text text;
    public GameObject tutorialPanel;
    private float alpha = 1.0f;
    private int mode = 0;

    private void Start()
    {

    }

    private void Update()
    {
        if (mode == 0)
        {
            if (alpha > 0)
            {
                alpha -= 0.02f;
            }
            else
            {
                mode = 1;
            }
        }
        else
        {
            if (alpha < 1)
            {
                alpha += 0.02f;
            }
            else
            {
                mode = 0;
            }
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }

    public void ToggleTutoPanel()
    {
        tutorialPanel.SetActive(!tutorialPanel.activeSelf);
    }
}