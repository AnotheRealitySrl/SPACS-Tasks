using UnityEditor;

using UnityEngine;

namespace SPACS.Tasks.Editor
{
    [CustomEditor(typeof(TaskSystem))]
    public class TaskSystemEditor : UnityEditor.Editor
    {
        private const float spaceBetweenElements = 5f;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TaskSystem taskSystem = target as TaskSystem;

            if (Application.isPlaying)
            {
                EditorGUILayout.Space(spaceBetweenElements);
                EditorGUILayout.LabelField("=> RUNTIME <=", EditorStyles.boldLabel);
                if (GUILayout.Button("Do Revert!"))
                {
                    Debug.Log($"Task System Reverter Editor: Revert PRESSED on object \"{taskSystem.name}\"!", taskSystem.gameObject);
                    taskSystem.Revert();
                }
            }
        }
    }
}
