using OverFixed.Scripts.Game.Behaviours.Items;
using OverFixed.Scripts.Game.Models.Items;
using Zenject;

namespace OverFixed.Scripts.Game.Controllers
{
    public class ItemController
    {
        public ItemController([InjectOptional] RifleBehaviour rifleBehaviour, [InjectOptional] ExtinguisherBehaviour extinguisherBehaviour, [InjectOptional] WrenchBehaviour wrenchBehaviour)
        {
            rifleBehaviour?.Bind(new Rifle());
            extinguisherBehaviour?.Bind(new Extinguisher());
            wrenchBehaviour?.Bind(new Wrench());
        }
    }
}