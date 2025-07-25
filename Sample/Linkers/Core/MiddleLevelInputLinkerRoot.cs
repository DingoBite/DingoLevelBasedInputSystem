using System.Collections.Generic;
using DingoUnityExtensions;
using LevelBasedInputSystem.InputControllerModels;
using LevelBasedInputSystem.InputsHandle.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DingoLevelBasedInputSystem.Sample.Linkers.Core
{
    public class MiddleLevelInputLinkerRoot : MonoBehaviour
    {
        [SerializeField] private List<MiddleLevelInputLinker> _linkers;
        [SerializeField] private EventSystem _eventSystem;

        private MiddleLevelSourceInput _middleLevelSourceInput;
        private bool _isFocused;

        public MiddleLevelSourceInput LinkFunction(SingleInputControllersModel inputControllersModel, LowLevelPlayerInputsWrapper lowLevelPlayerInputsWrapper)
        {
            _middleLevelSourceInput = new MiddleLevelSourceInput(lowLevelPlayerInputsWrapper);

            foreach (var linker in _linkers)
            {
                linker.Link(inputControllersModel, _middleLevelSourceInput);
            }
         
            CoroutineParent.AddLateUpdater(this, LateUpdateCheckEventSystem);
            return _middleLevelSourceInput;
        }

        private void LateUpdateCheckEventSystem()
        {
            var prevFocused = _isFocused;
            var selectedGameObject = _eventSystem.currentSelectedGameObject;
            _isFocused = _eventSystem.isFocused && 
                         selectedGameObject != null && 
                         selectedGameObject.TryGetComponent<Selectable>(out var selectable) && 
                         selectable.IsInteractable() && 
                         selectable is ISubmitHandler or ICancelHandler or IScrollHandler;
            
            if (prevFocused == _isFocused)
                return;
            Debug.Log($"{nameof(EventSystem)} is focused: {_eventSystem.isFocused}: {selectedGameObject}");
            if (!_isFocused)
                _middleLevelSourceInput.Enabled = true;
            else
                _middleLevelSourceInput.Enabled = false;
        }
    }
}