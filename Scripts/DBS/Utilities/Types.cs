namespace DBS.Utilities
{
    public static class Types
    {
        public enum ManaColors
        {
            Red,
            Yellow,
            Blue,
            Green,
            Purple
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
    }
}