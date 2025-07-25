using Bind;
using DingoProjectAppStructure.Core.Model;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DingoLevelBasedInputSystem.Sample
{
    public class InputProviderModel : AppModelBase
    {
        public readonly Bind<(Vector2, InputActionPhase)> MovementInput = new();
        public readonly Bind<Vector2> MouseScreenPosition = new();
        public readonly Bind<Vector2> MouseDelta = new();
        public readonly Bind<float> MouseScrollInput = new();
        public readonly Bind<InputActionPhase> LeftMouseButton = new();
        public readonly Bind<InputActionPhase> RightMouseButton = new();
        public readonly Bind<InputActionPhase> Escape = new();
        public readonly Bind<InputActionPhase> Focus = new();
        public readonly Bind<bool> LeftMouseDoubleClick = new(equalityCheck:true);

        public bool IsPointerOutOfScreen
        {
            get
            {
                if (!Application.isFocused)
                    return true;
                var position = MouseScreenPosition.V;
                return position.x < 0 || position.x > Screen.width || position.y < 0 || position.y > Screen.height;
            }
        }
        
        public void SendDefaultValues()
        {
            MouseScreenPosition.V = default;
            MouseDelta.V = default;
            MouseScrollInput.V = default;
            LeftMouseButton.V = default;
            LeftMouseDoubleClick.V = default;
            MovementInput.V = default;
            RightMouseButton.V = default;
            Escape.V = default;
            Focus.V = default;
        }
    }
}