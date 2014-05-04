using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AntsBattle
{
    public class WorldLoader
    {
        readonly Dictionary<char, Func<Point, WorldObject>> factories = new Dictionary<char, Func<Point, WorldObject>>();
        private char? borderSign;

        public void Load(string file, World world)
        {
            Load(File.ReadLines(file), world);
        }

        public void Load(IEnumerable<string> lines, World world)
        {
            var cells = lines
                .SelectMany((line, row) => line.ToCharArray().Select((c, col) => new { row, col, c }))
                .Where(cell => factories.ContainsKey(cell.c)).ToList();
            if (borderSign.HasValue)
            {
                int rowsCount = cells.Max(c => c.row) + 1;
                int colsCount = cells.Max(c => c.col) + 1;
                cells = cells.Select(c => new { row = c.row + 1, col = c.col + 1, c.c })
                    .Concat(Enumerable.Range(0, rowsCount + 2).Select(row => new { row, col = 0, c = borderSign.Value }))
                    .Concat(Enumerable.Range(0, rowsCount + 2).Select(row => new { row, col = colsCount + 1, c = borderSign.Value }))
                    .Concat(Enumerable.Range(1, colsCount).Select(col => new { row = 0, col, c = borderSign.Value }))
                    .Concat(Enumerable.Range(1, colsCount).Select(col => new { row = rowsCount + 1, col, c = borderSign.Value }))
                    .ToList();
            }
            foreach (var cell in cells)
            {
                var factory = factories[cell.c];
                world.AddObject(factory(new Point(cell.col, cell.row)));
            }
            world.FreezeWorldSize();
        }

        public WorldLoader AddBorder(char ch)
        {
            borderSign = ch;
            return this;
        }

        public WorldLoader AddRule(char ch, Func<Point, WorldObject> createObject)
        {
            factories.Add(ch, createObject);
            return this;
        }

    }
}