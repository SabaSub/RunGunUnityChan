using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SabaSimpleDIContainer;
using SabaSimpleDIContainer.Pipe;

public class CharacterUpdater 
{
    private ISubscriber<CharacterAddEvent> _subscriber;
    private List<BaseCharacter> _characterList=new();
    [Injection]
    private void InjectCharacterAddEvent(ISubscriber<CharacterAddEvent> subscriber)
    {
        _subscriber = subscriber;
        _subscriber.Subscribe(OnCharacterAdded);
    }
    private void OnCharacterAdded(CharacterAddEvent addEvent)
    {
        _characterList.Add(addEvent.character);
        Debug.Log(_characterList.Count);
    }
    public void UpdateCharacter()
    {
        foreach (BaseCharacter character in _characterList)
        {
            character.CharacterUpdate();
        }
    }
}
