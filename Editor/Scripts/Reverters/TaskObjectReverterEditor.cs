using UnityEditor;

using UnityEngine;

using SPACS.Tasks;

namespace SPACS.TasksEditor
{
    [CustomEditor(typeof(TaskObjectReverter)), CanEditMultipleObjects]
    public class TaskObjectReverterEditor : UnityEditor.Editor
    {
        private const float spaceBetweenElements = 5f;

        public override void OnInspectorGUI()
        {
            TaskObjectReverter taskObjectReverter = target as TaskObjectReverter;

            EditorGUILayout.LabelField("=> GameObject and Transform <=", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("revertGameObjectActivation"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("revertParent"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("revertPose"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("revertScale"));
            EditorGUILayout.Space(spaceBetweenElements);

            EditorGUILayout.LabelField("=> Components activation state <=", EditorStyles.boldLabel);
            SerializedProperty revertComponentsActivation = serializedObject.FindProperty("revertComponentsActivation");
            EditorGUILayout.PropertyField(revertComponentsActivation);
            if (revertComponentsActivation.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("revertComponents"));
            }

            if (taskObjectReverter.GetComponent<Rigidbody>() != null)
            {
                EditorGUILayout.Space(spaceBetweenElements);
                EditorGUILayout.LabelField("=> Rigidbody <=", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("revertRigidbody"));
            }

            EditorGUILayout.Space(spaceBetweenElements);
            EditorGUILayout.LabelField("=> Custom Revert Event <=", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("customRevertEvent"));

            if (Application.isPlaying)
            {
                EditorGUILayout.Space(spaceBetweenElements);
                EditorGUILayout.LabelField("=> RUNTIME <=", EditorStyles.boldLabel);
                if (taskObjectReverter.TaskSystem != null)
                {
                    EditorGUILayout.LabelField("TaskSystem", taskObjectReverter.TaskSystem.name);
                }
                else
                {
                    EditorGUILayout.LabelField("TaskSystem", "(null)");
                }
                EditorGUILayout.Space(spaceBetweenElements);
                if (GUILayout.Button("Do Revert!"))
                {
                    Debug.Log($"Task Reverter Editor: Revert PRESSED on object \"{taskObjectReverter.name}\"!", taskObjectReverter.gameObject);
                    taskObjectReverter.Revert();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
