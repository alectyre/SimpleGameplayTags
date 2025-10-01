using System.Collections.Generic;
using UnityEngine;

namespace GameplayTags
{
    public class TagComponent : MonoBehaviour
    {
        [SerializeField] private TagContainer m_tagContainer;
        
        public IEnumerable<Tag> Tags => m_tagContainer.Tags;
        
        public void Add(Tag tag) => m_tagContainer.Add(tag);

        public void Remove(Tag tag) => m_tagContainer.Remove(tag);

        /// <summary>
        /// Returns true if the container has any matching tag. A matching tag includes
        /// any children of the tag and the tag itself.
        /// E.g. the container has StatusEffect.Poison, Has(StatusEffect) returns true
        /// </summary>
        public bool Has(Tag tag) => m_tagContainer.Has(tag);

        /// <summary>
        /// Returns true if the container has the exact tag, not including children of the tag.
        /// E.g. the container has StatusEffect.Poison, Has(StatusEffect) returns false
        /// </summary>
        public bool HasExact(Tag tag) => m_tagContainer.HasExact(tag);

        /// <summary>
        /// Returns true if the container has any matching tags.
        /// </summary>
        public bool HasAny(IEnumerable<Tag> tags) => m_tagContainer.HasAny(tags);

        /// <summary>
        /// Returns true if the container has matching tags for all provided tags.
        /// </summary>
        public bool HasAll(IEnumerable<Tag> tags) => m_tagContainer.HasAll(tags);
    }
}
