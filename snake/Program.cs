using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace snake
{
    class Field
    {
        public char[,] field;
        public int dotsCount = 0;
        Field()
        {
            field = new char[20, 20];
            createField();
        }
        public Field(int x,int y)
        {
            field = new char[x, y];
            createField();
        }
        private void createField()
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int i1 = 0; i1 < field.GetLength(1); i1++)
                {
                    field[i, i1] = '0';
                }
            }
        }
        public void createDots()
        {
            Random rand = new Random();
            field[rand.Next(0,field.GetLength(0)-1), rand.Next(0, field.GetLength(1) - 1)] = '*';
        }
        public static void drawField(Field f)
        {
            for (int i = 0; i < f.field.GetLength(0); i++)
            {
                for (int i1 = 0; i1 < f.field.GetLength(1); i1++)
                {
                    Console.Write(f.field[i, i1] + " ");
                }
                Console.Write("\n");
            }
        }
    }
    class Unit
    {
        char X = 'X';
        bool upMove = true;
        bool downMove = false;
        bool rightMove = false;
        bool leftMove = false;
        public bool isAlive = true;
        int posX;
        int posY;
        List<int> posXList;
        List<int> posYList;

        int posXMem = 0;
        int posYMem = 0;
        int posXMem2 = 0;
        int posYMem2 = 0;
        int posXLast = 0;
        int posYLast = 0;
        int snakeLength = 3;

        public Unit(Field field)
        {
            posX = (field.field.GetLength(0) / 2);
            posY = (field.field.GetLength(1) / 2);
            posXMem = ((field.field.GetLength(0) / 2+1));
            posYMem = ((field.field.GetLength(1) / 2));
            posXList = new List<int>() { posX, posXMem, (field.field.GetLength(0) / 2 + 2) };
            posYList = new List<int>() { posY, posYMem, (field.field.GetLength(0) / 2) };
        }
        public void CheckKey()
        {
            if (Keyboard.IsKeyDown(Key.Up) && downMove != true && (posY + 1) != 0 && (posY - 1) != 19) 
            {
                upMove = true; downMove = false; rightMove = false; leftMove = false;
            }
            if (Keyboard.IsKeyDown(Key.Down) && upMove != true && (posY + 1) != 0 && (posY - 1) != 19)
            {
                upMove = false; downMove = true; rightMove = false; leftMove = false;
            }
            if (Keyboard.IsKeyDown(Key.Right) && leftMove != true && (posX+1) != 0 && (posX-1) != 19)
            {
                upMove = false; downMove = false; rightMove = true; leftMove = false;
            }
            if (Keyboard.IsKeyDown(Key.Left)&&rightMove!=true && (posX + 1) != 0 && (posX - 1) != 19)
            {
                upMove = false; downMove = false; rightMove = false; leftMove = true;
            }
        }
        public void addElement(Field field)
        {
            //todo: separate from this function
            //if (field.field[posXList[i], posYList[i]] == field.field[posXList[(i + 1)], posYList[(i + 1)]])
            //{
            //}
            if (posXList[(posXList.Count-1)] == posXList[(posXList.Count-2)])
            {
                if (posYList[(posYList.Count - 1)] > posYList[(posYList.Count - 2)])
                {
                    posXList.RemoveAt(posXList.Count - 1);
                    posYList.RemoveAt(posYList.Count - 1);
                    posXList.Add(posXList[(posXList.Count - 1)]);
                    posYList.Add((posYList[(posYList.Count - 1)] + 1));
                    posXList.Add(posXList[(posXList.Count - 1)]);
                    posYList.Add((posYList[(posYList.Count - 1)] + 2));
                }
                if (posYList[(posYList.Count - 1)] < posYList[(posYList.Count - 2)])
                {
                    posXList.Add(posXList[(posXList.Count - 1)]);
                    posYList.Add((posYList[(posYList.Count - 1)] - 1));
                }
            }
            if (posYList[(posYList.Count-1)] == posYList[(posYList.Count-2)])
            {
                if (posXList[(posXList.Count - 1)] > posXList[(posXList.Count - 2)])
                {
                    posYList.Add(posYList[(posYList.Count - 1)]);
                    posXList.Add((posXList[(posXList.Count - 1)] + 1));
                }
                if (posXList[(posXList.Count - 1)] < posXList[(posXList.Count - 2)])
                {
                    posYList.Add(posYList[(posYList.Count - 1)]);
                    posXList.Add((posXList[(posXList.Count - 1)] - 1));
                }
            }
        }
        public void Move(ref Field field)
        {
            CheckKey();
            posXMem = posX;
            posYMem = posY;

            if (upMove)
            {
                posXMem = posX;
                posX -= 1;
                posXList[0] = posX;
                if (posX != 0)
                {
                    if(field.field[posX, posY]=='*')
                    {
                        addElement(field);
                        snakeLength++;
                        field.dotsCount--;
                    }
                    if (field.field[posX, posY] == X)
                    {
                        isAlive = false;
                    }
                    field.field[posX, posY] = X;
                    if (posX == field.field.GetLength(0)-1)
                        field.field[0, posY] = '0';
                }
                if (posX == 0)
                {
                    field.field[posX, posY] = X;
                    posX = field.field.GetLength(0);
                }
            }
            if (downMove)
            {
                posXMem = posX;
                posX += 1;
                posXList[0] = posX;
                if (posX != field.field.GetLength(0) - 1)
                {
                    if (field.field[posX, posY] == '*')
                    {
                        addElement(field);
                        snakeLength++;
                        field.dotsCount--;
                    }
                    if (field.field[posX, posY] == X)
                    {
                        isAlive = false;
                    }
                    field.field[posX, posY] = X;
                    if (posX == 0)
                        field.field[field.field.GetLength(0) - 1, posY] = '0';
                }
                if (posX == field.field.GetLength(0) - 1)
                {
                    field.field[posX, posY] = X;
                    posX = -1;
                }
            }
            if (rightMove)
            {
                posYMem = posY;
                posY += 1;
                posYList[0] = posY;
                if (posY != field.field.GetLength(0) - 1)
                {
                    if (field.field[posX, posY] == '*')
                    {
                        addElement(field);
                        snakeLength++;
                        field.dotsCount--;
                    }
                    if (field.field[posX, posY] == X)
                    {
                        isAlive = false;
                    }
                    field.field[posX, posY] = X;
                    if (posY == 0)
                        field.field[posX, field.field.GetLength(0) - 1] = '0';
                }
                if (posY == field.field.GetLength(0) - 1)
                {
                    field.field[posX, posY] = X;
                    posY = -1;
                }
            }
            if (leftMove)
            {
                posYMem = posY;
                posY -= 1;
                posYList[0] = posY;
                if (posY != 0)
                {
                    if (field.field[posX, posY] == '*')
                    {
                        addElement(field);
                        snakeLength++;
                        field.dotsCount--;
                    }
                    if (field.field[posX, posY] == X)
                    {
                        isAlive = false;
                    }
                    field.field[posX, posY] = X;
                    if (posY == field.field.GetLength(0) - 1)
                        field.field[posX, 0] = '0';
                }
                if (posY == 0)
                {
                    field.field[posX, posY] = X;
                    posY = field.field.GetLength(1);
                }
            }
            
            if (snakeLength>1)
            {
                for (int i = 0; i < snakeLength - 1; i++)
                {
                    posXMem2 = posXList[i + 1];
                    posYMem2 = posYList[i + 1];
                    posXList[i + 1] = posXMem;
                    posYList[i + 1] = posYMem;
                    posXList[i + 1] = posXMem == 20 ? 0 : posXList[i + 1];
                    posYList[i + 1] = posYMem == 20 ? 0 : posYList[i + 1];
                    posXList[i + 1] = posXMem == -1 ? 19 : posXList[i + 1];
                    posYList[i + 1] = posYMem == -1 ? 19 : posYList[i + 1];
                    field.field[posXList[i + 1], posYList[i + 1]] = X;
                    posXMem = posXMem2;
                    posYMem = posYMem2;
                    if (i == snakeLength - 2)
                    {
                        posXLast = posXMem2;
                        posYLast = posYMem2;
                    }
                }
                field.field[posXLast, posYLast] = '0';
            }
        }
    }
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //todo: fix bug, hide outer edges, forbid dots to spawn at outer edges.
            Snake();
        }
        static void Snake()
        {
            Start();
        }
        static void Start()
        {
            Field field = new Field(20, 20);
            Unit unit = new Unit(field);
            Update(ref field,ref unit);
        }
        static void Update(ref Field field,ref Unit unit)
        {
            int i = 0;
            while(unit.isAlive)
            {
                Field.drawField(field);
                System.Threading.Thread.Sleep(300);
                Console.Clear();
                unit.Move(ref field);
                if (i % 10 == 0 && field.dotsCount < 4)
                {
                    field.dotsCount++;
                    field.createDots();
                }
                i++;
            }
            Field.drawField(field);
            Console.WriteLine("\n\nYou lose!");
        }
    }
}
