using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameplayTags
{
    [Serializable]
    public class TagContainer
    {
        [SerializeField] private List<Tag> m_tags = new();

        public IEnumerable<Tag> Tags => m_tags;

        public void Add(Tag tag)
        {
            m_tags.Add(tag);
        }

        public void Remove(Tag tag)
        {
            m_tags.Remove(tag);
        }

        /// <summary>
        /// Returns true if the container has any matching tag. A matching tag includes
        /// any children of the tag and the tag itself.
        /// E.g. the container has StatusEffect.Poison, Has(StatusEffect) returns true
        /// </summary>
        public bool Has(Tag tag)
        {
            if (!tag) return false;

            // Enumerate through all tags and check for IsA relationship
            foreach (Tag containerTag in m_tags)
            {
                if (!containerTag) continue;
                if (containerTag.IsA(tag)) return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the container has the exact tag, not including children of the tag.
        /// E.g. the container has StatusEffect.Poison, Has(StatusEffect) returns false
        /// </summary>
        public bool HasExact(Tag tag)
        {
            if (!tag) return false;
            
            // Enumerate through all tags and check for IsA relationship
            foreach (Tag containerTag in m_tags)
            {
                if (!containerTag) continue;
                if (containerTag == tag) return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the container has any matching tags.
        /// </summary>
        public bool HasAny(IEnumerable<Tag> tags)
        {
            if (tags == null) return false;
            using IEnumerator<Tag> enumerator = tags.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (Has(enumerator.Current)) return true;
            }
            return false;
        }
        
        /// <summary>
        /// Returns true if the container has matching tags for all provided tags.
        /// </summary>
        public bool HasAll(IEnumerable<Tag> tags)
        {
            if (tags == null) return false;
            using IEnumerator<Tag> enumerator = tags.GetEnumerator();
            bool anyMatches = false; // Avoids empty enumerable always returning true
            while (enumerator.MoveNext())
            {
                if (!enumerator.Current) continue; // Skip nulls in enumerable
                if (!Has(enumerator.Current)) return false;
                anyMatches = true;
            }
            return anyMatches;
        }
        
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(TagContainer))]
        public class TagContainerDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUI.BeginProperty(position, label, property);

                SerializedProperty itemsProp = property.FindPropertyRelative("m_tags");

                // Draw the list using default inspector handling
                Rect propertyRect = new Rect(
                    position.x, position.y + EditorGUIUtility.singleLineHeight + 2,
                    position.width, EditorGUI.GetPropertyHeight(itemsProp, true));
                EditorGUI.PropertyField(propertyRect, itemsProp, true);
              
                EditorGUI.EndProperty();
            }
            
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                SerializedProperty itemsProp = property.FindPropertyRelative("m_tags");
                
                if (!itemsProp.isExpanded)
                    return EditorGUIUtility.singleLineHeight + 2;
                
                return EditorGUIUtility.singleLineHeight + 2 + EditorGUI.GetPropertyHeight(itemsProp, true);
            }
        }
#endif
    }
}
