using UnityEngine;

namespace HighVoltage
{
    public class MobHighlight : MonoBehaviour
    {
        [SerializeField] Material material;
        private void Start()
        {
            OnDeselected();
        }

        public void OnSelected()
        {
            Debug.Log(material);
            material.SetFloat("_OutlineWidth", 0.06f);
        }

        public void OnDeselected()
        {
            material.SetFloat("_OutlineWidth", 0f);
        }


    }
}
