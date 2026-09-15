using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SPACS.Tasks
{
    public class SetPosistion : MonoBehaviour
    {
        [SerializeField]
        private PosRot[] posRots;
        [SerializeField]
        private bool setLocal;
        [SerializeField]
        private bool setScale = false;

        [SerializeField]
        private bool storeInitPos;
        private Vector3 initPos = Vector3.zero;
        private Vector3 initRot = Vector3.zero;

        [Serializable]
        public class PosRot
        {
            public Vector3 pos;
            public Vector3 rot;
            public Vector3 scale = Vector3.one;
        }

        private void Start()
        {
            if (storeInitPos)
            {
                initPos = transform.localPosition;
                initRot = transform.localEulerAngles;
            }
        }

        public void SetPosRot(int index)
        {
            PosRot posRot = posRots[index];
            if (setLocal)
            {
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
                transform.localPosition = posRot.pos;
                transform.localEulerAngles = posRot.rot;
            }
            else
            {
                transform.position = posRot.pos;
                transform.eulerAngles = posRot.rot;
            }

            if (setScale)
            {
                transform.localScale = posRot.scale;
            }
        }

        public void SetInitPos()
        {
            transform.localPosition = initPos;
            transform.localEulerAngles = initRot;
        }
    }
}
