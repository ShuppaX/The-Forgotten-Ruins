using UnityEngine;
using BananaSoup.SaveSystem;
using System.Collections;

namespace BananaSoup.PickupSystem
{
    public class Pickup : MonoBehaviour, ILootable
    {
        protected int isLooted = 1;
        protected string playerPrefsKey;
        private float delayTime = 0.002f;

        public virtual void Loot() { }

        public virtual void Start() { }

        public virtual void CheckIsSaved(string key)
        {
            StartCoroutine(Delayer(key));
        }

        // HACK: Added delayer because UI might not be ready and it will give null reference
        private IEnumerator Delayer(string key)
        {
            yield return new WaitForSeconds(delayTime);
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
