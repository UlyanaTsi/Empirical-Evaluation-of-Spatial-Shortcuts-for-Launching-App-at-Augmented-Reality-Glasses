using System.Collections;
using System.Collections.Generic;
using NRKernal;
using UnityEngine;

public class WizardOfOzController : MonoBehaviour
{
    public Canvas appUI = null;
    public CameraSmoothFollow follower = null;

    void Update()
    {
        if (NRInput.GetButtonDown(ControllerButton.TRIGGER) & !appUI.enabled)
        {
            follower.UpdatePosition();

            appUI.enabled = true;
            //NRDebugger.Info("работает wizard of oz");

            NRInput.SetInputSource(InputSourceEnum.Hands);
        }
    }
}
