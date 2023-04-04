using System;

namespace BananaSoup
{
    public interface IHealth
    {
        public int CurrentHealth { get; }
        public bool IsAlive { get; }
        public bool WasHit { get; set; }

        public void Setup();
        void IncreaseHealth(int amount);
        void DecreaseHealth(int amount);
        void Reset();
    }
}
