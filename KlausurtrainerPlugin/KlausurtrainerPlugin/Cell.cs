using System;

namespace KlausurtrainerPlugin
{
    public class Cell
    {
        public int Column { get; }
        public int Row { get; }

        public Cell(int row, int column)
        {
            if (column < 0)
            {
                throw new ArgumentException("Column cannot be negative");
            }
            if (row < 0)
            {
                throw new ArgumentException("Row cannot be negative");
            }

            Column = column;
            Row = row;
        }

        public override string ToString()
        {
            return $"{Row};{Column}";
        }

        public override bool Equals(object obj)
        {
            Cell cell = obj as Cell;
            if (ReferenceEquals(null, cell))
            {
                return false;
            }
            if (cell.Column == Column && cell.Row == Row)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (397 * Column.GetHashCode()) ^ (Row.GetHashCode() + 1);
        }

        public static bool operator ==(Cell c1, Cell c2)
        {
            if (ReferenceEquals(c1, c2))
            {
                return true;
            }

            if (ReferenceEquals(c1, null))
            {
                return false;
            }
            if (ReferenceEquals(c2, null))
            {
                return false;
            }

            return (c1.Equals(c2));
        }

        public static bool operator !=(Cell c1, Cell c2)
        {
            return !(c1 == c2);
        }
    }
}