using UnityEngine;

namespace Mvp
{
    /// <summary>
    /// Нельзя создавать динамически объект View в сцене через GameObject.Instantiate.
    /// Создание только через Фабрику или DiContainer
    /// 
    /// Do not use GameObject.Instantiate if you want your objects to have their dependencies injected
    ///
    /// If you want to instantiate a prefab at runtime and have any MonoBehaviour's automatically injected,
    /// we recommend using a factory. You can also instantiate a prefab by directly using the DiContainer
    /// by calling any of the InstantiatePrefab methods. Using these ways as opposed to GameObject.Instantiate
    /// will ensure any fields that are marked with the [Inject] attribute are filled in properly, and all [Inject]
    /// methods within the prefab are called.
    /// </summary>
    public abstract class View<V, P> : MonoBehaviour
        where V : View<V, P>
        where P : Presenter<V, P>, new()
    {
        // cached transform
        [HideInInspector] public Transform Transform;
        protected P Presenter;

        /// <summary>
        /// Ищем компоненты здесь. Вызываем base
        /// </summary>
        protected virtual void FindComponents()
        {
            Transform = transform;
        }

        /// <summary>
        /// Не будь дураком! Вызывай base.Awake
        /// </summary>
        protected virtual void Awake()
        {
            SetPresenter();
            FindComponents();
            
            Presenter.Init();
        }
        
        private void SetPresenter()
        {
            Presenter = new P();
            Presenter.SetView(this as V);
        }

    }
}