using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TranslatorParser
{
    public static class TranslatorParserCombinator
    {
        private static readonly string OPENSQUAREBRACKET = "[";
        private static readonly string CLOSESQUAREBRACKET = "]";
        private static readonly string COMMA = ",";

        public static TranslatorObject Deserialize(string googleTranslatorString)
        {
            TranslatorObjectBuilder translatorObjectBuilder = new TranslatorObjectBuilder();
            var translatorObject = ParserObjectWithDoubleHighComma(translatorObjectBuilder);
            var result = translatorObject(googleTranslatorString);
            return translatorObjectBuilder.MainTranslatorObject;
        }

        public static string Serialize(TranslatorObject jsonObject)
        {
            return string.Empty;
        }

        public static Parse<string> ParserObjectWithDoubleHighComma(TranslatorObjectBuilder translatorObjectBuilder)
        {
            var stringParseValue = Parser.Literal("\"").
               And_(_ => Parser.StringValue().CreateValue(translatorObjectBuilder)).And_(__ => Parser.Literal("\""));
            var stringParseValuePropertyObject = Parser.StringValue().Or(Parser.StringValueEmpty()).CreateValue(translatorObjectBuilder).
                 Then_(_ => Parser.Literal(COMMA).Then_(__ => Parser.StringValue().Or(Parser.StringValueEmpty()).CreateValue(translatorObjectBuilder)));
            var parseValue = Parser.StringValue().CreateValue(translatorObjectBuilder);
            var stringParseValueOrParseValue = stringParseValue.Or(parseValue);
            var innerValuePropertyObject = Parser.StringValue().Or(Parser.StringValueEmpty()).CreateArrayValue(translatorObjectBuilder).
                Then_(_ => Parser.Literal(COMMA).Then_(__ => Parser.StringValue().CreateArrayValue(translatorObjectBuilder)));
            var keyStringArrayValueObject =
                Parser.Literal(OPENSQUAREBRACKET).CreateArray(translatorObjectBuilder).
                And_(_ => innerValuePropertyObject).Repeat(innerValuePropertyObject).And_(_ => Parser.Literal(CLOSESQUAREBRACKET));
            var startParserLiteral = Parser.Literal(OPENSQUAREBRACKET).CreateGoogleTranslatorObject(translatorObjectBuilder).
                Then(_ =>
                stringParseValuePropertyObject.Or(keyStringArrayValueObject).
                Repeat(Parser.Literal(COMMA).And_(__ =>
                stringParseValuePropertyObject.Or(keyStringArrayValueObject))).
                Then_(___ => Parser.Literal(CLOSESQUAREBRACKET).EndGoogleTranslatorObject(translatorObjectBuilder)));
            return startParserLiteral;
        }

        public static Parse<T> CreateGoogleTranslatorObject<T>(this Parse<T> parse, TranslatorObjectBuilder translatorObjectBuilder)
        {
            return value =>
            {
                ParseResult<T> result = parse(value);
                if (result.Succeeded)
                {
                    translatorObjectBuilder.Create(new TranslatorObject());
                }
                return result;
            };
        }

        public static Parse<T> EndGoogleTranslatorObject<T>(this Parse<T> parse, TranslatorObjectBuilder translatorObjectBuilder)
        {
            return value =>
            {
                ParseResult<T> result = parse(value);
                if (result.Succeeded)
                {
                    translatorObjectBuilder.EndObject();
                }
                return result;
            };
        }

        public static Parse<T> CreateValue<T>(this Parse<T> parse, TranslatorObjectBuilder translatorObjectBuilder)
        {
            return value =>
            {
                ParseResult<T> result = parse(value);
                if (result.Succeeded)
                {
                    double decValue;
                    var culture = new CultureInfo("en-gb");
                    bool boolValue;
                    if (double.TryParse(result.Result.ToString(), NumberStyles.Number, culture, out decValue))
                    {
                        translatorObjectBuilder.AddValue(decValue.ToString());
                    }
                    else if (bool.TryParse(result.Result.ToString(), out boolValue))
                    {
                        translatorObjectBuilder.AddValue(boolValue.ToString());
                    }
                    else
                    {
                        var str =
                            result.Result.ToString().Replace("'", "").ToString();
                        translatorObjectBuilder.AddValue(str);
                    }
                }
                return result;
            };
        }

        public static Parse<T> CreateArray<T>(this Parse<T> parse, TranslatorObjectBuilder translatorObjectBuilder)
        {
            return value =>
            {
                ParseResult<T> result = parse(value);
                if (result.Succeeded)
                {
                    translatorObjectBuilder.AddArray();
                }
                return result;
            };
        }

        public static Parse<T> CreateArrayValue<T>(this Parse<T> parse, TranslatorObjectBuilder translatorObjectBuilder)
        {
            return value =>
            {
                ParseResult<T> result = parse(value);
                if (result.Succeeded)
                {
                    double decValue;
                    var culture = new CultureInfo("de-de");
                    bool boolValue;
                    if (double.TryParse(result.Result.ToString(), NumberStyles.Number, culture, out decValue))
                    {
                        translatorObjectBuilder.AddArrayValue(decValue.ToString());
                    }
                    else if (bool.TryParse(result.Result.ToString(), out boolValue))
                    {
                        translatorObjectBuilder.AddArrayValue(boolValue.ToString());
                    }
                    else
                    {
                        translatorObjectBuilder.AddArrayValue(result.Result.ToString());
                    }
                }
                return result;
            };
        }
    }
}
