
using System;

public interface IEquipSlot
{
    public void SetID(string _id, Action<string> grab);
}
