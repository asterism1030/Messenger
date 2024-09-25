using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace messenger.designPattern
{
    public class Singleton<T> where T : class
    {
        protected Singleton() { }

        private static T _instance;
        private static readonly object _lock = new object();


        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // 스레드 안전
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = Activator.CreateInstance(typeof(T), true) as T;
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
