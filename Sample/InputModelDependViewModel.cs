using System;
using Bind;
using DingoProjectAppStructure.Core.Model;
using DingoProjectAppStructure.Core.ViewModel;
using LevelBasedInputSystem.InputControllerModels;

namespace DingoLevelBasedInputSystem.Sample
{
    public abstract class InputModelDependViewModel<T> : AppViewModelBase where T : AppModelBase
    {
        protected abstract void EnableModel(T model);
        protected abstract void DisableModel(T model);

        private void EnableController(Type type, AppModelBase modelBase)
        {
            if (modelBase is T model)
                EnableModel(model);
        }

        private void DisableController(Type type, AppModelBase modelBase)
        {
            if (modelBase is T model)
                DisableModel(model);
        }
        
        protected InputModelDependViewModel(AppViewModelRoot appViewModelRoot, AppModelRoot appModelRoot) : base(appViewModelRoot, appModelRoot)
        {
            appModelRoot.Get<SingleInputControllersModel>().InputControllerModel.SafeSubscribeAndSet(InputControllerModelInitialized);
        }

        private void InputControllerModelInitialized(InputControllerModel inputControllerModel)
        {
            inputControllerModel?.SubscribeAndSet<T>(EnableController, DisableController);
        }
    }
}