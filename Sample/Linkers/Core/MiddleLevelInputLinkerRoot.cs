using System.Collections.Generic;
using DingoUnityExtensions;
using DingoUnityExtensions.MonoBehaviours.Singletons;
using DingoUnityExtensions.MonoBehaviours.UI.RaycastSafetyArea;
using LevelBasedInputSystem.InputControllerModels;
using LevelBasedInputSystem.InputsHandle.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DingoLevelBasedInputSystem.Sample.Linkers.Core
{
    public class MiddleLevelInputLinkerRoot : ProtectedSingletonBehaviour<MiddleLevelInputLinkerRoot>
    {
        [SerializeField] private List<MiddleLevelInputLinker> _linkers;
        [SerializeField] private EventSystem _eventSystem;

        private MiddleLevelSourceInput _middleLevelSourceInput;
        private bool _isFocused;
        private bool _isOverSafeArea;

        public MiddleLevelSourceInput LinkFunction(SingleInputControllersModel inputControllersModel, LowLevelPlayerInputsWrapper lowLevelPlayerInputsWrapper)
        {
            _middleLevelSourceInput = new MiddleLevelSourceInput(lowLevelPlayerInputsWrapper);

            foreach (var linker in _linkers)
            {
                linker.Link(inputControllersModel, _middleLevelSourceInput);
            }
         
            CoroutineParent.AddLateUpdater(this, UpdateCheckEventSystem, CoroutineOrderLayers.MAX_PRIORITY_SPECIAL);
            return _middleLevelSourceInput;
        }

        public static bool IsFocused => Instance._isFocused;
        public static bool IsOverUIArea => Instance._isOverSafeArea;
        public static bool IsAnyUIUnderPointer => IsFocused || IsOverUIArea;
        
        public static void ForceCheckFocused() => Instance.UpdateCheckEventSystem(false);
        private void UpdateCheckEventSystem() => UpdateCheckEventSystem(true);
        
        private void UpdateCheckEventSystem(bool manageInputSystemActiveness)
        {
            var selectedGameObject = _eventSystem.currentSelectedGameObject;
            _isOverSafeArea = RaycastSafetyArea.CheckOverSafetyArea();
            _isFocused = _eventSystem.isFocused && 
                         selectedGameObject != null && 
                         selectedGameObject.TryGetComponent<Selectable>(out var selectable) && 
                         selectable.IsInteractable() && 
                         selectable is ISubmitHandler or ICancelHandler;
            
            if (!manageInputSystemActiveness)
                return;
            if (!_isFocused)
                _middleLevelSourceInput.Enabled = true;
            else
                _middleLevelSourceInput.Enabled = false;
        }
    }
}