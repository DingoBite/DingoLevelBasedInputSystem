using DingoProjectAppStructure.Core.Model;

namespace LevelBasedInputSystem.InputControllerModels
{
    public interface IReadonlyInputControllerModel
    {
        public InputControllerProperties InputControllerProperties { get; }
        public string SourceId { get; }
        public bool HasModel<T>();
        public bool TryGetModel<T>(out T model) where T : AppModelBase;
        public T Model<T>() where T : AppModelBase;
    }
}