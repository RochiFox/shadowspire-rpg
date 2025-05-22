using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    public List<int> modifiers;

    public int GetValue()
    {
        return baseValue + modifiers.Sum();
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }

    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}