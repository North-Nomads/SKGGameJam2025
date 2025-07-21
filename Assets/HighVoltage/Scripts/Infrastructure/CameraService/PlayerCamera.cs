using Cinemachine;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Infrastructure.Tutorial;
using HighVoltage.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HighVoltage.Infrastructure.CameraService
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private float cameraMoveSpeed = 5f;

        private IEventSenderService _eventSenderService;
        private CinemachineVirtualCamera _virtualCamera;
        private InputAction _moveCameraAction;
        private CameraBounds _cameraBounds;
        private PlayerInput _playerInput; 
        private Vector2 _moveInput;
        private GameObject _runner;

        private void Awake()
        {
            _eventSenderService = AllServices.Container.Single<IEventSenderService>();
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _cameraBounds = FindObjectOfType<CameraBounds>();
            _runner = new GameObject
            {
                transform =
                {
                    position = new Vector3(transform.position.x, transform.position.y)
                }
            };
            transform.position += Vector3.back;
            _virtualCamera.Follow = _runner.transform;
            
            GameObject tilemap = GameObject.FindGameObjectWithTag(Constants.TilemapTagName);
            if (tilemap == null)
                Debug.LogError("Tilemap not found. It must be specified on \"Tilemap\" gameobject for camera boundries");
            
            _playerInput = new();
        }

        private void OnEnable()
        {
            // Fetch the MoveCamera action from the InputActionAsset
            _moveCameraAction = _playerInput.Editing.MoveCamera;
            _moveCameraAction.performed += OnMoveCamera;
            _moveCameraAction.canceled += OnMoveCamera; // Important to reset to (0,0) when input stops
            _moveCameraAction.Enable();
        }

        private void OnDisable()
        {
            _moveCameraAction.performed -= OnMoveCamera;
            _moveCameraAction.canceled -= OnMoveCamera;
            _moveCameraAction.Disable();
        }

        private void OnMoveCamera(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            _eventSenderService.NotifyEventHappened(TutorialEventType.WASD);
        }

        private void Update()
        {
            // Move the camera transform directly
            _runner.transform.position += (Vector3)_moveInput * (cameraMoveSpeed * Time.deltaTime);
            Vector3 newPosition = _runner.transform.position;
            newPosition.x = Mathf.Clamp(newPosition.x, _cameraBounds.Left, _cameraBounds.Right);
            newPosition.y = Mathf.Clamp(newPosition.y, _cameraBounds.Down, _cameraBounds.Top);
            _runner.transform.position = newPosition;
        }
    }
}