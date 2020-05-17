using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCreditsPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject creditsPanel;

    public void ToggleCredits()
    {
        if (!creditsPanel)
            return;

        creditsPanel.active = !creditsPanel.active;
    }
}
