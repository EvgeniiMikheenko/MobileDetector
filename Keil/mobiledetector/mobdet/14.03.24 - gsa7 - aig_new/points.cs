//#define joinEqual // при упрощении удалять одинаковые точки

using System;
using System.Collections.Generic;
using System.Text;
using ZedGraph;

namespace LGAR
{
    /// <summary>
    /// Класс коллекции точек с поддержкой различных уровней детализации
    /// </summary>
    class LODPPL : IPointList, IPointListEdit
    {
        #region Внутренние переменные   -   -   -   -   -   -   -   - Fields
        // верхний уровень, без упрощений
        List < PointPair > highList = new List<PointPair>();

        // текущий уровень детализации
        List<PointPair> currentList = null;

        // коллекция уровней детализации
        List<LODPPL_level> levels = new List<LODPPL_level>();

        // сравнение точек
        PointPairXComparer ppxcmp = new PointPairXComparer();

        // порог создания нового уровня детализации
        int bufferLimit = 50;

        // шаг детализации
        double stepDefault = 10 / XDate.SecondsPerDay; // секунды

        // множитель детализации следующего шага
        double stepLevelScale = 2;

        // текущий уровень детализации
        int selectedLevel = -1; // без упрощения

        // текущие границы в уровне
        int idxMin = 0;
        int idxMax = -1;

        #endregion

        #region Класс списка точек с усреднением    -   -   -   -   - Sub-Class
        /// <summary>
        /// Коллекция точек уровня детализации
        /// </summary>
        class LODPPL_level : List<PointPair>
        {
            double step; // шаг детализации
            double start = double.NaN; // начало последнего шага
            PointPair end = new PointPair();
            PointPair previous = new PointPair(); // последняя добавленная

            internal LODPPL_level(double step)
            {
                this.step = step;
            }

            public double Step
            { get { return step; } }

            /// <summary>
            /// Добавить точку с усреднением
            /// </summary>
            public new void Add(PointPair p)
            {
                if (base.Count == 0) // первая точка
                {
                    start = p.X;
                    base.Add(p);
                    end.X = p.X;
                    end.Y = p.Y;
                }
                else 
                {
                    if (!p.IsInvalid)
                    {
                        double a = end.X - start;

                        if ((p.X < start + step) && p.X > start) // в пределах шага
                        {
                            //# шаг: [start |end  |p  ]start+step
                            //#      <  a  ><  b  >
                            double b = p.X - end.X;
                            end.Y = (a * end.Y + b * p.Y) / (a + b); // среднее
                        }
#if joinEqual
                        else if (p.Y == previous.Y 
                            && Math.Abs(p.Y - end.Y) < 0.001
                            //&& p.Y == end.Y
                            && p.X > start)
                        {
                            start = p.X - step;
                        }
#endif
                        else // новый шаг
                        {
                            //# [start |end  ]start+step  |p
                            //# <  a  ><  b  >
                            double b = step - a;

                            previous = new PointPair
                                (start + step / 2, // середина шага
                                //(a * end.Y + b * p.Y) / step); // *** FIX!
                                end.Y);
                            base.Add(previous);

                            start += step; // (будет накапливаться ошибка)
                            if (p.X > start + step || p.X < start)
                                start = p.X; // неравномерный шаг

                            end.Y = p.Y;
                        }
                    }
                    else
                    { // пропущенные данные приоритетней
                        if (!double.IsNaN(end.Y))
                        {
                            previous = new PointPair
                                (start + step / 2, end.Y);
                            base.Add(previous);
                            start = p.X;
                            end.Y = double.NaN; // PointPair.Missing;
                        }
                    }
                    end.X = p.X; // расширить последний шаг
                }
            }

            /// <summary>
            /// Доступ к точке по индексу, последняя точка виртуальная
            /// </summary>
            public new PointPair this[int idx]
            {
                get
                {
                    if (idx == base.Count)
                        return end.Clone();
                    return base[idx];
                }
            }

            /// <summary>
            /// Количество точек в коллекции, включая последнюю виртуальную
            /// </summary>
            public new int Count
            {
                get { return !end.IsInvalid ? base.Count + 1 : base.Count; }
            }

        }


        #endregion

        #region Реализация интерфейса сравнения точек   -   -   -   - Comparer interface
        /// <summary>
        /// Сравнение точек по координате X
        /// </summary>
        class PointPairXComparer : IComparer<PointPair>
        {
            public int Compare(PointPair a, PointPair b)
            {
                return a.X.CompareTo(b.X);
            }
        }
        #endregion

        #region Конструкторы    -   -   -   -   -   -   -   -   -   - Constructors
        internal LODPPL()
        {
            currentList = highList;
        }
        internal LODPPL(double step)
        {
            this.stepDefault = step;
            currentList = highList;
        }
        internal LODPPL(double step, double scale)
        {
            this.stepDefault = step;
            this.stepLevelScale = scale;
            currentList = highList;
        }
        #endregion

