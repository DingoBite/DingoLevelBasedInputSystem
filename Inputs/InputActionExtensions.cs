using System;
using UnityEngine.InputSystem;

namespace DingoLevelBasedInputSystem.Inputs
{
    public static class InputActionExtensions
    {
        public static void SSubscribe(this InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.started += action;
        }
        
        public static void SPSubscribe(this InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.started += action;
            inputAction.performed += action;
        }
        
        public static void SCSubscribe(this InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.started += action;
            inputAction.canceled += action;
        }
        
        public static void PCSubscribe(this InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.performed += action;
            inputAction.canceled += action;
        }
        
        public static void FullSubscribe(this InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.started += action;
            inputAction.performed += action;
            inputAction.canceled += action;
        }
        
        public static void FullUnSubscribe(this InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            inputAction.started -= action;
            inputAction.performed -= action;
            inputAction.canceled -= action;
        }
    }
}