namespace Faux.Lib.Models;

public class FauxPropertyInfo(string name = null, int id = 0)
{
    protected bool Equals(FauxPropertyInfo other)
    {
        return PropertyName == other.PropertyName && PropertyId == other.PropertyId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((FauxPropertyInfo)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PropertyName, PropertyId);
    }

    public string PropertyName { get; set; } = name;
    public int PropertyId { get; set; } = id;

}