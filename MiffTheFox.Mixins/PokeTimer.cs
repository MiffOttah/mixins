using System;

namespace MiffTheFox
{
    public class PokeTimer
    {
        protected DateTime? _LastPoke = null;
        public DateTime LastPoke { get => _LastPoke ?? DateTime.MinValue; }
        public TimeSpan StaleAfter { get; set; }

        public bool IsStale => _LastPoke.HasValue ? (DateTime.UtcNow - _LastPoke.Value) > StaleAfter : true;

        public PokeTimer()
        {
            StaleAfter = TimeSpan.FromSeconds(30);
        }

        public PokeTimer(TimeSpan staleAfter)
        {
            StaleAfter = staleAfter;
        }

        public void Poke()
        {
            _LastPoke = DateTime.UtcNow;
        }

        public bool PokeIfStale()
        {
            if (IsStale)
            {
                Poke();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
