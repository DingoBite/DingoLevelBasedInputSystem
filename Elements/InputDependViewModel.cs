using System;
using Bind;
using DingoLevelBasedInputSystem.InputControllerModels;
using DingoProjectAppStructure.Core.Model;
using DingoProjectAppStructure.Core.ViewModel;

namespace DingoLevelBasedInputSystem.Elements
{
    public abstract class InputDependViewModel<T> : AppViewModelBase where T : AppModelBase
    {
        private InputControllerModel _inputControllerModel;
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

        protected InputDependViewModel(AppViewModelRoot appViewModelRoot, AppModelRoot appModelRoot) : base(appViewModelRoot, appModelRoot)
        {
            appModelRoot.ExternalDependencies.Get<SingleInputControllers>()?.InputControllerModel.SafeSubscribeAndSet(InputControllerModelInitialized);
        }

private void InputControllerModelInitialized(InputControllerModel inputControllerModel)
        {
            _inputControllerModel?.UnSubscribe(EnableController, DisableController);
            _inputControllerModel = inputControllerModel;
            _inputControllerModel?.SubscribeAndSet<T>(EnableController, DisableController);
        }
    }
}
