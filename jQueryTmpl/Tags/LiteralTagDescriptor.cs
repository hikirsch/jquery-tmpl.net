﻿using System.Text.RegularExpressions;

using jQueryTmpl.Parsing;
using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Tags
{
    public class LiteralTagDescriptor : ITagDescriptor
    {
        public Regex Pattern
        {
            get { return new Regex(@"^"); }
        }

        public bool IsStartTag
        {
            get { return false; }
        }

        public bool IsEndTag
        {
            get { return false; }
        }

        public bool CanClose(ITagDescriptor descriptor)
        {
            return false;
        }

        public Template CreateTemplate(Token token)
        {
            return new LiteralTemplate(token);
        }

        public bool IsFinalCloseFor(ITagDescriptor descriptor)
        {
            return false;
        }
    }
}