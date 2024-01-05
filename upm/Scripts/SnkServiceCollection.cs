using System;
using System.Collections;
using System.Collections.Generic;

namespace SnkFramework.DependencyInjection
{
    public class SnkServiceCollection : ISnkServiceCollection
    {
        /// <summary>
        /// 异常文本：服务容器不能被修改，因为它是只读的(read-only)
        /// </summary>
        private const string ServiceCollectionReadOnly = "The service collection cannot be modified because it is read-only.";
        
        private readonly List<SnkServiceDescriptor> _descriptors = new List<SnkServiceDescriptor>();
        private bool _isReadOnly;

        /// <summary>
        /// 待注入服务描述对象的数量
        /// </summary>
        public int Count => _descriptors.Count;

        /// <summary>
        /// 是否为只读容器
        /// </summary>
        public bool IsReadOnly => _isReadOnly;

        /// <summary>
        /// 数组访问（只读模式将会抛异常）
        /// </summary>
        /// <param name="index">数组下标</param>
        public SnkServiceDescriptor this[int index]
        {
            get => _descriptors[index];
            set
            {
                CheckReadOnly();
                _descriptors[index] = value;
            }
        }

        /// <summary>
        /// 清理所有服务描述对象。（只读模式：将会抛出异常InvalidOperationException）
        /// </summary>
        public void Clear()
        {
            CheckReadOnly();
            _descriptors.Clear();
        }

        /// <summary>
        /// 是否包含服务描述对象
        /// </summary>
        /// <param name="item">服务描述对象</param>
        /// <returns>是否成功</returns>
        public bool Contains(SnkServiceDescriptor item)
            => _descriptors.Contains(item);
        
        
        /// <summary>
        /// 复制到数组
        /// </summary>
        /// <param name="array">描述对象数组对象</param>
        /// <param name="arrayIndex">下标</param>
        public void CopyTo(SnkServiceDescriptor[] array, int arrayIndex)
            => _descriptors.CopyTo(array, arrayIndex);

        /// <summary>
        /// 移除服务描述对象（只读模式：将会抛出异常InvalidOperationException）
        /// </summary>
        /// <param name="item">服务描述对象</param>
        /// <returns>是否成功</returns>
        public bool Remove(SnkServiceDescriptor item)
        {
            CheckReadOnly();
            return _descriptors.Remove(item);
        }

        /// <summary>
        /// 获取服务描述对象迭代器
        /// </summary>
        /// <returns>迭代器对象</returns>
        public IEnumerator<SnkServiceDescriptor> GetEnumerator()
            => _descriptors.GetEnumerator();

        /// <summary>
        /// 添加服务描述对象
        /// </summary>
        /// <param name="item">服务描述对象</param>
        /// <exception cref="ArgumentNullException">传入参数为null是抛出</exception>
        void ICollection<SnkServiceDescriptor>.Add(SnkServiceDescriptor item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            CheckReadOnly();
            _descriptors.Add(item);
        }

        /// <summary>
        /// 获取服务描述对象迭代器
        /// </summary>
        /// <returns>迭代器对象</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// 根据服务描述对象获取在数组中的下标
        /// </summary>
        /// <param name="item">服务描述对象</param>
        /// <returns>数组中的下标</returns>
        public int IndexOf(SnkServiceDescriptor item)
            => _descriptors.IndexOf(item);

        /// <summary>
        /// 在下标位置插入服务描述对象（只读模式：将会抛出异常InvalidOperationException）
        /// </summary>
        /// <param name="index">下标</param>
        /// <param name="item">服务描述对象</param>
        public void Insert(int index, SnkServiceDescriptor item)
        {
            CheckReadOnly();
            _descriptors.Insert(index, item);
        }

        /// <summary>
        /// 移除下标处的服务描述对象（只读模式：将会抛出异常InvalidOperationException）
        /// </summary>
        /// <param name="index">下标</param>
        public void RemoveAt(int index)
        {
            CheckReadOnly();
            _descriptors.RemoveAt(index);
        }

        /// <summary>
        /// 将容器设置为只读模式。如果有对其修改操作，将会抛出异常InvalidOperationException。
        /// </summary>
        public void MakeReadOnly()
        {
            _isReadOnly = true;
        }

        /// <summary>
        /// 检查只读模式，如果是，将会抛出异常InvalidOperationException。
        /// </summary>
        private void CheckReadOnly()
        {
            if (_isReadOnly)
            {
                ThrowReadOnlyException();
            }
        }

        /// <summary>
        /// 异常封装
        /// </summary>
        /// <exception cref="InvalidOperationException">服务容器不能被修改，因为它是只读的(read-only)</exception>
        private static void ThrowReadOnlyException() =>
            throw new InvalidOperationException(ServiceCollectionReadOnly);

        /// <summary>
        /// 调试输出。（服务描述对象数量，以及是否是只读模式）
        /// </summary>
        /// <returns>调试日志</returns>
        private string DebuggerToString()
        {
            string debugText = $"Count = {_descriptors.Count}";
            if (_isReadOnly)
            {
                debugText += $", IsReadOnly = true";
            }
            return debugText;
        }
    }
}