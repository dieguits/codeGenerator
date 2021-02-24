namespace Generator
{
    partial class Form1
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtConeccion = new System.Windows.Forms.TextBox();
            this.gridTables = new System.Windows.Forms.DataGridView();
            this.btnGenerator = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridTables)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(695, 73);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(80, 26);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtConeccion
            // 
            this.txtConeccion.Location = new System.Drawing.Point(23, 25);
            this.txtConeccion.Name = "txtConeccion";
            this.txtConeccion.Size = new System.Drawing.Size(752, 20);
            this.txtConeccion.TabIndex = 1;
            // 
            // gridTables
            // 
            this.gridTables.AllowUserToAddRows = false;
            this.gridTables.AllowUserToDeleteRows = false;
            this.gridTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTables.Location = new System.Drawing.Point(23, 113);
            this.gridTables.Name = "gridTables";
            this.gridTables.Size = new System.Drawing.Size(751, 288);
            this.gridTables.TabIndex = 0;
            this.gridTables.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTables_CellClick);
            this.gridTables.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTables_CellContentClick);
            // 
            // btnGenerator
            // 
            this.btnGenerator.Location = new System.Drawing.Point(539, 420);
            this.btnGenerator.Name = "btnGenerator";
            this.btnGenerator.Size = new System.Drawing.Size(234, 32);
            this.btnGenerator.TabIndex = 3;
            this.btnGenerator.Text = "Generate";
            this.btnGenerator.UseVisualStyleBackColor = true;
            this.btnGenerator.Click += new System.EventHandler(this.btnGenerator_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 498);
            this.Controls.Add(this.btnGenerator);
            this.Controls.Add(this.gridTables);
            this.Controls.Add(this.txtConeccion);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridTables)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtConeccion;
        private System.Windows.Forms.DataGridView gridTables;
        private System.Windows.Forms.Button btnGenerator;
    }
}

