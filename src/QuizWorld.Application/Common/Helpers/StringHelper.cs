using Diacritics.Extensions;
using System.Text.RegularExpressions;

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

    /// <summary>Formats the string from LLM.</summary>
    /// <param name="text">The text.</param>
    /// <returns>The formatted string.</returns>
    public static string FormatFromLLM(this string text)
    {
        return Regex.Unescape(text).Replace("```json", "").Replace("```", "").Replace("\n", "");
    }
}
