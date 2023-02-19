using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BananaSoup.InteractSystem.CustomInspector
{
    [CustomEditor(typeof(Interactable))]
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

            // TODO: Ask Sami why this doesn't work.
            // NOTE: Works if 3 Interactable(s) are changed for example, to MovableBox

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
