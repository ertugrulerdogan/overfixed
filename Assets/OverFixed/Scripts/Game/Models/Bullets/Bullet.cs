namespace OverFixed.Scripts.Game.Models.Bullets
{
    public class Bullet
    {
        public float Damage { get; }
        public float Speed { get; }
        
        public Bullet(float damage)
        {
            Damage = damage;
            Speed = 20f;
        }
    }
}