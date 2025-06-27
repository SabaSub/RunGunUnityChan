using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController
{
    private Gamepad _gamePad;
    private bool _isGamePadSet = default;
    private BaseCharacter _controllCharacter;
    private bool _isCharacterSet = default;
   public void UpdateCpntroller()
    {
        
        if(!_isCharacterSet)
        {
            return;
        }
       
        if (!_isGamePadSet)
        {
            KeyBoardUpdate();
        }
        
        
    }
    private void KeyBoardUpdate()
    {
       
        Vector2 keybordMoveValue = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _controllCharacter.MoveCharacter((keybordMoveValue).normalized);
       
    }
    public void SetGamePad(Gamepad gamepad)
    {
        _gamePad = gamepad;
        Debug.Log("setControll");
        _isGamePadSet=true;
    }
    public void SetCharacter(BaseCharacter character)
    {
        _controllCharacter = character;
        Debug.Log("setCharacter");
        _isCharacterSet=true;
    }
}
