using System.Collections.Generic;
using System.Linq;
using OverFixed.Scripts.Game.Behaviours;
using OverFixed.Scripts.Game.Behaviours.Ship;
using OverFixed.Scripts.Game.Models;
using OverFixed.Scripts.Game.Models.Ship;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    public List<PlatformBehaviour> PlatformBehaviours;
    private ShipBehaviour.Pool _shipPool;

    [Inject]
    private void Construct(ShipBehaviour.Pool shipPool)
    {
        _shipPool = shipPool;
    }

    private void Awake()
    {
        foreach (var platformBehaviour in PlatformBehaviours)
        {
            platformBehaviour.Platform = new Platform(platformBehaviour.LandingTransform.position,
                platformBehaviour.SpawnTransform.position, platformBehaviour.SpawnTransform.rotation);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            var availablePlatforms = PlatformBehaviours.Where(p => !p.Platform.IsPlatformOccupied).ToList();

            if (availablePlatforms.Any())
            {
                var shipBehaviour = _shipPool.Spawn();
                var selectedPlatform = availablePlatforms[Random.Range(0, availablePlatforms.Count)].Platform;

                selectedPlatform.IsPlatformOccupied = true;
                shipBehaviour.transform.position = selectedPlatform.SpawnPosition;
                shipBehaviour.transform.position = selectedPlatform.SpawnPosition;
                shipBehaviour.transform.rotation = selectedPlatform.SpawnRotation;

                shipBehaviour.Ship = new Ship(100, 50, ShipState.Damaged, 50, 10, selectedPlatform); //for test purpose
                shipBehaviour.Land();
            }
        }

        if (Input.GetKeyUp(KeyCode.R)) //for test purpose
        {
            var ships = FindObjectsOfType<ShipBehaviour>().ToList();
            foreach (var ship in ships)
            {
                ship.TakeOff(() =>
                {
                    _shipPool.Despawn(ship);
                });
            }
        }
    }
}
