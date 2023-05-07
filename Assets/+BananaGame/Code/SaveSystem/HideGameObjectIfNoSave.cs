using UnityEngine;

namespace BananaSoup.SaveSystem
{
    public class HideGameObjectIfNoSave : MonoBehaviour
    {
        private void Start()
        {
            if ( PlayerPrefs.GetInt(SaveManager.saveKeyCheckpoint) < 1 )
            {
                Debug.Log("No Save data found");
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Key found: " + PlayerPrefs.GetInt(SaveManager.saveKeyCheckpoint));
            }
        }
    }
}
