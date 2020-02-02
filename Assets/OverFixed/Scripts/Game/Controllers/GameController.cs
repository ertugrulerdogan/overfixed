using System.Collections.Generic;
using System.Linq;
using OverFixed.Scripts.Game.Behaviours.Ships;
using OverFixed.Scripts.Game.Models.Data;
using OverFixed.Scripts.Game.Models.Ships;
using OverFixed.Scripts.Game.Views.Ships;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public List<PlatformBehaviour> PlatformBehaviours;
    public List<ShipInfo> ShipInfos;

    private TeamData _teamData;
    private ShipBehaviour.Pool _shipPool;
    private IList<PlatformBehaviour> _platformBehaviours;

    private float _timer;

    private const float MaxDifficulty = 100f; // for test purpose
    private float _currentDifficulty;

    [Inject]
    private void Initialize(TeamData teamData, ShipBehaviour.Pool shipPool, IList<PlatformBehaviour> platformBehaviours)
    {
        _teamData = teamData;
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
        //check game condition

        //CheckGameCondition();

        SendShip();
    }

    private void SendShip()
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
            ShipInfo info = new ShipInfo();

            if (SelectShipInfo(ref info))
            {
                _currentDifficulty += info.DifficultyPoint;

                var shipBehaviour = _shipPool.Spawn();
                var selectedPlatform = availablePlatforms[Random.Range(0, availablePlatforms.Count)].Platform;

                selectedPlatform.IsPlatformOccupied = true;

                var t = shipBehaviour.transform;
                t.position = selectedPlatform.SpawnPosition;
                t.position = selectedPlatform.SpawnPosition;
                t.rotation = selectedPlatform.SpawnRotation;
                
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

                Debug.Log($"Current Difficulty {_currentDifficulty}/100");

                shipBehaviour.Init(shipBehaviour.Ship.Info.State, f =>
                {
                    _teamData.WarStatus += f;
                    _currentDifficulty -= info.DifficultyPoint;


                    Debug.Log($"WarStatus: {_teamData.WarStatus}");
                    Debug.Log($"Current Difficulty {_currentDifficulty}/100");
                }); 
                shipBehaviour.Land();
                return true;
            }
        }

        return false;
    }

    private bool SelectShipInfo(ref ShipInfo info)
    {
        var budget = MaxDifficulty - _currentDifficulty;

        var availableShips = ShipInfos.Where(s => s.DifficultyPoint < budget).ToList();
        if (availableShips.Any())
        {
            info = availableShips[Random.Range(0, availableShips.Count)];
            return true;
        }

        return false;
    }
}
