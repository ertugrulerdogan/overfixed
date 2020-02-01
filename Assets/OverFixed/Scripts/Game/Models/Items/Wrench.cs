namespace OverFixed.Scripts.Game.Models.Items
{
    public class Wrench : Item
    {
        public float Strength { get; }

        public Wrench()
        {
            Strength = 10f;
        }
    }
}