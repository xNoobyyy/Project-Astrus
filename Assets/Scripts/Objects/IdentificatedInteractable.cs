using System;
using UnityEngine;

namespace Objects {
    [ExecuteAlways]
    public abstract class IdentificatedInteractable : MonoBehaviour {
        [SerializeField, HideInInspector] private string uuid = "";
        public string Uuid => uuid;

        public abstract long InteractedAt { get; }

        public abstract void SetInteractedAt(long timestamp);

#if UNITY_EDITOR
        protected void OnValidate() {
            GenerateUuidIfNeeded();
        }
#endif

        private void GenerateUuidIfNeeded() {
            if (!string.IsNullOrEmpty(uuid)) return;
            uuid = Guid.NewGuid().ToString();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}