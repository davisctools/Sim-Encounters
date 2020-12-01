using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TabStatusDeserializer : ICharEnumeratorDeserializer<TabStatus>
    {
        private const char END_CHAR = ' ';

        private readonly ICharEnumeratorDeserializer<PanelStatus> panelStatusParser;
        private readonly ICharEnumeratorDeserializer<string> keyParser;
        public TabStatusDeserializer(
            ICharEnumeratorDeserializer<PanelStatus> panelStatusParser,
            ICharEnumeratorDeserializer<string> keyParser)
        {
            this.panelStatusParser = panelStatusParser;
            this.keyParser = keyParser;
        }
        public TabStatus Deserialize(CharEnumerator enumerator)
        {
            var status = new TabStatus();

            if (enumerator.IsDone)
                return status;

            status.Read = enumerator.Current == '1';

            while (enumerator.MoveNext() && enumerator.Current != END_CHAR) {
                var key = keyParser.Deserialize(enumerator);
                var panelStatus = panelStatusParser.Deserialize(enumerator);
                status.AddPanelStatus(key, panelStatus);
            }

            return status;
        }
    }
    public class PanelStatusDeserializer : ICharEnumeratorDeserializer<PanelStatus>
    {
        private const char END_CHAR = ' ';

        private readonly ICharEnumeratorDeserializer<PinGroupStatus> pinGroupStatusParser;
        private readonly ICharEnumeratorDeserializer<string> keyParser;
        public PanelStatusDeserializer(
            ICharEnumeratorDeserializer<PinGroupStatus> pinGroupStatusParser,
            ICharEnumeratorDeserializer<string> keyParser)
        {
            this.pinGroupStatusParser = pinGroupStatusParser;
            this.keyParser = keyParser;
        }

        public PanelStatus Deserialize(CharEnumerator enumerator)
        {
            var status = new PanelStatus();
            if (enumerator.IsDone)
                return status;

            status.Read = enumerator.Current == '1';

            if (enumerator.MoveNext() && enumerator.Current != END_CHAR)
                status.PinGroupStatus = pinGroupStatusParser.Deserialize(enumerator);

            while (enumerator.MoveNext() && enumerator.Current != END_CHAR) {
                var key = keyParser.Deserialize(enumerator);
                var panelStatus = Deserialize(enumerator);
                status.AddPanelStatus(key, panelStatus);
            }

            return status;
        }
    }
    public class PinGroupStatusDeserializer : ICharEnumeratorDeserializer<PinGroupStatus>
    {
        private const char END_CHAR = ' ';

        private readonly ICharEnumeratorDeserializer<DialogueStatus> dialogueStatusParser;
        private readonly ICharEnumeratorDeserializer<QuizStatus> quizStatusParser;
        public PinGroupStatusDeserializer(
            ICharEnumeratorDeserializer<DialogueStatus> dialogueStatusParser,
            ICharEnumeratorDeserializer<QuizStatus> quizStatusParser)
        {
            this.dialogueStatusParser = dialogueStatusParser;
            this.quizStatusParser = quizStatusParser;
        }

        public PinGroupStatus Deserialize(CharEnumerator enumerator)
        {
            var status = new PinGroupStatus();
            if (enumerator.IsDone)
                return status;

            status.Read = enumerator.Current == '1';

            if (enumerator.MoveNext() && enumerator.Current != END_CHAR)
                status.DialogueStatus = dialogueStatusParser.Deserialize(enumerator);

            if (enumerator.MoveNext() && enumerator.Current != END_CHAR)
                status.QuizStatus = quizStatusParser.Deserialize(enumerator);

            return status;
        }
    }
    public class DialogueStatusDeserializer : ICharEnumeratorDeserializer<DialogueStatus>
    {
        private const char END_CHAR = ' ';

        private readonly LazyInject<ICharEnumeratorDeserializer<PanelStatus>> panelStatusParser;
        private readonly ICharEnumeratorDeserializer<string> keyParser;
        public DialogueStatusDeserializer(
            LazyInject<ICharEnumeratorDeserializer<PanelStatus>> panelStatusParser,
            ICharEnumeratorDeserializer<string> keyParser)
        {
            this.panelStatusParser = panelStatusParser;
            this.keyParser = keyParser;
        }

        public DialogueStatus Deserialize(CharEnumerator enumerator)
        {
            var status = new DialogueStatus();

            if (enumerator.IsDone)
                return status;

            status.Read = enumerator.Current == '1';

            while (enumerator.MoveNext() && enumerator.Current != END_CHAR) {
                var sectionKey = keyParser.Deserialize(enumerator);
                var panelStatus = panelStatusParser.Value.Deserialize(enumerator);
                status.AddPanelStatus(sectionKey, panelStatus);
            }

            return status;
        }
    }
    public class QuizStatusDeserializer : ICharEnumeratorDeserializer<QuizStatus>
    {
        private const char END_CHAR = ' ';

        private readonly LazyInject<ICharEnumeratorDeserializer<PanelStatus>> panelStatusParser;
        private readonly ICharEnumeratorDeserializer<string> keyParser;
        public QuizStatusDeserializer(
            LazyInject<ICharEnumeratorDeserializer<PanelStatus>> panelStatusParser,
            ICharEnumeratorDeserializer<string> keyParser)
        {
            this.panelStatusParser = panelStatusParser;
            this.keyParser = keyParser;
        }

        public QuizStatus Deserialize(CharEnumerator enumerator)
        {
            var status = new QuizStatus();

            if (enumerator.IsDone)
                return status;

            status.Read = enumerator.Current == '1';

            while (enumerator.MoveNext() && enumerator.Current != END_CHAR) {
                var sectionKey = keyParser.Deserialize(enumerator);
                var panelStatus = panelStatusParser.Value.Deserialize(enumerator);
                status.AddPanelStatus(sectionKey, panelStatus);
            }

            return status;
        }
    }
}