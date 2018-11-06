﻿// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// <auto-generated/>

using DotNet.Globbing.Token;

namespace DotNet.Globbing.Evaluation
{
    internal interface IGlobTokenEvaluatorFactory
    {
        IGlobTokenEvaluator CreateTokenEvaluator(AnyCharacterToken token);
        IGlobTokenEvaluator CreateTokenEvaluator(CharacterListToken token);
        IGlobTokenEvaluator CreateTokenEvaluator(LetterRangeToken token);
        IGlobTokenEvaluator CreateTokenEvaluator(LiteralToken token);
        IGlobTokenEvaluator CreateTokenEvaluator(NumberRangeToken token);
        IGlobTokenEvaluator CreateTokenEvaluator(PathSeperatorToken token);
        IGlobTokenEvaluator CreateTokenEvaluator(WildcardDirectoryToken token, CompositeTokenEvaluator nestedCompositeTokenEvaluator);
        IGlobTokenEvaluator CreateTokenEvaluator(WildcardToken token, CompositeTokenEvaluator nestedCompositeTokenEvaluator);

    }

}