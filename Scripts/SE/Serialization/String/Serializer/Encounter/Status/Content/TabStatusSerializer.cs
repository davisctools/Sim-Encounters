using System.Data;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TabStatusSerializer : IStatusSerializer<TabStatus>
    {
        private const char END_CHAR = ' ';
        private readonly IStatusSerializer<PanelStatus> panelStatusSerializer;
        public TabStatusSerializer(IStatusSerializer<PanelStatus> panelStatusSerializer)
        {
            this.panelStatusSerializer = panelStatusSerializer;
        }

        public string Serialize(TabStatus status, bool parentRead)
        {
            var str = "";
            foreach (var panel in status.PanelStatuses) {
                var panelStr = panelStatusSerializer.Serialize(panel.Value, status.Read);
                if (!string.IsNullOrWhiteSpace(panelStr))
                    str += panel.Key + panelStr + END_CHAR;
            }

            if (str.Length == 0 && status.Read == parentRead)
                return null;

            var readChar = status.Read ? '1' : '0';
            str = readChar + str;

            return str;
        }
    }
    public class PanelStatusSerializer : IStatusSerializer<PanelStatus>
    {
        private const char END_CHAR = ' ';
        private readonly IStatusSerializer<PinGroupStatus> pinGroupStatusSerializer;
        public PanelStatusSerializer(IStatusSerializer<PinGroupStatus> pinGroupStatusSerializer)
        {
            this.pinGroupStatusSerializer = pinGroupStatusSerializer;
        }

        public string Serialize(PanelStatus status, bool parentRead)
        {
            var str = "";

            var pinStr = pinGroupStatusSerializer.Serialize(status.PinGroupStatus, status.Read);
            if (!string.IsNullOrWhiteSpace(pinStr))
                str += pinStr;

            foreach (var panel in status.ChildPanelStatuses) {
                var panelStr = Serialize(panel.Value, status.Read);
                if (!string.IsNullOrWhiteSpace(panelStr))
                    str += panel.Key + panelStr + END_CHAR;
            }

            if (string.IsNullOrWhiteSpace(str) && status.Read == parentRead)
                return null;

            var readChar = status.Read ? '1' : '0';
            str = readChar + str;

            return str;
        }
    }

    public interface IStatusSerializer<T>
    {
        string Serialize(T status, bool parentRead);
    }

    public class PinGroupStatusSerializer : IStatusSerializer<PinGroupStatus>
    {
        private const char END_CHAR = ' ';
        private readonly IStatusSerializer<DialogueStatus> dialogueStatusSerializer;
        private readonly IStatusSerializer<QuizStatus> quizStatusSerializer;
        public PinGroupStatusSerializer(
            IStatusSerializer<DialogueStatus> dialogueStatusSerializer,
            IStatusSerializer<QuizStatus> quizStatusSerializer)
        {
            this.dialogueStatusSerializer = dialogueStatusSerializer;
            this.quizStatusSerializer = quizStatusSerializer;
        }

        public string Serialize(PinGroupStatus status, bool parentRead)
        {
            var str = "";

            var dialogueStr = dialogueStatusSerializer.Serialize(status.DialogueStatus, status.Read);
            if (!string.IsNullOrWhiteSpace(dialogueStr))
                str += dialogueStr;
            str += END_CHAR;

            var quizStr = quizStatusSerializer.Serialize(status.QuizStatus, status.Read);
            if (!string.IsNullOrWhiteSpace(quizStr))
                str += quizStr;
            str += END_CHAR;

            if (string.IsNullOrWhiteSpace(str) && status.Read == parentRead)
                return null;

            var readChar = status.Read ? '1' : '0';
            str = readChar + str;

            return str;
        }
    }

    public class QuizStatusSerializer : IStatusSerializer<QuizStatus>
    {
        private const char END_CHAR = ' ';
        private readonly LazyInject<IStatusSerializer<PanelStatus>> panelStatusSerializer;
        public QuizStatusSerializer(LazyInject<IStatusSerializer<PanelStatus>> panelStatusSerializer)
        {
            this.panelStatusSerializer = panelStatusSerializer;
        }

        public string Serialize(QuizStatus status, bool parentRead)
        {
            var str = "";
            foreach (var panel in status.PanelStatuses) {
                var panelStr = panelStatusSerializer.Value.Serialize(panel.Value, status.Read);
                if (!string.IsNullOrWhiteSpace(panelStr))
                    str += panel.Key + panelStr + END_CHAR;
            }

            if (str.Length == 0 && status.Read == parentRead)
                return null;

            var readChar = status.Read ? '1' : '0';
            str = readChar + str;

            return str;
        }
    }
    public class DialogueStatusSerializer : IStatusSerializer<DialogueStatus>
    {
        private const char END_CHAR = ' ';
        private readonly LazyInject<IStatusSerializer<PanelStatus>> panelStatusSerializer;
        public DialogueStatusSerializer(LazyInject<IStatusSerializer<PanelStatus>> panelStatusSerializer)
        {
            this.panelStatusSerializer = panelStatusSerializer;
        }

        public string Serialize(DialogueStatus status, bool parentRead)
        {
            var str = "";
            foreach (var panel in status.PanelStatuses) {
                var panelStr = panelStatusSerializer.Value.Serialize(panel.Value, status.Read);
                if (!string.IsNullOrWhiteSpace(panelStr))
                    str += panel.Key + panelStr + END_CHAR;
            }

            if (str.Length == 0 && status.Read == parentRead)
                return null;

            var readChar = status.Read ? '1' : '0';
            str = readChar + str;

            return str;
        }
    }
}