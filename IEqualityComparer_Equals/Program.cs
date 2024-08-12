using System;
using System.Collections.Generic;

class Box
{
    public int? Height { get; }
    public int? Length { get; }
    public int? Width { get; }

    public Box(int? height, int? length, int? width)
    {
        Height = height;
        Length = length;
        Width = width;
    }

    public override string ToString() => $"({Height}, {Length}, {Width})";

    // This overrides all equality whenever Box is used. Might not always want this if implementing a custom comparison.
    // Should override GetHashCode() too if doing this.
    public override bool Equals(object? obj)
    {
        if (obj is not Box) return false;

        return Height == ((Box)obj).Height;
    }

    // GetHashCode() is typically used in HashSet and Dictionary, along with LINQ operations like Distinct() and Group()
    // Note: 2 objects that are equal have the same hash code, but 2 hash codes that are equal don't mean the objects are the same
    public override int GetHashCode()
    {
        return Height.GetHashCode();
    }
}

// Good to use for implementing a custom comparison. Shouldn't always override an object's Equals(), especially if it's from a library.
class BoxEqualityComparer : IEqualityComparer<Box>
{
    // Equals() should match with GetHashCode()
    public bool Equals(Box? b1, Box? b2)
    {
        if (ReferenceEquals(b1, b2))
            return true;

        if (b2 is null || b1 is null)
            return false;

        return b1.Height == b2.Height
            && b1.Length == b2.Length
            && b1.Width == b2.Width;
    }

    // GetHashCode() is typically used in HashSet and Dictionary, along with LINQ operations like Distinct() and Group().
    // Note: 2 objects that are equal have the same hash code, but 2 hash codes that are equal don't mean the objects are the same
    public int GetHashCode(Box box)
    {
        int hashHeight = box.Height == null ? 0 : box.Height.GetHashCode();
        int hashLength = box.Length == null ? 0 : box.Length.GetHashCode();
        int hashWidth = box.Width == null ? 0 : box.Width.GetHashCode();

        return hashHeight ^ hashLength ^ hashWidth;
    }
}

// The example displays the following output:
//    Unable to add (4, 3, 4): An item with the same key has already been added.
//    The dictionary contains 2 Box objects.
static class Example
{
    static void Main()
    {
        BoxEqualityComparer comparer = new();

        Dictionary<Box, string> boxes = new(comparer);

        var box1 = new Box(4, 3, 4);
        var box2 = new Box(4, 3, 4);
        var box3 = new Box(3, 4, 3);
        var box4 = new Box(4, 4, 3);

        AddBox(box1, "red");
        AddBox(box2, "blue");
        AddBox(box3, "green");

        Console.WriteLine($"The dictionary contains {boxes.Count} Box objects.");

        void AddBox(Box box, string name)
        {
            try
            {
                boxes.Add(box, name);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Unable to add {box}: {e.Message}");
            }
        }

        Console.WriteLine("Does box1 equals box2 with IEqualityComparer: " + comparer.Equals(box1, box2));
        Console.WriteLine("Does box1 equals box3 with IEqualityComparer: " + comparer.Equals(box1, box3));
        Console.WriteLine("Does box1 equals box3 with IEqualityComparer: " + comparer.Equals(box1, box4));
        Console.WriteLine("Does box1 equals box2 with overriden Equals(): " + box1.Equals(box2));
        Console.WriteLine("Does box1 equals box3 with overriden Equals(): " + box1.Equals(box3));
        Console.WriteLine("Does box1 equals box3 with overriden Equals(): " + box1.Equals(box4));

        // Since this uses GetHashCode() in Box, this should only have 2 due to many having the same Height.
        var boxList = new HashSet<Box>() { box1, box2, box3, box4 };
        Console.WriteLine("Number of elements in the Box HasSet: " + boxList.Count);

        var boxList2 = new List<Box>() { box1, box2, box3, box4 };
        Console.WriteLine("Number of Distinct elements in the Box List: " + boxList2.Distinct().Count());
    }
}
