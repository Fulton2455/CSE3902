namespace _3902sprint0.Environment.Doors
{
    //The kinds of door a room border can hold. Each maps to one symbol in a room file.
    public enum DoorKind
    {
        Doorway,   // always open
        Keyhole,   // opens with a small key
        Diamond,   // opens with a diamond key
        Bombable,  // looks like wall until a fireball explosion hits it
        Shutter    // closed while enemies are alive in the room
    }
}
