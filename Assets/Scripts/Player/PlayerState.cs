public enum PlayerMoveState
{
    IDLE = 0,
    RUN = 1,
    JUMP = 2,
    FALLING = 3,
    STRAFE = 4,
    RUNSTRAFE = 5,
    LEFTSTRAFE = 6
}

public enum PlayerAttackState
{
    NONE,
    SWORD,
    SLASH
}

public enum PlayerStandState { 
    GROUND,
    AIR
}

