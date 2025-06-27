using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;





namespace SabaSimpleDIContainer
{
    public interface IContainer
    {
        public void StartContainer();
        public void Register<T>(LifeTime lifeTime) where T : class;
        public void Register<T>(object instance, LifeTime lifeTime) where T : class;
        public void Register<Key, Value>() where Key : class where Value : Key;
        public void RegisterEntryPoint<T>() where T : class;
        public void DisposeAll();
    }
    public interface IResolver
    {
        public void Resolve<T>(object instance) where T : class;
    }

    public class DIContainer : IContainer, IResolver
    {
        private Dictionary<Type, List<object>> _resolvedInstances = new(); // 解決済み
        private Dictionary<Type, List<object>> _unresolvedInstances = new(); // 未解決
        private Dictionary<Type, InjectInfo> _infos = new();
        private Dictionary<Type, LifeTime> _lifeTimes = new();
        private HashSet<Type> _resolvingTypes = new();
        private Tickable _frameManager;
        private Startable _startableManager;
        
        /// <summary>
        /// 初期化
        /// </summary>
        public DIContainer()
        {
            Register<IResolver>(this, LifeTime.singleton);            
            _frameManager = new(60,SynchronizationContext.Current);
            _startableManager = new Startable();
        }
        /// <summary>
        /// フレーム指定
        /// </summary>
        /// <param name="frameLate"></param>
        public DIContainer(int frameLate)
        {
            Register<IResolver>(this, LifeTime.singleton);
            _frameManager = new(frameLate, SynchronizationContext.Current);
            _startableManager = new Startable();
        }
        #region IContainer
        public async void StartContainer()
        {
            try
            {
                InjectAll();
                _startableManager.StartAll();
                await _frameManager.Start();
            }
            catch (Exception ex)
            {
                
                Debug.LogError($"DIContainer Error: {ex.Message}");
            }
            finally
            {
                _frameManager.Stop();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InjectAll()
        {
            foreach (Type type in _unresolvedInstances.Keys.ToList()) // 未解決の型を順に解決
            {
                InjectSearchTree(type);
            }
        }
        public void RegisterEntryPoint<T>() where T : class
        {
            Type type = typeof(T);
            object instance = Activator.CreateInstance(type);
            _infos[type] = GetInjectValue(type);
            Register<T>(instance, LifeTime.singleton);
            // 未解決として登録
            RegisterUnresolvedInstance(type, instance);
            if (instance is IStartable startable)
            {
                _startableManager.AddStartable(startable);
            }
            if (instance is ITickable tickable)
            {
                _frameManager.AddTickable(tickable);
            }



        }

        public void Register<T>(LifeTime lifeTime) where T : class
        {
            Type type = typeof(T);

            if (_unresolvedInstances.ContainsKey(type) || _resolvedInstances.ContainsKey(type)) return;

            _infos[type] = GetInjectValue(type);
            _lifeTimes[type] = lifeTime;

            //// 未解決として登録
            //RegisterUnresolvedInstance(type, Activator.CreateInstance(type));
        }

        public void Register<T>(object instance, LifeTime lifeTime) where T : class
        {
            Type type = typeof(T);

            if (instance is not T)
            {
                throw new ArgumentException($"Instance type {instance.GetType()} does not match the expected type {type}");
            }

            _infos[type] = GetInjectValue(type);
            _lifeTimes[type] = lifeTime;

            // 未解決として登録
            RegisterUnresolvedInstance(type, instance);
        }

        public void Register<Key, Value>() where Key : class where Value : Key
        {
            _infos[typeof(Key)] = GetInjectValue(typeof(Value));
            //Value instance = (Value)Activator.CreateInstance(typeof(Value));
            //RegisterUnresolvedInstance(typeof(Key), instance);
        }

        private void RegisterUnresolvedInstance(Type type, object instance)
        {
            if (!_unresolvedInstances.ContainsKey(type))
            {
                _unresolvedInstances[type] = new List<object>();
            }

            // 未解決インスタンスとして登録
            _unresolvedInstances[type].Add(instance);
        }

        private void RegisterResolvedInstance(Type type, object instance)
        {
            if (!_resolvedInstances.ContainsKey(type))
            {
                _resolvedInstances[type] = new List<object>();
            }

            // 解決済みインスタンスとして登録
            _resolvedInstances[type].Add(instance);

            // 未解決リストから削除
            if (_unresolvedInstances.ContainsKey(type))
            {
                _unresolvedInstances[type].Remove(instance);
                if (_unresolvedInstances[type].Count == 0)
                {
                    _unresolvedInstances.Remove(type);
                }
            }
        }

        private object InjectSearchTree(Type type)
        {
            

            if (_lifeTimes[type] == LifeTime.singleton && _resolvedInstances.TryGetValue(type, out List<object> values))
            {
                return values[0];
            }

            if (_resolvingTypes.Contains(type))
            {
                throw new InvalidOperationException($"循環依存が発生しました: {type}");
            }

            _resolvingTypes.Add(type);

            try
            {
                if (!_infos.ContainsKey(type))
                {
                    return null;
                }

                object instance;

                if (_lifeTimes[type] == LifeTime.singleton && _unresolvedInstances.TryGetValue(type, out List<object> instances))
                {
                    instance = instances[0];
                }
                else
                {
                    instance = Activator.CreateInstance(type);
                    RegisterUnresolvedInstance(type, instance);
                }


                InjectTrees(type,instance);
                // 解決済みとして登録
                if (_lifeTimes[type] == LifeTime.singleton)
                {
                    RegisterResolvedInstance(type, instance);
                }

                return instance;
            }
            finally
            {
                _resolvingTypes.Remove(type);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InjectTrees(Type type,object instance)
        {
            InjectFieldTree(type, instance);
            InjectMethodTree(type, instance);
            InjectPropertyTree(type, instance);
        }
        private object InjectFieldTree(Type type, object instance)
        {
            if (!_infos.ContainsKey(type))
            {
                return null;
            }
            foreach (FieldInfo info in _infos[type].fieldInfos)
            {
                if (!IsDefinedInjection(info))
                {
                    continue;
                }
                info.SetValue(instance, InjectSearchTree(info.FieldType));
            }
            return instance;
        }

        private object InjectMethodTree(Type type, object instance)
        {
            if (!_infos.ContainsKey(type)) return null;

            foreach (InjectMethodInfo info in _infos[type].methodInfos)
            {
                if (!IsDefinedInjection(info.info)) continue;

                object[] parameters = info.parameters
                    .Select(param => InjectSearchTree(param.ParameterType))
                    .ToArray();

                info.info.Invoke(instance, parameters);
            }
            return instance;
        }

        private object InjectPropertyTree(Type type, object instance)
        {
            if (!_infos.ContainsKey(type)) return null;

            foreach (PropertyInfo info in _infos[type].propertyInfos)
            {
                if (!IsDefinedInjection(info)) continue;

                object injectInstance = InjectSearchTree(info.PropertyType);
                info.SetValue(instance, injectInstance);
            }
            return instance;
        }

        private InjectInfo GetInjectValue(Type type)
        {
            return new InjectInfo(
                type,
                GetInjectMethodInfos(type),
                GetFieldInfos(type),
                GetPropertyInfos(type));
        }

        private List<InjectMethodInfo> GetInjectMethodInfos(Type type)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            return methods.Select(method => new InjectMethodInfo(method)).ToList();
        }

        private List<FieldInfo> GetFieldInfos(Type type)
        {
            return type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList();
        }

        private List<PropertyInfo> GetPropertyInfos(Type type)
        {
            return type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).ToList();
        }

        private bool IsDefinedInjection(MemberInfo info)
        {
            return info.IsDefined(typeof(Injection), false);
        }

        public void DisposeAll()
        {
            _resolvedInstances.Clear();
            _unresolvedInstances.Clear();
            _infos.Clear();
            _frameManager.Stop();
        }
        #endregion
        #region IResolver
        void IResolver.Resolve<T>(object instance) where T : class
        {
            Register<T>(instance, LifeTime.Transient);
            InjectTrees(typeof(T), instance);
        }
        #endregion
    }


    public enum LifeTime
    {
        singleton,
        Transient
    }

    /// <summary>
    /// 注入可能属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class Injection : Attribute
    {

    }


}

