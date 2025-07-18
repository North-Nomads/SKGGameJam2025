using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HighVoltage
{
    public class SwitchMain : MonoBehaviour
    {
        [SerializeField] private Sprite outSprite;
        [SerializeField] private Sprite onSprite;
        [SerializeField] private Sprite offSprite;

        [SerializeField] private SwitchInput input;
        [SerializeField] private SwitchOutput output;

        private SpriteRenderer _renderer;

        public bool IsActive => input.CurrentSource != null && input.CurrentSource.IsActive && _enabled;

        private bool _enabled = true;
        

        public float Consumption
        {
            get
            {
                return _enabled ? output.Output : 0;
            }
        }


        void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            ChangeSprite();
        }


        public void Switch()
        {
            _enabled = !_enabled;
            output.ChangeState(_enabled);
        }

        private void ChangeSprite()
        {
            if(input.CurrentSource == null || !input.CurrentSource.IsActive)
            {
                _renderer.sprite = outSprite;
                return;
            }
            
            if(!_enabled)
            {
                _renderer.sprite = offSprite;
                return;
            }
            _renderer.sprite = onSprite;
        }
    }
}
