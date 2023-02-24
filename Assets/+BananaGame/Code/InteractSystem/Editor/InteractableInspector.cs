using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BananaSoup.InteractSystem.CustomInspector
{
    [CustomEditor(typeof(Interactable), true)]
    public class InteractableInspector : Editor
    {
        private Interactable interactable;

        private void OnEnable()
        {
            interactable = (Interactable)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if ( GUILayout.Button("Add Interact point") )
            {
                int interactPointCount = interactable.transform.childCount;
                string name = $"InteractPoint ({interactPointCount + 1})";

                GameObject interactPoint = new GameObject(name);
                interactPoint.transform.parent = interactable.transform;

                interactPoint.AddComponent<InteractPoint>();

                interactable.OnValidate();

                Selection.activeGameObject = interactPoint;
            }
        }
    }
}
