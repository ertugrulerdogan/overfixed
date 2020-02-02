namespace OverFixed.Scripts.Game.Models.Data
{
    public class TeamData
    {
        public const float WarWinTarget = 1000;

        public float Scrap { get; set; } = GameRules.MaxScrapAmount;
        public float WarStatus = 500f; //max 1000
    }
}