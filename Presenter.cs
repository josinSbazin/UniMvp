namespace Mvp
{
    public abstract class Presenter<V, P>
        where V : View<V, P>
        where P : Presenter<V, P>, new()
    {
        protected V View;

        public void SetView(V view)
        {
            View = view;
        }

        /// <summary>
        /// Инициализация в Awake
        /// </summary>
        public virtual void Init()
        {
            //override me
            //здесь инициализируем Rx при необходимости
        }
    }
}