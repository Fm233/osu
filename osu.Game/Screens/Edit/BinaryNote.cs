using System.Collections;
using System.Collections.Generic;

public struct BinaryNote
{
    public int id;

    /// <summary>
    /// The time position of the dot in samples when it hits.
    /// </summary>
    public int p;

    /// <summary>
    /// The direction that the dot will go. 0 = centre, 1 = up, 2 = down, 3 = left, 4 = right.
    /// </summary>
    public int d;

    /// <summary>
    /// The strength type of the dot. 1 = light, 2 = midium, 3 = strong.
    /// </summary>
    public int s;

    public BinaryNote(int p, int d, int s, int id)
    {
        this.p = p;
        this.d = d;
        this.s = s;
        this.id = id;
    }

    public override bool Equals(object obj)
    {
        return obj is BinaryNote note &&
               p == note.p &&
               d == note.d &&
               s == note.s;
    }

    public override int GetHashCode()
    {
        int hashCode = 449283404;
        hashCode = hashCode * -1521134295 + p.GetHashCode();
        hashCode = hashCode * -1521134295 + d.GetHashCode();
        hashCode = hashCode * -1521134295 + s.GetHashCode();
        return hashCode;
    }

    public static bool operator >(BinaryNote a, BinaryNote b)
    {
        if (a.p > b.p)
        {
            return true;
        }
        if (a.p == b.p && a.d > b.d)
        {
            return true;
        }
        return false;
    }

    public static bool operator <(BinaryNote a, BinaryNote b)
    {
        if (a.p < b.p)
        {
            return true;
        }
        if (a.p == b.p && a.d < b.d)
        {
            return true;
        }
        return false;
    }
}
