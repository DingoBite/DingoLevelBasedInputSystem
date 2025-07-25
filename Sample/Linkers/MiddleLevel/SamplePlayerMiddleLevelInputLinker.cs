using LevelBasedInputSystem.InputControllerModels;
using LevelBasedInputSystem.Inputs;
using LevelBasedInputSystem.InputsHandle.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DingoLevelBasedInputSystem.Sample.Linkers.MiddleLevel
{
    public class SamplePlayerMiddleLevelInputLinker : Core.MiddleLevelInputLinker
    {
        [SerializeField] private float _doubleClickDelta;

        private float _leftClickTime = -1;
        
        private static readonly SampleInputProviderModel BlankProviderModel = new();
        private SingleInputControllersModel _inputControllersModel;
        private SampleInputProviderModel M => _inputControllersModel?.InputControllerModel.V?.Model<SampleInputProviderModel>() ?? BlankProviderModel;

        public override void Link(SingleInputControllersModel inputControllersModel, MiddleLevelSourceInput middleLevelSourceInput)
        {
            _inputControllersModel = inputControllersModel;
            var lowLevelPlayerInputsWrapper = middleLevelSourceInput.LowLevelPlayerInputsWrapper;
            if (lowLevelPlayerInputsWrapper == null)
                return;
            
            var escape = lowLevelPlayerInputsWrapper.PlayerInput.actions.FindAction("Escape");
            var focus = lowLevelPlayerInputsWrapper.PlayerInput.actions.FindAction("Focus");
            var mouseViewPosition = lowLevelPlayerInputsWrapper.PlayerInput.actions.FindAction("MouseViewPosition");
            var mouseDelta = lowLevelPlayerInputsWrapper.PlayerInput.actions.FindAction("MouseDelta");
            var movementInput = lowLevelPlayerInputsWrapper.PlayerInput.actions.FindAction("MovementInput");
            var mouseScrollInput = lowLevelPlayerInputsWrapper.PlayerInput.actions.FindAction("MouseScrollInput");
            var leftMouseButton = lowLevelPlayerInputsWrapper.PlayerInput.actions.FindAction("LeftMouseButton");
            var rightMouseButton = lowLevelPlayerInputsWrapper.PlayerInput.actions.FindAction("RightMouseButton");
            
            middleLevelSourceInput.Enable += subscribe;
            middleLevelSourceInput.Disable += unsubscribe;
            lowLevelPlayerInputsWrapper.OnDispose += unsubscribeNoParams;
            
            return;
            
            void mouseViewPositionHandle(InputAction.CallbackContext c) => M.MouseScreenPosition.V = c.ReadValue<Vector2>();
            void mouseDeltaHandle(InputAction.CallbackContext c) => M.MouseDelta.V = c.ReadValue<Vector2>();
            void movementInputHandle(InputAction.CallbackContext c) => M.MovementInput.V = (c.ReadValue<Vector2>(), c.phase);
            void mouseScrollInputHandle(InputAction.CallbackContext c) => M.MouseScrollInput.V = c.ReadValue<float>();
            void leftMouseButtonHandle(InputAction.CallbackContext c)
            {
                if (c.phase is InputActionPhase.Started)
                {
                    var diff = Time.time - _leftClickTime;
                    if (_leftClickTime < 0)
                    {
                        _leftClickTime = Time.time;
                    }
                    else if (diff < _doubleClickDelta)
                    {
                        M.LeftMouseDoubleClick.V = true;
                        _leftClickTime = -1;
                    }
                    else
                    {
                        _leftClickTime = Time.time;
                    }
                }

                if (M.LeftMouseDoubleClick.V)
                {
                    if (c.phase is InputActionPhase.Canceled)
                        M.LeftMouseDoubleClick.V = false;
                    return;
                }
                M.LeftMouseButton.V = c.phase;
                M.LeftMouseDoubleClick.V = false;
            }

            void focusHandle(InputAction.CallbackContext c) => M.Focus.V = c.phase;

            void rightMouseButtonHandle(InputAction.CallbackContext c) => M.RightMouseButton.V = c.phase;
            void escapeHandle(InputAction.CallbackContext c) => M.Escape.V = c.phase;

            void subscribe(MiddleLevelSourceInput _)
            {
                unsubscribeNoParams();
                movementInput.FullSubscribe(movementInputHandle);
                mouseDelta.FullSubscribe(mouseDeltaHandle);
                mouseViewPosition.FullSubscribe(mouseViewPositionHandle);
                mouseScrollInput.FullSubscribe(mouseScrollInputHandle);
                leftMouseButton.FullSubscribe(leftMouseButtonHandle);
                rightMouseButton.FullSubscribe(rightMouseButtonHandle);
                escape.FullSubscribe(escapeHandle);
                focus.FullSubscribe(focusHandle);
            }

            void unsubscribeNoParams() => unsubscribe(null);
            
            void unsubscribe(MiddleLevelSourceInput _)
            {
                M.SendDefaultValues();
                movementInput.FullUnSubscribe(movementInputHandle);
                mouseDelta.FullUnSubscribe(mouseDeltaHandle);
                mouseViewPosition.FullUnSubscribe(mouseViewPositionHandle);
                mouseScrollInput.FullUnSubscribe(mouseScrollInputHandle);
                leftMouseButton.FullUnSubscribe(leftMouseButtonHandle);
                rightMouseButton.FullUnSubscribe(rightMouseButtonHandle);
                escape.FullUnSubscribe(escapeHandle);
                focus.FullUnSubscribe(focusHandle);
            }
        }
    }
}