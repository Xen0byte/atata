﻿namespace Atata
{
    /// <summary>
    /// Specifies the term settings.
    /// </summary>
    public class TermSettingsAttribute : MulticastAttribute, ITermSettings
    {
        public TermSettingsAttribute()
        {
        }

        public TermSettingsAttribute(TermCase termCase)
        {
            Case = termCase;
        }

        public TermSettingsAttribute(TermMatch match)
        {
            Match = match;
        }

        public TermSettingsAttribute(TermMatch match, TermCase termCase)
        {
            Match = match;
            Case = termCase;
        }

        /// <summary>
        /// Gets the match.
        /// </summary>
        public new TermMatch Match
        {
            get { return Properties.Get(nameof(Match), TermMatch.Equals); }
            private set { Properties[nameof(Match)] = value; }
        }

        /// <summary>
        /// Gets the term case.
        /// </summary>
        public TermCase Case
        {
            get { return Properties.Get(nameof(Case), TermCase.None); }
            private set { Properties[nameof(Case)] = value; }
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public string Format
        {
            get { return Properties.Get<string>(nameof(Format)); }
            set { Properties[nameof(Format)] = value; }
        }
    }
}
