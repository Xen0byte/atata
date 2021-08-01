﻿using System.Globalization;
using System.Linq;

namespace Atata.TermFormatting
{
    public class CamelTermFormatter : ITermFormatter
    {
        public string Format(string[] words)
        {
            return string.Concat(
                new[] { words.First().ToLower(CultureInfo.CurrentCulture) }.
                    Concat(words.Skip(1).Select(x => char.ToUpper(x[0], CultureInfo.CurrentCulture) + x.Substring(1).ToLower(CultureInfo.CurrentCulture))));
        }
    }
}
