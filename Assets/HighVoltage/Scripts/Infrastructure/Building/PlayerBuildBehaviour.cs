using System;
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
        private Tilemap _tilemap;
        private IPlayerBuildingService _buildingService;
        private Vector2 _cursorPosition;
        private PlayerInput _inputActions;
        private IStaticDataService _dataService;
        private IGameWindowService _gameWindowService;
        private EditingMode _editingMode;



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
                    _buildingService.BuildStructure(GetSelectedCellWorldPosition());
                    break;
                case EditingMode.Demolition:
                    Destroy(GetSelectedBuilding());
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
            _dataService = dataService;
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
            sw.Switch();
        }

        private void OnEditingModeChanged(InputAction.CallbackContext obj)
        {
            if (obj.control is not KeyControl control)
                return;


            if (control.keyCode == Key.Q)
                _editingMode = EditingMode.Building;
            else if(control.keyCode == Key.Tab)
                _editingMode = EditingMode.Wiring;
            else if(control.keyCode == Key.E)
                _editingMode = EditingMode.Demolition;

            _buildingService.ChangedEditingMode(_editingMode);
        }
    }
}
