namespace OverFixed.Scripts.Game.Models.Items
{
    public class Extinguisher : Item
    {
        public float Strength { get; }
        public float GasLeft { get; set; }
        
        public Extinguisher()
        {
            Strength = 10f;
        }
    }
}