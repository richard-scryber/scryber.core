using System;
using System.Collections;
using System.Collections.Generic;

namespace Scryber.Expressive.Tokenisation
{
    public interface ITokeniser
    {
        TokenList Tokenise(string expression); 
    }
}
