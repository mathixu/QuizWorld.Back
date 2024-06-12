namespace QuizWorld.Application.Common.Helpers;

public static class IEnumerableHelper
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }
}
