using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NRKernal.NRExamples
{
    public class AppManager : MonoBehaviour
    {
        public Canvas appUI = null;

        void OnTriggerEnter(Collider other)
        { 
            if (gameObject.name == "CloskCloseButton" && appUI.enabled){

                NRDebugger.Info("закрыли часы");
                NRInput.SetInputSource(InputSourceEnum.Controller);
            }

            appUI.enabled = false;
        }
    }
}