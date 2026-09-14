
using System;

using UnityEngine;


namespace Virtuademy.SDK.Tasks.Detectors
{
    public class MeshSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject targetObject = default;

        [SerializeField]
        private Mesh mesh = default;

        [SerializeField]
        private Material material = default;


        [NonSerialized]
        private MeshRenderer meshRenderer = null;

        [NonSerialized]
        private MeshFilter meshFilter = null;


        ///////////////////////////////////////////////////////////////////////////
        private void OnEnable()
        {
            meshFilter = targetObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            meshRenderer = targetObject.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnDisable()
        {
            Destroy(meshRenderer);
            Destroy(meshFilter);
        }
    }
}