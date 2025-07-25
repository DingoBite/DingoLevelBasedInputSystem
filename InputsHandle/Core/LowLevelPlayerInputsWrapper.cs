using System;
using UnityEngine.InputSystem;

namespace LevelBasedInputSystem.InputsHandle.Core
{
    public class LowLevelPlayerInputsWrapper : IDisposable
    {
        public event Action OnDispose;
        
        public readonly string Id;
        public readonly PlayerInput PlayerInput;

        public LowLevelPlayerInputsWrapper(string id, PlayerInput playerInput)
        {
            PlayerInput = playerInput;
            Id = id;
        }

        public bool Enabled
        {
            get => PlayerInput.enabled;
            set => PlayerInput.enabled = value;
        }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }
    }
}