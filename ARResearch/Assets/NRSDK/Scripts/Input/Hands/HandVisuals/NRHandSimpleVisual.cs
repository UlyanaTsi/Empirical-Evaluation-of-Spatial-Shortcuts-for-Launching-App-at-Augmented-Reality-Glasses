/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
* Этот файл изменен
* 
*****************************************************************************/

namespace NRKernal
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class NRHandSimpleVisual : MonoBehaviour
    {
        public HandEnum handEnum;
        public GameObject jointPrefab;
        public Text indicator;
        public Transform MiddleMiddleTransform;
        public Transform wristTransform;

        public readonly Dictionary<HandJointID, Transform> joints = new Dictionary<HandJointID, Transform>();

        private void OnEnable()
        {
            NRInput.Hands.OnHandStatesUpdated += OnHandStatesUpdated;
            NRInput.Hands.OnHandTrackingStopped += OnHandTrackingStopped;
        }

        private void OnDisable()
        {
            NRInput.Hands.OnHandStatesUpdated -= OnHandStatesUpdated;
            NRInput.Hands.OnHandTrackingStopped += OnHandTrackingStopped;
        }

        private void OnHandStatesUpdated()
        {
            UpdateHandVisual();
        }

        private void OnHandTrackingStopped()
        {
            OnHandTrackingLost();
        }

        private void UpdateHandVisual()
        {
            var handState = NRInput.Hands.GetHandState(handEnum);
            if (handState != null && handState.isTracked)
            {
                foreach (var jointID in handState.jointsPoseDict.Keys)
                {
                    Transform jointTransform = null;
                   
                    if (joints.TryGetValue(jointID, out jointTransform))
                    {
                        jointTransform.gameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject jointObj = CreateJointObj(jointID);
                        if (jointObj == null)
                        {
                            Debug.LogError("Create joint failed, joint ID = " + jointID);
                            continue;
                        }

                        if (handEnum == HandEnum.LeftHand)
                        {
                            jointObj.tag = "Left Hand Joint";
                        }
                        if (handEnum == HandEnum.RightHand)
                        {
                            jointObj.tag = "Right Hand Joint";
                        }

                        jointTransform = jointObj.transform;
                        jointTransform.name = jointID.ToString() + " Transform";
                        jointTransform.SetParent(transform);
                        jointTransform.localScale = Vector3.one * 0.01f;


                        joints.Add(jointID, jointTransform);

                        if (jointID == HandJointID.IndexTip)
                        {
                            //NRDebugger.Info("MiddleMiddle joint активен" + jointTransform);
                            MiddleMiddleTransform = jointTransform;
                        }

                        if (jointID == HandJointID.Wrist)
                        {
                            //NRDebugger.Info("Wrist joint активен" + jointTransform);
                            wristTransform = jointTransform;
                        }

                    }
                    if (jointTransform)
                    {
                        jointTransform.position = handState.jointsPoseDict[jointID].position;
                        jointTransform.rotation = handState.jointsPoseDict[jointID].rotation;
                    }
                }

                SetHandCollidersEnabled(true);
            }
            else
            {
                OnHandTrackingLost();
            }
        }

        private void OnHandTrackingLost()
        {
            foreach (Transform jointTransform in joints.Values)
            {
                if (jointTransform)
                {
                    jointTransform.gameObject.SetActive(false);
                }
            }
        } 

        private GameObject CreateJointObj(HandJointID handJointID)
        {
            return Instantiate(jointPrefab);
        }

        public void SetHandCollidersEnabled(bool isEnabled)
        {
            var collidersInChildren = transform.GetComponentsInChildren<Collider>(true);
            if (collidersInChildren != null)
            {
                for (int i = 0; i < collidersInChildren.Length; i++)
                {
                    collidersInChildren[i].enabled = isEnabled;
                }
                indicator.text = "active";
            }
        }
    }
}
