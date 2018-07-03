using System;
using System.Collections.Generic;
using ZedGraph;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace LGAR
{
    public static class Trend
    {

        // Границы значений для шкал и масштабирования.
        public static double X0 = 0;
        public static double X1 = 15;
        public static double Y0 = -3.3;
        public static double Y1 = 3.3;
        public static ushort RawY1 = ushort.MaxValue; // (ushort)short.MaxValue;

        public static bool Blocked = false; // перерисовка заблокирована
        private static int blockMask = 0; // маска блокировки по кнопкам мышеуказателя
        public static bool LOD = true; // разрешить уровни детализации
        public static double simplifyScale = 3; // коэффициент упрощения при детализации
        public static bool AutoScale = true; // Флаг автоматического масштабирования.

        private const float defaultMainLineWidth = 2; // толщина линии графика

        private static TextObj textCoord = null;
        public static TextObj textPos = null;
        public static TextObj textNeg = null;

        /// <summary>
        /// Режим работы элемента графиков.
        /// </summary>
        public enum enumTrendMode
        {
            Normal
        };

        /// <summary>
        /// Первичная настройка компонента вывода графиков.
        /// </summary>
        /// <param name="zc"></param>
        public static void SetupGraph(ZedGraphControl zc)
        {
            var mas = zc.MasterPane;
            mas.Legend.IsVisible = false;
            mas.IsCommonScaleFactor = false;
            mas.Border.IsVisible = false;

            var gp = zc.GraphPane;
            var sx = gp.XAxis.Scale;
            var sy = gp.YAxis.Scale;
            gp.Border.IsVisible = false;

            // axis
            gp.IsFontsScaled = false;
            gp.XAxis.Title.Text = "Время, с";
            gp.YAxis.Title.Text = "Напряжение, В";
            gp.Legend.IsVisible = false;
            gp.Title.IsVisible = false;
            //gp.XAxis.Type = AxisType.Date;

            // grid
            /*gp.XAxis.MinorGrid.DashOff = gp.XAxis.MajorGrid.DashOff =
                gp.YAxis.MinorGrid.DashOff = gp.YAxis.MajorGrid.DashOff = 0;*/
            gp.XAxis.MinorGrid.DashOn = gp.XAxis.MajorGrid.DashOn =
                gp.YAxis.MinorGrid.DashOn = gp.YAxis.MajorGrid.DashOn = 0;
            gp.XAxis.MinorGrid.Color = gp.YAxis.MinorGrid.Color = Color.FromArgb(240,240,240);
            gp.XAxis.MajorGrid.Color = gp.YAxis.MajorGrid.Color = Color.FromArgb(220,220,220);
            gp.XAxis.MajorGrid.IsVisible = gp.YAxis.MajorGrid.IsVisible =
                gp.XAxis.MinorGrid.IsVisible = gp.YAxis.MinorGrid.IsVisible = true;

            // scale
            sx.Min = X0;
            //sx.Max = X1;
            //sy.MinAuto = sy.MaxAuto = true;
            sy.Min = Y0;
            sy.Max = Y1;
            sx.MaxAuto = true;

            // curves
            gp.CurveList.Clear();
            LineItem curve;
            // main curve
            curve = new LineItem("V+", new LODPPL(), Color.Black, SymbolType.None, defaultMainLineWidth);
            gp.CurveList.Add(curve);
            curve = new LineItem("V-", new LODPPL(), Color.Red, SymbolType.None, defaultMainLineWidth);
            gp.CurveList.Add(curve);

            curve = new LineItem("S+", new LODPPL(), Color.Blue, SymbolType.None, defaultMainLineWidth);
            gp.CurveList.Add(curve);
            curve = new LineItem("S-", new LODPPL(), Color.Red, SymbolType.None, defaultMainLineWidth);
            gp.CurveList.Add(curve);


            curve = new LineItem("AA", new LODPPL(), Color.Blue, SymbolType.None, defaultMainLineWidth);
            gp.CurveList.Add(curve);


            curve = new LineItem("BB", new LODPPL(), Color.Blue, SymbolType.None, defaultMainLineWidth);
            gp.CurveList.Add(curve);

            // snapshot
            curve = new LineItem("SNAPSHOT", new PointPairList(), Color.Green, SymbolType.None);
           // gp.CurveList.Add(curve);
            // hidden autoscale limit hint points
            curve = new LineItem("5min", new PointPairList(), Color.Transparent, SymbolType.None);
            curve.AddPoint(0, double.NaN);
            curve.AddPoint(4*60, double.NaN); // FIXME: manual tuned for DUMB USER
            gp.CurveList.Add(curve);

            // objects
            LineObj hline;
            hline = new LineObj(Color.Blue, 0, 0, 1, 0) { Tag = "P+", IsVisible = true };
            hline.ZOrder = ZOrder.E_BehindCurves;
            hline.Location.CoordinateFrame = CoordType.XChartFractionYScale;
            gp.GraphObjList.Add(hline);
            hline = new LineObj(Color.Blue, 0, 0, 1, 0) { Tag = "P-", IsVisible = true };
            hline.ZOrder = ZOrder.E_BehindCurves;
            hline.Location.CoordinateFrame = CoordType.XChartFractionYScale;
            gp.GraphObjList.Add(hline);
            hline = new LineObj(Color.Red, 0, 0, 1, 0) { Tag = "D+", IsVisible = true };
            hline.ZOrder = ZOrder.E_BehindCurves;
            hline.Location.CoordinateFrame = CoordType.XChartFractionYScale;
            gp.GraphObjList.Add(hline);
            hline = new LineObj(Color.Red, 0, 0, 1, 0) { Tag = "D-", IsVisible = true };
            hline.ZOrder = ZOrder.E_BehindCurves;
            hline.Location.CoordinateFrame = CoordType.XChartFractionYScale;
            gp.GraphObjList.Add(hline);
            TextObj to;
            to = new TextObj("00:00:00  000.000", 0.98, 0.99, CoordType.PaneFraction, AlignH.Right, AlignV.Bottom) 
            { Tag = "COORD", IsVisible = true };
            to.FontSpec.Size = 16;
            gp.GraphObjList.Add(textCoord = to);
            to = new TextObj("+0.000", 1, 0, CoordType.ChartFraction, AlignH.Right, AlignV.Top) { Tag = "V+", IsVisible = true };
            to.FontSpec.Size = 16;
            gp.GraphObjList.Add(textPos = to);
            to = new TextObj("-0.000", 1, 1, CoordType.ChartFraction, AlignH.Right, AlignV.Bottom) { Tag = "V-", IsVisible = true };
            to.FontSpec.Size = 16;
            gp.GraphObjList.Add(textNeg = to);

            // events
            zc.MouseDownEvent += new ZedGraphControl.ZedMouseEventHandler(zc_MouseDownEvent);
            zc.MouseMoveEvent += new ZedGraphControl.ZedMouseEventHandler(zc_MouseMoveEvent);
            zc.MouseUpEvent += new ZedGraphControl.ZedMouseEventHandler(zc_MouseUpEvent);
            zc.MouseWheel += new System.Windows.Forms.MouseEventHandler(zc_MouseWheel);
            zc.MouseEnter += new EventHandler(zc_MouseEnter);
            zc.ZoomEvent += new ZedGraphControl.ZoomEventHandler(zc_ZoomEvent);
            zc.Paint += new PaintEventHandler(zc_Paint);
            zc.GraphPane.XAxis.ScaleFormatEvent += new Axis.ScaleFormatHandler(XAxis_ScaleFormatEvent);

            zc.AxisChange();
        }

        static string XAxis_ScaleFormatEvent(GraphPane pane, Axis axis, double val, int index)
        {
            if (double.IsNaN(val)) return "-";
            return new TimeSpan((long)(val * 1e7)).ToString();
        }

        /// <summary>
        /// МЫШЬ КОЛЕСО.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void zc_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.None && e.Delta != 0)
            {
                bool up = e.Delta > 0;

            }
        }

        /// <summary>
        /// МЫШЬ ВВЕРХ СОБЫТИЕ.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        static bool zc_MouseUpEvent(ZedGraphControl sender, System.Windows.Forms.MouseEventArgs e)
        {
            Blocked = false; // blockMask != 0;

            //if(e.Button == MouseButtons.Left)
            sender.Invalidate();

            return false;
        }

        /// <summary>
        /// МЫШЬ ДВИЖЕНИЕ СОБЫТИЕ.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        static bool zc_MouseMoveEvent(ZedGraphControl sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.None)
            {
                Blocked = false; // FIX hack
                blockMask = 0;
                //
                double x, y;
                sender.GraphPane.ReverseTransform(new PointF(e.X, e.Y), out x, out y);

                
                textCoord.Text = string.Format("{0}  {1:F3}", 
                    new TimeSpan((long)(x * 1e7)).ToString("hh\\:mm\\:ss\\.f"), y);
                //sender.Invalidate(Rectangle.Round(textCoord.Location.Rect));
            }
            else
            {
                Blocked = true;
                blockMask = (int)e.Button;
            }

            return false;
        }

        /// <summary>
        /// МАСШТАБ СОБЫТИЕ.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        static void zc_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            // enforce value constraints
            var xs = sender.GraphPane.XAxis.Scale;
            var ys = sender.GraphPane.YAxis.Scale;

            double xw = xs.Max - xs.Min;
            double yw = ys.Max - ys.Min;
            double XW = X1 - X0;
            double YW = Y1 - Y0;
            bool ch = false;

            if (yw >= YW) // DUMB USER is not allowed to scale out of range
            {
                if (YW > 0)
                {
                    ys.Min = Y0;
                    ys.Max = Y1;
                }
                else ys.MinAuto = ys.MaxAuto = true;
                sender.IsEnableVZoom = sender.IsEnableVPan = false;
                ch = true;
            }

            // update
            if (ch) { sender.AxisChange(); sender.Invalidate(); }
        }

        static void zc_MouseEnter(object sender, EventArgs e)
        {
            // focus auto entry
            ZedGraphControl zc = sender as ZedGraphControl;
            //if (zc != null) zc.Focus();
        }

        static bool zc_MouseDownEvent(ZedGraphControl sender, System.Windows.Forms.MouseEventArgs e)
        {

            // reset zoom by right mouse button
            if (e.Button == MouseButtons.Right)
            {
                ResetZoom(sender);
                sender.Invalidate();
                return true;
            }
            else if (e.Button == MouseButtons.Left)
            {
                var zc = sender;
                zc.IsEnableVPan = zc.IsEnableVZoom = true; // DUMB USER
            }
        
            // update block while mouse button is down
            blockMask = blockMask | ((int)e.Button & ((int)MouseButtons.Left | (int)MouseButtons.Middle));
            Blocked = blockMask != 0;

            return false;
        }

        /// <summary>
        /// Сброс масштаба отображения графических данных.
        /// </summary>
        /// <param name="zc"></param>
        public static void ResetZoom(ZedGraphControl zc)
        {
            zc.ZoomOutAll(zc.GraphPane);
            var ys = zc.GraphPane.YAxis.Scale;
            var xs = zc.GraphPane.XAxis.Scale;
            if (AutoScale)
            {
                xs.MinAuto = xs.MaxAuto = true;
                zc.IsEnableVPan = zc.IsEnableVZoom = false;
            }
            else
            {
                zc.GraphPane.ZoomStack.Clear();
                xs.Min = Trend.X0; xs.MaxAuto = true; //xs.Max = Trend.X1;
                if (Y1 - Y0 > 0)
                {
                    ys.Min = Trend.Y0; ys.Max = Trend.Y1;
                }
                else ys.MinAuto = ys.MaxAuto = true;
            }
            zc.AxisChange();
            //zc.Invalidate();
            OnScaleReset();
        }

        /// <summary>
        /// Режим работы элемента графиков.
        /// </summary>
        public static enumTrendMode TrendMode = enumTrendMode.Normal;

        /// <summary>
        /// Фунция преобразования к требуемому масштабу.
        /// </summary>
        /// <param name="raw">сырое значение</param>
        /// <param name="a0">начальное значение масштаба</param>
        /// <param name="a1">конечное значение масштаба</param>
        /// <returns></returns>
        public static double ConvertScale(ushort raw, double a0, double a1)
        {
            return a0 + (a1 - a0) * raw / ushort.MaxValue; // / RawY1;
        }

        /// <summary>
        /// Преобразование точки к масштабу графиков.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static PointPair CovertPoint(ushort x, ushort y)
        {
            return new PointPair(ConvertScale(x, X0, X1), ConvertScale(y, Y0, Y1));
        }

        public static ushort RevertScale(double value, double a0, double a1)
        {
            return (ushort)((value - a0) / (a1 - a0) * ushort.MaxValue);
        }

        /// <summary>
        /// Тестовый набор точек в пределах стандартного масштаба.
        /// </summary>
        /// <param name="points">Количество</param>
        /// <returns></returns>
        public static PointPair[] SampleData(int points)
        {
            double xw = X1 - X0;
            double yw = Y1 - Y0;
            PointPair[] res = new PointPair[points];
            for (int i = 0; i < points; ++i)
                res[i] = new PointPair(xw * i / points + X0, 
                    ((Math.Sin(i / 50d) + 1.2) * yw / 4 + (i & 1) * yw / 10));
            return res;
        }

        /// <summary>
        /// Снимок загрузить.
        /// </summary>
        /// <param name="zc"></param>
        /// <param name="filename"></param>
        public static void SnapshotLoad(ZedGraphControl zc, string filename)
        {
            var line = zc.GraphPane.CurveList["SNAPSHOT"] as LineItem;
            var list = line.Points as PointPairList;
            var tr = File.OpenText(filename);
            try
            {

                list.Clear();
                while (!tr.EndOfStream)
                {
                    string[] sline = tr.ReadLine().Split(' ');
                    double x = double.Parse(sline[0], System.Globalization.CultureInfo.InvariantCulture);
                    double y = sline[1] != "-" ?
                        double.Parse(sline[1], System.Globalization.CultureInfo.InvariantCulture)
                        : double.NaN;

                    list.Add(new PointPair(x, y));
                }

                line.IsVisible = true;
            }
            finally { tr.Close(); }
        }

        /// <summary>
        /// Снимок получить.
        /// </summary>
        /// <param name="zc"></param>
        /// <returns></returns>
        public static string SnapshotGet(ZedGraphControl zc)
        {
            StringBuilder sb = new StringBuilder();
            //var line = zc.GraphPane.CurveList["MAIN"] as LineItem;
            var line = zc.GraphPane.CurveList["V+"] as LineItem;
            var list = line.Points as PointPairList;

            for (int i = 0, l = list.Count; i < l; ++i)
            {
                double x = list[i].X, y = list[i].Y;
                if(!double.IsNaN(y) && !double.IsInfinity(y))
                sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, 
                    "{0} {1}\r\n", x, y);
                else sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                    "{0} {1}\r\n", x, '-');
            }

            line = zc.GraphPane.CurveList["V-"] as LineItem;
            list = line.Points as PointPairList;
            sb.AppendLine("0 -"); // HACK
            for (int i = 0, l = list.Count; i < l; ++i)
            {
                double x = list[i].X, y = list[i].Y;
                if (!double.IsNaN(y) && !double.IsInfinity(y))
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                        "{0} {1}\r\n", x, y);
                else sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                    "{0} {1}\r\n", x, '-');
            }

            return sb.ToString();
        }

        /// <summary>
        /// Снимок схоронить.
        /// </summary>
        /// <param name="zc"></param>
        /// <param name="filename"></param>
        public static void SnapshotStore(ZedGraphControl zc, string filename)
        {
            File.WriteAllText(filename, SnapshotGet(zc));
        }


        /// <summary>
        /// Снимок видимость установить.
        /// </summary>
        /// <param name="zc"></param>
        /// <param name="vis"></param>
        public static void SnapshotVisibility(ZedGraphControl zc, bool vis)
        {
            var line = zc.GraphPane.CurveList["SNAPSHOT"] as LineItem;
            if (line != null) line.IsVisible = vis;
        }

        /// <summary>
        /// Событие сброса масштаба.
        /// </summary>
        public static event EventHandler ScaleReset = null;

        /// <summary>
        /// Произведен сброс масштаба.
        /// </summary>
        private static void OnScaleReset()
        {
            try
            {
                if (ScaleReset != null)
                    ScaleReset.Invoke(null, null);
            }
            catch { }
        }

        /// <summary>
        /// Событие отрисовка.
        /// </summary>
        static void zc_Paint(object sender, PaintEventArgs e)
        {
            var gp = sender as ZedGraphControl;
            if (gp == null) return;
            var xs = gp.GraphPane.XAxis.Scale;
            double x0 = xs.Min, x1 = xs.Max; // y limits
            try
            {
                // уровень детализации
                if (SetBounds(gp.MasterPane, x0, x1,
                    LOD ? (int)(gp.ClientSize.Width / simplifyScale) : 0))
                {
                    gp.AxisChange();
                    gp.Invalidate(true);
                }
            }
#if !DEBUG
            catch { } // fail-safe
#else
            catch (Exception ex)
            {
                Console.WriteLine("zc_PAINT: " + ex.Message);
            }
#endif
        }

        /// <summary>
        /// Выбрать уровень детализации для всех поддерживающих кривых
        /// </summary>
        public static bool SetBounds(MasterPane m, double min, double max, int maxPts)
        {
            bool flag = false;
            foreach (GraphPane gp in m.PaneList)
                foreach (CurveItem cu in gp.CurveList)
                {
                    LineItem li = cu as LineItem;
                    if (li != null)
                    {
                        LODPPL lp = li.Points as LODPPL;
                        if (lp != null)
                            flag |= lp.SetBounds(min, max, maxPts);
                    }
                }
            return flag;
        }


        /// <summary>
        /// Добавить маркер аварии
        /// </summary>
        public static void AlarmMarker(MasterPane m, double t, Color c, string mark)
        {
            LineObj cursorLine = new LineObj(c, t, 0.0, t, 1.0);
            cursorLine.Location.CoordinateFrame = CoordType.XScaleYChartFraction;
            cursorLine.IsVisible = true;
            cursorLine.Line.Style = DashStyle.Solid;
            cursorLine.Line.Width = 3; // bold
            cursorLine.Tag = "mark_" + XDate.ToString(t, "yyMMdd_HHmmss");
            cursorLine.ZOrder = ZOrder.E_BehindCurves;
            cursorLine.IsClippedToChartRect = true;
            TextObj to = null;
            if (mark != null)
            {
                to = new TextObj(mark,
                    t, 1, CoordType.XScaleYChartFraction,
                    AlignH.Left, AlignV.Bottom);
                to.ZOrder = ZOrder.E_BehindCurves;
                to.IsClippedToChartRect = true;
                to.FontSpec.Angle = 90;
                //to.FontSpec.FontColor = c;
                to.FontSpec.Border.IsVisible = false;
            }

            foreach (var a in m.PaneList) // добавить всюду
            {
                a.GraphObjList.Add(cursorLine);
                if (to != null) a.GraphObjList.Add(to);
            }
        }
        public static void AlarmMarker(MasterPane m, double t, Color c)
        {
            AlarmMarker(m, t, c, null);
        }
    }
}
