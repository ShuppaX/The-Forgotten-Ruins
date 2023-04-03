using System;

namespace BananaSoup
{
    public interface IHealth
    {
        public bool IsAlive { get; }

        public void Setup();
        void IncreaseHealth(int amount);
        void DecreaseHealth(int amount);
        void Reset();
        void OnDeath();
    }
}
