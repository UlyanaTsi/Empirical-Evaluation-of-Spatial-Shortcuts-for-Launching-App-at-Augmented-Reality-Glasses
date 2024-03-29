﻿/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* edited
*****************************************************************************/

namespace NRKernal
{
    using UnityEngine;
    using UnityEngine.UI;
    using System;

    /// <summary> A nr grabbable object. </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class NRGrabbableObject : MonoBehaviour {
        /// <summary> Gets a value indicating whether we can grab. </summary>
        /// <value> True if we can grab, false if not. </value>
        public bool CanGrab { get { return Grabber == null; } }
        /// <summary> Gets a value indicating whether this object is being grabbed. </summary>
        /// <value> True if this object is being grabbed, false if not. </value>
        public bool IsBeingGrabbed { get { return Grabber; } }
        /// <summary> Gets or sets the grabber. </summary>
        /// <value> The grabber. </value>
        public NRGrabber Grabber { get; private set; }
        /// <summary> Позиция, к которой был прикреплен объект. </summary>
        /// <value> Позиция </value>
        public Transform placement = null;

        public Canvas MusicAppUI = null;
        public Canvas ClockAppUI = null;

        public NRHandSimpleVisual leftHand = null;
        public CameraSmoothFollow follower = null;


        /// <summary> Gets the attached colliders. </summary>
        /// <value> The attached colliders. </value>
        public Collider[] AttachedColliders
        {
            get
            {
                CheckAttachedColliders();
                return m_AttachedColliders;
            }
        }

        /// <summary> Event queue for all listeners interested in OnGrabBegan events. </summary>
        public event Action OnGrabBegan { add { m_OnGrabBegan += value; } remove { m_OnGrabBegan -= value; } }
        /// <summary> Event queue for all listeners interested in OnGrabEnded events. </summary>
        public event Action OnGrabEnded { add { m_OnGrabEnded += value; } remove { m_OnGrabEnded -= value; } }

        /// <summary> The attached rigidbody. </summary>
        protected Rigidbody m_AttachedRigidbody;
        /// <summary> The attached colliders. </summary>
        [SerializeField]
        private Collider[] m_AttachedColliders;

        /// <summary> True to origin rigidbody kinematic. </summary>
        private bool m_OriginRigidbodyKinematic;
        /// <summary> The on grab began. </summary>
        private Action m_OnGrabBegan;
        /// <summary> The on grab ended. </summary>
        private Action m_OnGrabEnded;

        public bool isOpened = false;

        /// <summary> Awakes this object. </summary>
        protected virtual void Awake()
        {
            m_AttachedRigidbody = GetComponent<Rigidbody>();
            m_OriginRigidbodyKinematic = m_AttachedRigidbody.isKinematic;
            CheckAttachedColliders();

        }

        private void Update()
        {
            if (gameObject.name == "MusicApp")
            {
                if (MusicAppUI.enabled)
                    isOpened = true;
                else
                    isOpened = false;

                if (leftHand.MiddleMiddleTransform != null && !isOpened)
                {
                    gameObject.GetComponent<MeshRenderer>().enabled = true;
                    transform.position = leftHand.MiddleMiddleTransform.position;
                    //transform.rotation = leftHand.MiddleMiddleTransform.rotation;
                }
                else
                {
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }

            if (gameObject.name == "ClockApp")
            {
                if (ClockAppUI.enabled)
                    isOpened = true;
                else
                    isOpened = false;

                if (leftHand.wristTransform != null && !isOpened)
                {
                    gameObject.GetComponent<MeshRenderer>().enabled = true;
                    transform.position = leftHand.wristTransform.position;
                    //transform.rotation = leftHand.wristTransform.rotation;
                }
                else
                {
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }



        }


        /// <summary> Grab begin. </summary>
        /// <param name="grabber"> The grabber.</param>
        public void GrabBegin(NRGrabber grabber)
        {
            if (IsBeingGrabbed || grabber == null)
                return;
            //gameObject.GetComponent<Rigidbody>().isKinematic = true;
            Grabber = grabber;
            if (m_OnGrabBegan != null)
            {
                m_OnGrabBegan();
            }
        }

        /// <summary> Grab end. </summary>
        public void GrabEnd()
        {
            m_AttachedRigidbody.isKinematic = m_OriginRigidbodyKinematic;
            Grabber = null;
            if (m_OnGrabEnded != null)
            {
                m_OnGrabEnded();
            }
        }

        /// <summary> Move rigidbody. </summary>
        /// <param name="targetPos"> Target position.</param>
        /// <param name="targetRot"> Target rot.</param>
        public void MoveRigidbody(Vector3 targetPos, Quaternion targetRot)
        {
            if (!IsBeingGrabbed)
                return;
            m_AttachedRigidbody.MovePosition(targetPos);
            m_AttachedRigidbody.MoveRotation(targetRot);
        }

        /// <summary> Move transform. </summary>
        /// <param name="targetPos"> Target position.</param>
        /// <param name="targetRot"> Target rot.</param>
        public void MoveTransform(Vector3 targetPos, Quaternion targetRot)
        {
            if (!IsBeingGrabbed)
                return;
            transform.position = targetPos;
            transform.rotation = targetRot;
        }

        /// <summary> Check attached colliders. </summary>
        private void CheckAttachedColliders()
        {
            NRDebugger.Info(m_AttachedColliders.Length);
            if (m_AttachedColliders != null && m_AttachedColliders.Length > 0)
                return;
            m_AttachedColliders = GetComponentsInChildren<Collider>();
            if (m_AttachedColliders == null)
            {
                NRDebugger.Error("AttachedColliders can not be null for NRGrabbableObject, please set collider !");
            }
        }

        void OnTriggerEnter(Collider other)
        {
            NRDebugger.Info(other.gameObject.tag);
            PointApp();
        }

        public void PointApp(){
            follower.UpdatePosition();
            if (gameObject.name == "MusicApp")
            {
                MusicAppUI.enabled = true;
                isOpened = true;
            }

            if (gameObject.name == "ClockApp")
            {
                ClockAppUI.enabled = true;
                isOpened = true;
            }
        }
    }
}
