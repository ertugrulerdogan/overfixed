namespace OverFixed.Scripts.Game.Models.Bullets
{
    public class Bullet
    {
        public float Damage { get; }
        public bool IsShipBullet { get; }
        public float Speed { get; }
        
        public Bullet(float damage, bool isShipBullet = false)
        {
            Damage = damage;
            IsShipBullet = isShipBullet;
            Speed = 20f;
        }
    }
}