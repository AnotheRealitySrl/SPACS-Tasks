using UnityEngine;

namespace SPACS.Tasks.Detectors
{
    public class MaterialController : MonoBehaviour
    {
        [SerializeField] private Material sharedMaterial = default;

        public void EnableBoolean(string paramName)
        {
            ToggleBoolean(paramName, true);
        }
        public void DisableBoolean(string paramName)
        {
            ToggleBoolean(paramName, false);
        }

        public void ToggleBoolean(string paramName, bool value)
        {
            if (!sharedMaterial)
            {
                Debug.LogWarning("<b>[MaterialController]</b> no Material specified.");
                return;
            }

            if (sharedMaterial.HasProperty(paramName))
            {
                Debug.LogWarning("<b>[MaterialController]</b> Type: " + sharedMaterial.GetType().ToString());
                sharedMaterial.SetFloat(paramName, value ? 1f : 0f);
            }
            else
            {
                Debug.LogWarning($"<b>[MaterialController]</b> \"{paramName}\" property not found in Material \"{sharedMaterial.name}\".", this.gameObject);
            }
        }
    }
}
