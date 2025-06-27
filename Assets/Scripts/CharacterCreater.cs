using System;
using SabaSimpleDIContainer;
using SabaSimpleDIContainer.Pipe;
using UnityEngine;
public class CharacterCreater
{
    [Injection]
    private IResolver _resolver;
    [Injection]
    private IPublisher<CharacterAddEvent> _addEvent;
    private ISubscriber<CharacterCreateRequest> _requestEvent;
    #region
    [Injection]
    private void InjectRequestEvent(ISubscriber<CharacterCreateRequest> subscriber)
    {
        _requestEvent = subscriber;
        _requestEvent.Subscribe(OnRequest);
    }
    #endregion
    #region Event
    private void OnRequest(CharacterCreateRequest createRequest)
    {
        BaseCharacter character;
        character = MonoBehaviour.Instantiate(createRequest.character, createRequest.position, createRequest.rotation, createRequest.parent);
        _resolver.Resolve<BaseCharacter>(character);
        _addEvent.Publish(new CharacterAddEvent(character));
    }
    #endregion
}