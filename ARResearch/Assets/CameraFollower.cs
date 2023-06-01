using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace NRKernal.NRExamples
{
    public class CameraFollower : MonoBehaviour
    {
        public Transform cameraCenter = null;

        private UnityEngine.Vector3 positionOffset;
        private void Awake()
        {
            positionOffset = UnityEngine.Vector3.zero;
        }

        void Update()
        {
            transform.position = cameraCenter.position + positionOffset;
            transform.rotation = cameraCenter.rotation;
        }
    }
}
