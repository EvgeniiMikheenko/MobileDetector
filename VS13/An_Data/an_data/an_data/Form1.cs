using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
 




namespace an_data
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pane = graph.GraphPane;
            list = new PointPairList();
            list2 = new PointPairList();
            list3 = new PointPairList();
            list4 = new PointPairList();
        }

        GraphPane pane;
        PointPairList list;
        PointPairList list2;
        PointPairList list3;
        PointPairList list4;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        Utils.IO.Files.IntelHexFile File;
        byte[] RDB = new byte[1024];
        
        ushort[] buf111 = new ushort[1024];
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);

                sr.Close();



                File = new Utils.IO.Files.IntelHexFile(openFileDialog1.FileName);


               int fff =  File.Read(RDB, 0, 1024);

                byte[] Data = new byte[File.GetDataSize()];
                Data = File.GetData();

                File.Close();
                int j = 0;
                for (int i = 0; i < 1024; i++)
                {
               
                    buf111[i] = (ushort)((Data[j]) + (Data[j + 1]<<8));
                    j += 2;
                }

                DrawData(buf111);



            }
        }

        double[] corr_buf = new double[200];
        public void DrawData(ushort[] data)
        {

            pane.CurveList.Clear();




            for (int i = 0; i < 1024; i++)
            {
                list.Add(i, buf111[i]);
            }
            LineItem myCurve = pane.AddCurve("Signal", list, Color.Blue, SymbolType.None);

            graph.AxisChange();


            graph.Invalidate();


            for (int i = 0; i < 200; i++)
            {
                if (i % 5 < 2)
                    corr_buf[i] = 26000;
                if (i % 5 == 2)
                    corr_buf[i] = 25000;
                if (i % 5 > 2)
                    corr_buf[i] = 24000;
                //corr_buf[i] = 1000 * Math.Sin(4 * i  * 180 / 100) + 25000;
                //corr_buf[i] = buf111[i + 240]; 
                list2.Add(i, corr_buf[i]);
            }







            LineItem myCurve1 = pane.AddCurve("plus", list2, Color.Red, SymbolType.None);






            CrossCorrelation(corr_buf, buf111);
            //MessageBox.Show(sr.ReadToEnd());
        }
        double[] corr_f = new double[1024];
        double[] cf1 = new double[1024];
        double[] cf2 = new double[1024];
        double[] rising_line = new double[1024];
        double[] falling_line = new double[1024];
        public void CrossCorrelation(double[] corr, ushort[] data  )
        {


            double corr_norm = 0;
            ushort data_norm = 0;
            for(int k = 0; k < 200; k++)
            {
                corr_norm += corr[k];
            }
            for( int k = 0; k < 1024; k++)
            {
                data_norm += data[k];
            }


            for(int i = 0; i < 1024-200; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    corr_f[i] += corr[j] * data[j+i]; 
                }
                corr_f[i] -= (corr_norm + data_norm);
                list3.Add(i, corr_f[i]);
            }
       //     LineItem myCurve3 = pane.AddCurve("rise", list3, Color.Green, SymbolType.None);



            int max = Array.IndexOf(corr_f, corr_f.Max());

            PointPairList c_point = new PointPairList();

            c_point.Add(max, 40000);
            c_point.Add(max, 0);
      //      LineItem myCurve78 = pane.AddCurve("plus", c_point, Color.Black, SymbolType.Star);





            double[] dx_rise = new double[1024];
            double[] dx_fall = new double[1024];
            PointPairList dxr = new PointPairList();
            PointPairList dxf = new PointPairList();

            for ( int i = 1; i < 1023; i++)   
            {
                if( (data[i] > data[i + 1]) && (data[i] > data[i - 1]))
                    rising_line[i] = data[i];
                else
                    rising_line[i] = rising_line[i - 1];


                dx_rise[i] = rising_line[i] - rising_line[i - 1];
                dxr.Add(i, rising_line[i]);

            }

            for (int i = 1; i < 1023; i++)
            {
                if ((data[i] < data[i + 1]) && (data[i] < data[i - 1]))
                    falling_line[i] = data[i];
                else
                    falling_line[i] = falling_line[i - 1];


                dx_fall[i] = falling_line[i] - falling_line[i - 1];
                dxf.Add(i, falling_line[i]);


            }



            for (int i = 0; i < 1024; i++)
            {

                //list3.Add(i, rising_line[i]);
                //list2.Add(i, falling_line[i]);
            }

            // int max = Array.IndexOf(corr_f, corr_f.Max());



        //    LineItem myCurve3 = pane.AddCurve("rise", list3, Color.Green, SymbolType.None);
            //LineItem myCurve4 = pane.AddCurve("fall", list2, Color.Green, SymbolType.None);

            LineItem myCurve5 = pane.AddCurve("dxr", dxr, Color.Red, SymbolType.None);
            LineItem myCurve6 = pane.AddCurve("dxf", dxf, Color.Black, SymbolType.None);

            double[] cb1 = new double[200];
            double[] cb2 = new double[200];


            for( int i = 0; i< 200; i++)
            {
                cb1[i] = rising_line[i + 200];
                cb2[i] = falling_line[i + 200];
            }


            list3.Clear();
            list4.Clear();


            for (int i = 0; i < 1024 - 200; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    cf1[i] += cb1[j] * data[j + i];
                    cf2[i] += cb2[j] * data[j + i];
                }
                corr_f[i] -= (corr_norm + data_norm);


                list3.Add(i, cf1[i]);
                list4.Add(i, cf2[i]);
                

            }


            LineItem myCurve7 = pane.AddCurve("44", list3, Color.Red, SymbolType.None);
            LineItem myCurve8 = pane.AddCurve("55", list4, Color.Black, SymbolType.None);



            int max1 = Array.IndexOf(cf1, cf1.Max());

            PointPairList c_point1 = new PointPairList();

            c_point1.Add(max1, 40000);
            c_point1.Add(max1, 0);
            LineItem myCurve78 = pane.AddCurve("plus", c_point1, Color.Black, SymbolType.Star);




            int max2 = Array.IndexOf(cf1, cf1.Max());

            PointPairList c_point2 = new PointPairList();

            c_point2.Add(max1, 40000);
            c_point2.Add(max1, 0);
            LineItem myCurve79 = pane.AddCurve("plus", c_point2, Color.Red, SymbolType.Star);

//


          //graph.AxisChange();


            //graph.Invalidate();

        }

    }
}
