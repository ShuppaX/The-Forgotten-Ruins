using UnityEditor;
using UnityEngine;

namespace BananaSoup.Utilities
{
    public class ExitGame : MonoBehaviour
    {
        public void Exit()
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}
