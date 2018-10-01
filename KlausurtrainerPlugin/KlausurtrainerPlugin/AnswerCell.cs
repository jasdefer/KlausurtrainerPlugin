using KlausurtrainerPlugin.Enum;
using System;

namespace KlausurtrainerPlugin
{
    public class AnswerCell
    {
        public InputTypes InputType { get; }
        public string[] Options { get; }
        public string Label { get; set; }

        public AnswerCell(InputTypes inputType, string[] options = null)
        {
            if (inputType == InputTypes.MultipleChoice && options == null)
            {
                throw new ArgumentException("Define an option list for multiple choice questions");
            }

            Options = options;
            InputType = inputType;
        }
    }
}