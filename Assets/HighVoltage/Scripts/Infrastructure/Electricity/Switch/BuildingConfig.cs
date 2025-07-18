using UnityEngine;

namespace HighVoltage
{
    public class BuildingConfig : ScriptableObject
    {
        [Header("General")]
        [SerializeField] protected GameObject prefab;
        [SerializeField] protected int buildingID;

        [Header("UI settings")]
        [SerializeField] protected Sprite hudIcon;
        [SerializeField] protected int buildPrice;
        
        public GameObject Prefab => prefab;
        public int BuildingID => buildingID;
        public Sprite HUDIcon => hudIcon;
        public int BuildPrice => buildPrice;
    }
}