using KlausurtrainerPlugin.DefaultResults;
using KlausurtrainerPlugin.Enum;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace KlausurtrainerPlugin
{
    public abstract class ExerciseDefinition<T> : IExerciseDefinition
    {
        /// <summary>
        /// The name of the exercise. It must be unique in a course.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The name of the view file for this exercise. The view is a *.cshtml or *.html file
        /// </summary>
        public abstract string FileName { get; }

        /// <summary>
        /// The chapter of this exercise in the course. The chapters are ordered alphabetically.
        /// </summary>
        public virtual string Chapter => string.Empty;

        /// <summary>
        /// The order of the exercise within the chapter.
        /// </summary>
        public virtual int Order => 0;

        /// <summary>
        /// The values for this exercise, which are of the generic type.
        /// </summary>
        protected T Values { get; set; }

        /// <summary>
        /// This Random object can be used for generating random numbers. It is seeded with a constant value for each student.
        /// </summary>
        protected Random Rnd { get; set; }

        /// <summary>
        /// Get the answer cells to inform the user where what kind of answer is expected.
        /// </summary>
        public Dictionary<Cell, AnswerCell> AnswerCells { get; protected set; }

        /// <summary>
        /// Contains the collection of results with the corresponding expected solutions for each answer cell.
        /// </summary>
        protected Dictionary<Cell, IResult> Solutions { get; set; }

        /// <summary>
        /// Validate a given input and calculates the answer cell statuses for each answer cell of the defined solution.
        /// </summary>
        public Dictionary<Cell, AnswerCellStatus> GetAnswerDetails(Dictionary<Cell, string> inputs)
        {
            GenerateSolutions();
            Dictionary<Cell, AnswerCellStatus> status = new Dictionary<Cell, AnswerCellStatus>();
            foreach (var item in inputs)
            {
                if (AnswerCells[item.Key].InputType != InputTypes.Label)
                {
                    bool success = Solutions.TryGetValue(item.Key, out IResult result);
                    if (!success) throw new Exception($"No answer cell is defined for {item.Key} in {Name}.");
                    status.Add(item.Key, result.Validate(item.Value));
                }
            }
            return status;
        }

        /// <summary>
        /// A brief description of the exercise available for the course instructor.
        /// </summary>
        public virtual string[] GetExerciseDescription()
        {
            return new string[0];
        }

        public Dictionary<Cell, string> GetExpectedSolutions()
        {
            Dictionary<Cell, string> solutions = new Dictionary<Cell, string>();
            GenerateSolutions();
            foreach (var item in Solutions)
            {
                solutions.Add(item.Key, item.Value.GetSolution());
            }
            return solutions;
        }

        /// <summary>
        /// Get the exercise values as an object.
        /// </summary>
        public object GetValues()
        {
            return Values;
        }

        /// <summary>
        /// Set the exercise values externally.
        /// </summary>
        /// <param name="values"></param>
        public void SetValues(T values)
        {
            AnswerCells = new Dictionary<Cell, AnswerCell>();
            Solutions = new Dictionary<Cell, IResult>();
            Values = values;
            DefineAnswerCells();
        }

        /// <summary>
        /// Instantiates the exercise for a given seed.
        /// The random gets instantiated with the hashcode of the lower invariant of the given seed.
        /// The exercise values are calculated and the answer cells get defined.
        /// </summary>
        /// <param name="seed"></param>
        public void Instantiate(string seed)
        {
            Rnd = new Random(Hash(seed));
            AnswerCells = new Dictionary<Cell, AnswerCell>();
            Solutions = new Dictionary<Cell, IResult>();
            SetValues();
            DefineAnswerCells();
        }

        /// <summary>
        /// Converts the string seed to an integer seed.
        /// </summary>
        private int Hash(string seed)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(seed);
            var sha = new SHA256Managed();
            var hashedBytes = sha.ComputeHash(bytes);
            int hash = BitConverter.ToInt32(hashedBytes, 0);
            return hash;
        }

        /// <summary>
        /// Calcualte the exercise values.
        /// </summary>
        protected abstract void SetValues();

        /// <summary>
        /// Calculate the expected solutions.
        /// </summary>
        protected abstract void GenerateSolutions();

        /// <summary>
        /// Define what kind of answer cells are expected.
        /// </summary>
        protected abstract void DefineAnswerCells();

        /// <summary>
        /// Add a answer cell.
        /// </summary>
        /// <param name="inputType">The input type of the answer cell</param>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="options">Add an array of strings, if the input type is multiple choice. Those are the options displayed for the multiple choice answer cell.</param>
        /// <param name="label">The label of this answer cell. Default value is null: No label will be displayed</param>
        protected void DefineAnswerCell(InputTypes inputType, int row, int column, string label = null, string[] options = null)
        {
            var cell = new Cell(row, column);
            if (AnswerCells.ContainsKey(cell)) throw new Exception($"There is more than on answer cell defined for {cell}.");
            AnswerCells.Add(new Cell(row, column), new AnswerCell(inputType, options) { Label = label });
        }

        /// <summary>
        /// Define an answer cell which only accepts integers. The input type is int.
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="label">The label of this answer cell. Default value is null: No label will be displayed</param>
        protected void DefineIntAnswerCell(int row, int column, string label = null)
        {
            DefineAnswerCell(InputTypes.Int, row, column, label);
        }

        /// <summary>
        /// Define an answer cell which only accepts strings. The input type is string.
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="label">The label of this answer cell. Default value is null: No label will be displayed</param>
        protected void DefineStringAnswerCell(int row, int column, string label = null)
        {
            DefineAnswerCell(InputTypes.String, row, column, label);
        }

        /// <summary>
        /// Define an answer cell which only accepts decimals. The input type is decimal.
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="label">The label of this answer cell. Default value is null: No label will be displayed</param>
        protected void DefineDecimalAnswerCell(int row, int column, string label = null)
        {
            DefineAnswerCell(InputTypes.Decimal, row, column, label);
        }

        /// <summary>
        /// Define a checkbox as the answer cell. The input type is TrueFalse
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="label">The label of this answer cell. Default value is null: No label will be displayed</param>
        protected void DefineTrueFalseAnswerCell(int row, int column, string label = null)
        {
            DefineAnswerCell(InputTypes.TrueFalse, row, column, label);
        }

        /// <summary>
        /// Add a answer cell for a multiple choice question
        /// </summary>
        /// <param name="options">The set of options for the multiple choice question.</param>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="label">The label of this answer cell. Default value is null: No label will be displayed</param>
        protected void DefineMultipleChoiceAnswer(string[] options, int row, int column, string label = null)
        {
            if (options?.Length < 1)
            {
                throw new ArgumentException($"Define a set of options for the multiple choice cell {row}/{column} on exercise {Name}");
            }
            DefineAnswerCell(InputTypes.MultipleChoice, row, column, label, options);
        }

        /// <summary>
        /// Add a label to the answer cells. The user cannot enter any answers. It's only a label.
        /// </summary>
        protected void DefineLabel(string label, int row, int column)
        {
            AnswerCells.Add(new Cell(row, column), new AnswerCell(InputTypes.Label) { Label = label });
        }

        /// <summary>
        /// Add a solution to any answer cell
        /// </summary>
        /// <param name="result">The result that validates the input for this answer cell</param>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        protected void AddSolution(IResult result, int row, int column)
        {
            AddSolution(result, new Cell(row, column));
        }

        /// <summary>
        /// Add a solution to any answer cell
        /// </summary>
        /// <param name="result">The result that validates the input for this answer cell</param>
        /// <param name="cell">The cell to which this solution should be added</param>
        protected void AddSolution(IResult result, Cell cell)
        {
            Solutions[cell] = result;
        }

        /// <summary>
        /// Add an integer solution to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        protected void AddIntegerSolution(int row, int column, int expectedValue)
        {
            IResult result = new IntResult(expectedValue);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add a solution which can either be an integer or null to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        protected void AddIntegerOrNullSolution(int row, int column, int? expectedValue)
        {
            IResult result = new IntOrNullResult(expectedValue);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add a decimal solution to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="epsilon">The tolerance range for the input. See the decimal result for a detailed description</param>
        protected void AddDecimalSolution(int row, int column, decimal expectedValue, decimal epsilon = 0)
        {
            IResult result = new DecimalResult(expectedValue, epsilon);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add a decimal solution to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="epsilon">The tolerance range for the input. See the decimal result for a detailed description</param>
        protected void AddDoubleSolution(int row, int column, double expectedValue, decimal epsilon = 0)
        {
            AddDecimalSolution(row, column, Convert.ToDecimal(expectedValue), epsilon);
        }

        /// <summary>
        /// Add a decimal solution to the answer cell. Round the expected value to a given amount of digits. The epsilon is calculated accordingly.
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="digits">The amount of digits which are expected behind the comma. The epsilon for a 3 digits is 0.0005.</param>
        protected void AddRoundedDoubleSolution(int row, int column, double expectedValue, int digits = 3)
        {
            AddRoundedDecimalSolution(row, column, Convert.ToDecimal(expectedValue), digits);
        }

        /// <summary>
        /// Add a decimal solution to the answer cell. Round the expected value to a given amount of digits. The epsilon is calculated accordingly.
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="digits">The amount of digits which are expected behind the comma. The epsilon for a 3 digits is 0.0005.</param>
        protected void AddRoundedDecimalSolution(int row, int column, decimal expectedValue, int digits = 3)
        {
            expectedValue = Math.Round(expectedValue, digits, MidpointRounding.AwayFromZero);
            decimal epsilon = Convert.ToDecimal(Math.Pow(10, -digits) * 0.5);
            AddDecimalSolution(row, column, expectedValue, epsilon);
        }

        /// <summary>
        /// Add a decimal solution to the answer cell. It also accepts empty answer cells.
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="epsilon">The tolerance range for the input. See the decimal result for a detailed description</param>
        protected void AddDecimalOrNullSolution(int row, int column, decimal? expectedValue, decimal epsilon = 0)
        {
            IResult result = new DecimalOrNullResult(expectedValue, epsilon);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add a decimal solution to the answer cell. It also accepts empty answer cells.
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="epsilon">The tolerance range for the input. See the decimal result for a detailed description</param>
        protected void AddDoubleOrNullSolution(int row, int column, double? expectedValue, decimal epsilon = 0)
        {
            decimal? converted;
            if (expectedValue.HasValue)
            {
                converted = Convert.ToDecimal(expectedValue);
            }
            else converted = null;

            AddDecimalOrNullSolution(row, column, converted, epsilon);
        }

        /// <summary>
        /// Add an integer solution to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="compareMode">Defines how the expected value and input are compared (case sensitivity)</param>
        protected void AddStringSolution(int row, int column, string expectedValue, StringCompareModes compareMode = StringCompareModes.Exact)
        {
            IResult result = new StringResult(expectedValue, compareMode);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add an integer solution to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="compareMode">Defines how the expected value and input are compared (case sensitivity)</param>
        protected void AddStringOrNullSolution(int row, int column, string expectedValue, StringCompareModes compareMode = StringCompareModes.Exact)
        {
            IResult result = new StringOrNullResult(expectedValue, compareMode);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add a true or false solution to the answer cell
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        protected void AddTrueOrFalseSolution(int row, int column, bool expectedValue)
        {
            IResult result = new TrueOrFalseResult(expectedValue);
            AddSolution(result, row, column);
        }

        /// <summary>
        /// Add a fraction result. If the answer cell is defined as a fraction the solution must be a fraction as well.
        /// </summary>
        /// <param name="row">The row of the answer cell</param>
        /// <param name="column">The column of the answer cell</param>
        /// <param name="expectedValue">The expected value of the answer cell</param>
        /// <param name="denominator">For displaying the correct solution. Has no effect on the validation</param>
        /// <param name="numerator">For displaying the correct solution. Has no effect on the validation</param>
        /// <param name="epsilon">The input is correct as long as the difference between the input and expected value is not greater than this epsilon</param>
        protected void AddFractionSolution(int row, int column, decimal expectedValue, decimal epsilon = 0.01m, int? numerator = null, int? denominator = null)
        {
            IResult result = new FractionResult(expectedValue, epsilon, numerator, denominator);
            AddSolution(result, row, column);
        }
    }
}