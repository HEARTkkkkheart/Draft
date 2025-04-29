using System;
using UnityEngine;
using Utility;

[RequireComponent(typeof(Animator))]
public class PlayerAnimController : MonoBehaviour
{
    public enum PlayerAnimState
    {
        Idle,
        Walk,
        Jump,
    }
    
    public bool CanPlayAnim = true;

    private Animator _animator;
    private Func<PlayerAnimState> _updateAnimState;
    
    private PlayerAnimState _currentAnimState = PlayerAnimState.Idle;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        if (!_animator.runtimeAnimatorController)
        {
            DebugUtil.LogError($"{name}未挂载Animator Controller");
            CanPlayAnim = false;
        }
    }

    public void SetUpdateAnimState(Func<PlayerAnimState> onUpdateAnimState) => _updateAnimState = onUpdateAnimState;
    
    public void UpdateAnim()
    {
        if (!CanPlayAnim)
            return;
        
        var animState = _updateAnimState?.Invoke() ?? PlayerAnimState.Idle;
        
        if (_currentAnimState != animState)
        {
            // DebugUtil.Log($"AnimState : {Time.frameCount} :  {_currentAnimState.ToString()} == > {animState.ToString()}", "cyan");
            _currentAnimState = animState;
            _animator.Play(_currentAnimState.ToString());
        }
    }
}
