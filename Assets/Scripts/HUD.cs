using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : ProjectManager
{
    [SerializeField] private TMP_Text fpsText;

    float time = 0;

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= 0.5f)
        {
            time = 0;
            var i = 1.0f / Time.deltaTime;
            fpsText.text = i.ToString("0.0");
        }
    }
}
