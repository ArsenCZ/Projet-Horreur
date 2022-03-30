using System.Diagnostics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [Header("EVENTS MONSTER")]
    [SerializeField, Tooltip("Event arret du monstre")]private EventsTrigger m_monsterEventEnd;
    [SerializeField, Tooltip("Event relancement du monstre")]private EventsTrigger m_monsterEventStart;
    
    private States m_state;
    private States m_prevState;

    [HideInInspector]
    public States State => m_state;
    [HideInInspector]
    public States PrevState => m_prevState;

    public delegate void UiActivePauseGame(int p_isActive = 1);

    public UiActivePauseGame DoUiActivePauseGame;
    
    public enum States
    {
        NULL,
        LOADING,
        MAIN_MENU,
        PLAYING,
        PAUSE,
        CINEMATIC
    }

    public void SwitchPauseGame()
    {
        m_prevState = m_state;
        
        //Si on était dans les option on revien dans le menu pause
        if (m_state == States.PAUSE && UIManager.Instance.m_isOption)
        {
            DoUiActivePauseGame?.Invoke(2);
            return;
        }
        if (m_state == States.PLAYING)
        {
            m_monsterEventEnd.Raise();
            
            m_state = States.PAUSE;
            DoUiActivePauseGame?.Invoke();
            return;
        }

        m_monsterEventStart.Raise();
        
        m_state = States.PLAYING;
        DoUiActivePauseGame?.Invoke(0);
    }

    public void SetState(States p_state)
    {
        m_prevState = m_state;
        m_state = p_state;
    }

    protected override string GetSingletonName()
    {
        return "GameManager";
    }
}