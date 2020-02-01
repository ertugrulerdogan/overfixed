namespace OverFixed.Scripts.Game.Models.Drones
{
    public class Drone
    {
        public float Health { get; set; }
        public float Damage { get; }
        public float FirePeriod { get; }
        public float Speed { get; }

        public Drone()
        {
            Health = 1f;
            Damage = 10f;
            FirePeriod = 3f;
            Speed = 0.5f;
        }
    }
}