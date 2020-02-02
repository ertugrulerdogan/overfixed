namespace OverFixed.Scripts.Game.Models.Items
{
    public class Rifle : Item
    {
        public int Ammo { get; set; }
        public float FirePeriod { get; }
        public float Damage { get; }

        public Rifle()
        {
            FirePeriod = 0.1f;
            Damage = 0.2f;
        }
    }
}