using HighVoltage.Map.Building;
using HighVoltage.StaticData;
using System;
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
        private IPlayerBuildingService _buildingService;
        private EditingMode _editingMode;

        private void Update()
        {
            _cursorPosition = _inputActions.Editing.Cursor.ReadValue<Vector2>();
            //TODO: tile highlight
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

        private void OnEditingMainAction(InputAction.CallbackContext context)
        {
            if(_editingMode == EditingMode.Building)
                _buildingService.BuildStructure(GetSelectedCellWorldPosition());
            else if(_editingMode == EditingMode.Demolition)
            {
                Destroy(GetSelectedBuilding());
            }
            else
            {
                var building = GetSelectedBuilding();
                if (building == null || !building.TryGetComponent(out ICurrentObject obj))
                    return;

                _buildingService.SelectTargetForWiring(obj);
            }

        }

        private void OnEditingSecondaryAction(InputAction.CallbackContext obj)
        {
            // Prevent UI-through clicks
            if (EventSystem.current.IsPointerOverGameObject() || _gameWindowService.HasOpenedWindows())
                return;
            
            var cursorPos = Camera.main.ScreenToWorldPoint(_cursorPosition);
            var hit = Physics2D.Raycast(cursorPos, Vector2.zero, Mathf.Infinity, 1 << 9);

            if (hit.collider == null)
            if (_editingMode != EditingMode.Wiring)
                return; //there just isn't anything for other modes
            
            var building = GetSelectedBuilding();
            if (building == null || !building.TryGetComponent(out ICurrentObject currentObject))
                return;
            _buildingService.SelectTargetForUnwiring(currentObject);
        }

        public void Initialize(IPlayerBuildingService buildingService)
        {
            // Prevent UI-through clicks
            if (EventSystem.current.IsPointerOverGameObject() || _gameWindowService.HasOpenedWindows())
                return;
            
            _buildingService.BuildStructure(GetSelectedCellWorldPosition());
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
        }

        private void OnEditingModeChanged(InputAction.CallbackContext obj)
        {
            if (obj.control is not KeyControl control)
                return;

            if (control.keyCode == Key.Q)
                _editingMode = EditingMode.Building;
            else if(control.keyCode == Key.W)
                _editingMode = EditingMode.Wiring;
            else if(control.keyCode == Key.E)
                _editingMode = EditingMode.Demolition;
        }
    }
}
