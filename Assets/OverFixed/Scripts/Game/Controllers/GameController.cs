using System;
using System.Collections.Generic;
using System.Linq;
using OverFixed.Scripts.Game.Behaviours.Ships;
using OverFixed.Scripts.Game.Models.Ships;
using OverFixed.Scripts.Game.Views.Ships;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public List<PlatformBehaviour> PlatformBehaviours;
    public List<ShipInfo> ShipInfos;

    private ShipBehaviour.Pool _shipPool;
    private IList<PlatformBehaviour> _platformBehaviours;

    private float _timer;

    [Inject]
    private void Construct(ShipBehaviour.Pool shipPool, IList<PlatformBehaviour> platformBehaviours)
    {
        _shipPool = shipPool;
        _platformBehaviours = platformBehaviours;
    }

    private void Awake()
    {
        foreach (var platformBehaviour in _platformBehaviours)
        {
            platformBehaviour.Platform = new Platform(platformBehaviour.LandingTransform.position,
                platformBehaviour.SpawnTransform.position, platformBehaviour.SpawnTransform.rotation);
        }
    }

    private void Update()
    {
        if (_timer <= 0 && SendShipToAvailablePlatform())
        {
            _timer = Random.Range(5f, 10f);
        }
        else
        {
            _timer -= Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space)) // for test purpose
        {
            SendShipToAvailablePlatform();
        }
    }

    private bool SendShipToAvailablePlatform()
    {
        var availablePlatforms = _platformBehaviours.Where(p => !p.Platform.IsPlatformOccupied).ToList();

        if (availablePlatforms.Any())
        {
            var shipBehaviour = _shipPool.Spawn();

            var selectedPlatform = availablePlatforms[Random.Range(0, availablePlatforms.Count)].Platform;

            selectedPlatform.IsPlatformOccupied = true;
            var t = shipBehaviour.transform;
            t.position = selectedPlatform.SpawnPosition;
            t.position = selectedPlatform.SpawnPosition;
            t.rotation = selectedPlatform.SpawnRotation;


            //difficulty calculation here
            shipBehaviour.Ship = new Ship(ShipInfos[Random.Range(0,ShipInfos.Count)], selectedPlatform); 

            var shipView = shipBehaviour.GetComponent<ShipView>();
            if (shipView != null)
            {
                shipView.Bind(shipBehaviour.Ship);
            }

            var shipSectionBehaviours = shipBehaviour.GetComponentsInChildren<ShipSectionBehaviour>().ToList();
            for (var i = 0; i < shipSectionBehaviours.Count; i++)
            {
                shipSectionBehaviours[i].Init(shipBehaviour, shipBehaviour.Ship.ShipSections[i]);
            }

            Debug.Log(shipBehaviour.Ship.Info.State);

            shipBehaviour.Init(shipBehaviour.Ship.Info.State); 
            shipBehaviour.Land();
            return true;
        }

        return false;
    }
}
