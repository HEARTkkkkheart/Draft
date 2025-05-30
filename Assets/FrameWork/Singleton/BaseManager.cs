namespace Framework
{
    /// <summary>
    /// 不继承Mono的单例基类
    /// </summary>
    /// <typeparam name="T"> 单例类型 </typeparam>
    public class BaseManager<T> where T : new()
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new T();
                return _instance;
            }
        }
    }
}