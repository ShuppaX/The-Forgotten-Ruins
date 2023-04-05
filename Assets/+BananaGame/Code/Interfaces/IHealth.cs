using System;

namespace BananaSoup
{
    public interface IHealth
    {
        public int CurrentHealth { get; }
        public bool IsAlive { get; }
        public bool WasHit { get; set; }

        public void Setup();
        public void IncreaseHealth(int amount);
        public void DecreaseHealth(int amount);
        public void Reset();
        public void OnDeath();
    }
}
