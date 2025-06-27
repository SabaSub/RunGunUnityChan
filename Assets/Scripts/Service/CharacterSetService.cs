using System.Collections;
using System.Collections.Generic;
using SabaSimpleDIContainer;
using SabaSimpleDIContainer.Pipe;
using UnityEngine;


public class CharacterSetService
{
    private Queue<PlayerController> _unPairedControllers = new Queue<PlayerController>();
    private Queue<BaseCharacter> _unPairedCharacters= new Queue<BaseCharacter>();
    #region pipe
    private ISubscriber<ControllerAddEvent> _controllerAddEvent;
    private ISubscriber<CharacterAddEvent> _characterAddEvent;
    #endregion
    #region DImethods
    [Injection]
    private void InjectControllerAddEvent(ISubscriber<ControllerAddEvent> subscriber)
    {
        _controllerAddEvent = subscriber;
        _controllerAddEvent.Subscribe(OnControllerAdded);
    }
    [Injection]
    private void InjectCharacterAddEvent(ISubscriber<CharacterAddEvent> subscriber)
    {
        _characterAddEvent = subscriber;
        _characterAddEvent.Subscribe(OnCharacterAdded);
    }
    #endregion
    #region Event
    private void OnCharacterAdded(CharacterAddEvent addEvent)
    {
        if(TryPairUp(addEvent.character))
        {
            return;
        }
        _unPairedCharacters.Enqueue(addEvent.character);
    }
    private void OnControllerAdded(ControllerAddEvent addEvent)
    {
        if (TryPairUp(addEvent.controller))
        {
            return;
        }
        _unPairedControllers.Enqueue(addEvent.controller);
    }
    #endregion
    #region SeUp
    private bool TryPairUp(PlayerController controller)
    {
        if(_unPairedCharacters.Count<=0)
        {
            return false;
        }
        BaseCharacter character = _unPairedCharacters.Dequeue();
        controller.SetCharacter(character);
        return true;
    }
    private bool TryPairUp(BaseCharacter character)
    {
        if(_unPairedControllers.Count<=0)
        {
            return false;
        }
        PlayerController controller = _unPairedControllers.Dequeue();
        controller.SetCharacter(character);
        return true;
    }
    #endregion



}
