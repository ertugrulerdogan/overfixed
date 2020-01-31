namespace OverFixed.Scripts.Game.Models
{
    public enum ShipState
    {
        None,
        OnFire,
        Damaged,
    }
    public class Ship
    {
        public float Health;
        public ShipState State;
    }
}