using System.Collections.Generic;

public static class ListExtensions
{
    public static T Shift<T>(this IList<T> list)
    {
        var item = list[0];
        list.RemoveAt(0);
        return item;
    }
}