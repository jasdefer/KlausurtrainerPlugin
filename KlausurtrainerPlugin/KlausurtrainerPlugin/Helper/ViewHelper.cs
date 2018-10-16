using KlausurtrainerPlugin.Enum;

namespace KlausurtrainerPlugin.Helper
{
    /// <summary>
    /// Helps converting exercise properties to bootstrap 4 friendly strings.
    /// </summary>
    public static class ViewHelper
    {
        /// <summary>
        /// Convert an answer cell status to the bootstrap 4 equivalent for a form.
        /// </summary>
        /// <param name="status">The status of an answer cell</param>
        /// <returns>Returns a bootstrap 4 string, which sets the correct form class.</returns>
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

        /// <summary>
        /// Convert an answer cell status to the bootstrap 4 equivalent for the background.
        /// </summary>
        /// <param name="status">The status of an answer cell</param>
        /// <returns>Returns a bootstrap 4 string, which sets the correct background.</returns>
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

        /// <summary>
        /// Convert an answer cell status to the bootstrap 4 equivalent for the border.
        /// </summary>
        /// <param name="status">The status of an answer cell</param>
        /// <returns>Returns a bootstrap 4 string, which sets the correct border.</returns>
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