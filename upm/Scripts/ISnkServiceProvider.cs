using System;
using System.Collections.Generic;

namespace SnkFramework.DependencyInjection
{
    public interface ISnkServiceProvider
    {
        /// <summary>
        /// 对target对象进行注入
        /// </summary>
        /// <param name="target">注入对象</param>
        void Inject(object target);
        
        /// <summary>
        /// 获取服务对象
        /// </summary>
        /// <param name="serviceType">服务类型（一般为接口）</param>
        /// <returns>服务实例对象</returns>
        object GetService(Type serviceType);
        
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T">服务类型（一般为接口）</typeparam>
        /// <returns>服务实例对象</returns>
        T GetService<T>() where T : class;
        
        /// <summary>
        /// 获取服务集
        /// </summary>
        /// <typeparam name="T">服务类型（一般为接口）</typeparam>
        /// <returns>服务集</returns>
        IEnumerable<T> GetServices<T>() where T : class;
        
        /// <summary>
        /// 创建实例对象
        /// </summary>
        /// <param name="inject">是否需要对该对象进行注入</param>
        /// <param name="parameters">创建实例的构造方法参数</param>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>对象实例</returns>
        public T CreateInstance<T>(bool inject = true, params object[] parameters) where T : class;
        
        /// <summary>
        /// 创建实例对象
        /// </summary>
        /// <param name="instanceType">对象类型</param>
        /// <param name="inject">是否需要对该对象进行注入</param>
        /// <param name="parameters">创建实例的构造方法参数</param>
        /// <returns>对象实例</returns>
        public object CreateInstance(Type instanceType, bool inject = true, params object[] parameters);
    }
}