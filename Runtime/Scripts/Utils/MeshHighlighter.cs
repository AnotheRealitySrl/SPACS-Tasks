using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SPACS.Tasks.Utils
{
    public class MeshHighlighter : MonoBehaviour
    {
        [SerializeField]
        private Material highlightMaterial = default;
        [SerializeField]
        private bool includeParent = false;

        ///////////////////////////////////////////////////////////////////////////
        private void OnEnable()
        {
            Highlight(true);
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OnDisable()
        {
            Highlight(false);
        }

        ///////////////////////////////////////////////////////////////////////////
        private void Highlight(bool enabled)
        {
            foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>(true))
            {
                List<Material> materials = meshRenderer.sharedMaterials.ToList();
                materials.Remove(highlightMaterial);
                if (enabled)
                    materials.Add(highlightMaterial);
                meshRenderer.sharedMaterials = materials.ToArray();
            }

            if (includeParent)
            {
                Renderer meshRenderer = GetComponent<Renderer>();
                List<Material> materials = meshRenderer.sharedMaterials.ToList();
                materials.Remove(highlightMaterial);
                if (enabled)
                    materials.Add(highlightMaterial);
                meshRenderer.sharedMaterials = materials.ToArray();
            }
        }
    }
}
