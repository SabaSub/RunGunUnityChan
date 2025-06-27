using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SabaSimpleDIContainer;
using SabaSimpleDIContainer.Pipe;

public class CharacterHolder : MonoBehaviour
{
    [SerializeField]
    private BaseCharacter _player1;
    [SerializeField]
    private Transform _player1Pos;
    [Injection]
    private IPublisher<CharacterCreateRequest> _characterAddEvent;
    // Start is called before the first frame update
    void Start()
    {
        if (_player1 != null)
        {
            _characterAddEvent.Publish(new (_player1,_player1Pos.position,Quaternion.identity,null));

        }
    }

    
}
