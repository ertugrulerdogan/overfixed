namespace OverFixed.Scripts.Game.Models.Items
{
    public class Rifle : Item
    {
        public int Ammo { get; set; }
        public float FirePeriod { get; }
        public float Damage { get; }

        public Rifle()
        {
            FirePeriod = 0.25f;
            Damage = 0.5f;
        }
    }
}