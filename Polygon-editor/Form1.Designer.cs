namespace Polygon_editor
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel_main = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel_right = new System.Windows.Forms.TableLayoutPanel();
			this.groupBox_edit = new System.Windows.Forms.GroupBox();
			this.button_clear = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.radioButton_movePolygon = new System.Windows.Forms.RadioButton();
			this.radioButton_moveEdge = new System.Windows.Forms.RadioButton();
			this.radioButton_edgeVertex = new System.Windows.Forms.RadioButton();
			this.radioButton_deleteVertex = new System.Windows.Forms.RadioButton();
			this.radioButton_moveVertex = new System.Windows.Forms.RadioButton();
			this.radioButton_editPolygon = new System.Windows.Forms.RadioButton();
			this.radioButton_deletePolygon = new System.Windows.Forms.RadioButton();
			this.radioButton_addPolygon = new System.Windows.Forms.RadioButton();
			this.groupBox_drawMode = new System.Windows.Forms.GroupBox();
			this.radioButton_defaultDrawing = new System.Windows.Forms.RadioButton();
			this.radioButton_bresenham = new System.Windows.Forms.RadioButton();
			this.pictureBox_workingArea = new System.Windows.Forms.PictureBox();
			this.groupBox_constraints = new System.Windows.Forms.GroupBox();
			this.radioButton_sameLength = new System.Windows.Forms.RadioButton();
			this.radioButton_parallel = new System.Windows.Forms.RadioButton();
			this.tableLayoutPanel_main.SuspendLayout();
			this.tableLayoutPanel_right.SuspendLayout();
			this.groupBox_edit.SuspendLayout();
			this.groupBox_drawMode.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_workingArea)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel_main
			// 
			this.tableLayoutPanel_main.ColumnCount = 2;
			this.tableLayoutPanel_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tableLayoutPanel_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel_main.Controls.Add(this.tableLayoutPanel_right, 1, 0);
			this.tableLayoutPanel_main.Controls.Add(this.pictureBox_workingArea, 0, 0);
			this.tableLayoutPanel_main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel_main.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel_main.Name = "tableLayoutPanel_main";
			this.tableLayoutPanel_main.RowCount = 1;
			this.tableLayoutPanel_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel_main.Size = new System.Drawing.Size(800, 450);
			this.tableLayoutPanel_main.TabIndex = 0;
			// 
			// tableLayoutPanel_right
			// 
			this.tableLayoutPanel_right.ColumnCount = 1;
			this.tableLayoutPanel_right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel_right.Controls.Add(this.groupBox_edit, 0, 0);
			this.tableLayoutPanel_right.Controls.Add(this.groupBox_drawMode, 0, 1);
			this.tableLayoutPanel_right.Controls.Add(this.groupBox_constraints, 0, 2);
			this.tableLayoutPanel_right.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel_right.Location = new System.Drawing.Point(603, 3);
			this.tableLayoutPanel_right.Name = "tableLayoutPanel_right";
			this.tableLayoutPanel_right.RowCount = 3;
			this.tableLayoutPanel_right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 77.56232F));
			this.tableLayoutPanel_right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.43767F));
			this.tableLayoutPanel_right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
			this.tableLayoutPanel_right.Size = new System.Drawing.Size(194, 444);
			this.tableLayoutPanel_right.TabIndex = 1;
			// 
			// groupBox_edit
			// 
			this.groupBox_edit.Controls.Add(this.radioButton_parallel);
			this.groupBox_edit.Controls.Add(this.button_clear);
			this.groupBox_edit.Controls.Add(this.radioButton_sameLength);
			this.groupBox_edit.Controls.Add(this.label1);
			this.groupBox_edit.Controls.Add(this.radioButton_movePolygon);
			this.groupBox_edit.Controls.Add(this.radioButton_moveEdge);
			this.groupBox_edit.Controls.Add(this.radioButton_edgeVertex);
			this.groupBox_edit.Controls.Add(this.radioButton_deleteVertex);
			this.groupBox_edit.Controls.Add(this.radioButton_moveVertex);
			this.groupBox_edit.Controls.Add(this.radioButton_editPolygon);
			this.groupBox_edit.Controls.Add(this.radioButton_deletePolygon);
			this.groupBox_edit.Controls.Add(this.radioButton_addPolygon);
			this.groupBox_edit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox_edit.Location = new System.Drawing.Point(3, 3);
			this.groupBox_edit.Name = "groupBox_edit";
			this.groupBox_edit.Size = new System.Drawing.Size(188, 312);
			this.groupBox_edit.TabIndex = 0;
			this.groupBox_edit.TabStop = false;
			this.groupBox_edit.Text = "Edit";
			// 
			// button_clear
			// 
			this.button_clear.Location = new System.Drawing.Point(113, 228);
			this.button_clear.Name = "button_clear";
			this.button_clear.Size = new System.Drawing.Size(75, 23);
			this.button_clear.TabIndex = 9;
			this.button_clear.Text = "Clear";
			this.button_clear.UseVisualStyleBackColor = true;
			this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 232);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 15);
			this.label1.TabIndex = 8;
			this.label1.Text = "label1";
			// 
			// radioButton_movePolygon
			// 
			this.radioButton_movePolygon.AutoSize = true;
			this.radioButton_movePolygon.Location = new System.Drawing.Point(6, 197);
			this.radioButton_movePolygon.Name = "radioButton_movePolygon";
			this.radioButton_movePolygon.Size = new System.Drawing.Size(102, 19);
			this.radioButton_movePolygon.TabIndex = 7;
			this.radioButton_movePolygon.Text = "Move polygon";
			this.radioButton_movePolygon.UseVisualStyleBackColor = true;
			// 
			// radioButton_moveEdge
			// 
			this.radioButton_moveEdge.AutoSize = true;
			this.radioButton_moveEdge.Location = new System.Drawing.Point(6, 172);
			this.radioButton_moveEdge.Name = "radioButton_moveEdge";
			this.radioButton_moveEdge.Size = new System.Drawing.Size(84, 19);
			this.radioButton_moveEdge.TabIndex = 6;
			this.radioButton_moveEdge.Text = "Move edge";
			this.radioButton_moveEdge.UseVisualStyleBackColor = true;
			// 
			// radioButton_edgeVertex
			// 
			this.radioButton_edgeVertex.AutoSize = true;
			this.radioButton_edgeVertex.Location = new System.Drawing.Point(6, 147);
			this.radioButton_edgeVertex.Name = "radioButton_edgeVertex";
			this.radioButton_edgeVertex.Size = new System.Drawing.Size(148, 19);
			this.radioButton_edgeVertex.TabIndex = 5;
			this.radioButton_edgeVertex.Text = "Add vertex on the edge";
			this.radioButton_edgeVertex.UseVisualStyleBackColor = true;
			// 
			// radioButton_deleteVertex
			// 
			this.radioButton_deleteVertex.AutoSize = true;
			this.radioButton_deleteVertex.Location = new System.Drawing.Point(6, 122);
			this.radioButton_deleteVertex.Name = "radioButton_deleteVertex";
			this.radioButton_deleteVertex.Size = new System.Drawing.Size(93, 19);
			this.radioButton_deleteVertex.TabIndex = 4;
			this.radioButton_deleteVertex.Text = "Delete vertex";
			this.radioButton_deleteVertex.UseVisualStyleBackColor = true;
			// 
			// radioButton_moveVertex
			// 
			this.radioButton_moveVertex.AutoSize = true;
			this.radioButton_moveVertex.Location = new System.Drawing.Point(6, 97);
			this.radioButton_moveVertex.Name = "radioButton_moveVertex";
			this.radioButton_moveVertex.Size = new System.Drawing.Size(90, 19);
			this.radioButton_moveVertex.TabIndex = 3;
			this.radioButton_moveVertex.Text = "Move vertex";
			this.radioButton_moveVertex.UseVisualStyleBackColor = true;
			// 
			// radioButton_editPolygon
			// 
			this.radioButton_editPolygon.AutoSize = true;
			this.radioButton_editPolygon.Location = new System.Drawing.Point(6, 72);
			this.radioButton_editPolygon.Name = "radioButton_editPolygon";
			this.radioButton_editPolygon.Size = new System.Drawing.Size(121, 19);
			this.radioButton_editPolygon.TabIndex = 2;
			this.radioButton_editPolygon.Text = "(NO) Edit polygon";
			this.radioButton_editPolygon.UseVisualStyleBackColor = true;
			// 
			// radioButton_deletePolygon
			// 
			this.radioButton_deletePolygon.AutoSize = true;
			this.radioButton_deletePolygon.Location = new System.Drawing.Point(6, 47);
			this.radioButton_deletePolygon.Name = "radioButton_deletePolygon";
			this.radioButton_deletePolygon.Size = new System.Drawing.Size(105, 19);
			this.radioButton_deletePolygon.TabIndex = 1;
			this.radioButton_deletePolygon.Text = "Delete polygon";
			this.radioButton_deletePolygon.UseVisualStyleBackColor = true;
			// 
			// radioButton_addPolygon
			// 
			this.radioButton_addPolygon.AutoSize = true;
			this.radioButton_addPolygon.Checked = true;
			this.radioButton_addPolygon.Location = new System.Drawing.Point(6, 22);
			this.radioButton_addPolygon.Name = "radioButton_addPolygon";
			this.radioButton_addPolygon.Size = new System.Drawing.Size(94, 19);
			this.radioButton_addPolygon.TabIndex = 0;
			this.radioButton_addPolygon.TabStop = true;
			this.radioButton_addPolygon.Text = "Add polygon";
			this.radioButton_addPolygon.UseVisualStyleBackColor = true;
			this.radioButton_addPolygon.CheckedChanged += new System.EventHandler(this.radioButton_addPolygon_CheckedChanged);
			// 
			// groupBox_drawMode
			// 
			this.groupBox_drawMode.Controls.Add(this.radioButton_defaultDrawing);
			this.groupBox_drawMode.Controls.Add(this.radioButton_bresenham);
			this.groupBox_drawMode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox_drawMode.Location = new System.Drawing.Point(3, 321);
			this.groupBox_drawMode.Name = "groupBox_drawMode";
			this.groupBox_drawMode.Size = new System.Drawing.Size(188, 86);
			this.groupBox_drawMode.TabIndex = 1;
			this.groupBox_drawMode.TabStop = false;
			this.groupBox_drawMode.Text = "Drawing mode";
			// 
			// radioButton_defaultDrawing
			// 
			this.radioButton_defaultDrawing.AutoSize = true;
			this.radioButton_defaultDrawing.Checked = true;
			this.radioButton_defaultDrawing.Location = new System.Drawing.Point(6, 22);
			this.radioButton_defaultDrawing.Name = "radioButton_defaultDrawing";
			this.radioButton_defaultDrawing.Size = new System.Drawing.Size(143, 19);
			this.radioButton_defaultDrawing.TabIndex = 1;
			this.radioButton_defaultDrawing.TabStop = true;
			this.radioButton_defaultDrawing.Text = "Default drawing mode";
			this.radioButton_defaultDrawing.UseVisualStyleBackColor = true;
			// 
			// radioButton_bresenham
			// 
			this.radioButton_bresenham.AutoSize = true;
			this.radioButton_bresenham.Location = new System.Drawing.Point(6, 47);
			this.radioButton_bresenham.Name = "radioButton_bresenham";
			this.radioButton_bresenham.Size = new System.Drawing.Size(139, 19);
			this.radioButton_bresenham.TabIndex = 0;
			this.radioButton_bresenham.Text = "Bresenham algorithm";
			this.radioButton_bresenham.UseVisualStyleBackColor = true;
			// 
			// pictureBox_workingArea
			// 
			this.pictureBox_workingArea.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox_workingArea.Location = new System.Drawing.Point(3, 3);
			this.pictureBox_workingArea.Name = "pictureBox_workingArea";
			this.pictureBox_workingArea.Size = new System.Drawing.Size(594, 444);
			this.pictureBox_workingArea.TabIndex = 2;
			this.pictureBox_workingArea.TabStop = false;
			this.pictureBox_workingArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_workingArea_MouseDown);
			this.pictureBox_workingArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_workingArea_MouseMove);
			this.pictureBox_workingArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_workingArea_MouseUp);
			// 
			// groupBox_constraints
			// 
			this.groupBox_constraints.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox_constraints.Location = new System.Drawing.Point(3, 413);
			this.groupBox_constraints.Name = "groupBox_constraints";
			this.groupBox_constraints.Size = new System.Drawing.Size(188, 28);
			this.groupBox_constraints.TabIndex = 2;
			this.groupBox_constraints.TabStop = false;
			this.groupBox_constraints.Text = "Constraints";
			// 
			// radioButton_sameLength
			// 
			this.radioButton_sameLength.AutoSize = true;
			this.radioButton_sameLength.Location = new System.Drawing.Point(5, 262);
			this.radioButton_sameLength.Name = "radioButton_sameLength";
			this.radioButton_sameLength.Size = new System.Drawing.Size(91, 19);
			this.radioButton_sameLength.TabIndex = 0;
			this.radioButton_sameLength.TabStop = true;
			this.radioButton_sameLength.Text = "Same length";
			this.radioButton_sameLength.UseVisualStyleBackColor = true;
			// 
			// radioButton_parallel
			// 
			this.radioButton_parallel.AutoSize = true;
			this.radioButton_parallel.Location = new System.Drawing.Point(6, 287);
			this.radioButton_parallel.Name = "radioButton_parallel";
			this.radioButton_parallel.Size = new System.Drawing.Size(117, 19);
			this.radioButton_parallel.TabIndex = 1;
			this.radioButton_parallel.TabStop = true;
			this.radioButton_parallel.Text = "Parallel segments";
			this.radioButton_parallel.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.tableLayoutPanel_main);
			this.Name = "Form1";
			this.Text = "Form1";
			this.tableLayoutPanel_main.ResumeLayout(false);
			this.tableLayoutPanel_right.ResumeLayout(false);
			this.groupBox_edit.ResumeLayout(false);
			this.groupBox_edit.PerformLayout();
			this.groupBox_drawMode.ResumeLayout(false);
			this.groupBox_drawMode.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_workingArea)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private TableLayoutPanel tableLayoutPanel_main;
		private GroupBox groupBox_edit;
		private RadioButton radioButton_deletePolygon;
		private RadioButton radioButton_addPolygon;
		private RadioButton radioButton_editPolygon;
		private TableLayoutPanel tableLayoutPanel_right;
		private RadioButton radioButton_movePolygon;
		private RadioButton radioButton_moveEdge;
		private RadioButton radioButton_edgeVertex;
		private RadioButton radioButton_deleteVertex;
		private RadioButton radioButton_moveVertex;
		private GroupBox groupBox_drawMode;
		private RadioButton radioButton_defaultDrawing;
		private RadioButton radioButton_bresenham;
		private PictureBox pictureBox_workingArea;
		private Label label1;
		private Button button_clear;
		private RadioButton radioButton_parallel;
		private RadioButton radioButton_sameLength;
		private GroupBox groupBox_constraints;
	}
}