using System.Collections.Generic;
using System.Linq;

namespace VaccineSlotAvailabilityMonitor.Extentions
{
public static class Extension
{
    public static List<T> Join<T>(this List<T> first, List<T> second)
    {
        if (first == null) {
            return second;
        }
        if (second == null) {
            return first;
        }
 
        return first.Concat(second).ToList();
    }
}
}