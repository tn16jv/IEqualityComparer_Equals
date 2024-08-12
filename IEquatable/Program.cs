using System;
using System.Collections.Generic;

class Box2 : IEquatable<Box2>
{
    public int? Height { get; }
    public int? Length { get; }
    public int? Width { get; }

    public Box2(int? height, int? length, int? width)
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
        if (obj is not Box2) return false;

        return Height == ((Box2)obj).Height;
    }

    // GetHashCode() is typically used in HashSet and Dictionary, along with LINQ operations like Distinct() and Group()
    // Note: 2 objects that are equal have the same hash code, but 2 hash codes that are equal don't mean the objects are the same
    public override int GetHashCode()
    {
        return Height.GetHashCode();
    }

    // Need to implement this function for IEquatable interface
    public bool Equals(Box2? other)
    {
        return Height == ((Box2)other).Height;
    }

    // Need to implement this operator for IEquatable interface
    public static bool operator ==(Box2 person1, Box2 person2)
    {
        if (((object)person1) == null || ((object)person2) == null)
            return Object.Equals(person1, person2);

        return person1.Equals(person2);
    }

    // Need to implement this operator for IEquatable interface
    public static bool operator !=(Box2 person1, Box2 person2)
    {
        if (((object)person1) == null || ((object)person2) == null)
            return !Object.Equals(person1, person2);

        return !(person1.Equals(person2));
    }
}

// The example displays the following output:
//    Unable to add (4, 3, 4): An item with the same key has already been added.
//    The dictionary contains 2 Box objects.
static class Example2
{
    static void Main()
    {
        Dictionary<Box2, string> boxes = new Dictionary<Box2, string>();

        var box1 = new Box2(4, 3, 4);
        var box2 = new Box2(4, 3, 4);
        var box3 = new Box2(3, 4, 3);
        var box4 = new Box2(4, 4, 3);

        AddBox(box1, "red");
        AddBox(box2, "blue");
        AddBox(box3, "green");

        Console.WriteLine($"The dictionary contains {boxes.Count} Box objects.");

        void AddBox(Box2 box, string name)
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

        Console.WriteLine("Does box1 equals box2 with overriden Equals(): " + box1.Equals(box2));
        Console.WriteLine("Does box1 equals box3 with overriden Equals(): " + box1.Equals(box3));
        Console.WriteLine("Does box1 equals box3 with overriden Equals(): " + box1.Equals(box4));
        Console.WriteLine("Does box1 equals box2 with overriden == operator: " + (box1 == box2));
        Console.WriteLine("Does box1 equals box3 with overriden == operator: " + (box1 == box3));
        Console.WriteLine("Does box1 equals box3 with overriden == operator: " + (box1 == box4));

        // Since this uses GetHashCode() in Box, this should only have 2 due to many having the same Height.
        var boxList = new HashSet<Box2>() { box1, box2, box3, box4 };
        Console.WriteLine("Number of elements in the Box HasSet: " + boxList.Count);

        var boxList2 = new List<Box2>() { box1, box2, box3, box4 };
        Console.WriteLine("Number of Distinct elements in the Box List: " + boxList2.Distinct().Count());
    }
}
