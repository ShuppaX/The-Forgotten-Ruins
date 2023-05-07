using UnityEngine;
using BananaSoup.SaveSystem;

namespace BananaSoup.PickupSystem
{
    public class Pickup : MonoBehaviour, ILootable
    {
        protected int isLooted = 1;
        protected string playerPrefsKey;

        public virtual void Loot() { }

        public virtual void Start() { }

        public virtual void CheckIsSaved(string key)
        {
            if ( PlayerPrefs.HasKey(key) && PlayerPrefs.GetInt(key) == isLooted )
            {
                Loot();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ( other.GetComponent<PlayerBase>() != null )
            {
                Loot();
            }
        }

        public void DisablePickup()
        {
            gameObject.SetActive(false);
        }

        public virtual void SetToPlayerPrefs(string key)
        {
            // Setting to player prefs only if not looted and saved already
            if ( PlayerPrefs.GetInt(key) != isLooted )
            {
                SaveManager.Instance.SetInt(key, isLooted);
            }
        }
    }
}
