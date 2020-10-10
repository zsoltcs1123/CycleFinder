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
        [Description("arrowUp")]
        ArrowUp,
        [Description("arrowDown")]
        ArrowDown,
        [Description("circle")]
        Circle,
        [Description("square")]
        Square
    }
}
