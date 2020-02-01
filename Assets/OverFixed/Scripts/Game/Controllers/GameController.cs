using System.Collections.Generic;
using OverFixed.Scripts.Game.Behaviours;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    public HangarBehaviour HangarBehaviour;
    private ShipBehaviour.Pool _shipPool;

    [Inject]
    private void Construct(ShipBehaviour.Pool shipPool)
    {
        _shipPool = shipPool;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            var availablePlatforms = new List<int>();
            var platforms = HangarBehaviour.Hangar.IsPlatformOccupied;
            
            for (var i = 0; i < platforms.Length; i++)
            {
                if (!platforms[i])
                {
                    availablePlatforms.Add(i);
                }   
            }

            if (availablePlatforms.Count > 0)
            {
                var ship = _shipPool.Spawn();
                var selectedPlatform = availablePlatforms[Random.Range(0, availablePlatforms.Count)];
                ship.transform.position = HangarBehaviour.SpawnPositions[selectedPlatform].position;
                var landingPosition =  HangarBehaviour.LandingPositions[selectedPlatform].position;
                
                ship.Land(landingPosition);
            }
        }
    }
}
