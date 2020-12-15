using System.Text.RegularExpressions;

namespace ClinicalTools.UI
{
    public class ListTagsReplacer
    {
        protected static Regex UnorderedListRegex = new Regex(@"\n?<ul>(.*?)</ul>\n?", RegexOptions.IgnoreCase ^ RegexOptions.Compiled ^ RegexOptions.Singleline);
        protected static Regex OrderedListRegex = new Regex(@"\n?<ol>(.*(<ol>.*</ol>.*?)*)</ol>\n?", RegexOptions.IgnoreCase ^ RegexOptions.Compiled ^ RegexOptions.Singleline);
        protected static Regex ListElementRegex = new Regex(@"\s*<li>(.*?)</li>\s*", RegexOptions.IgnoreCase ^ RegexOptions.Compiled ^ RegexOptions.Singleline);
        public string ReplaceLists(string input)
        {
            var unorderedListEvaluator = new MatchEvaluator(EvaluateUnorderedListMatch);
            var str = UnorderedListRegex.Replace(input, unorderedListEvaluator);
            var orderedListEvaluator = new MatchEvaluator(EvaluateOrderedListMatch);
            str = OrderedListRegex.Replace(str, orderedListEvaluator);
            return str;
        }

        protected virtual string EvaluateUnorderedListMatch(Match match)
        {
            var listElementEvaluator = new MatchEvaluator(EvaluateUnorderedListElementMatch);
            var listElementsText = ListElementRegex.Replace(match.Groups[1].Value, listElementEvaluator).Trim();
            return $"\n<margin-left=1em>{listElementsText}</margin>\n";
        }

        protected char BulletChar = '\u2022';
        protected virtual string EvaluateUnorderedListElementMatch(Match match)
            => $"\n{BulletChar}<indent=1em>{match.Groups[1].Value}</indent>";

        protected virtual string EvaluateOrderedListMatch(Match match)
        {
            var orderedListElementMatchEvaluator = new OrderedListElementMatchEvaluator();
            var listElementEvaluator = new MatchEvaluator(orderedListElementMatchEvaluator.EvaluateUnorderedListElementMatch);
            var listElementsText = ListElementRegex.Replace(match.Groups[1].Value, listElementEvaluator).Trim();
            return $"\n<margin-left=1em>{listElementsText}</margin>\n";
        }

        public class OrderedListElementMatchEvaluator
        {
            protected int Count = 1;
            public virtual string EvaluateUnorderedListElementMatch(Match match)
                => $"\n{Count++}.<indent=1em>{match.Groups[1].Value}</indent>";
        }
    }
}