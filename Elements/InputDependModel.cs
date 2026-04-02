using System;
using System.Threading.Tasks;
using Bind;
using DingoLevelBasedInputSystem.InputControllerModels;
using DingoProjectAppStructure.Core.Model;

namespace DingoLevelBasedInputSystem.Elements
{
    public abstract class InputDependModel<T> : HardLinkAppModelBase where T : AppModelBase
    {
        private InputControllerModel _inputControllerModel;
        public override Task PostInitialize(ExternalDependencies externalDependencies)
        {
            externalDependencies.Get<SingleInputControllers>()?.InputControllerModel.SafeSubscribeAndSet(InputControllerModelInitialized);
            return base.PostInitialize(externalDependencies);
        }

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

private void InputControllerModelInitialized(InputControllerModel inputControllerModel)
        {
            _inputControllerModel?.UnSubscribe(EnableController, DisableController);
            _inputControllerModel = inputControllerModel;
            _inputControllerModel?.SubscribeAndSet<T>(EnableController, DisableController);
        }
    }
}
