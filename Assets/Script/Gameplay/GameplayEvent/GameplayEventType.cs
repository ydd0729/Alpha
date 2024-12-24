namespace Yd.Gameplay
{
    public enum GameplayEventType
    {
        DamageDetectionStart,
        DamageDetectionEnd,

        ComboDetectionStart,
        ComboDetectionEnd,

        Attack,

        Interact,

        StepLeft,
        StepRight,

        None,

        SwitchWeaponForward,
        SwitchWeaponBackward
    }

    public class GameplayEventArgs
    {
        public static readonly GameplayEventArgs Empty = new();
        public GameplayEventType EventType = GameplayEventType.None;
    }

    public class GameplayAttackEventArgs : GameplayEventArgs
    {
        public int AttackId;
    }
}