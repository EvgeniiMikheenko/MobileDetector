namespace LGAR
{
    partial class formMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
            this.splitter = new System.Windows.Forms.SplitContainer();
            this.lblMaterial = new System.Windows.Forms.Label();
            this.lblDespair = new System.Windows.Forms.Label();
            this.lblDangerPositive = new System.Windows.Forms.Label();
            this.lblDangerNegative = new System.Windows.Forms.Label();
            this.graph = new ZedGraph.ZedGraphControl();
            this.cmdWrite = new System.Windows.Forms.CheckBox();
            this.cmdStartStop = new System.Windows.Forms.Button();
            this.scroller = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.cmdControl = new System.Windows.Forms.CheckBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.cmdZone_timerDown = new System.Windows.Forms.Button();
            this.lblY = new System.Windows.Forms.Label();
            this.cmdFilterUp = new System.Windows.Forms.Button();
            this.Zonelbl = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdK2maxUp = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cmdZone_timerUp = new System.Windows.Forms.Button();
            this.cmdKmax = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdK2maxDown = new System.Windows.Forms.Button();
            this.cmdFilter = new System.Windows.Forms.CheckBox();
            this.numK2max = new System.Windows.Forms.TrackBar();
            this.lblReady = new System.Windows.Forms.Label();
            this.StateSelector = new System.Windows.Forms.ComboBox();
            this.cmdYDown = new System.Windows.Forms.Button();
            this.lblK2max = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numY = new System.Windows.Forms.TrackBar();
            this.select_addr = new System.Windows.Forms.ComboBox();
            this.inputT = new System.Windows.Forms.Label();
            this.InputEcm = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.grpK2max = new System.Windows.Forms.GroupBox();
            this.grpY = new System.Windows.Forms.GroupBox();
            this.Zone_timer = new System.Windows.Forms.TrackBar();
            this.cmdFilterDown = new System.Windows.Forms.Button();
            this.cmdYUp = new System.Windows.Forms.Button();
            this.inputUm = new System.Windows.Forms.Label();
            this.inputF = new System.Windows.Forms.Label();
            this.grpUm = new System.Windows.Forms.GroupBox();
            this.cmdUmDown = new System.Windows.Forms.Button();
            this.cmdUmUp = new System.Windows.Forms.Button();
            this.lblUm = new System.Windows.Forms.Label();
            this.numUm = new System.Windows.Forms.TrackBar();
            this.grpKmax = new System.Windows.Forms.GroupBox();
            this.cmdKmaxDown = new System.Windows.Forms.Button();
            this.cmdKmaxUp = new System.Windows.Forms.Button();
            this.lblKmax = new System.Windows.Forms.Label();
            this.numKmax = new System.Windows.Forms.TrackBar();
            this.grpFilter = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdEnum = new System.Windows.Forms.CheckBox();
            this.cmdLock = new System.Windows.Forms.CheckBox();
            this.numFilter = new System.Windows.Forms.TrackBar();
            this.grpTP = new System.Windows.Forms.GroupBox();
            this.cmdTPDown = new System.Windows.Forms.Button();
            this.cmdTPDownSmall = new System.Windows.Forms.Button();
            this.cmdTPUpSmall = new System.Windows.Forms.Button();
            this.cmdTPUp = new System.Windows.Forms.Button();
            this.lblTP = new System.Windows.Forms.Label();
            this.numTP = new System.Windows.Forms.TrackBar();
            this.grpTN = new System.Windows.Forms.GroupBox();
            this.cmdTNDown = new System.Windows.Forms.Button();
            this.cmdTNDownSmall = new System.Windows.Forms.Button();
            this.cmdTNUp = new System.Windows.Forms.Button();
            this.cmdTNUpSmall = new System.Windows.Forms.Button();
            this.lblTN = new System.Windows.Forms.Label();
            this.numTN = new System.Windows.Forms.TrackBar();
            this.grpDN = new System.Windows.Forms.GroupBox();
            this.cmdDNDOwn = new System.Windows.Forms.Button();
            this.cmdDNDownSmall = new System.Windows.Forms.Button();
            this.cmdDNUp = new System.Windows.Forms.Button();
            this.cmdDNUpSmall = new System.Windows.Forms.Button();
            this.lblDN = new System.Windows.Forms.Label();
            this.numDN = new System.Windows.Forms.TrackBar();
            this.grpDP = new System.Windows.Forms.GroupBox();
            this.cmdDPDown = new System.Windows.Forms.Button();
            this.cmdDPDownSmall = new System.Windows.Forms.Button();
            this.cmdDPUpSmall = new System.Windows.Forms.Button();
            this.cmdDPUp = new System.Windows.Forms.Button();
            this.lblDP = new System.Windows.Forms.Label();
            this.numDP = new System.Windows.Forms.TrackBar();
            this.grpEcm = new System.Windows.Forms.GroupBox();
            this.cmdEcmDown = new System.Windows.Forms.Button();
            this.cmdEcmUp = new System.Windows.Forms.Button();
            this.lblEcm = new System.Windows.Forms.Label();
            this.numEcm = new System.Windows.Forms.TrackBar();
            this.grpF = new System.Windows.Forms.GroupBox();
            this.cmdFDown = new System.Windows.Forms.Button();
            this.cmdFUp = new System.Windows.Forms.Button();
            this.lblF = new System.Windows.Forms.Label();
            this.numF = new System.Windows.Forms.TrackBar();
            this.menu_ = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuF_1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFImport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuF_2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFTerminate = new System.Windows.Forms.ToolStripMenuItem();
            this.menuConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCSocket = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCSerialPort = new System.Windows.Forms.ToolStripMenuItem();
            this.menuC_Delay = new System.Windows.Forms.ToolStripMenuItem();
            this.menuC_1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuCDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuC_Bottom = new System.Windows.Forms.ToolStripSeparator();
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuV_Panel = new System.Windows.Forms.ToolStripMenuItem();
            this.menuV_1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuVDebugConsole = new System.Windows.Forms.ToolStripMenuItem();
            this.menuV_2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuVSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWrite = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWWrite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuWHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBottom = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.stMD5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.stID = new System.Windows.Forms.ToolStripStatusLabel();
            this.stStep = new System.Windows.Forms.ToolStripStatusLabel();
            this.stErrors = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.COM_thing = new System.IO.Ports.SerialPort(this.components);
            this.poller = new System.Windows.Forms.Timer(this.components);
            this.flasher = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
            this.splitter.Panel1.SuspendLayout();
            this.splitter.Panel2.SuspendLayout();
            this.splitter.SuspendLayout();
            this.scroller.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numK2max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).BeginInit();
            this.grpK2max.SuspendLayout();
            this.grpY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Zone_timer)).BeginInit();
            this.grpUm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUm)).BeginInit();
            this.grpKmax.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numKmax)).BeginInit();
            this.grpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFilter)).BeginInit();
            this.grpTP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTP)).BeginInit();
            this.grpTN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTN)).BeginInit();
            this.grpDN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDN)).BeginInit();
            this.grpDP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDP)).BeginInit();
            this.grpEcm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEcm)).BeginInit();
            this.grpF.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numF)).BeginInit();
            this.menu_.SuspendLayout();
            this.statusBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitter
            // 
            this.splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitter.Location = new System.Drawing.Point(0, 24);
            this.splitter.Name = "splitter";
            this.splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitter.Panel1
            // 
            this.splitter.Panel1.BackColor = System.Drawing.Color.White;
            this.splitter.Panel1.Controls.Add(this.lblMaterial);
            this.splitter.Panel1.Controls.Add(this.lblDespair);
            this.splitter.Panel1.Controls.Add(this.lblDangerPositive);
            this.splitter.Panel1.Controls.Add(this.lblDangerNegative);
            this.splitter.Panel1.Controls.Add(this.graph);
            // 
            // splitter.Panel2
            // 
            this.splitter.Panel2.Controls.Add(this.cmdWrite);
            this.splitter.Panel2.Controls.Add(this.cmdStartStop);
            this.splitter.Panel2.Controls.Add(this.scroller);
            this.splitter.Panel2MinSize = 180;
            this.splitter.Size = new System.Drawing.Size(1908, 692);
            this.splitter.SplitterDistance = 401;
            this.splitter.TabIndex = 0;
            // 
            // lblMaterial
            // 
            this.lblMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMaterial.AutoSize = true;
            this.lblMaterial.BackColor = System.Drawing.Color.Yellow;
            this.lblMaterial.Location = new System.Drawing.Point(12, 375);
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaterial.Size = new System.Drawing.Size(154, 20);
            this.lblMaterial.TabIndex = 1;
            this.lblMaterial.Text = "Опасно Вещество";
            this.lblMaterial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMaterial.Visible = false;
            // 
            // lblDespair
            // 
            this.lblDespair.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblDespair.BackColor = System.Drawing.Color.Red;
            this.lblDespair.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDespair.ForeColor = System.Drawing.Color.White;
            this.lblDespair.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDespair.Location = new System.Drawing.Point(752, 160);
            this.lblDespair.Name = "lblDespair";
            this.lblDespair.Size = new System.Drawing.Size(404, 80);
            this.lblDespair.TabIndex = 2;
            this.lblDespair.Text = "ОШИБКА СВЯЗИ";
            this.lblDespair.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDespair.Visible = false;
            this.lblDespair.Click += new System.EventHandler(this.lblDespair_Click);
            // 
            // lblDangerPositive
            // 
            this.lblDangerPositive.BackColor = System.Drawing.Color.Yellow;
            this.lblDangerPositive.Location = new System.Drawing.Point(123, 8);
            this.lblDangerPositive.Name = "lblDangerPositive";
            this.lblDangerPositive.Size = new System.Drawing.Size(105, 23);
            this.lblDangerPositive.TabIndex = 1;
            this.lblDangerPositive.Text = "+ Опасно";
            this.lblDangerPositive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDangerPositive.Visible = false;
            // 
            // lblDangerNegative
            // 
            this.lblDangerNegative.BackColor = System.Drawing.Color.Yellow;
            this.lblDangerNegative.Location = new System.Drawing.Point(12, 8);
            this.lblDangerNegative.Name = "lblDangerNegative";
            this.lblDangerNegative.Size = new System.Drawing.Size(105, 23);
            this.lblDangerNegative.TabIndex = 1;
            this.lblDangerNegative.Text = "- Опасно";
            this.lblDangerNegative.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDangerNegative.Visible = false;
            // 
            // graph
            // 
            this.graph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graph.IsEnableVPan = false;
            this.graph.IsEnableVZoom = false;
            this.graph.IsShowContextMenu = false;
            this.graph.IsShowCopyMessage = false;
            this.graph.IsZoomOnMouseCenter = true;
            this.graph.Location = new System.Drawing.Point(0, 34);
            this.graph.Name = "graph";
            this.graph.ScrollGrace = 0D;
            this.graph.ScrollMaxX = 0D;
            this.graph.ScrollMaxY = 0D;
            this.graph.ScrollMaxY2 = 0D;
            this.graph.ScrollMinX = 0D;
            this.graph.ScrollMinY = 0D;
            this.graph.ScrollMinY2 = 0D;
            this.graph.Size = new System.Drawing.Size(1882, 365);
            this.graph.TabIndex = 0;
            this.graph.ZoomStepFraction = 0.5D;
            // 
            // cmdWrite
            // 
            this.cmdWrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdWrite.Appearance = System.Windows.Forms.Appearance.Button;
            this.cmdWrite.Location = new System.Drawing.Point(1771, 40);
            this.cmdWrite.Name = "cmdWrite";
            this.cmdWrite.Size = new System.Drawing.Size(125, 34);
            this.cmdWrite.TabIndex = 2;
            this.cmdWrite.Text = "Запись";
            this.cmdWrite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cmdWrite.UseVisualStyleBackColor = false;
            this.cmdWrite.CheckedChanged += new System.EventHandler(this.Control_CheckedChanged);
            // 
            // cmdStartStop
            // 
            this.cmdStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdStartStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdStartStop.Location = new System.Drawing.Point(1772, 171);
            this.cmdStartStop.Name = "cmdStartStop";
            this.cmdStartStop.Size = new System.Drawing.Size(125, 34);
            this.cmdStartStop.TabIndex = 1;
            this.cmdStartStop.Text = "Старт";
            this.cmdStartStop.UseVisualStyleBackColor = true;
            this.cmdStartStop.Click += new System.EventHandler(this.cmdStartStop_Click);
            // 
            // scroller
            // 
            this.scroller.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scroller.Controls.Add(this.richTextBox1);
            this.scroller.Controls.Add(this.cmdControl);
            this.scroller.Controls.Add(this.lblFilter);
            this.scroller.Controls.Add(this.cmdZone_timerDown);
            this.scroller.Controls.Add(this.lblY);
            this.scroller.Controls.Add(this.cmdFilterUp);
            this.scroller.Controls.Add(this.Zonelbl);
            this.scroller.Controls.Add(this.label3);
            this.scroller.Controls.Add(this.cmdK2maxUp);
            this.scroller.Controls.Add(this.checkBox1);
            this.scroller.Controls.Add(this.cmdZone_timerUp);
            this.scroller.Controls.Add(this.cmdKmax);
            this.scroller.Controls.Add(this.label1);
            this.scroller.Controls.Add(this.cmdK2maxDown);
            this.scroller.Controls.Add(this.cmdFilter);
            this.scroller.Controls.Add(this.numK2max);
            this.scroller.Controls.Add(this.lblReady);
            this.scroller.Controls.Add(this.StateSelector);
            this.scroller.Controls.Add(this.cmdYDown);
            this.scroller.Controls.Add(this.lblK2max);
            this.scroller.Controls.Add(this.label4);
            this.scroller.Controls.Add(this.numY);
            this.scroller.Controls.Add(this.select_addr);
            this.scroller.Controls.Add(this.inputT);
            this.scroller.Controls.Add(this.InputEcm);
            this.scroller.Controls.Add(this.button1);
            this.scroller.Controls.Add(this.grpK2max);
            this.scroller.Controls.Add(this.inputUm);
            this.scroller.Controls.Add(this.inputF);
            this.scroller.Controls.Add(this.grpUm);
            this.scroller.Controls.Add(this.grpKmax);
            this.scroller.Controls.Add(this.grpFilter);
            this.scroller.Controls.Add(this.grpTP);
            this.scroller.Controls.Add(this.grpTN);
            this.scroller.Controls.Add(this.grpDN);
            this.scroller.Controls.Add(this.grpDP);
            this.scroller.Controls.Add(this.grpEcm);
            this.scroller.Location = new System.Drawing.Point(3, 3);
            this.scroller.Name = "scroller";
            this.scroller.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.scroller.Size = new System.Drawing.Size(1762, 284);
            this.scroller.TabIndex = 0;
            this.scroller.Paint += new System.Windows.Forms.PaintEventHandler(this.scroller_Paint);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(1105, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(650, 284);
            this.richTextBox1.TabIndex = 15;
            this.richTextBox1.Text = "";
            // 
            // cmdControl
            // 
            this.cmdControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdControl.Appearance = System.Windows.Forms.Appearance.Button;
            this.cmdControl.ForeColor = System.Drawing.Color.Black;
            this.cmdControl.Location = new System.Drawing.Point(1484, 10);
            this.cmdControl.Name = "cmdControl";
            this.cmdControl.Size = new System.Drawing.Size(125, 34);
            this.cmdControl.TabIndex = 4;
            this.cmdControl.Text = "Контроль";
            this.cmdControl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cmdControl.UseVisualStyleBackColor = false;
            this.cmdControl.CheckedChanged += new System.EventHandler(this.Control_CheckedChanged);
            // 
            // lblFilter
            // 
            this.lblFilter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFilter.BackColor = System.Drawing.SystemColors.Window;
            this.lblFilter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFilter.Location = new System.Drawing.Point(1376, 50);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(80, 48);
            this.lblFilter.TabIndex = 1;
            this.lblFilter.Tag = "{0}";
            this.lblFilter.Text = "###.#";
            this.lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdZone_timerDown
            // 
            this.cmdZone_timerDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdZone_timerDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdZone_timerDown.ForeColor = System.Drawing.Color.Black;
            this.cmdZone_timerDown.Location = new System.Drawing.Point(1568, 76);
            this.cmdZone_timerDown.Name = "cmdZone_timerDown";
            this.cmdZone_timerDown.Size = new System.Drawing.Size(65, 37);
            this.cmdZone_timerDown.TabIndex = 12;
            this.cmdZone_timerDown.TabStop = false;
            this.cmdZone_timerDown.Text = "-";
            this.cmdZone_timerDown.UseVisualStyleBackColor = true;
            // 
            // lblY
            // 
            this.lblY.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblY.BackColor = System.Drawing.SystemColors.Window;
            this.lblY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblY.Location = new System.Drawing.Point(1468, 57);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(80, 48);
            this.lblY.TabIndex = 1;
            this.lblY.Tag = "{0:F2}";
            this.lblY.Text = "###.#";
            this.lblY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblY.Click += new System.EventHandler(this.lblY_Click);
            // 
            // cmdFilterUp
            // 
            this.cmdFilterUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdFilterUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdFilterUp.Location = new System.Drawing.Point(1544, 10);
            this.cmdFilterUp.Name = "cmdFilterUp";
            this.cmdFilterUp.Size = new System.Drawing.Size(65, 37);
            this.cmdFilterUp.TabIndex = 2;
            this.cmdFilterUp.TabStop = false;
            this.cmdFilterUp.Text = "+";
            this.cmdFilterUp.UseVisualStyleBackColor = true;
            // 
            // Zonelbl
            // 
            this.Zonelbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Zonelbl.BackColor = System.Drawing.SystemColors.Window;
            this.Zonelbl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Zonelbl.ForeColor = System.Drawing.Color.Black;
            this.Zonelbl.Location = new System.Drawing.Point(1508, 99);
            this.Zonelbl.Name = "Zonelbl";
            this.Zonelbl.Size = new System.Drawing.Size(80, 34);
            this.Zonelbl.TabIndex = 10;
            this.Zonelbl.Tag = "{0:F3}";
            this.Zonelbl.Text = "###.##";
            this.Zonelbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1398, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(246, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Время в выбранном диапозоне";
            // 
            // cmdK2maxUp
            // 
            this.cmdK2maxUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdK2maxUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdK2maxUp.ForeColor = System.Drawing.Color.Black;
            this.cmdK2maxUp.Location = new System.Drawing.Point(1554, 91);
            this.cmdK2maxUp.Name = "cmdK2maxUp";
            this.cmdK2maxUp.Size = new System.Drawing.Size(65, 37);
            this.cmdK2maxUp.TabIndex = 2;
            this.cmdK2maxUp.TabStop = false;
            this.cmdK2maxUp.Text = "+";
            this.cmdK2maxUp.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(1402, 81);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(281, 24);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Откл. переключение диапозонов";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // cmdZone_timerUp
            // 
            this.cmdZone_timerUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdZone_timerUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdZone_timerUp.ForeColor = System.Drawing.Color.Black;
            this.cmdZone_timerUp.Location = new System.Drawing.Point(1523, 103);
            this.cmdZone_timerUp.Name = "cmdZone_timerUp";
            this.cmdZone_timerUp.Size = new System.Drawing.Size(65, 37);
            this.cmdZone_timerUp.TabIndex = 11;
            this.cmdZone_timerUp.TabStop = false;
            this.cmdZone_timerUp.Text = "+";
            this.cmdZone_timerUp.UseVisualStyleBackColor = true;
            // 
            // cmdKmax
            // 
            this.cmdKmax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdKmax.Appearance = System.Windows.Forms.Appearance.Button;
            this.cmdKmax.Location = new System.Drawing.Point(1456, 72);
            this.cmdKmax.Name = "cmdKmax";
            this.cmdKmax.Size = new System.Drawing.Size(125, 55);
            this.cmdKmax.TabIndex = 2;
            this.cmdKmax.Text = "Принудительно Готов";
            this.cmdKmax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cmdKmax.UseVisualStyleBackColor = false;
            this.cmdKmax.CheckedChanged += new System.EventHandler(this.Control_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1331, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(313, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Номер выбранного диапозона настроек";
            // 
            // cmdK2maxDown
            // 
            this.cmdK2maxDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdK2maxDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdK2maxDown.ForeColor = System.Drawing.Color.Black;
            this.cmdK2maxDown.Location = new System.Drawing.Point(1523, 99);
            this.cmdK2maxDown.Name = "cmdK2maxDown";
            this.cmdK2maxDown.Size = new System.Drawing.Size(65, 37);
            this.cmdK2maxDown.TabIndex = 2;
            this.cmdK2maxDown.TabStop = false;
            this.cmdK2maxDown.Text = "-";
            this.cmdK2maxDown.UseVisualStyleBackColor = true;
            // 
            // cmdFilter
            // 
            this.cmdFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdFilter.Appearance = System.Windows.Forms.Appearance.Button;
            this.cmdFilter.Location = new System.Drawing.Point(1180, 131);
            this.cmdFilter.Name = "cmdFilter";
            this.cmdFilter.Size = new System.Drawing.Size(125, 34);
            this.cmdFilter.TabIndex = 2;
            this.cmdFilter.Text = "Shunt";
            this.cmdFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cmdFilter.UseVisualStyleBackColor = false;
            this.cmdFilter.CheckedChanged += new System.EventHandler(this.Control_CheckedChanged);
            // 
            // numK2max
            // 
            this.numK2max.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numK2max.Location = new System.Drawing.Point(1638, 45);
            this.numK2max.Name = "numK2max";
            this.numK2max.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.numK2max.Size = new System.Drawing.Size(45, 120);
            this.numK2max.TabIndex = 0;
            // 
            // lblReady
            // 
            this.lblReady.BackColor = System.Drawing.Color.LimeGreen;
            this.lblReady.Location = new System.Drawing.Point(1200, 104);
            this.lblReady.Name = "lblReady";
            this.lblReady.Size = new System.Drawing.Size(105, 23);
            this.lblReady.TabIndex = 3;
            this.lblReady.Text = "Shunt ON";
            this.lblReady.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblReady.Visible = false;
            // 
            // StateSelector
            // 
            this.StateSelector.FormattingEnabled = true;
            this.StateSelector.Items.AddRange(new object[] {
            "-1",
            "-1",
            "-1"});
            this.StateSelector.Location = new System.Drawing.Point(1376, 14);
            this.StateSelector.Name = "StateSelector";
            this.StateSelector.Size = new System.Drawing.Size(136, 28);
            this.StateSelector.TabIndex = 3;
            this.StateSelector.SelectedIndexChanged += new System.EventHandler(this.StateSelector_SelectedIndexChanged);
            // 
            // cmdYDown
            // 
            this.cmdYDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdYDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdYDown.Location = new System.Drawing.Point(1601, 48);
            this.cmdYDown.Name = "cmdYDown";
            this.cmdYDown.Size = new System.Drawing.Size(65, 37);
            this.cmdYDown.TabIndex = 2;
            this.cmdYDown.TabStop = false;
            this.cmdYDown.Text = "-";
            this.cmdYDown.UseVisualStyleBackColor = true;
            // 
            // lblK2max
            // 
            this.lblK2max.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblK2max.BackColor = System.Drawing.SystemColors.Window;
            this.lblK2max.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblK2max.ForeColor = System.Drawing.Color.Black;
            this.lblK2max.Location = new System.Drawing.Point(1557, 57);
            this.lblK2max.Name = "lblK2max";
            this.lblK2max.Size = new System.Drawing.Size(77, 48);
            this.lblK2max.TabIndex = 1;
            this.lblK2max.Tag = "{0:F3}";
            this.lblK2max.Text = "###.##";
            this.lblK2max.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1488, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Выбранный адресс";
            // 
            // numY
            // 
            this.numY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numY.LargeChange = 100;
            this.numY.Location = new System.Drawing.Point(1488, 50);
            this.numY.Maximum = 3300;
            this.numY.Name = "numY";
            this.numY.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.numY.Size = new System.Drawing.Size(45, 67);
            this.numY.SmallChange = 10;
            this.numY.TabIndex = 0;
            this.numY.TickFrequency = 500;
            this.numY.Value = 3300;
            this.numY.ValueChanged += new System.EventHandler(this.numY_Scroll);
            // 
            // select_addr
            // 
            this.select_addr.FormattingEnabled = true;
            this.select_addr.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.select_addr.Location = new System.Drawing.Point(1317, 16);
            this.select_addr.Name = "select_addr";
            this.select_addr.Size = new System.Drawing.Size(121, 28);
            this.select_addr.TabIndex = 13;
            this.select_addr.SelectedIndexChanged += new System.EventHandler(this.select_addr_SelectedIndexChanged);
            // 
            // inputT
            // 
            this.inputT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.inputT.BackColor = System.Drawing.Color.White;
            this.inputT.Location = new System.Drawing.Point(1403, 10);
            this.inputT.Name = "inputT";
            this.inputT.Size = new System.Drawing.Size(130, 23);
            this.inputT.TabIndex = 1;
            this.inputT.Tag = "T={0:F1}°C";
            this.inputT.Text = "T=";
            this.inputT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InputEcm
            // 
            this.InputEcm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InputEcm.BackColor = System.Drawing.Color.Gainsboro;
            this.InputEcm.Location = new System.Drawing.Point(1403, 10);
            this.InputEcm.Name = "InputEcm";
            this.InputEcm.Size = new System.Drawing.Size(130, 23);
            this.InputEcm.TabIndex = 1;
            this.InputEcm.Tag = "Eсм={0:F1} В";
            this.InputEcm.Text = "Eсм=";
            this.InputEcm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(565, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(1174, 10);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // grpK2max
            // 
            this.grpK2max.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpK2max.Controls.Add(this.grpY);
            this.grpK2max.Controls.Add(this.cmdFilterDown);
            this.grpK2max.Controls.Add(this.cmdYUp);
            this.grpK2max.ForeColor = System.Drawing.Color.Blue;
            this.grpK2max.Location = new System.Drawing.Point(1524, 50);
            this.grpK2max.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.grpK2max.Name = "grpK2max";
            this.grpK2max.Size = new System.Drawing.Size(120, 44);
            this.grpK2max.TabIndex = 2;
            this.grpK2max.TabStop = false;
            this.grpK2max.Text = "Уст \"0\" АХ, мВ";
            // 
            // grpY
            // 
            this.grpY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpY.Controls.Add(this.Zone_timer);
            this.grpY.Location = new System.Drawing.Point(8, 108);
            this.grpY.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.grpY.Name = "grpY";
            this.grpY.Size = new System.Drawing.Size(120, 0);
            this.grpY.TabIndex = 0;
            this.grpY.TabStop = false;
            this.grpY.Text = "Масштаб, В";
            // 
            // Zone_timer
            // 
            this.Zone_timer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Zone_timer.Location = new System.Drawing.Point(-119, 109);
            this.Zone_timer.Maximum = 20;
            this.Zone_timer.Name = "Zone_timer";
            this.Zone_timer.Size = new System.Drawing.Size(255, 45);
            this.Zone_timer.TabIndex = 0;
            this.Zone_timer.Scroll += new System.EventHandler(this.Zone_timer_Scroll);
            this.Zone_timer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Zone_timer_MouseUp);
            // 
            // cmdFilterDown
            // 
            this.cmdFilterDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdFilterDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdFilterDown.Location = new System.Drawing.Point(0, 10);
            this.cmdFilterDown.Name = "cmdFilterDown";
            this.cmdFilterDown.Size = new System.Drawing.Size(65, 37);
            this.cmdFilterDown.TabIndex = 2;
            this.cmdFilterDown.TabStop = false;
            this.cmdFilterDown.Text = "-";
            this.cmdFilterDown.UseVisualStyleBackColor = true;
            // 
            // cmdYUp
            // 
            this.cmdYUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdYUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdYUp.Location = new System.Drawing.Point(15, 19);
            this.cmdYUp.Name = "cmdYUp";
            this.cmdYUp.Size = new System.Drawing.Size(65, 37);
            this.cmdYUp.TabIndex = 2;
            this.cmdYUp.TabStop = false;
            this.cmdYUp.Text = "+";
            this.cmdYUp.UseVisualStyleBackColor = true;
            // 
            // inputUm
            // 
            this.inputUm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.inputUm.BackColor = System.Drawing.Color.Gainsboro;
            this.inputUm.Location = new System.Drawing.Point(1416, 10);
            this.inputUm.Name = "inputUm";
            this.inputUm.Size = new System.Drawing.Size(130, 23);
            this.inputUm.TabIndex = 1;
            this.inputUm.Tag = "Um={0} В";
            this.inputUm.Text = "Um=";
            this.inputUm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // inputF
            // 
            this.inputF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.inputF.BackColor = System.Drawing.Color.Gainsboro;
            this.inputF.Location = new System.Drawing.Point(1295, 10);
            this.inputF.Name = "inputF";
            this.inputF.Size = new System.Drawing.Size(130, 23);
            this.inputF.TabIndex = 1;
            this.inputF.Tag = "F={0} Гц";
            this.inputF.Text = "F=";
            this.inputF.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpUm
            // 
            this.grpUm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpUm.Controls.Add(this.cmdUmDown);
            this.grpUm.Controls.Add(this.cmdUmUp);
            this.grpUm.Controls.Add(this.lblUm);
            this.grpUm.Controls.Add(this.numUm);
            this.grpUm.Controls.Add(this.grpF);
            this.grpUm.Location = new System.Drawing.Point(1204, 10);
            this.grpUm.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.grpUm.Name = "grpUm";
            this.grpUm.Size = new System.Drawing.Size(120, 284);
            this.grpUm.TabIndex = 0;
            this.grpUm.TabStop = false;
            this.grpUm.Text = "Var. res.";
            // 
            // cmdUmDown
            // 
            this.cmdUmDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdUmDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdUmDown.Location = new System.Drawing.Point(46, 241);
            this.cmdUmDown.Name = "cmdUmDown";
            this.cmdUmDown.Size = new System.Drawing.Size(65, 37);
            this.cmdUmDown.TabIndex = 2;
            this.cmdUmDown.TabStop = false;
            this.cmdUmDown.Text = "-";
            this.cmdUmDown.UseVisualStyleBackColor = true;
            // 
            // cmdUmUp
            // 
            this.cmdUmUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdUmUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdUmUp.Location = new System.Drawing.Point(46, 23);
            this.cmdUmUp.Name = "cmdUmUp";
            this.cmdUmUp.Size = new System.Drawing.Size(65, 37);
            this.cmdUmUp.TabIndex = 2;
            this.cmdUmUp.TabStop = false;
            this.cmdUmUp.Text = "+";
            this.cmdUmUp.UseVisualStyleBackColor = true;
            // 
            // lblUm
            // 
            this.lblUm.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblUm.BackColor = System.Drawing.SystemColors.Window;
            this.lblUm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblUm.Location = new System.Drawing.Point(34, 126);
            this.lblUm.Name = "lblUm";
            this.lblUm.Size = new System.Drawing.Size(77, 48);
            this.lblUm.TabIndex = 1;
            this.lblUm.Tag = "{0:F1}";
            this.lblUm.Text = "###.#";
            this.lblUm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numUm
            // 
            this.numUm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numUm.Location = new System.Drawing.Point(8, 23);
            this.numUm.Name = "numUm";
            this.numUm.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.numUm.Size = new System.Drawing.Size(45, 255);
            this.numUm.TabIndex = 0;
            // 
            // grpKmax
            // 
            this.grpKmax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpKmax.Controls.Add(this.cmdKmaxDown);
            this.grpKmax.Controls.Add(this.cmdKmaxUp);
            this.grpKmax.Controls.Add(this.lblKmax);
            this.grpKmax.Controls.Add(this.numKmax);
            this.grpKmax.ForeColor = System.Drawing.Color.Red;
            this.grpKmax.Location = new System.Drawing.Point(363, 0);
            this.grpKmax.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.grpKmax.Name = "grpKmax";
            this.grpKmax.Size = new System.Drawing.Size(120, 284);
            this.grpKmax.TabIndex = 0;
            this.grpKmax.TabStop = false;
            this.grpKmax.Text = "Ref. null";
            // 
            // cmdKmaxDown
            // 
            this.cmdKmaxDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdKmaxDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdKmaxDown.ForeColor = System.Drawing.Color.Black;
            this.cmdKmaxDown.Location = new System.Drawing.Point(46, 241);
            this.cmdKmaxDown.Name = "cmdKmaxDown";
            this.cmdKmaxDown.Size = new System.Drawing.Size(65, 37);
            this.cmdKmaxDown.TabIndex = 2;
            this.cmdKmaxDown.TabStop = false;
            this.cmdKmaxDown.Text = "-";
            this.cmdKmaxDown.UseVisualStyleBackColor = true;
            // 
            // cmdKmaxUp
            // 
            this.cmdKmaxUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdKmaxUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdKmaxUp.ForeColor = System.Drawing.Color.Black;
            this.cmdKmaxUp.Location = new System.Drawing.Point(46, 23);
            this.cmdKmaxUp.Name = "cmdKmaxUp";
            this.cmdKmaxUp.Size = new System.Drawing.Size(65, 37);
            this.cmdKmaxUp.TabIndex = 2;
            this.cmdKmaxUp.TabStop = false;
            this.cmdKmaxUp.Text = "+";
            this.cmdKmaxUp.UseVisualStyleBackColor = true;
            // 
            // lblKmax
            // 
            this.lblKmax.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblKmax.BackColor = System.Drawing.SystemColors.Window;
            this.lblKmax.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblKmax.ForeColor = System.Drawing.Color.Black;
            this.lblKmax.Location = new System.Drawing.Point(34, 126);
            this.lblKmax.Name = "lblKmax";
            this.lblKmax.Size = new System.Drawing.Size(77, 48);
            this.lblKmax.TabIndex = 1;
            this.lblKmax.Tag = "{0:F3}";
            this.lblKmax.Text = "###.#";
            this.lblKmax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numKmax
            // 
            this.numKmax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numKmax.Location = new System.Drawing.Point(8, 23);
            this.numKmax.Name = "numKmax";
            this.numKmax.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.numKmax.Size = new System.Drawing.Size(45, 255);
            this.numKmax.TabIndex = 0;
            // 
            // grpFilter
            // 
            this.grpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpFilter.Controls.Add(this.label2);
            this.grpFilter.Controls.Add(this.cmdEnum);
            this.grpFilter.Controls.Add(this.cmdLock);
            this.grpFilter.Controls.Add(this.numFilter);
            this.grpFilter.Location = new System.Drawing.Point(1499, 85);
            this.grpFilter.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.grpFilter.Name = "grpFilter";
            this.grpFilter.Size = new System.Drawing.Size(120, 31);
            this.grpFilter.TabIndex = 0;
            this.grpFilter.TabStop = false;
            this.grpFilter.Text = "Фильтр";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "c";
            // 
            // cmdEnum
            // 
            this.cmdEnum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdEnum.Appearance = System.Windows.Forms.Appearance.Button;
            this.cmdEnum.ForeColor = System.Drawing.Color.Black;
            this.cmdEnum.Location = new System.Drawing.Point(-94, 79);
            this.cmdEnum.Name = "cmdEnum";
            this.cmdEnum.Size = new System.Drawing.Size(125, 34);
            this.cmdEnum.TabIndex = 5;
            this.cmdEnum.Text = "2-й канал";
            this.cmdEnum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cmdEnum.UseVisualStyleBackColor = false;
            this.cmdEnum.CheckedChanged += new System.EventHandler(this.Control_CheckedChanged);
            // 
            // cmdLock
            // 
            this.cmdLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdLock.Appearance = System.Windows.Forms.Appearance.Button;
            this.cmdLock.Location = new System.Drawing.Point(-44, 52);
            this.cmdLock.Name = "cmdLock";
            this.cmdLock.Size = new System.Drawing.Size(125, 34);
            this.cmdLock.TabIndex = 3;
            this.cmdLock.Text = "Блокировка";
            this.cmdLock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cmdLock.UseVisualStyleBackColor = false;
            this.cmdLock.CheckedChanged += new System.EventHandler(this.Control_CheckedChanged);
            // 
            // numFilter
            // 
            this.numFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numFilter.Location = new System.Drawing.Point(13, 23);
            this.numFilter.Name = "numFilter";
            this.numFilter.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.numFilter.Size = new System.Drawing.Size(45, 2);
            this.numFilter.TabIndex = 0;
            // 
            // grpTP
            // 
            this.grpTP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpTP.Controls.Add(this.cmdTPDown);
            this.grpTP.Controls.Add(this.cmdTPDownSmall);
            this.grpTP.Controls.Add(this.cmdTPUpSmall);
            this.grpTP.Controls.Add(this.cmdTPUp);
            this.grpTP.Controls.Add(this.lblTP);
            this.grpTP.Controls.Add(this.numTP);
            this.grpTP.ForeColor = System.Drawing.Color.Blue;
            this.grpTP.Location = new System.Drawing.Point(484, 0);
            this.grpTP.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.grpTP.Name = "grpTP";
            this.grpTP.Size = new System.Drawing.Size(120, 284);
            this.grpTP.TabIndex = 0;
            this.grpTP.TabStop = false;
            this.grpTP.Text = "Pos. tresh.";
            // 
            // cmdTPDown
            // 
            this.cmdTPDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTPDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdTPDown.ForeColor = System.Drawing.Color.Black;
            this.cmdTPDown.Location = new System.Drawing.Point(31, 241);
            this.cmdTPDown.Name = "cmdTPDown";
            this.cmdTPDown.Size = new System.Drawing.Size(54, 37);
            this.cmdTPDown.TabIndex = 2;
            this.cmdTPDown.TabStop = false;
            this.cmdTPDown.Text = "-";
            this.cmdTPDown.UseVisualStyleBackColor = true;
            // 
            // cmdTPDownSmall
            // 
            this.cmdTPDownSmall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTPDownSmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdTPDownSmall.ForeColor = System.Drawing.Color.Black;
            this.cmdTPDownSmall.Location = new System.Drawing.Point(85, 241);
            this.cmdTPDownSmall.Name = "cmdTPDownSmall";
            this.cmdTPDownSmall.Size = new System.Drawing.Size(26, 37);
            this.cmdTPDownSmall.TabIndex = 2;
            this.cmdTPDownSmall.TabStop = false;
            this.cmdTPDownSmall.Text = "-";
            this.cmdTPDownSmall.UseVisualStyleBackColor = true;
            // 
            // cmdTPUpSmall
            // 
            this.cmdTPUpSmall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTPUpSmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdTPUpSmall.ForeColor = System.Drawing.Color.Black;
            this.cmdTPUpSmall.Location = new System.Drawing.Point(85, 23);
            this.cmdTPUpSmall.Name = "cmdTPUpSmall";
            this.cmdTPUpSmall.Size = new System.Drawing.Size(26, 37);
            this.cmdTPUpSmall.TabIndex = 2;
            this.cmdTPUpSmall.TabStop = false;
            this.cmdTPUpSmall.Text = "+";
            this.cmdTPUpSmall.UseVisualStyleBackColor = true;
            // 
            // cmdTPUp
            // 
            this.cmdTPUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTPUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdTPUp.ForeColor = System.Drawing.Color.Black;
            this.cmdTPUp.Location = new System.Drawing.Point(31, 23);
            this.cmdTPUp.Name = "cmdTPUp";
            this.cmdTPUp.Size = new System.Drawing.Size(54, 37);
            this.cmdTPUp.TabIndex = 2;
            this.cmdTPUp.TabStop = false;
            this.cmdTPUp.Text = "+";
            this.cmdTPUp.UseVisualStyleBackColor = true;
            // 
            // lblTP
            // 
            this.lblTP.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTP.BackColor = System.Drawing.SystemColors.Window;
            this.lblTP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTP.ForeColor = System.Drawing.Color.Black;
            this.lblTP.Location = new System.Drawing.Point(31, 126);
            this.lblTP.Name = "lblTP";
            this.lblTP.Size = new System.Drawing.Size(80, 48);
            this.lblTP.TabIndex = 1;
            this.lblTP.Tag = "{0:F3}";
            this.lblTP.Text = "###.##";
            this.lblTP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numTP
            // 
            this.numTP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numTP.Location = new System.Drawing.Point(13, 23);
            this.numTP.Name = "numTP";
            this.numTP.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.numTP.Size = new System.Drawing.Size(45, 255);
            this.numTP.TabIndex = 0;
            // 
            // grpTN
            // 
            this.grpTN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpTN.Controls.Add(this.cmdTNDown);
            this.grpTN.Controls.Add(this.cmdTNDownSmall);
            this.grpTN.Controls.Add(this.cmdTNUp);
            this.grpTN.Controls.Add(this.cmdTNUpSmall);
            this.grpTN.Controls.Add(this.lblTN);
            this.grpTN.Controls.Add(this.numTN);
            this.grpTN.ForeColor = System.Drawing.Color.Blue;
            this.grpTN.Location = new System.Drawing.Point(605, 0);
            this.grpTN.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.grpTN.Name = "grpTN";
            this.grpTN.Size = new System.Drawing.Size(120, 284);
            this.grpTN.TabIndex = 0;
            this.grpTN.TabStop = false;
            this.grpTN.Text = "Neg. tresh.";
            // 
            // cmdTNDown
            // 
            this.cmdTNDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTNDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdTNDown.ForeColor = System.Drawing.Color.Black;
            this.cmdTNDown.Location = new System.Drawing.Point(31, 241);
            this.cmdTNDown.Name = "cmdTNDown";
            this.cmdTNDown.Size = new System.Drawing.Size(54, 37);
            this.cmdTNDown.TabIndex = 2;
            this.cmdTNDown.TabStop = false;
            this.cmdTNDown.Text = "-";
            this.cmdTNDown.UseVisualStyleBackColor = true;
            // 
            // cmdTNDownSmall
            // 
            this.cmdTNDownSmall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTNDownSmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdTNDownSmall.ForeColor = System.Drawing.Color.Black;
            this.cmdTNDownSmall.Location = new System.Drawing.Point(85, 241);
            this.cmdTNDownSmall.Name = "cmdTNDownSmall";
            this.cmdTNDownSmall.Size = new System.Drawing.Size(26, 37);
            this.cmdTNDownSmall.TabIndex = 2;
            this.cmdTNDownSmall.TabStop = false;
            this.cmdTNDownSmall.Text = "-";
            this.cmdTNDownSmall.UseVisualStyleBackColor = true;
            // 
            // cmdTNUp
            // 
            this.cmdTNUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTNUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdTNUp.ForeColor = System.Drawing.Color.Black;
            this.cmdTNUp.Location = new System.Drawing.Point(31, 23);
            this.cmdTNUp.Name = "cmdTNUp";
            this.cmdTNUp.Size = new System.Drawing.Size(54, 37);
            this.cmdTNUp.TabIndex = 2;
            this.cmdTNUp.TabStop = false;
            this.cmdTNUp.Text = "+";
            this.cmdTNUp.UseVisualStyleBackColor = true;
            // 
            // cmdTNUpSmall
            // 
            this.cmdTNUpSmall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTNUpSmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdTNUpSmall.ForeColor = System.Drawing.Color.Black;
            this.cmdTNUpSmall.Location = new System.Drawing.Point(85, 23);
            this.cmdTNUpSmall.Name = "cmdTNUpSmall";
            this.cmdTNUpSmall.Size = new System.Drawing.Size(26, 37);
            this.cmdTNUpSmall.TabIndex = 2;
            this.cmdTNUpSmall.TabStop = false;
            this.cmdTNUpSmall.Text = "+";
            this.cmdTNUpSmall.UseVisualStyleBackColor = true;
            // 
            // lblTN
            // 
            this.lblTN.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTN.BackColor = System.Drawing.SystemColors.Window;
            this.lblTN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTN.ForeColor = System.Drawing.Color.Black;
            this.lblTN.Location = new System.Drawing.Point(31, 126);
            this.lblTN.Name = "lblTN";
            this.lblTN.Size = new System.Drawing.Size(80, 48);
            this.lblTN.TabIndex = 1;
            this.lblTN.Tag = "{0:F3}";
            this.lblTN.Text = "###.#";
            this.lblTN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numTN
            // 
            this.numTN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numTN.Location = new System.Drawing.Point(13, 23);
            this.numTN.Name = "numTN";
            this.numTN.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.numTN.Size = new System.Drawing.Size(45, 255);
            this.numTN.TabIndex = 0;
            // 
            // grpDN
            // 
            this.grpDN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpDN.Controls.Add(this.cmdDNDOwn);
            this.grpDN.Controls.Add(this.cmdDNDownSmall);
            this.grpDN.Controls.Add(this.cmdDNUp);
            this.grpDN.Controls.Add(this.cmdDNUpSmall);
            this.grpDN.Controls.Add(this.lblDN);
            this.grpDN.Controls.Add(this.numDN);
            this.grpDN.ForeColor = System.Drawing.Color.Red;
            this.grpDN.Location = new System.Drawing.Point(847, 0);
            this.grpDN.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.grpDN.Name = "grpDN";
            this.grpDN.Size = new System.Drawing.Size(120, 284);
            this.grpDN.TabIndex = 0;
            this.grpDN.TabStop = false;
            this.grpDN.Text = "Neg. tr. dang.";
            // 
            // cmdDNDOwn
            // 
            this.cmdDNDOwn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDNDOwn.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdDNDOwn.ForeColor = System.Drawing.Color.Black;
            this.cmdDNDOwn.Location = new System.Drawing.Point(31, 241);
            this.cmdDNDOwn.Name = "cmdDNDOwn";
            this.cmdDNDOwn.Size = new System.Drawing.Size(54, 37);
            this.cmdDNDOwn.TabIndex = 2;
            this.cmdDNDOwn.TabStop = false;
            this.cmdDNDOwn.Text = "-";
            this.cmdDNDOwn.UseVisualStyleBackColor = true;
            // 
            // cmdDNDownSmall
            // 
            this.cmdDNDownSmall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDNDownSmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdDNDownSmall.ForeColor = System.Drawing.Color.Black;
            this.cmdDNDownSmall.Location = new System.Drawing.Point(85, 241);
            this.cmdDNDownSmall.Name = "cmdDNDownSmall";
            this.cmdDNDownSmall.Size = new System.Drawing.Size(26, 37);
            this.cmdDNDownSmall.TabIndex = 2;
            this.cmdDNDownSmall.TabStop = false;
            this.cmdDNDownSmall.Text = "-";
            this.cmdDNDownSmall.UseVisualStyleBackColor = true;
            // 
            // cmdDNUp
            // 
            this.cmdDNUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDNUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdDNUp.ForeColor = System.Drawing.Color.Black;
            this.cmdDNUp.Location = new System.Drawing.Point(31, 23);
            this.cmdDNUp.Name = "cmdDNUp";
            this.cmdDNUp.Size = new System.Drawing.Size(54, 37);
            this.cmdDNUp.TabIndex = 2;
            this.cmdDNUp.TabStop = false;
            this.cmdDNUp.Text = "+";
            this.cmdDNUp.UseVisualStyleBackColor = true;
            // 
            // cmdDNUpSmall
            // 
            this.cmdDNUpSmall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDNUpSmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdDNUpSmall.ForeColor = System.Drawing.Color.Black;
            this.cmdDNUpSmall.Location = new System.Drawing.Point(85, 23);
            this.cmdDNUpSmall.Name = "cmdDNUpSmall";
            this.cmdDNUpSmall.Size = new System.Drawing.Size(26, 37);
            this.cmdDNUpSmall.TabIndex = 2;
            this.cmdDNUpSmall.TabStop = false;
            this.cmdDNUpSmall.Text = "+";
            this.cmdDNUpSmall.UseVisualStyleBackColor = true;
            // 
            // lblDN
            // 
            this.lblDN.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDN.BackColor = System.Drawing.SystemColors.Window;
            this.lblDN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDN.ForeColor = System.Drawing.Color.Black;
            this.lblDN.Location = new System.Drawing.Point(31, 126);
            this.lblDN.Name = "lblDN";
            this.lblDN.Size = new System.Drawing.Size(80, 48);
            this.lblDN.TabIndex = 1;
            this.lblDN.Tag = "{0:F3}";
            this.lblDN.Text = "###.#";
            this.lblDN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numDN
            // 
            this.numDN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numDN.Location = new System.Drawing.Point(13, 23);
            this.numDN.Name = "numDN";
            this.numDN.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.numDN.Size = new System.Drawing.Size(45, 255);
            this.numDN.TabIndex = 0;
            // 
            // grpDP
            // 
            this.grpDP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpDP.Controls.Add(this.cmdDPDown);
            this.grpDP.Controls.Add(this.cmdDPDownSmall);
            this.grpDP.Controls.Add(this.cmdDPUpSmall);
            this.grpDP.Controls.Add(this.cmdDPUp);
            this.grpDP.Controls.Add(this.lblDP);
            this.grpDP.Controls.Add(this.numDP);
            this.grpDP.ForeColor = System.Drawing.Color.Red;
            this.grpDP.Location = new System.Drawing.Point(726, 0);
            this.grpDP.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.grpDP.Name = "grpDP";
            this.grpDP.Size = new System.Drawing.Size(120, 284);
            this.grpDP.TabIndex = 0;
            this.grpDP.TabStop = false;
            this.grpDP.Text = "Pos. tr. dang.";
            // 
            // cmdDPDown
            // 
            this.cmdDPDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDPDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdDPDown.ForeColor = System.Drawing.Color.Black;
            this.cmdDPDown.Location = new System.Drawing.Point(31, 241);
            this.cmdDPDown.Name = "cmdDPDown";
            this.cmdDPDown.Size = new System.Drawing.Size(54, 37);
            this.cmdDPDown.TabIndex = 2;
            this.cmdDPDown.TabStop = false;
            this.cmdDPDown.Text = "-";
            this.cmdDPDown.UseVisualStyleBackColor = true;
            // 
            // cmdDPDownSmall
            // 
            this.cmdDPDownSmall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDPDownSmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdDPDownSmall.ForeColor = System.Drawing.Color.Black;
            this.cmdDPDownSmall.Location = new System.Drawing.Point(85, 241);
            this.cmdDPDownSmall.Name = "cmdDPDownSmall";
            this.cmdDPDownSmall.Size = new System.Drawing.Size(26, 37);
            this.cmdDPDownSmall.TabIndex = 2;
            this.cmdDPDownSmall.TabStop = false;
            this.cmdDPDownSmall.Text = "-";
            this.cmdDPDownSmall.UseVisualStyleBackColor = true;
            // 
            // cmdDPUpSmall
            // 
            this.cmdDPUpSmall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDPUpSmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdDPUpSmall.ForeColor = System.Drawing.Color.Black;
            this.cmdDPUpSmall.Location = new System.Drawing.Point(85, 23);
            this.cmdDPUpSmall.Name = "cmdDPUpSmall";
            this.cmdDPUpSmall.Size = new System.Drawing.Size(26, 37);
            this.cmdDPUpSmall.TabIndex = 2;
            this.cmdDPUpSmall.TabStop = false;
            this.cmdDPUpSmall.Text = "+";
            this.cmdDPUpSmall.UseVisualStyleBackColor = true;
            // 
            // cmdDPUp
            // 
            this.cmdDPUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDPUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdDPUp.ForeColor = System.Drawing.Color.Black;
            this.cmdDPUp.Location = new System.Drawing.Point(31, 23);
            this.cmdDPUp.Name = "cmdDPUp";
            this.cmdDPUp.Size = new System.Drawing.Size(54, 37);
            this.cmdDPUp.TabIndex = 2;
            this.cmdDPUp.TabStop = false;
            this.cmdDPUp.Text = "+";
            this.cmdDPUp.UseVisualStyleBackColor = true;
            // 
            // lblDP
            // 
            this.lblDP.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDP.BackColor = System.Drawing.SystemColors.Window;
            this.lblDP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDP.ForeColor = System.Drawing.Color.Black;
            this.lblDP.Location = new System.Drawing.Point(31, 126);
            this.lblDP.Name = "lblDP";
            this.lblDP.Size = new System.Drawing.Size(80, 48);
            this.lblDP.TabIndex = 1;
            this.lblDP.Tag = "{0:F3}";
            this.lblDP.Text = "###.##";
            this.lblDP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numDP
            // 
            this.numDP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numDP.Location = new System.Drawing.Point(13, 23);
            this.numDP.Name = "numDP";
            this.numDP.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.numDP.Size = new System.Drawing.Size(45, 255);
            this.numDP.TabIndex = 0;
            // 
            // grpEcm
            // 
            this.grpEcm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpEcm.Controls.Add(this.cmdEcmDown);
            this.grpEcm.Controls.Add(this.cmdEcmUp);
            this.grpEcm.Controls.Add(this.lblEcm);
            this.grpEcm.Controls.Add(this.numEcm);
            this.grpEcm.Location = new System.Drawing.Point(1212, 3);
            this.grpEcm.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.grpEcm.Name = "grpEcm";
            this.grpEcm.Size = new System.Drawing.Size(120, 284);
            this.grpEcm.TabIndex = 0;
            this.grpEcm.TabStop = false;
            this.grpEcm.Text = "Shunt tresh.";
            // 
            // cmdEcmDown
            // 
            this.cmdEcmDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdEcmDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdEcmDown.Location = new System.Drawing.Point(46, 241);
            this.cmdEcmDown.Name = "cmdEcmDown";
            this.cmdEcmDown.Size = new System.Drawing.Size(64, 37);
            this.cmdEcmDown.TabIndex = 2;
            this.cmdEcmDown.TabStop = false;
            this.cmdEcmDown.Text = "-";
            this.cmdEcmDown.UseVisualStyleBackColor = true;
            // 
            // cmdEcmUp
            // 
            this.cmdEcmUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdEcmUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdEcmUp.Location = new System.Drawing.Point(47, 23);
            this.cmdEcmUp.Name = "cmdEcmUp";
            this.cmdEcmUp.Size = new System.Drawing.Size(65, 37);
            this.cmdEcmUp.TabIndex = 2;
            this.cmdEcmUp.TabStop = false;
            this.cmdEcmUp.Text = "+";
            this.cmdEcmUp.UseVisualStyleBackColor = true;
            // 
            // lblEcm
            // 
            this.lblEcm.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblEcm.BackColor = System.Drawing.SystemColors.Window;
            this.lblEcm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblEcm.Location = new System.Drawing.Point(34, 126);
            this.lblEcm.Name = "lblEcm";
            this.lblEcm.Size = new System.Drawing.Size(77, 48);
            this.lblEcm.TabIndex = 1;
            this.lblEcm.Tag = "{0:F2}";
            this.lblEcm.Text = "###.#";
            this.lblEcm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numEcm
            // 
            this.numEcm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numEcm.Location = new System.Drawing.Point(8, 23);
            this.numEcm.Name = "numEcm";
            this.numEcm.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.numEcm.Size = new System.Drawing.Size(45, 255);
            this.numEcm.TabIndex = 0;
            // 
            // grpF
            // 
            this.grpF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpF.Controls.Add(this.cmdFDown);
            this.grpF.Controls.Add(this.cmdFUp);
            this.grpF.Controls.Add(this.lblF);
            this.grpF.Controls.Add(this.numF);
            this.grpF.Location = new System.Drawing.Point(8, 0);
            this.grpF.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.grpF.Name = "grpF";
            this.grpF.Size = new System.Drawing.Size(120, 284);
            this.grpF.TabIndex = 0;
            this.grpF.TabStop = false;
            this.grpF.Text = "Shunt time";
            // 
            // cmdFDown
            // 
            this.cmdFDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdFDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdFDown.Location = new System.Drawing.Point(46, 241);
            this.cmdFDown.Name = "cmdFDown";
            this.cmdFDown.Size = new System.Drawing.Size(66, 37);
            this.cmdFDown.TabIndex = 2;
            this.cmdFDown.TabStop = false;
            this.cmdFDown.Text = "-";
            this.cmdFDown.UseVisualStyleBackColor = true;
            // 
            // cmdFUp
            // 
            this.cmdFUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdFUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdFUp.Location = new System.Drawing.Point(45, 23);
            this.cmdFUp.Name = "cmdFUp";
            this.cmdFUp.Size = new System.Drawing.Size(65, 37);
            this.cmdFUp.TabIndex = 2;
            this.cmdFUp.TabStop = false;
            this.cmdFUp.Text = "+";
            this.cmdFUp.UseVisualStyleBackColor = true;
            // 
            // lblF
            // 
            this.lblF.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblF.BackColor = System.Drawing.SystemColors.Window;
            this.lblF.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblF.Location = new System.Drawing.Point(34, 126);
            this.lblF.Name = "lblF";
            this.lblF.Size = new System.Drawing.Size(77, 48);
            this.lblF.TabIndex = 1;
            this.lblF.Tag = "{0:F1}";
            this.lblF.Text = "###.#";
            this.lblF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numF
            // 
            this.numF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numF.Location = new System.Drawing.Point(8, 23);
            this.numF.Name = "numF";
            this.numF.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.numF.Size = new System.Drawing.Size(45, 255);
            this.numF.TabIndex = 0;
            // 
            // menu_
            // 
            this.menu_.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuConnect,
            this.menuView,
            this.menuWrite});
            this.menu_.Location = new System.Drawing.Point(0, 0);
            this.menu_.Name = "menu_";
            this.menu_.Size = new System.Drawing.Size(1908, 24);
            this.menu_.TabIndex = 1;
            this.menu_.Text = "menu";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFOpen,
            this.menuFSave,
            this.menuF_1,
            this.menuFImport,
            this.menuFExport,
            this.menuF_2,
            this.menuFTerminate});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(48, 20);
            this.menuFile.Text = "Файл";
            this.menuFile.DropDownOpening += new System.EventHandler(this.menuFile_DropDownOpening);
            // 
            // menuFOpen
            // 
            this.menuFOpen.Name = "menuFOpen";
            this.menuFOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuFOpen.Size = new System.Drawing.Size(237, 22);
            this.menuFOpen.Text = "Открыть...";
            this.menuFOpen.Click += new System.EventHandler(this.menuFOpen_Click);
            // 
            // menuFSave
            // 
            this.menuFSave.Name = "menuFSave";
            this.menuFSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuFSave.Size = new System.Drawing.Size(237, 22);
            this.menuFSave.Text = "Сохранить...";
            this.menuFSave.Click += new System.EventHandler(this.menuFSave_Click);
            // 
            // menuF_1
            // 
            this.menuF_1.Name = "menuF_1";
            this.menuF_1.Size = new System.Drawing.Size(234, 6);
            // 
            // menuFImport
            // 
            this.menuFImport.Name = "menuFImport";
            this.menuFImport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.menuFImport.Size = new System.Drawing.Size(237, 22);
            this.menuFImport.Text = "Импорт параметров...";
            this.menuFImport.Click += new System.EventHandler(this.menuFImport_Click);
            // 
            // menuFExport
            // 
            this.menuFExport.Name = "menuFExport";
            this.menuFExport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.menuFExport.Size = new System.Drawing.Size(237, 22);
            this.menuFExport.Text = "Экспорт параметров...";
            this.menuFExport.Click += new System.EventHandler(this.menuFExport_Click);
            // 
            // menuF_2
            // 
            this.menuF_2.Name = "menuF_2";
            this.menuF_2.Size = new System.Drawing.Size(234, 6);
            // 
            // menuFTerminate
            // 
            this.menuFTerminate.Name = "menuFTerminate";
            this.menuFTerminate.Size = new System.Drawing.Size(237, 22);
            this.menuFTerminate.Text = "Выход";
            this.menuFTerminate.Click += new System.EventHandler(this.menuFTerminate_Click);
            // 
            // menuConnect
            // 
            this.menuConnect.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCSocket,
            this.menuCSerialPort,
            this.menuC_Delay,
            this.menuC_1,
            this.menuCDisconnect,
            this.menuC_Bottom});
            this.menuConnect.Name = "menuConnect";
            this.menuConnect.Size = new System.Drawing.Size(97, 20);
            this.menuConnect.Text = "Подключение";
            this.menuConnect.DropDownOpening += new System.EventHandler(this.menuConnect_DropDownOpening);
            // 
            // menuCSocket
            // 
            this.menuCSocket.Name = "menuCSocket";
            this.menuCSocket.Size = new System.Drawing.Size(210, 22);
            this.menuCSocket.Text = "Ethernet";
            this.menuCSocket.Click += new System.EventHandler(this.menuCSocket_Click);
            // 
            // menuCSerialPort
            // 
            this.menuCSerialPort.Name = "menuCSerialPort";
            this.menuCSerialPort.Size = new System.Drawing.Size(210, 22);
            this.menuCSerialPort.Text = "Последовательный порт";
            this.menuCSerialPort.Click += new System.EventHandler(this.menuCSerialPort_Click);
            // 
            // menuC_Delay
            // 
            this.menuC_Delay.Name = "menuC_Delay";
            this.menuC_Delay.Size = new System.Drawing.Size(210, 22);
            this.menuC_Delay.Text = "Задержка запросов...";
            this.menuC_Delay.Visible = false;
            this.menuC_Delay.Click += new System.EventHandler(this.menuC_Delay_Click);
            // 
            // menuC_1
            // 
            this.menuC_1.Name = "menuC_1";
            this.menuC_1.Size = new System.Drawing.Size(207, 6);
            // 
            // menuCDisconnect
            // 
            this.menuCDisconnect.Name = "menuCDisconnect";
            this.menuCDisconnect.Size = new System.Drawing.Size(210, 22);
            this.menuCDisconnect.Text = "Отключить";
            this.menuCDisconnect.Click += new System.EventHandler(this.menuCDisconnect_Click);
            // 
            // menuC_Bottom
            // 
            this.menuC_Bottom.Name = "menuC_Bottom";
            this.menuC_Bottom.Size = new System.Drawing.Size(207, 6);
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuV_Panel,
            this.menuV_1,
            this.menuVDebugConsole,
            this.menuV_2,
            this.menuVSettings});
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size(39, 20);
            this.menuView.Text = "Вид";
            this.menuView.DropDownOpening += new System.EventHandler(this.menuView_DropDownOpening);
            // 
            // menuV_Panel
            // 
            this.menuV_Panel.Name = "menuV_Panel";
            this.menuV_Panel.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.menuV_Panel.Size = new System.Drawing.Size(202, 22);
            this.menuV_Panel.Text = "Панель управления";
            this.menuV_Panel.Click += new System.EventHandler(this.menuV_Panel_Click);
            // 
            // menuV_1
            // 
            this.menuV_1.Name = "menuV_1";
            this.menuV_1.Size = new System.Drawing.Size(199, 6);
            // 
            // menuVDebugConsole
            // 
            this.menuVDebugConsole.Name = "menuVDebugConsole";
            this.menuVDebugConsole.Size = new System.Drawing.Size(202, 22);
            this.menuVDebugConsole.Text = "Консоль отладки";
            this.menuVDebugConsole.Click += new System.EventHandler(this.menuVDebugConsole_Click);
            // 
            // menuV_2
            // 
            this.menuV_2.Name = "menuV_2";
            this.menuV_2.Size = new System.Drawing.Size(199, 6);
            // 
            // menuVSettings
            // 
            this.menuVSettings.Name = "menuVSettings";
            this.menuVSettings.Size = new System.Drawing.Size(202, 22);
            this.menuVSettings.Text = "Настройки...";
            this.menuVSettings.Click += new System.EventHandler(this.menuVSettings_Click);
            // 
            // menuWrite
            // 
            this.menuWrite.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuWWrite,
            this.toolStripMenuItem1,
            this.menuWHistory});
            this.menuWrite.Name = "menuWrite";
            this.menuWrite.Size = new System.Drawing.Size(92, 20);
            this.menuWrite.Text = "Запись в ГСА";
            // 
            // menuWWrite
            // 
            this.menuWWrite.Name = "menuWWrite";
            this.menuWWrite.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.menuWWrite.Size = new System.Drawing.Size(220, 22);
            this.menuWWrite.Text = "Запись...";
            this.menuWWrite.Click += new System.EventHandler(this.menuWWrite_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(217, 6);
            // 
            // menuWHistory
            // 
            this.menuWHistory.Name = "menuWHistory";
            this.menuWHistory.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.menuWHistory.Size = new System.Drawing.Size(220, 22);
            this.menuWHistory.Text = "История записей...";
            this.menuWHistory.Click += new System.EventHandler(this.menuWHistory_Click);
            // 
            // statusBottom
            // 
            this.statusBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status,
            this.stMD5,
            this.stID,
            this.stStep,
            this.stErrors,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusBottom.Location = new System.Drawing.Point(0, 716);
            this.statusBottom.Name = "statusBottom";
            this.statusBottom.Size = new System.Drawing.Size(1908, 24);
            this.statusBottom.TabIndex = 2;
            this.statusBottom.Text = "%";
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(1238, 19);
            this.status.Spring = true;
            this.status.Text = "<^>";
            this.status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stMD5
            // 
            this.stMD5.Name = "stMD5";
            this.stMD5.Size = new System.Drawing.Size(156, 19);
            this.stMD5.Text = "MD5:####:####:####:####";
            // 
            // stID
            // 
            this.stID.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.stID.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.stID.Name = "stID";
            this.stID.Size = new System.Drawing.Size(95, 19);
            this.stID.Text = "############";
            // 
            // stStep
            // 
            this.stStep.AutoSize = false;
            this.stStep.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.stStep.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.stStep.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.stStep.Name = "stStep";
            this.stStep.Size = new System.Drawing.Size(48, 19);
            this.stStep.Text = "---";
            // 
            // stErrors
            // 
            this.stErrors.AutoSize = false;
            this.stErrors.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.stErrors.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.stErrors.Image = ((System.Drawing.Image)(resources.GetObject("stErrors.Image")));
            this.stErrors.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.stErrors.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.stErrors.ImageTransparentColor = System.Drawing.Color.White;
            this.stErrors.Name = "stErrors";
            this.stErrors.Size = new System.Drawing.Size(120, 19);
            this.stErrors.Text = "Ошибок: 0";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // COM_thing
            // 
            this.COM_thing.BaudRate = 115200;
            this.COM_thing.ReadTimeout = 250;
            this.COM_thing.WriteTimeout = 500;
            this.COM_thing.ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(this.COM_thing_ErrorReceived);
            // 
            // poller
            // 
            this.poller.Enabled = true;
            this.poller.Interval = 500;
            this.poller.Tick += new System.EventHandler(this.poller_Tick);
            // 
            // flasher
            // 
            this.flasher.Enabled = true;
            this.flasher.Interval = 300;
            this.flasher.Tick += new System.EventHandler(this.flasher_Tick);
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1908, 740);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.menu_);
            this.Controls.Add(this.statusBottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "formMain";
            this.Text = "АИГ2";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formMain_FormClosing);
            this.splitter.Panel1.ResumeLayout(false);
            this.splitter.Panel1.PerformLayout();
            this.splitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
            this.splitter.ResumeLayout(false);
            this.scroller.ResumeLayout(false);
            this.scroller.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numK2max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).EndInit();
            this.grpK2max.ResumeLayout(false);
            this.grpY.ResumeLayout(false);
            this.grpY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Zone_timer)).EndInit();
            this.grpUm.ResumeLayout(false);
            this.grpUm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUm)).EndInit();
            this.grpKmax.ResumeLayout(false);
            this.grpKmax.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numKmax)).EndInit();
            this.grpFilter.ResumeLayout(false);
            this.grpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFilter)).EndInit();
            this.grpTP.ResumeLayout(false);
            this.grpTP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTP)).EndInit();
            this.grpTN.ResumeLayout(false);
            this.grpTN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTN)).EndInit();
            this.grpDN.ResumeLayout(false);
            this.grpDN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDN)).EndInit();
            this.grpDP.ResumeLayout(false);
            this.grpDP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDP)).EndInit();
            this.grpEcm.ResumeLayout(false);
            this.grpEcm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEcm)).EndInit();
            this.grpF.ResumeLayout(false);
            this.grpF.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numF)).EndInit();
            this.menu_.ResumeLayout(false);
            this.menu_.PerformLayout();
            this.statusBottom.ResumeLayout(false);
            this.statusBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitter;
        private System.Windows.Forms.Button cmdStartStop;
        private System.Windows.Forms.Panel scroller;
        private System.Windows.Forms.MenuStrip menu_;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFOpen;
        private System.Windows.Forms.ToolStripMenuItem menuFSave;
        private System.Windows.Forms.ToolStripSeparator menuF_1;
        private System.Windows.Forms.ToolStripMenuItem menuFImport;
        private System.Windows.Forms.ToolStripMenuItem menuFExport;
        private System.Windows.Forms.ToolStripSeparator menuF_2;
        private System.Windows.Forms.ToolStripMenuItem menuFTerminate;
        private System.Windows.Forms.ToolStripMenuItem menuConnect;
        private System.Windows.Forms.ToolStripMenuItem menuCSocket;
        private System.Windows.Forms.ToolStripMenuItem menuCSerialPort;
        private System.Windows.Forms.ToolStripSeparator menuC_1;
        private System.Windows.Forms.ToolStripMenuItem menuCDisconnect;
        private System.Windows.Forms.ToolStripSeparator menuC_Bottom;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem menuV_Panel;
        private System.Windows.Forms.ToolStripSeparator menuV_1;
        private System.Windows.Forms.ToolStripMenuItem menuVSettings;
        private System.Windows.Forms.ToolStripMenuItem menuWrite;
        private System.Windows.Forms.ToolStripMenuItem menuWWrite;
        private System.Windows.Forms.ToolStripMenuItem menuVDebugConsole;
        private System.Windows.Forms.ToolStripSeparator menuV_2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuWHistory;
        private System.Windows.Forms.StatusStrip statusBottom;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.ToolStripStatusLabel stStep;
        private System.Windows.Forms.ToolStripStatusLabel stErrors;
        private System.IO.Ports.SerialPort COM_thing;
        private ZedGraph.ZedGraphControl graph;
        private System.Windows.Forms.ToolStripMenuItem menuC_Delay;
        private System.Windows.Forms.Timer poller;
        private System.Windows.Forms.Label InputEcm;
        private System.Windows.Forms.Label inputF;
        private System.Windows.Forms.Label inputUm;
        private System.Windows.Forms.Label lblDangerPositive;
        private System.Windows.Forms.Label lblDangerNegative;
        private System.Windows.Forms.GroupBox grpUm;
        private System.Windows.Forms.Button cmdUmDown;
        private System.Windows.Forms.Button cmdUmUp;
        private System.Windows.Forms.Label lblUm;
        private System.Windows.Forms.TrackBar numUm;
        private System.Windows.Forms.GroupBox grpTP;
        private System.Windows.Forms.Button cmdTPDown;
        private System.Windows.Forms.Button cmdTPUp;
        private System.Windows.Forms.Label lblTP;
        private System.Windows.Forms.TrackBar numTP;
        private System.Windows.Forms.GroupBox grpEcm;
        private System.Windows.Forms.Button cmdEcmDown;
        private System.Windows.Forms.Button cmdEcmUp;
        private System.Windows.Forms.Label lblEcm;
        private System.Windows.Forms.TrackBar numEcm;
        private System.Windows.Forms.GroupBox grpF;
        private System.Windows.Forms.Button cmdFDown;
        private System.Windows.Forms.Button cmdFUp;
        private System.Windows.Forms.Label lblF;
        private System.Windows.Forms.TrackBar numF;
        private System.Windows.Forms.GroupBox grpTN;
        private System.Windows.Forms.Button cmdTNDown;
        private System.Windows.Forms.Button cmdTNUp;
        private System.Windows.Forms.Label lblTN;
        private System.Windows.Forms.TrackBar numTN;
        private System.Windows.Forms.Button cmdTNDownSmall;
        private System.Windows.Forms.Button cmdTNUpSmall;
        private System.Windows.Forms.Button cmdTPDownSmall;
        private System.Windows.Forms.Button cmdTPUpSmall;
        private System.Windows.Forms.Timer flasher;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox grpY;
        private System.Windows.Forms.Button cmdYDown;
        private System.Windows.Forms.Button cmdYUp;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.TrackBar numY;
        private System.Windows.Forms.GroupBox grpKmax;
        private System.Windows.Forms.Button cmdKmaxDown;
        private System.Windows.Forms.Button cmdKmaxUp;
        private System.Windows.Forms.Label lblKmax;
        private System.Windows.Forms.TrackBar numKmax;
        private System.Windows.Forms.Label lblDespair;
        private System.Windows.Forms.CheckBox cmdFilter;
        private System.Windows.Forms.CheckBox cmdKmax;
        private System.Windows.Forms.CheckBox cmdWrite;
        private System.Windows.Forms.Label lblMaterial;
        private System.Windows.Forms.Label inputT;
        private System.Windows.Forms.GroupBox grpFilter;
        private System.Windows.Forms.Button cmdFilterDown;
        private System.Windows.Forms.Button cmdFilterUp;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.TrackBar numFilter;
        private System.Windows.Forms.GroupBox grpDN;
        private System.Windows.Forms.Button cmdDNDOwn;
        private System.Windows.Forms.Button cmdDNDownSmall;
        private System.Windows.Forms.Button cmdDNUp;
        private System.Windows.Forms.Button cmdDNUpSmall;
        private System.Windows.Forms.Label lblDN;
        private System.Windows.Forms.TrackBar numDN;
        private System.Windows.Forms.GroupBox grpDP;
        private System.Windows.Forms.Button cmdDPDown;
        private System.Windows.Forms.Button cmdDPDownSmall;
        private System.Windows.Forms.Button cmdDPUpSmall;
        private System.Windows.Forms.Button cmdDPUp;
        private System.Windows.Forms.Label lblDP;
        private System.Windows.Forms.TrackBar numDP;
        private System.Windows.Forms.ToolStripStatusLabel stID;
        private System.Windows.Forms.Label lblReady;
        private System.Windows.Forms.CheckBox cmdLock;
        private System.Windows.Forms.ToolStripStatusLabel stMD5;
        private System.Windows.Forms.CheckBox cmdEnum;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.GroupBox grpK2max;
        private System.Windows.Forms.Button cmdK2maxDown;
        private System.Windows.Forms.Button cmdK2maxUp;
        private System.Windows.Forms.Label lblK2max;
        private System.Windows.Forms.TrackBar numK2max;
        private System.Windows.Forms.CheckBox cmdControl;
        private System.Windows.Forms.ComboBox StateSelector;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar Zone_timer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Zonelbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdZone_timerDown;
        private System.Windows.Forms.Button cmdZone_timerUp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox select_addr;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

