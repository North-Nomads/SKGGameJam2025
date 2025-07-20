using HighVoltage.Infrastructure.Services;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HighVoltage.Map.Building
{
    public interface IPlayerBuildingService : IService
    {
        public Tilemap MapTilemap { get; set; }
        public void BuildStructure(Vector3 worldCoordinates);
        public void SelectedSentryChanged(int selectedSentry);
        public void SelectTargetForWiring(ICurrentObject building);
        public void SelectTargetForUnwiring(ICurrentObject building);
        public void ChangedEditingMode(EditingMode mode);
        public void OnSceneLoaded();
        void SetCursorPosition(Vector2 cursorPosition);
    }
}
