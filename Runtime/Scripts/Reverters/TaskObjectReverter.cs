using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SPACS.Tasks
{
    public class TaskObjectReverter : TaskObjectReverterBase
    {
        #region Inspector Info

        /////////////////////
        //
        // This script has a CUSTOM EDITOR!
        // Check the script "TaskObjectReverterEditor"!
        //
        /////////////////////

        // GameObject and Transform
        [SerializeField] private bool revertGameObjectActivation = false;

        [SerializeField] private bool revertParent = false;

        [SerializeField] private bool revertPose = false;
        [SerializeField] private bool revertScale = false;

        // Components activation state
        [SerializeField] private bool revertComponentsActivation = false;
        [SerializeField] private List<MonoBehaviour> revertComponents = new List<MonoBehaviour>();

        // Rigidbody
        [SerializeField] private bool revertRigidbody = false;

        // Rigidbody
        [SerializeField] private UnityEvent customRevertEvent = new UnityEvent();

        #endregion

        #region Private stuff

        private bool _alreadySetup = false;
        private TaskSystem _taskSystem = null;

        private bool _initialGameObjectActivation = false;

        private List<(MonoBehaviour, bool)> _initialComponents = new List<(MonoBehaviour, bool)>();

        private Transform _initialParent = null;
        private string _initialParentName = string.Empty;

        private Vector3 _initialPos = Vector3.zero;
        private Quaternion _initialRot = Quaternion.identity;
        private Vector3 _initialLocalScale = Vector3.zero;

        private Rigidbody _rigidbody = null;
        private bool _initialGravity = false;
        private bool _initialKinematic = false;

        #endregion

#if UNITY_EDITOR
        public TaskSystem TaskSystem { get { return _taskSystem; } }
#endif

        public override void Prepare(TaskSystem taskSystem)
        {
            if (_alreadySetup)
            {
                Debug.LogWarning($"Task Reverter: the object \"{gameObject.name}\" is already set up from TaskSystem \"{_taskSystem.name}\"!", gameObject);
                return;
            }

            _alreadySetup = true;

            // Saving reference of preparing TaskSystem.
            _taskSystem = taskSystem;

            // GameObject
            if (revertGameObjectActivation)
            {
                _initialGameObjectActivation = gameObject.activeSelf;
            }

            // Components
            if (revertComponentsActivation && revertComponents.Count > 0)
            {
                foreach (var component in revertComponents)
                {
                    _initialComponents.Add(new(component, component.enabled));
                }
            }

            // Parent
            if (revertParent)
            {
                _initialParent = transform.parent;
                if (_initialParent != null)
                {
                    _initialParentName = _initialParent.name;
                }
            }

            // Transform
            if (revertPose)
            {
                _initialPos = transform.position;
                _initialRot = transform.rotation;
            }
            if (revertScale)
            {
                _initialLocalScale = transform.localScale;
            }

            // Rigidbody
            if (revertRigidbody && TryGetComponent<Rigidbody>(out _rigidbody))
            {
                _initialGravity = _rigidbody.useGravity;
                _initialKinematic = _rigidbody.isKinematic;
            }
        }

        public override void Revert()
        {
            if (!_alreadySetup)
            {
                Debug.LogError($"Task Reverter: trying to revert the object \"{gameObject.name}\" but it is not set up!", gameObject);
                return;
            }

            // GameObject
            if (revertGameObjectActivation && gameObject.activeSelf != _initialGameObjectActivation)
            {
                gameObject.SetActive(_initialGameObjectActivation);
            }

            // Components
            if (revertComponentsActivation && _initialComponents.Count > 0)
            {
                foreach (var component in _initialComponents)
                {
                    if (component.Item1.enabled != component.Item2)
                    {
                        component.Item1.enabled = component.Item2;
                    }
                }
            }

            // Parent
            if (revertParent)
            {
                if (_initialParent != null)
                {
                    transform.SetParent(_initialParent);
                }
                else if (string.IsNullOrEmpty(_initialParentName))
                {
                    transform.SetParent(null);
                }
                else
                {
                    // Name is filled but object is empty: it could be destroyed during the session!
                    Debug.LogError($"Task Reverter: trying to set an invalid parent to object \"{gameObject.name}\": object \"{_initialParentName}\" is undefined!", gameObject);
                }
            }

            // Transform
            if (revertPose)
            {
                transform.SetPositionAndRotation(_initialPos, _initialRot);
            }
            if (revertScale)
            {
                transform.localScale = _initialLocalScale;
            }

            // Rigidbody
            if (revertRigidbody)
            {
                if (_rigidbody != null)
                {
                    _rigidbody.useGravity = _initialGravity;
                    _rigidbody.isKinematic = _initialKinematic;
                }
                else
                {
                    Debug.LogError($"Task Reverter: trying to reset Rigidbody on object \"{gameObject.name}\": Rigidbody is missing!", gameObject);
                }
            }

            // Trigger Custom Revert Event
            customRevertEvent?.Invoke();
        }
    }
}