        #region Свойства    -   -   -   -   -   -   -   -   -   -   - Properties
        /// <summary>
        /// Доступ к точкам по индексу с учетом текущего уровня детализации
        /// </summary>
        public PointPair this[int idx]
        {
            get 
            {
                /*
                LODPPL_level a = currentList as LODPPL_level;
                if (a != null)
                {
                    PointPair pa = a[idx + idxMin];
                    PointPair pb = currentList[idx + idxMin];
                    //return a[idx + idxMin];
                }
                return currentList[idx + idxMin]; 
                */
                if (selectedLevel >= 0)
                    return levels[selectedLevel][idx + idxMin];
                return highList[idx + idxMin];
            }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Количество точек в верхнем уровне детализации
        /// </summary>
        public int NPts
        {
            get { return highList.Count; }
        }

        /// <summary>
        /// Количество точек в текущем уровне детализации
        /// </summary>
        public int Count
        {
            get
            {
                if(idxMax > 0) 
                    return idxMax - idxMin + 1;
                return selectedLevel >= 0 ? levels[selectedLevel].Count 
                    : highList.Count;
            }
        }

        /// <summary>
        /// Количество уровней детализации
        /// </summary>
        public int Levels
        {
            get { return levels.Count; }
        }

        /// <summary>
        /// Нижний уровень детализации или null
        /// </summary>
        private LODPPL_level LowestLevel
        {
            get { return levels.Count > 0 ? levels[levels.Count - 1] : null; }
        }

        /// <summary>
        /// Текущий уровень детализации, -1 - без упрощения
        /// </summary>
        public int SelectedLevel
        {
            get { return selectedLevel; }
            set
            {
                if (value < -1 || value >= levels.Count)
                    throw new ArgumentOutOfRangeException();
                selectedLevel = value;
                idxMin = 0;
                idxMax = -1;
                currentList = selectedLevel >= 0 ? levels[selectedLevel] : highList;
            }
        }

        #endregion

        #region Методы  -   -   -   -   -   -   -   -   -   -   -   - Methods
        /// <summary>
        /// Добавить точку
        /// </summary>
        public void Add(double x, double y)
        {
            lock (highList)
            {
                // добавить во все уровни детализаций
                PointPair npx = new PointPair(x, y);
                highList.Add(npx);
                for (int lv = 0; lv < levels.Count; lv++)
                    levels[lv].Add(npx);

                // нижний уровень детализации
                List<PointPair> L = Levels > 0 ? LowestLevel : highList;
                if (L.Count > bufferLimit)
                {
                    // добавить новый нижний уровень детализации
                    levels.Add(new LODPPL_level
                        (Levels == 0 ? stepDefault
                        : LowestLevel.Step * stepLevelScale));
                    // заполнить точками
                    LODPPL_level lower = LowestLevel;
                    for (int itt = 0; itt < L.Count; itt++)
                        lower.Add(L[itt]);
                }
            }
        }
        public void Add(PointPair p)
        {
            Add(p.X, p.Y);
        }

        /// <summary>
        /// Удалить все точки и уровни детализации
        /// </summary>
        public void Clear()
        {
            lock (highList)
            {
                currentList = highList;
                highList.Clear();
                levels.Clear();
            }
        }

        public void RemoveAt(int i)
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Выбрать уровень детализации
        /// </summary>
        public bool SetBounds(double min, double max, int maxPts)
        {
            // [УПРОЩЕНИЕ] выбрать по шагу между точками
            int lv;
            double desiredStep = -1;
            bool flag = false;
            lock (highList)
            {
                if (maxPts > 0)
                {
                    desiredStep = (max - min) / maxPts;
                    for (lv = 0; lv < levels.Count; lv++)
                        if (levels[lv].Step > desiredStep)
                            break;
                    lv--; // предыдущий уровень детализации
                }
                else lv = -1; // отключить

                // уровень детализации
                if (selectedLevel != lv)
                {
                    SelectedLevel = lv;
                    //Console.WriteLine("LEVEL " + lv + ", step=" + (desiredStep *XDate.SecondsPerDay));
                    flag = true;
                }
                // обрезка по границам
                if (currentList.Count > maxPts * 3) // точек много
                {
                    // найти индексы в коллекции текущего уровня детализации
                    int first = currentList.BinarySearch
                        (new PointPair(min, double.NaN), ppxcmp);
                    int last = currentList.BinarySearch
                        (new PointPair(max, double.NaN), ppxcmp);

                    if (first < 0)
                    {
                        if (first == -1)
                            first = 0;
                        else
                            first = ~(first + 1);
                    }

                    if (last < 0)
                        last = ~last;

                    idxMin = first > 0 ? first : 0;
                    idxMax = last < currentList.Count - 1 ? last
                        : currentList.Count - 1;
                    /*// добавить до предела
                    if(idxMax - idxMin < maxPts)
                        idxMax = Math.Min(idxMin + maxPts, currentList.Count - 1); //*/

                    if (selectedLevel >= 0)
                        idxMax++; // hack: виртуальная точка
                        //first = levels[selectedLevel].Count;
                }
                else
                {
                    idxMin = 0;
                    idxMax = -1; // без обрезки
                }
            }
            return flag;
        }

        /// <summary>
        /// Приближение.
        /// </summary>
        public double Interpolate(double x)
        {
            lock (highList)
            {
                if (currentList.Count == 0)
                    return double.NaN;
                int first = currentList.BinarySearch
                    (new PointPair(x, double.NaN), ppxcmp);
                if (first < 0)
                {
                    if (first == -1)
                        first = 0;
                    else
                        first = ~(first + 1);
                }
                // за границей
                if (first >= currentList.Count)
                        first = currentList.Count - 1;
                // больше заданного
                if (currentList[first].X > x) first--;

                return first >= 0 ? currentList[first].Y : double.NaN;
            }
        }

        #endregion
    }


}
