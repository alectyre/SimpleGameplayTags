using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameplayTags
{
    [CreateAssetMenu(menuName = "Tags/Create Tag", fileName = "NewTag")]
    public class Tag : ScriptableObject
    {
        [SerializeField] private Tag m_parent;

        public Tag Parent => m_parent;

        /// <summary>
        /// Returns true if the argument is this tag or one of its parents.
        /// </summary>
        public bool IsA(Tag tag)
        {
            if (!tag) return false;
            Tag toCheck = this;
            while (toCheck)
            {
                if (tag == toCheck) return true;
                toCheck = toCheck.Parent;
            }

            return false;
        }

#if UNITY_EDITOR
        private Tag m_previousParent;
        
        private void OnValidate()
        {

            if (ValidateParents(this))
            {
                ApplyParentNamePrefix(this);
            }
            else
            {
                Debug.LogError("New Tag parent would create circular hierarchy.");
                m_parent = m_previousParent;
            }
            
            m_previousParent = Parent;
        }
        
        [MenuItem("Assets/Create/Tags/Create Child of Selected", false, 0)]
        public static void DoSomething()
        {
            Tag newTag = CreateInstance<Tag>();
            newTag.name = "NewTag";
            newTag.m_parent = Selection.count == 1 ? Selection.objects[0] as Tag : null;
            ProjectWindowUtil.CreateAsset(newTag,  string.Format("{0}.asset", GetPreferredTagName(newTag)));
        }

        [MenuItem("Assets/Create/Tags/Create Child of Selected", true, 0)] // Note the 'true' for validation
        public static bool ValidateDoSomething()
        {
            return Selection.count == 1 && Selection.objects[0] is Tag;
        }
        
        /// <summary>
        /// Returns false if circular hierarchy is found.
        /// </summary>
        private static bool ValidateParents(Tag tag)
        {
            HashSet<Tag> visitedTags = new () { tag };

            Tag tagToCheck = tag;
            while (tagToCheck)
            {
                if (visitedTags.Contains(tagToCheck.m_parent))
                {
                    
                    return false;
                }

                visitedTags.Add(tagToCheck);
                tagToCheck = tagToCheck.Parent;
            }

            return true;
        }

        private static string GetPreferredTagName(Tag tag)
        {
            Tag parentTag = tag.Parent;
            string name = GetTagNameWithoutParentPrefix(tag.name);
            while (parentTag)
            {
                name = string.Format("{0}.{1}", GetTagNameWithoutParentPrefix(parentTag.name), name);
                parentTag = parentTag.Parent;
            }

            return name;
        }

        private static void ApplyParentNamePrefix(Tag tag)
        {
            SetTagAssetName(tag, GetPreferredTagName(tag));
        }

        private static void SetTagAssetName(Tag tag, string tagName)
        {
            if (tag.name == tagName) return;
            
            if (AssetDatabase.IsAssetImportWorkerProcess()) return;
            
            // Get the asset path of the ScriptableObject
            string assetPath = AssetDatabase.GetAssetPath(tag);
            
            if (string.IsNullOrEmpty(assetPath))
            {
                Debug.LogError("Could not find asset path for the target ScriptableObject.");
                return;
            }
            
            // Check if the new path already exists
            if (AssetDatabase.AssetPathExists(GetNewAssetPath(assetPath, tagName))) return;

            // Rename the asset
            string result = AssetDatabase.RenameAsset(assetPath, tagName);

            if (string.IsNullOrEmpty(result))
            {
                // Debug.Log($"Successfully renamed ScriptableObject to: {tagName}");
                EditorUtility.SetDirty(tag);
                AssetDatabase.SaveAssets();
            }
            else
            {
                Debug.LogError($"Failed to rename ScriptableObject: {result}");
            }
            
            string GetNewAssetPath(string oldPath, string newName)
            {
                string directory = Path.GetDirectoryName(oldPath);
                string extension = Path.GetExtension(oldPath);
                return $"{directory}/{newName}{extension}";
            }

        }
        
        private static string GetTagNameWithoutParentPrefix(string tagName)
        {
            return tagName.Split('.', StringSplitOptions.RemoveEmptyEntries)[^1];
        }
#endif
    }
}
