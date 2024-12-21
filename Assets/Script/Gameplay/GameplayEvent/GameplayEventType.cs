namespace Yd.Gameplay
{
    public enum GameplayEventType
    {
        DamageDetectionStart,
        DamageDetectionEnd,
        
        ComboDetectionStart,
        ComboDetectionEnd,
        
        NormalAttack,
        
        Interact,
        
        StepLeft,
        StepRight,
        
        // obsolete
        StepLeftMiddle,
        StepRightMiddle,
        PunchSound,
        KickSound,
        
        Attack01,
        Attack02,
        Attack03,
        
        // obsolete
        BoarStepSound,
        BoarAttackSound
    }
}