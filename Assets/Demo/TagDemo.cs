using UnityEditor;
using UnityEngine;

namespace GameplayTags
{
    public class TagDemo : MonoBehaviour
    {
        [SerializeField] private TagComponent m_tagComponent;
        [Space]
        [SerializeField] private Tag m_tag;
        [SerializeField] private Tag[] m_tags;

        [CustomEditor(typeof(TagDemo))]
        public class TagTesterEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                TagDemo tagDemo = (TagDemo)target;

                if (!tagDemo.m_tagComponent) return;
                
                using (new EditorGUI.DisabledScope(true))
                {
                    GUILayout.Space(EditorGUIUtility.singleLineHeight);
                    
                    string hasTag = tagDemo.m_tagComponent.Has(tagDemo.m_tag) ? "True" : "False";
                    string hasTagExact = tagDemo.m_tagComponent.HasExact(tagDemo.m_tag) ? "True" : "False";
                    GUILayout.TextField($"Has Tag: {hasTag} (exact: {hasTagExact})");
                    
                    GUILayout.Space(EditorGUIUtility.singleLineHeight);
                    
                    string hasAllTags = tagDemo.m_tagComponent.HasAll(tagDemo.m_tags) ? "True" : "False";
                    string hasAnyTags = tagDemo.m_tagComponent.HasAny(tagDemo.m_tags) ? "True" : "False";
                    GUILayout.TextField($"HasAll Tags: {hasAllTags}");
                    GUILayout.TextField($"HasAny Tags: {hasAnyTags}");
                    
                    GUILayout.Space(EditorGUIUtility.singleLineHeight);
                   
                    for (int i = 0; i < tagDemo.m_tags.Length; i++)
                    {
                        Tag tag = tagDemo.m_tags[i];
                        if (!tag) continue;
                        hasTag = tagDemo.m_tagComponent.Has(tag) ? "True" : "False";
                        hasTagExact = tagDemo.m_tagComponent.HasExact(tag) ? "True" : "False";
                        GUILayout.TextField($"Has Tag - {tag.name}: {hasTag} (exact: {hasTagExact})");
                    }
                }
            }
        }
    }
}
