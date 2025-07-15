using HighVoltage.Map.Building;
using HighVoltage.StaticData;
using System;
using HighVoltage.UI.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace HighVoltage
{
    public class PlayerBuildBehaviour : MonoBehaviour
    {
        private Tilemap _tilemap;
        private Vector2 _cursorPosition;
        private PlayerInput _inputActions;
        private IStaticDataService _dataService;
        private IGameWindowService _gameWindowService;
        private IPlayerBuildingService _buildingService;

        private void Update()
        {
            _cursorPosition = _inputActions.Gameplay.Cursor.ReadValue<Vector2>();
            //TODO: tile highlight
        }

        private Vector3 GetSelectedCellWorldPosition()
        {
            var cursorPos = Camera.main.ScreenToWorldPoint(_cursorPosition);
            return _tilemap.GetCellCenterWorld(_tilemap.WorldToCell(cursorPos));
        }
        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void Awake()
        {
            _inputActions = new();
        }

        private void OnPlayerDestroy(InputAction.CallbackContext context)
        {
            // Prevent UI-through clicks
            if (EventSystem.current.IsPointerOverGameObject() || _gameWindowService.HasOpenedWindows())
                return;
            
            var cursorPos = Camera.main.ScreenToWorldPoint(_cursorPosition);
            var hit = Physics2D.Raycast(cursorPos, Vector2.zero, Mathf.Infinity, 1 << 9);

            if (hit.collider == null)
                return;

            Destroy(hit.collider.gameObject);

        }

        private void OnPlayerBuild(InputAction.CallbackContext obj)
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

            _inputActions.Gameplay.BuildAction.performed += OnPlayerBuild;
            _inputActions.Gameplay.DestroyAction.performed += OnPlayerDestroy;
        }
    }
}
