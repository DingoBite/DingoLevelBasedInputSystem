using System;
using System.Threading.Tasks;
using DingoLevelBasedInputSystem.InputControllerModels;
using DingoProjectAppStructure.Core.AppRootCore;
using DingoProjectAppStructure.Core.Model;

namespace DingoLevelBasedInputSystem.Elements
{
    public abstract class InputDependAppStateElementBehaviour<T> : AppStateElementBehaviour where T : AppModelBase
    {
        private AppModelRoot _appModel;
        private SingleInputControllers _inputControllers;

        protected T InputModel { get; private set; }

        public override Task BindAsync(AppModelRoot appModel)
        {
            _appModel = appModel;
            _inputControllers = appModel.ExternalDependencies.Get<SingleInputControllers>();
            return base.BindAsync(appModel);
        }

        protected abstract void EnableModel(T model);
        protected abstract void DisableModel(T model);

        private void EnableController(Type type, AppModelBase modelBase)
        {
            if (modelBase is T model)
            {
                InputModel = model;
                EnableModel(model);
            }
        }

        private void DisableController(Type type, AppModelBase modelBase)
        {
            if (modelBase is T model)
            {
                DisableModel(model);
                InputModel = null;
            }
        }

        protected override void SubscribeOnly() => _inputControllers?.InputControllerModel.V?.SubscribeAndSet<T>(EnableController, DisableController);
        protected override void UnsubscribeOnly() => _inputControllers?.InputControllerModel.V?.UnSubscribe(EnableController, DisableController);
    }
}
