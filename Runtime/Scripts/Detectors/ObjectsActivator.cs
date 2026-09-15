using UnityEngine;

namespace SPACS.Tasks.Detectors
{
    public class ObjectsActivator : MonoBehaviour
    {
        [SerializeField]
        private Object[] objects = null;

        ///////////////////////////////////////////////////////////////////////////
        private void OnEnable()
        {
            SetObjectsActive(true);
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnDisable()
        {
            SetObjectsActive(false);
        }

        ///////////////////////////////////////////////////////////////////////////
        private void SetObjectsActive(bool b)
        {
            if (objects == null)
                return;

            foreach (var obj in objects)
            {
                if (obj == null)
                    continue;

                if (obj is GameObject gameObject)
                    gameObject.SetActive(b);

                else if (obj is MonoBehaviour behaviour)
                    behaviour.enabled = b;

                else if (obj is Renderer renderer)
                    renderer.enabled = b;

                else if (obj is Collider collider)
                    collider.enabled = b;
            }
        }
    }
}