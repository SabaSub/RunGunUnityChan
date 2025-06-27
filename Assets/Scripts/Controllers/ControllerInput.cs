using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public static class ControllerInput
{
    public static Keyboard Keyboard { get => _keyboard; }
    private static Keyboard _keyboard;
    private static Gamepad _gamepad;
    public static Gamepad Gamepad { get => _gamepad; }
    public static bool IsGamepadConnect { get => _isGamepadConnect; }
    private static bool _isGamepadConnect = false;

    public static void UpdateInput()
    {
        _isGamepadConnect = IsGamePadConnection();
        _keyboard = Keyboard.current;

        if (_isGamepadConnect)
        {
            return;
        }
        _gamepad = Gamepad.current;
    }


    private static bool IsGamePadConnection()
    {
        if (Gamepad.current == null)
        {
            return false;
        }
        return true;
    }
}

public class Controller
{ }

public enum ControllerType
{
    keybord,
    controller1,
    controller2,
    controller3,
    controller4


}
