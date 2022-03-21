
namespace Asistencia
{
    partial class MainScreen
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tActualizar = new System.Windows.Forms.Timer(this.components);
            this.Reloj = new System.Windows.Forms.Timer(this.components);
            this.Render = new System.Windows.Forms.Timer(this.components);
            this.bannerStart = new System.Windows.Forms.Timer(this.components);
            this.lblBackReloj = new System.Windows.Forms.Label();
            this.lblMensaje = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblEmpresa = new System.Windows.Forms.Label();
            this.txtArea = new System.Windows.Forms.Label();
            this.txtNoEmpleado = new System.Windows.Forms.Label();
            this.txtCargo = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.Label();
            this.EmpleadoFoto = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblAsistencia = new System.Windows.Forms.Label();
            this.lblRegistro = new System.Windows.Forms.Label();
            this.lblTipo = new System.Windows.Forms.Label();
            this.lblHorario = new System.Windows.Forms.Label();
            this.lblReloj = new System.Windows.Forms.Label();
            this.Enviar = new System.Windows.Forms.Timer(this.components);
            this.torniquete = new System.Windows.Forms.Timer(this.components);
            this.RelojView = new System.Windows.Forms.Timer(this.components);
            this.NFC_stop = new System.Windows.Forms.Timer(this.components);
            this.RenderTexto = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker4 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EmpleadoFoto)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // tActualizar
            // 
            this.tActualizar.Enabled = true;
            this.tActualizar.Interval = 480000;
            this.tActualizar.Tick += new System.EventHandler(this.tActualizar_Tick);
            // 
            // Reloj
            // 
            this.Reloj.Enabled = true;
            this.Reloj.Interval = 500;
            this.Reloj.Tick += new System.EventHandler(this.Reloj_Tick);
            // 
            // Render
            // 
            this.Render.Enabled = true;
            this.Render.Interval = 60000;
            this.Render.Tick += new System.EventHandler(this.Render_Tick);
            // 
            // bannerStart
            // 
            this.bannerStart.Interval = 4000;
            this.bannerStart.Tick += new System.EventHandler(this.bannerStart_Tick);
            // 
            // lblBackReloj
            // 
            this.lblBackReloj.AutoSize = true;
            this.lblBackReloj.BackColor = System.Drawing.Color.Transparent;
            this.lblBackReloj.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBackReloj.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblBackReloj.Location = new System.Drawing.Point(773, 532);
            this.lblBackReloj.MinimumSize = new System.Drawing.Size(373, 89);
            this.lblBackReloj.Name = "lblBackReloj";
            this.lblBackReloj.Size = new System.Drawing.Size(373, 91);
            this.lblBackReloj.TabIndex = 7;
            this.lblBackReloj.Text = "12:59:59";
            // 
            // lblMensaje
            // 
            this.lblMensaje.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblMensaje.Enabled = false;
            this.lblMensaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblMensaje.ImeMode = System.Windows.Forms.ImeMode.On;
            this.lblMensaje.Location = new System.Drawing.Point(0, 1152);
            this.lblMensaje.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblMensaje.MinimumSize = new System.Drawing.Size(1920, 32);
            this.lblMensaje.Multiline = true;
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(1920, 60);
            this.lblMensaje.TabIndex = 8;
            this.lblMensaje.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.lblEmpresa);
            this.groupBox1.Controls.Add(this.txtArea);
            this.groupBox1.Controls.Add(this.txtNoEmpleado);
            this.groupBox1.Controls.Add(this.txtCargo);
            this.groupBox1.Controls.Add(this.txtNombre);
            this.groupBox1.Controls.Add(this.EmpleadoFoto);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(11, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(939, 1301);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Empleado";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEmpresa.AutoSize = true;
            this.lblEmpresa.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmpresa.Location = new System.Drawing.Point(155, 1004);
            this.lblEmpresa.MinimumSize = new System.Drawing.Size(444, 39);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(604, 42);
            this.lblEmpresa.TabIndex = 9;
            this.lblEmpresa.Text = "Suinpac SA de SV (Administración)";
            this.lblEmpresa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtArea
            // 
            this.txtArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtArea.AutoSize = true;
            this.txtArea.BackColor = System.Drawing.Color.AliceBlue;
            this.txtArea.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArea.Location = new System.Drawing.Point(89, 866);
            this.txtArea.MinimumSize = new System.Drawing.Size(747, 39);
            this.txtArea.Name = "txtArea";
            this.txtArea.Size = new System.Drawing.Size(747, 39);
            this.txtArea.TabIndex = 4;
            this.txtArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtNoEmpleado
            // 
            this.txtNoEmpleado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtNoEmpleado.AutoSize = true;
            this.txtNoEmpleado.BackColor = System.Drawing.Color.AliceBlue;
            this.txtNoEmpleado.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtNoEmpleado.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoEmpleado.Location = new System.Drawing.Point(89, 786);
            this.txtNoEmpleado.MinimumSize = new System.Drawing.Size(747, 39);
            this.txtNoEmpleado.Name = "txtNoEmpleado";
            this.txtNoEmpleado.Size = new System.Drawing.Size(747, 39);
            this.txtNoEmpleado.TabIndex = 3;
            this.txtNoEmpleado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCargo
            // 
            this.txtCargo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCargo.AutoSize = true;
            this.txtCargo.BackColor = System.Drawing.Color.AliceBlue;
            this.txtCargo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtCargo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCargo.Location = new System.Drawing.Point(89, 708);
            this.txtCargo.MinimumSize = new System.Drawing.Size(747, 39);
            this.txtCargo.Name = "txtCargo";
            this.txtCargo.Size = new System.Drawing.Size(747, 39);
            this.txtCargo.TabIndex = 2;
            this.txtCargo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtNombre
            // 
            this.txtNombre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtNombre.AutoSize = true;
            this.txtNombre.BackColor = System.Drawing.Color.AliceBlue;
            this.txtNombre.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.Location = new System.Drawing.Point(89, 633);
            this.txtNombre.MinimumSize = new System.Drawing.Size(747, 39);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(747, 39);
            this.txtNombre.TabIndex = 1;
            this.txtNombre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EmpleadoFoto
            // 
            this.EmpleadoFoto.Enabled = false;
            this.EmpleadoFoto.Image = global::Asistencia.Properties.Resources.notfound;
            this.EmpleadoFoto.Location = new System.Drawing.Point(129, 55);
            this.EmpleadoFoto.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.EmpleadoFoto.Name = "EmpleadoFoto";
            this.EmpleadoFoto.Size = new System.Drawing.Size(667, 554);
            this.EmpleadoFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.EmpleadoFoto.TabIndex = 0;
            this.EmpleadoFoto.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = "";
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(947, 11);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            this.groupBox2.Size = new System.Drawing.Size(983, 1131);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Asistencia";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblAsistencia, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblRegistro, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTipo, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblHorario, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblReloj, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 22);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 226F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 364F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 281F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(969, 1106);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblAsistencia
            // 
            this.lblAsistencia.AutoSize = true;
            this.lblAsistencia.Location = new System.Drawing.Point(3, 485);
            this.lblAsistencia.MinimumSize = new System.Drawing.Size(951, 400);
            this.lblAsistencia.Name = "lblAsistencia";
            this.lblAsistencia.Size = new System.Drawing.Size(951, 400);
            this.lblAsistencia.TabIndex = 5;
            this.lblAsistencia.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblRegistro
            // 
            this.lblRegistro.AutoSize = true;
            this.lblRegistro.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegistro.Location = new System.Drawing.Point(3, 431);
            this.lblRegistro.MinimumSize = new System.Drawing.Size(951, 0);
            this.lblRegistro.Name = "lblRegistro";
            this.lblRegistro.Size = new System.Drawing.Size(951, 54);
            this.lblRegistro.TabIndex = 4;
            this.lblRegistro.Text = "Reloj";
            this.lblRegistro.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipo.Location = new System.Drawing.Point(3, 205);
            this.lblTipo.MinimumSize = new System.Drawing.Size(951, 160);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(951, 160);
            this.lblTipo.TabIndex = 3;
            this.lblTipo.Text = "Estado";
            this.lblTipo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHorario
            // 
            this.lblHorario.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHorario.Location = new System.Drawing.Point(3, 87);
            this.lblHorario.MinimumSize = new System.Drawing.Size(951, 0);
            this.lblHorario.Name = "lblHorario";
            this.lblHorario.Size = new System.Drawing.Size(951, 118);
            this.lblHorario.TabIndex = 2;
            this.lblHorario.Text = "Reloj";
            this.lblHorario.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReloj
            // 
            this.lblReloj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReloj.AutoSize = true;
            this.lblReloj.BackColor = System.Drawing.Color.Transparent;
            this.lblReloj.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReloj.Location = new System.Drawing.Point(3, 49);
            this.lblReloj.Margin = new System.Windows.Forms.Padding(3, 49, 3, 0);
            this.lblReloj.MinimumSize = new System.Drawing.Size(951, 0);
            this.lblReloj.Name = "lblReloj";
            this.lblReloj.Size = new System.Drawing.Size(963, 38);
            this.lblReloj.TabIndex = 1;
            this.lblReloj.Text = "Reloj";
            this.lblReloj.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Enviar
            // 
            this.Enviar.Enabled = true;
            this.Enviar.Interval = 300000;
            this.Enviar.Tick += new System.EventHandler(this.Enviar_Tick);
            // 
            // torniquete
            // 
            this.torniquete.Interval = 3000;
            this.torniquete.Tick += new System.EventHandler(this.torniquete_Tick);
            // 
            // RelojView
            // 
            this.RelojView.Interval = 60000;
            this.RelojView.Tick += new System.EventHandler(this.RelojView_Tick);
            // 
            // NFC_stop
            // 
            this.NFC_stop.Interval = 5000;
            this.NFC_stop.Tick += new System.EventHandler(this.NFC_stop_Tick);
            // 
            // RenderTexto
            // 
            this.RenderTexto.Interval = 50;
            this.RenderTexto.Tick += new System.EventHandler(this.RenderTexto_Tick);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            // 
            // backgroundWorker3
            // 
            this.backgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker3_DoWork);
            // 
            // backgroundWorker4
            // 
            this.backgroundWorker4.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker4_DoWork);
            // 
            // MainScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1387, 736);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.lblBackReloj);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximumSize = new System.Drawing.Size(2160, 1440);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1364, 736);
            this.Name = "MainScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainScreen";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainScreen_Load);
            this.Click += new System.EventHandler(this.button1_Click);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainScreen_MouseClick);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EmpleadoFoto)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer tActualizar;
        private System.Windows.Forms.Timer Reloj;
        private System.Windows.Forms.Timer Render;
        private System.Windows.Forms.Timer bannerStart;
        private System.Windows.Forms.Label lblBackReloj;
        private System.Windows.Forms.TextBox lblMensaje;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblEmpresa;
        private System.Windows.Forms.Label txtArea;
        private System.Windows.Forms.Label txtNoEmpleado;
        private System.Windows.Forms.Label txtCargo;
        private System.Windows.Forms.Label txtNombre;
        private System.Windows.Forms.PictureBox EmpleadoFoto;
        //private MaterialSkin.Controls.MaterialDivider Div1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblAsistencia;
        private System.Windows.Forms.Label lblRegistro;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.Label lblHorario;
        private System.Windows.Forms.Label lblReloj;
        private System.Windows.Forms.Timer Enviar;
        private System.Windows.Forms.Timer torniquete;
        private System.Windows.Forms.Timer RelojView;
        private System.Windows.Forms.Timer NFC_stop;
        private System.Windows.Forms.Timer RenderTexto;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.ComponentModel.BackgroundWorker backgroundWorker4;
    }
}