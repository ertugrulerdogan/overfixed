using OverFixed.Scripts.Game.Behaviours.Items;
using OverFixed.Scripts.Game.Models.Items;
using Zenject;

namespace OverFixed.Scripts.Game.Controllers
{
    public class ItemController
    {
        public ItemController([InjectOptional] RifleBehaviour rifleBehaviour)
        {
            rifleBehaviour?.Bind(new Rifle());
        }
    }
}