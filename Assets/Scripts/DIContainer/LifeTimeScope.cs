using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SabaSimpleDIContainer;

namespace SabaSimpleDIContainer
{

    /// <summary>
    /// gameObjectÇ…Ç¬ÇØÇƒDIContainerÇÃinstanceÇï€éùÇµÇƒÇ¢ÇÈ
    /// </summary>

    public class LifeTimeScope : MonoBehaviour
    {
        private IContainer _container;
        private delegate void StartUpDel();
        private StartUpDel _startUpDel;
        private void Awake()
        {
            _startUpDel += CallConfigure;
            _startUpDel += CallInjectAll;
            int frame = Mathf.FloorToInt(1 / Time.fixedDeltaTime);
            _container = new DIContainer(frame);
            _startUpDel.Invoke();

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        private void CallConfigure()
        {
            Configure(_container);
        }
        private void CallInjectAll()
        {
            _container.StartContainer();
        }
        protected virtual void Configure(IContainer container)
        {

        }
        private void OnDestroy()
        {
            DisposeAll();
            
        }
        private void DisposeAll()
        {
            _container.DisposeAll();
            _container = null;
        }

       
    }
}
