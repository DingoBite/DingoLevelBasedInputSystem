using LevelBasedInputSystem.InputControllerModels;
using LevelBasedInputSystem.InputsHandle.Core;
using UnityEngine;

namespace DingoLevelBasedInputSystem.Sample.Linkers.Core
{
    public abstract class MiddleLevelInputLinker : MonoBehaviour
    {
        public abstract void Link(SingleInputControllersModel inputControllersModel, MiddleLevelSourceInput middleLevelSourceInput);
    }
}