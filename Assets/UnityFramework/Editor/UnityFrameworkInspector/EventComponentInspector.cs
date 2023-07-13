using UnityEditor;
using UnityFramework.Runtime;

namespace UnityFramework.Editor
{
    [CustomEditor(typeof(EventComponent))]
    internal sealed class EventComponentInspector : UnityFrameworkInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            EventComponent cpt = (EventComponent)target;

            if (IsPrefabInHierarchy(cpt.gameObject))
            {
                EditorGUILayout.LabelField("Event Handler Count", cpt.EventHandlerCount.ToString());
                EditorGUILayout.LabelField("Event Count", cpt.EventCount.ToString());
            }

            Repaint();
        }
    }
}