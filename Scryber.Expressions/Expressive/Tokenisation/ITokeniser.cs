using System;
using System.Collections;
using System.Collections.Generic;

namespace Scryber.Expressive.Tokenisation
{
    public interface ITokeniser
    {
        public TokenList Tokenise(string expression); 
    }
}
