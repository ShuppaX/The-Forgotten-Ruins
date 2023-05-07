namespace BananaSoup.PickupSystem
{
    public interface ILootable
    {
        void Loot();
        void DisablePickup();
        void Start();
        void CheckIsSaved(string key);
        void SetToPlayerPrefs(string key);
    }
}
