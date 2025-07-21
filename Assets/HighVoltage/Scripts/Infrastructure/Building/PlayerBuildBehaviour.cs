using System;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Infrastructure.Tutorial;
using HighVoltage.Map.Building;
using HighVoltage.StaticData;
using HighVoltage.UI.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Tilemaps;

namespace HighVoltage
{
    public class PlayerBuildBehaviour : MonoBehaviour
    {
        private IGameWindowService _gameWindowService;
        private IPlayerBuildingService _buildingService;
        private IEventSenderService _eventSender;
        private PlayerInput _inputActions;
        private EditingMode _editingMode;
        private Vector2 _cursorPosition;
        private Tilemap _tilemap;

        private void Update()
        {
            _cursorPosition = _inputActions.Editing.Cursor.ReadValue<Vector2>();
            //TODO: tile highlight
            if (_editingMode == EditingMode.Wiring)
                _buildingService.SetCursorPosition(Camera.main.ScreenToWorldPoint(_cursorPosition));
        }

        private Vector3 GetSelectedCellWorldPosition()
        {
            var cursorPos = Camera.main.ScreenToWorldPoint(_cursorPosition);
            return _tilemap.GetCellCenterWorld(_tilemap.WorldToCell(cursorPos));
        }
        private GameObject GetSelectedBuilding()
        {
            var cursorPos = Camera.main.ScreenToWorldPoint(_cursorPosition);
            var hit = Physics2D.Raycast(cursorPos, Vector2.zero, Mathf.Infinity, 1 << 9);

            if (hit.collider == null)
                return null;
            return hit.collider.gameObject;
        }
        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void Awake()
        {
            _inputActions = new();
            _eventSender = AllServices.Container.Single<IEventSenderService>();
        }

        private void OnDestroy()
        {
            _inputActions.Editing.EditingActionMain.performed -= OnEditingMainAction;
            _inputActions.Editing.EditingActionSecondary.performed -= OnEditingSecondaryAction;
            _inputActions.Editing.SwitchEditingMode.performed -= OnEditingModeChanged;
        }

        private void OnEditingMainAction(InputAction.CallbackContext context)
        {
            // Prevent UI-through clicks
            if (EventSystem.current.IsPointerOverGameObject() || _gameWindowService.HasOpenedWindows())
                return;

            switch (_editingMode)
            {
                case EditingMode.Building:
                    if (GetSelectedBuilding() == null)
                        _buildingService.BuildStructure(GetSelectedCellWorldPosition());
                    break;
                case EditingMode.Demolition:
                    _buildingService.DemolishStructure(GetSelectedBuilding());
                    _eventSender.NotifyEventHappened(TutorialEventType.SentryDestroyed);
                    break;
                case EditingMode.Wiring:
                    var building = GetSelectedBuilding();
                    if (building == null || !building.TryGetComponent(out ICurrentObject obj))
                        return;

                    _buildingService.SelectTargetForWiring(obj);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Editing mode {_editingMode} has no handler");
            }
        }

        private void OnEditingSecondaryAction(InputAction.CallbackContext obj)
        {
            if (_editingMode != EditingMode.Wiring)
                return; //there just isn't anything for other modes
            
            var building = GetSelectedBuilding();
            if (building == null || !building.TryGetComponent(out ICurrentObject currentObject))
                return;
            _buildingService.SelectTargetForUnwiring(currentObject);
        }

        public void Initialize(IStaticDataService dataService, IPlayerBuildingService buildingService,
            IGameWindowService gameWindowService)
        {
            _buildingService = buildingService;
            _gameWindowService = gameWindowService;
            _tilemap = _buildingService.MapTilemap;

            _inputActions.Editing.EditingActionMain.performed += OnEditingMainAction;
            _inputActions.Editing.EditingActionSecondary.performed += OnEditingSecondaryAction;
            _inputActions.Editing.SwitchEditingMode.performed += OnEditingModeChanged;
            _inputActions.Editing.MiddleMouseButtonClick.performed += SwitchSelectedSwitch;
        }

        private void SwitchSelectedSwitch(InputAction.CallbackContext obj)
        {
            var selectedSwitch = GetSelectedBuilding();
            if (selectedSwitch == null || !selectedSwitch.TryGetComponent(out SwitchMain sw))
                return;
            _eventSender.NotifyEventHappened(TutorialEventType.FuseBoxChanged);
            sw.Switch();
        }

        private void OnEditingModeChanged(InputAction.CallbackContext obj)
        {
            if (obj.control is not KeyControl control)
                return;

            switch (control.keyCode)
            {
                case Key.Q:
                    _editingMode = EditingMode.Building;
                    _eventSender.NotifyEventHappened(TutorialEventType.Q);
                    break;
                case Key.Tab:
                    _editingMode = EditingMode.Wiring;
                    _eventSender.NotifyEventHappened(TutorialEventType.TAB);
                    break;
                case Key.E:
                    _editingMode = EditingMode.Demolition;
                    _eventSender.NotifyEventHappened(TutorialEventType.E);
                    break;
            }

            _buildingService.ChangedEditingMode(_editingMode);
        }
    }
}
