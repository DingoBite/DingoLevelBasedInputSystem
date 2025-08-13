using System;
using System.Threading.Tasks;
using DingoProjectAppStructure.Core.AppRootCore;
using DingoProjectAppStructure.Core.Model;
using LevelBasedInputSystem.InputControllerModels;

namespace DingoLevelBasedInputSystem.Sample
{
    public abstract class InputModelAppStateElementBehaviour<T> : AppStateElementBehaviour where T : AppModelBase
    {
        private AppModelRoot _appModel;
        
        protected T InputModel { get; private set; }

        public override Task BindAsync(AppModelRoot appModel)
        {
            _appModel = appModel;
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

        protected override void SubscribeOnly() => _appModel.Get<SingleInputControllersModel>().InputControllerModel.V.SubscribeAndSet<T>(EnableController, DisableController);
        protected override void UnsubscribeOnly() => _appModel.Get<SingleInputControllersModel>().InputControllerModel.V.UnSubscribe(EnableController, DisableController);
    }
}