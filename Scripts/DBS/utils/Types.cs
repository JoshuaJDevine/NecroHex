namespace DBS.utils
{
    public static class Types
    {
        public enum UnitAnimations
        {
            Reborn,
            Attack,
            Run,
            Idle
        }
        public enum ManaColors
        {
            Red,
            Yellow,
            Blue,
            Green,
            Purple,
            None
        }

        public enum PlayerAction
        {
            Idle,
            Dragging,
            LeftClick,
            RightClick,
            Action1
        }

        public enum HexDirections
        {
            Sw,
            W,
            Nw,
            Ne,
            E,
            Se,
            None
        }

        public enum Units
        {
            SkeletonWarrior,
            SkeletonArcher,
            SkeletonBerskerker,
            SkeletonMage,
            SkeletonWolf,
            HumanFootman,
            None
        }

        public enum NecromancerAnimations
        {
            Cast,
        }
    }
}