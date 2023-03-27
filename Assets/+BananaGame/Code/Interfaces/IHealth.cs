using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup
{
    public interface IHealth
    {
        public event System.Action<int> HealthChanged;
        public int CurrentHealth { get; }
        public int MaxHealth { get; }
        public bool IsAlive { get; }

        public void Setup();
        void IncreaseHealth(int amount);
        void DecreaseHEalth(int amount);
        void Reset();
    }
}
