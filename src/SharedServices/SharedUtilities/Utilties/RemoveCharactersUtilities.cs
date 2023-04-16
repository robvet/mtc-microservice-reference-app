using System.Collections.Generic;

namespace SharedUtilities.Utilties
{
    public static class RemoveCharactersUtilities
    {
        /// <summary>
        ///     Utility to remove specified characters
        /// </summary>
        /// <param name="target"></param>
        /// <param name="charsToRemove"></param>
        /// <returns></returns>
        public static string RemoveCharacters(this string target, List<char> charsToRemove)
        {
            foreach (var c in charsToRemove) target = target.Replace(c.ToString(), string.Empty);

            return target;
        }
    }
}