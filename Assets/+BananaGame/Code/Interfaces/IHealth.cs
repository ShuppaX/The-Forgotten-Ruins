using System;

namespace BananaSoup
{
    public interface IHealth
    {
        public event Action<int> HealthChanged;
        public int CurrentHealth { get; }
        public int MaxHealth { get; }
        public bool IsAlive { get; }
        public bool WasHit { get; }

        public void Setup();
        void IncreaseHealth(int amount);
        void DecreaseHealth(int amount);
        void Reset();
        void OnDeath();
    }
}
