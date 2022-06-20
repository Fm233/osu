namespace osu.Game.Rulesets.Objects.Types
{
    /// <summary>
    /// A type of hit object which lies in one of a number of predetermined columns.
    /// </summary>
    public interface ICanSetColumn
    {
        /// <summary>
        /// The column which the hit object lies in.
        /// </summary>
        int Column { get; set; }
    }
}
