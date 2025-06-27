using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SabaSimpleDIContainer;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace SabaSimpleDIContainer.Unity
{
    public static class DiExtend
    {
        public static void RegisterComponent<T>(this IContainer container, T component) where T : MonoBehaviour
        {
            container.Register<T>(component, LifeTime.singleton);
        }
    }
   
    
    
}
