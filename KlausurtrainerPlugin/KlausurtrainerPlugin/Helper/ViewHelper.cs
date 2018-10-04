using KlausurtrainerPlugin.Enum;

namespace KlausurtrainerPlugin.Helper
{
    public static class ViewHelper
    {
        public static string ConvertStatus(AnswerCellStatus? status)
        {
            switch (status)
            {
                case AnswerCellStatus.Correct:
                    return "is-valid";
                case AnswerCellStatus.Incorrect:
                    return "is-invalid";
                case AnswerCellStatus.Missing:
                    return "is-invalid";
                case AnswerCellStatus.SyntaxError:
                    return "is-invalid";
                default:
                    return "";
            }
        }

        public static string ConvertStatusToBackground(AnswerCellStatus? status)
        {
            switch (status)
            {
                case AnswerCellStatus.Correct:
                    return "bg-success";
                case AnswerCellStatus.Incorrect:
                    return "bg-danger";
                case AnswerCellStatus.Missing:
                    return "bg-warning";
                case AnswerCellStatus.SyntaxError:
                    return "bg-warning";
                default:
                    return "";
            }
        }

        public static string ConvertStatusToBorder(AnswerCellStatus? status)
        {
            switch (status)
            {
                case AnswerCellStatus.Correct:
                    return "border-success";
                case AnswerCellStatus.Incorrect:
                    return "border-danger";
                case AnswerCellStatus.Missing:
                    return "border-warning";
                case AnswerCellStatus.SyntaxError:
                    return "border-warning";
                default:
                    return "";
            }
        }
    }
}