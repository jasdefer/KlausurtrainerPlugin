using KlausurtrainerPlugin.Enum;
using System.Collections.Generic;
using System.Linq;

namespace KlausurtrainerPlugin.Helper
{
    public static class AnswerHelper
    {
        public static AnswerStatus GetStatus(this Dictionary<Cell, AnswerCellStatus> answerStatus)
        {
            var values = answerStatus.Values;
            if (values.Any(x => x == AnswerCellStatus.NotValidated))
            {
                return AnswerStatus.NotValidated;
            }
            else if (values.Any(x => x == AnswerCellStatus.SyntaxError || x == AnswerCellStatus.Missing))
            {
                return AnswerStatus.SyntaxError;
            }
            else if(values.Any(x => x == AnswerCellStatus.Incorrect))
            {
                return AnswerStatus.Incorrect;
            }
            return AnswerStatus.Correct;
        }
    }
}