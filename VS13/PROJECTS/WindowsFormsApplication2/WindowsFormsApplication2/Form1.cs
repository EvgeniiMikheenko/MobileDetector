using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using ZedGraph;



namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {


        bool receive = false;
        int recflag = 0;
        string[] buf = new string[100];
        int cnt = 0;
        string[] strarr = null;


        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (COM4.IsOpen)
                    COM4.Close();
                status.Text = "закрыт";
            }
            catch (System.IO.IOException)
            {
                status.Text="ошибка";
                MessageBox.Show("НЕТ");
            }
        }

        private void com4_open_Click(object sender, EventArgs e)
        {

            try
            {
                if (!COM4.IsOpen)
                    COM4.Open();
                status.Text = "открыт";
            }
            catch (System.IO.IOException)
            {
                status.Text="ошибка";
                MessageBox.Show("НЕТ");
                
            }
        }


      

        private void COM4_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate));
        }





        PointPairList data = new PointPairList();
        PointPairList filtereddata = new PointPairList();
        



        private void DoUpdate(object s, EventArgs e)
        {
            string message = COM4.ReadExisting();
            string datamsg;


            GraphPane pane = Graph.GraphPane;
            pane.CurveList.Clear();   //!!!!!!!!!
             
            try
            {
               // adc.Text += "event\n";
                number.Text = message.Length.ToString();
                adc.Text += message[0];
                adc.Text += " ";
                adc.Text += message[message.Length-2];
                adc.Text += "\n";

                string firstbyte = "#";
                string lastbyte = "&";
                
               

                if(message[0] == firstbyte.ToCharArray()[0]   && message[message.Length-2] == lastbyte.ToCharArray()[0])
                {
                    datamsg = message.Substring(2, message.Length - 6);
                    yn.Text += datamsg;
                    yn.Text += "\n";

                    string splitstring = " \n\r ";


                    strarr = datamsg.Split(splitstring.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);



                    try
                    {
                        data.Add(Int32.Parse(strarr[0]), Int32.Parse(strarr[1]));
                        filtereddata.Add(Int32.Parse(strarr[0]), Int32.Parse(strarr[2]));
                        
                    }
                    catch (FormatException)
                    {

                        
                    }

                    LineItem data_Curve = pane.AddCurve("adc_data", data, Color.Blue, SymbolType.None);
                    LineItem filter_myCurve = pane.AddCurve("filtered_data", filtereddata, Color.Red, SymbolType.None);

                    Graph.AxisChange();

                    
                    Graph.Invalidate();

                    counter.Text += "a=";
                    counter.Text += strarr[0];
                    counter.Text += "   ";
                    counter.Text += "b=";
                    counter.Text += strarr[1];
                    counter.Text += "   ";
                    counter.Text += "c=";
                    counter.Text += strarr[2];
                    counter.Text += "   ";
                    counter.Text += "d\n";
                    counter.Text += strarr[3];
                    counter.Text += "\n";





                }






            }
            catch (IndexOutOfRangeException)
            {

              //  throw;
            }


            textbox.Text += message;
            textbox.Text += "_____\n";
        }


        private void button1_Click(object sender, EventArgs e)
        {

            
            receive = true;
            if (COM4.IsOpen)
            {
                while (receive)
                {
                    try
                    {
                       // string message = COM4.ReadLine();
                       // textbox.Text += message;

                       //if(message == "###\n\r")
                       // {
                       //     //recflag++;
                       //     number.Text += ++cnt ;
                       // }


                         

                       // buf[++recflag] = message;

                       // if(message == "#**#")
                       // {
                       //     recflag = 0;                           
                       //     counter.Text += buf[3];
                       //     adc.Text += buf[4];
                       //     yn.Text += buf[5];
                       // }





                    }
                    catch (TimeoutException)
                    {
                        textbox.Text += "ошибка, нет ничего\n";
                        receive = false;
                    }
                    finally
                    {
                    }
                }
            }
        }





        

    private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void stop_Click(object sender, EventArgs e)
        {
            receive = false;
        }

        private void clr_Click(object sender, EventArgs e)
        {
            textbox.Clear();
            counter.Clear();
            adc.Clear();
            yn.Clear();
            number.Clear();

            GraphPane pane1 = Graph.GraphPane;
            pane1.CurveList.Clear();   //!!!!!!!!!
        }


        private void Graph_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (COM4.IsOpen)
            {
                COM4.Close(); 
            }
        }
    }
}
