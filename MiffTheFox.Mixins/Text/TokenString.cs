using System;
using System.Collections.Generic;
using System.Text;

namespace MiffTheFox.Text
{
    /*
    public readonly struct TokenString
    {
        private readonly TokenStringToken[] _Tokens;

        public TokenString(string baseString, char openToken = '{', char closeToken = '}')
        {
            int tokenCount = 1;
            for (int i = 0; i < baseString.Length; i++)
            {
                if (baseString[i] == openToken || baseString[i] == closeToken)
                {
                    tokenCount++;
                }
            }

            var tokenData = new TokenStringToken[tokenCount];
            int tokenDataIndex = 0;
            int currentTokenStart = 0;
            bool currentTokenSpecial = false;

            for (int i = 0; i < baseString.Length; i++)
            {
                if (baseString[i] == openToken && !currentTokenSpecial)
                {
                    int l = i - currentTokenStart;
                    if (l > 0)
                    {
                        tokenData[tokenDataIndex] = new TokenStringToken(baseString.Substring(currentTokenStart, l), false);
                        currentTokenStart = i + 1;
                    }
                }
            }
        }
    }

    public readonly struct TokenStringToken
    {
        public string Text { get; }
        public bool IsSpecialToken { get; }

        public TokenStringToken(string text, bool isSpecialToken)
        {
            Text = text;
            IsSpecialToken = isSpecialToken;
        }
    }
    */
}
