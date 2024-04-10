using Diacritics.Extensions;

namespace QuizWorld.Application.Common.Helpers;

/// <summary>
/// Represents the string helper.
/// </summary>
public static class StringHelper
{
    /// <summary>
    /// Normalizes the string.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The normalized string.</returns>
    public static string ToNormalizedFormat(this string text)
    {
        return text.RemoveDiacritics().ToLowerInvariant().Trim();
    }
}
