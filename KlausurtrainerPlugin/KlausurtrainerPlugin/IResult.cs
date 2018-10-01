using KlausurtrainerPlugin.Enum;

namespace KlausurtrainerPlugin
{
    public interface IResult
    {
        string GetSolution();
        AnswerCellStatus Validate(string input);
    }
}