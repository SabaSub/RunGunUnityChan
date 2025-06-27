using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SabaSimpleDIContainer;
using SabaSimpleDIContainer.Unity;
using SabaSimpleDIContainer.Pipe;

public class GameLifeTimeScope : LifeTimeScope
{
    [SerializeField]
    private CharacterHolder _characterHolder;
    
    protected override void Configure(IContainer container)
    {
        #region RregisterComponent
        if(_characterHolder!=null)
        {
            container.RegisterComponent(_characterHolder);
        }
        
        #endregion
        #region register
        //
        container.Register<CharacterUpdater>(LifeTime.singleton);
        //
        container.Register<ControllerUpdater>(LifeTime.singleton);
        //
        container.Register<CollisionChecker>(LifeTime.singleton);
        
        #endregion
        #region entry
        container.RegisterEntryPoint<CharacterSetService>();
        container.RegisterEntryPoint<ControllerProvider>();
        container.RegisterEntryPoint<ControllerUpdater>();
        container.RegisterEntryPoint<CharacterUpdater>();
        container.RegisterEntryPoint<CollisionableUpdater>();
        container.RegisterEntryPoint<GameUpdater>();
        container.RegisterEntryPoint<CollisionableProvider>();
        container.RegisterEntryPoint<CharacterCreater>();
        #endregion
        #region pipe
        container.RegisterBroker<ControllerAddEvent>();
        container.RegisterBroker<CharacterAddEvent>();
        container.RegisterBroker<CharacterCreateRequest>();
        container.RegisterBroker<CollisionableAddEvent>();
        #endregion
    }
}
