using System;
using System.Collections.Generic;
using DingoProjectAppStructure.Core.Model;
using UnityEngine;

namespace LevelBasedInputSystem.InputControllerModels
{
    public class InputControllerModel : IReadonlyInputControllerModel
    {
        public event Action<Type, AppModelBase> ModelEnabled;
        public event Action<Type, AppModelBase> ModelDisabled;
        
        private readonly Dictionary<Type, AppModelBase> _models = new();
        private readonly Dictionary<Type, AppModelBase> _disabled = new();
        private readonly Dictionary<Type, AppModelBase> _enabled = new();

        public InputControllerProperties InputControllerProperties { get; }
        public string SourceId => InputControllerProperties.SourceId;
        
        public InputControllerModel(InputControllerProperties inputControllerProperties)
        {
            InputControllerProperties = inputControllerProperties;
        }

        public bool HasModel<T>() => _models.ContainsKey(typeof(T));
        
        /// <summary>
        /// Auto Enable model
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterModel<T>(T model) where T : AppModelBase
        {
            try
            {
                if (model == null)
                    throw new NullReferenceException(nameof(T));
                _models.Add(typeof(T), model);
                if (_enabled.ContainsKey(typeof(T)))
                {
                    _enabled[typeof(T)] = model;
                    ModelEnabled?.Invoke(typeof(T), model);
                }
                else if (_disabled.ContainsKey(typeof(T)))
                {
                    _disabled[typeof(T)] = model;
                    ModelDisabled?.Invoke(typeof(T), model);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void EnableModel<T>() where T : AppModelBase
        {
            if (_disabled.Remove(typeof(T), out var model) && model != null)
            {
                _enabled[typeof(T)] = model;
                ModelEnabled?.Invoke(typeof(T), model);
            }
            else
            {
                _enabled.TryAdd(typeof(T), null);
            }
        }
        
        public void DisableModel<T>() where T : AppModelBase
        {
            if (_enabled.Remove(typeof(T), out var model) && model != null)
            {
                _disabled[typeof(T)] = model;
                ModelDisabled?.Invoke(typeof(T), model);
            }
            else
            {
                _disabled.TryAdd(typeof(T), null);
            }
        }
        
        public bool TryGetModel<T>(out T model) where T : AppModelBase
        {
            model = default;
            if (!_enabled.TryGetValue(typeof(T), out var modelBase))
                return false;

            if (modelBase is T)
            {
                model = modelBase as T;
                return true;
            }

            return false;
        }

        public T Model<T>() where T : AppModelBase
        {
            if (!_enabled.TryGetValue(typeof(T), out var modelBase))
                return default;

            return modelBase as T;
        }
        
        public void Subscribe(Action<Type, AppModelBase> subscribeOnly, Action<Type, AppModelBase> unsubscribeOnly)
        {
            UnSubscribe(subscribeOnly, unsubscribeOnly);
            ModelEnabled += subscribeOnly;
            ModelDisabled += unsubscribeOnly;
        }

        public void SubscribeAndSet<T>(Action<Type, AppModelBase> subscribeOnly, Action<Type, AppModelBase> unsubscribeOnly) where T : AppModelBase
        {
            Subscribe(subscribeOnly, unsubscribeOnly);
            if (TryGetModel<T>(out var model))
                subscribeOnly(typeof(T), model);
        }

        public void UnSubscribe(Action<Type, AppModelBase> subscribeOnly, Action<Type, AppModelBase> unsubscribeOnly)
        {
            ModelEnabled -= subscribeOnly;
            ModelDisabled -= unsubscribeOnly;
        }
    }
}