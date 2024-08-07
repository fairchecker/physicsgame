public struct EntryButtonData
{
    public Directions Direction;

    public enum Directions
    {
        Kinematics,
        Dynamics,
        Electrodynamics
    }

    public EntryButtonData(Directions direction)
    {
        Direction = direction;
    }
}