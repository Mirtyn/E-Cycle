using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : ProjectManager
{
    private enum _ActivePanel { None, Start, Options };
    private _ActivePanel activePanel = _ActivePanel.None;

    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject optionsPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activePanel == _ActivePanel.None)
            {
                QuitButtonPressed();
            }
            else if (activePanel == _ActivePanel.Start)
            {
                BackStartButtonPressed();
            }
            else if (activePanel == _ActivePanel.Options)
            {
                BackOptionsButtonPressed();
            }
        }
    }

    public void StartButtonPressed()
    {
        activePanel = _ActivePanel.Start;
        startPanel.SetActive(true);
    }

    public void BackStartButtonPressed()
    {
        activePanel = _ActivePanel.None;
        startPanel.SetActive(false);
    }



    public void OptionsButtonPressed()
    {
        activePanel = _ActivePanel.Options;
        optionsPanel.SetActive(true);
    }

    public void BackOptionsButtonPressed()
    {
        activePanel = _ActivePanel.None;
        optionsPanel.SetActive(false);
    }



    public void QuitButtonPressed()
    {
        Application.Quit();
    }
}
