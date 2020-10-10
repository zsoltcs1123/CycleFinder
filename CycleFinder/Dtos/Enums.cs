using System.ComponentModel;

namespace CycleFinder.Dtos
{
    public enum MarkerPosition
    {
        [Description("aboveBar")]
        AboveBar,
        [Description("belowBar")]
        BelowBar
    }

    public enum MarkerShape
    {
        ArrowUp,
        ArrowDown,
        Circle,
        Square
    }
}
