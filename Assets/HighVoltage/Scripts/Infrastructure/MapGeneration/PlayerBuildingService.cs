using HighVoltage.Infrastructure.BuildingStore;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Map.Building;
using HighVoltage.StaticData;
using System.Collections.Generic;
using HighVoltage.Infrastructure.Tutorial;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

namespace HighVoltage
{
    public class PlayerBuildingService : IPlayerBuildingService
    {
        private int _selectedBuildingID;
        private ICurrentReceiver _selectedReceiver;
        private ICurrentSource _selectedSource;

        private readonly IStaticDataService _staticDataService;
        private readonly IGameFactory _gameFactory;
        private readonly IMobSpawnerService _mobSpawnerService;
        private readonly IBuildingStoreService _buildingStoreService;
        private readonly IEventSenderService _eventSender;


        private readonly List<LineRenderer> _wires = new();
        private LineRenderer _previewWire;
        private bool _canBuild;

        public Tilemap MapTilemap { get; set; }

        public PlayerBuildingService(IStaticDataService staticDataService, IGameFactory gameFactory,
            IMobSpawnerService mobSpawnerService, IBuildingStoreService buildingStoreService,
            IEventSenderService eventSender)
        {
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
            _mobSpawnerService = mobSpawnerService;
            _buildingStoreService = buildingStoreService;
            _eventSender = eventSender;
            _selectedBuildingID = 1;
            
        }

        public void ToggleBuildingAllowance(bool isBuildingAllowed)
        {
            _canBuild = isBuildingAllowed;
        }

        public void BuildStructure(Vector3 worldCoordinates)
        {
            if (!_canBuild)
                return;
            
            TileBase tile = MapTilemap.GetTile(MapTilemap.WorldToCell(worldCoordinates));
            if (tile is not BuildableTile { isBuildable: true })
                return;

            SentryConfig sentryConfig = _staticDataService.ForSentryID(_selectedBuildingID);
            SwitchConfig switchConfig = _staticDataService.ForSwitchID(_selectedBuildingID);
            
            BuildingConfig thingToBuild = sentryConfig == null ? switchConfig : sentryConfig;
            
            if (!_buildingStoreService.CanAfford(thingToBuild))
                return;

            if (sentryConfig == null)
            {
                SwitchMain switchMain =
                    _gameFactory.CreateSwitch(worldCoordinates, switchConfig);
                _eventSender.NotifyEventHappened(TutorialEventType.FuseBoxPlaced);   
            }
            else
            {
                SentryTower sentryTower = _gameFactory.CreateSentry(worldCoordinates, sentryConfig);
                sentryTower.Initialize(sentryConfig, _mobSpawnerService, _gameFactory);
                _eventSender.NotifyEventHappened(TutorialEventType.SentryPlaced);
            }
            
            
            _buildingStoreService.SpendMoneyOn(thingToBuild);
        }


        public void SelectedSentryChanged(int selectedSentry)
            => _selectedBuildingID = selectedSentry;

        public void SelectTargetForWiring(ICurrentObject building)
        {
            _previewWire.enabled = true;
            _previewWire.SetPosition(0, (building as MonoBehaviour).transform.position);
            if (building is ICurrentReceiver receiver)
                _selectedReceiver = receiver;
            else if (building is ICurrentSource source)
                _selectedSource = source;

            if (_selectedSource != null && _selectedReceiver != null)
            {
                if (ConnectionHasLoops(_selectedReceiver))
                    return;
                
                if (_selectedReceiver.Wire != null)
                {
                    _selectedReceiver.CurrentSource.Wires.Remove(_selectedReceiver.Wire);
                    _wires.Remove(_selectedReceiver.Wire);
                    Object.Destroy(_selectedReceiver.Wire);
                }
                
                _selectedReceiver.CurrentSource?.DetachReceiver(_selectedReceiver);
                _selectedReceiver.AttachToSource(_selectedSource);
                _selectedSource.AttachReceiver(_selectedReceiver);
                AddWire();
                _eventSender.NotifyEventHappened(TutorialEventType.SentryEnabled);

                _previewWire.enabled = false;
                _selectedReceiver = null;
                _selectedSource = null;
            }
        }

#pragma warning disable CS0253
        private bool ConnectionHasLoops(ICurrentReceiver receiver)
        {
            if (receiver is not SwitchInput input)
                return false;
            var source = input.SwitchMain.Output;
            if (source == _selectedSource)
                return true;

            foreach (var connectedReceiver in source.Receivers)
            {
                if (connectedReceiver is not SwitchInput nextInput)
                    continue;

                var nextSource = nextInput.SwitchMain.Output;

                if (ConnectionHasLoops(nextSource.SwitchMain.Input))
                    return true;
            }
            return false;
        }
#pragma warning restore CS0253  

