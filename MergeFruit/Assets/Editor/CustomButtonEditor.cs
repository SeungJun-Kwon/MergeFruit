using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(CustomButton), true)]
    [CanEditMultipleObjects]
    /// <summary>
    ///   Custom Editor for the Button Component.
    ///   Extend this class to write a custom editor for a component derived from Button.
    /// </summary>
    public class ButtonEditor : SelectableEditor
    {
        SerializedProperty _onClickProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _onClickProperty = serializedObject.FindProperty("_onClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(_onClickProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
