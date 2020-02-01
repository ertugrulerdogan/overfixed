namespace OverFixed.Scripts.Game.Models.Drones
{
    public class Drone
    {
        public float Health { get; set; }
        public float Damage { get; }
        public float FirePeriod { get; }

        public Drone()
        {
            Health = 1f;
            Damage = 25f;
            FirePeriod = 1f;
        }
    }
}