        public void ChangedEditingMode(EditingMode newMode)
        {
            if (newMode == EditingMode.Wiring)
            {
                _wires.ForEach(x => x.enabled = true);
            }
            else
            {
                _wires.ForEach(x => x.enabled = false);
                if (_previewWire)
                    _previewWire.enabled = false;
                _selectedReceiver = null;
                _selectedSource = null;
            }
        }

        public void SelectTargetForUnwiring(ICurrentObject building)
        {
            //I went for 2in1 approach on deselction
            //not the best Idea, oh well
            if(building == _selectedReceiver)
            {
                _selectedReceiver = null;
                _previewWire.enabled = false;
            }

            if (building == _selectedSource)
            {
                _selectedSource = null;
                _previewWire.enabled = false;
            }

            if (building is ICurrentSource source)
            {
                source.DetachAllReceivers();
                source.Wires.ForEach(x => Object.Destroy(x));
                source.Wires.Clear();
            }
            else if (building is ICurrentReceiver receiver)
            {
                //i didn't feel like adding DetachFromSource, mb
                _wires.Remove(receiver.Wire);
                receiver.CurrentSource.Wires.Remove(receiver.Wire);
                Object.Destroy(receiver.Wire);

                receiver.CurrentSource.DetachReceiver(receiver);
                receiver.AttachToSource(null);
            }
        }
        private void AddWire()
        {
            var outPos = (_selectedReceiver as MonoBehaviour).gameObject.transform.position;
            var inPos = (_selectedSource as MonoBehaviour).gameObject.transform.position;

            var wire = Object.Instantiate(_staticDataService.GetWirePrefab());
            wire.SetPositions(new Vector3[] { outPos, inPos });
            _selectedReceiver.Wire = wire;
            _selectedSource.Wires.Add(wire);
            _wires.Add(wire);
        }

        public void OnSceneLoaded()
        {
            _wires.ForEach(wire => Object.Destroy(wire));
            _wires.Clear();
            _previewWire = Object.Instantiate(_staticDataService.GetWirePrefab());
            ChangedEditingMode(EditingMode.Building);
        }

        public void SetCursorPosition(Vector2 cursorPosition)
        {
            if(_previewWire.enabled)
                _previewWire.SetPosition(1, cursorPosition);
        }

        public void DemolishStructure(GameObject structure)
        {
            if (structure == null || structure.CompareTag("PlayerCore"))
                return;
            ICurrentReceiver receiver = null;
            ICurrentSource source = null;
            if(structure.TryGetComponent(out SwitchInput input))
            {
                structure = input.SwitchMain.gameObject;
            }
            else if(structure.TryGetComponent(out SwitchOutput output))
            {
                structure = output.SwitchMain.gameObject;
            }
            if(structure.TryGetComponent(out SwitchMain switchMain))
            {
                receiver = switchMain.Input;
                source = switchMain.Output;
            }
            else if (structure.TryGetComponent(out ICurrentObject connection))
            {
                if (connection is ICurrentReceiver)
                    receiver = connection as ICurrentReceiver;
                if(connection is ICurrentSource)
                    source = connection as ICurrentSource;
            }
            if (source != null)
            {
                source.DetachAllReceivers();
                foreach (var wire in source.Wires.ToList())
                {
                    _wires.Remove(wire);
                    Object.Destroy(wire);
                }
            }

            if (receiver?.CurrentSource != null)
            {
                receiver.CurrentSource.Wires.Remove(receiver.Wire);
                receiver.CurrentSource.DetachReceiver(receiver);
            }

            if (receiver?.Wire != null)
            {
                _wires.Remove(receiver.Wire);
                Object.Destroy(receiver.Wire);
            }

            Object.Destroy(structure);
        }
    }
}
