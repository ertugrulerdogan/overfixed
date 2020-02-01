namespace OverFixed.Scripts.Game.Models
{
    public class Hangar
    {
        public bool[] IsPlatformOccupied;

        public Hangar()
        {
            IsPlatformOccupied = new bool[3];
        }
    }
}