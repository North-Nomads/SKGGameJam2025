using HighVoltage.Map.Building;
using HighVoltage.StaticData;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace HighVoltage
{
    public class PlayerBuildBehaviour : MonoBehaviour
    {
        private Tilemap _tilemap;
        private IPlayerBuildingService _buildingService;
        private IStaticDataService _dataService;
        private Vector2 _cursorPostion;
        private PlayerInput inputActions;

        private int _selectedBuildingID = 0;

        private void Update()
        {
            _cursorPostion = inputActions.Gameplay.Cursor.ReadValue<Vector2>();
            //TODO: tile highlight
        }

        private Vector3 GetSelectedCellWorldPosition()
        {
            var cursorPos = Camera.main.ScreenToWorldPoint(_cursorPostion);
            return _tilemap.GetCellCenterWorld(_tilemap.WorldToCell(cursorPos));
        }
        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void Awake()
        {
            inputActions = new();
        }

        private void OnPlayerDestroy(InputAction.CallbackContext context)
        {
            var cursorPos = Camera.main.ScreenToWorldPoint(_cursorPostion);
            var hit = Physics2D.Raycast(cursorPos, Vector2.zero, Mathf.Infinity, 1 << 9);

            if (hit.collider == null)
                return;

            Destroy(hit.collider.gameObject);

        }

        private void OnPlayerBuild(InputAction.CallbackContext obj)
        {
            _buildingService.BuildStructure(GetSelectedCellWorldPosition(), 
                _dataService.GetBuildingPrefab(_selectedBuildingID));
        }

        public void Initialize(IStaticDataService dataService, IPlayerBuildingService buildingService)
        {
            _dataService = dataService;
            _buildingService = buildingService;
            _tilemap = _buildingService.MapTilemap;

            inputActions.Gameplay.BuildAction.performed += OnPlayerBuild;
            inputActions.Gameplay.DestroyAction.performed += OnPlayerDestroy;
        }
    }
}
