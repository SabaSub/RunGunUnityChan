using System.Collections;
using System.Collections.Generic;
using SabaSimpleDIContainer.Pipe;
using SabaSimpleDIContainer;
using UnityEngine;

public class ControllerUpdater 
{
    private List<PlayerController> _controllers = new List<PlayerController>();
    private ISubscriber<ControllerAddEvent> _controllerAddEvent;
    [Injection]
    private void InjectControllerAddEvent(ISubscriber<ControllerAddEvent> subscriber)
    {
        _controllerAddEvent = subscriber;
        _controllerAddEvent.Subscribe(OnControllerAdded);
    }
    private void OnControllerAdded(ControllerAddEvent addEvent)
    {
        _controllers.Add(addEvent.controller);
        Debug.Log("ControllerAdded");
    }
   public void UpdateController()
    {
        foreach (PlayerController controller in _controllers)
        {
            controller.UpdateCpntroller();
        }
    }
}
