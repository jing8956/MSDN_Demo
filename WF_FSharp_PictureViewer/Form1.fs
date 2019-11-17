namespace WF_FSharp_PictureViewer

open System.Windows.Forms
open System.Drawing
open System

type Form1() as this =
    inherit Form()
    [<DefaultValue>]val mutable components:System.ComponentModel.IContainer
    
    [<DefaultValue>]val mutable tableLayoutPanel1:TableLayoutPanel
    [<DefaultValue>]val mutable pictureBox1:PictureBox
    [<DefaultValue>]val mutable checkBox1:CheckBox
    [<DefaultValue>]val mutable flowLayoutPanel1:FlowLayoutPanel
    [<DefaultValue>]val mutable showButton:Button
    [<DefaultValue>]val mutable clearButton:Button
    [<DefaultValue>]val mutable backgroundButton:Button
    [<DefaultValue>]val mutable closeButton:Button
    [<DefaultValue>]val mutable openFileDialog1:OpenFileDialog
    [<DefaultValue>]val mutable colorDialog1:ColorDialog

    do this.InitializeComponent()
    member private this.InitializeComponent() = 
        this.tableLayoutPanel1 <- new TableLayoutPanel()
        this.pictureBox1 <- new PictureBox()
        this.checkBox1 <- new CheckBox()
        this.flowLayoutPanel1 <- new FlowLayoutPanel()
        this.showButton <- new Button()
        this.clearButton <- new Button()
        this.backgroundButton <- new Button()
        this.closeButton <- new Button()
        this.openFileDialog1 <- new OpenFileDialog()
        this.colorDialog1 <- new ColorDialog()

        this.tableLayoutPanel1.SuspendLayout()
        (this.pictureBox1 :> System.ComponentModel.ISupportInitialize).BeginInit()
        this.flowLayoutPanel1.SuspendLayout()
        this.SuspendLayout()
        //
        // tableLayoutPanel1
        //
        this.tableLayoutPanel1.ColumnCount <- 2
        this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,15.0f)) |> ignore
        this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,85.0f)) |> ignore
        this.tableLayoutPanel1.Controls.Add(this.pictureBox1,0,0)
        this.tableLayoutPanel1.Controls.Add(this.checkBox1,0,1)
        this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1,1,1)
        this.tableLayoutPanel1.Dock <- DockStyle.Fill
        this.tableLayoutPanel1.Location <- new Point(0,0)
        this.tableLayoutPanel1.Name <- "tableLayoutPanel1"
        this.tableLayoutPanel1.RowCount <- 2
        this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent,90.0f)) |> ignore
        this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent,10.0f)) |> ignore
        this.tableLayoutPanel1.Size <- new Size(534,311)
        this.tableLayoutPanel1.TabIndex <- 0
        // 
        // pictureBox1
        // 
        this.tableLayoutPanel1.SetColumnSpan(this.pictureBox1,2)
        this.pictureBox1.Dock <- DockStyle.Fill
        this.pictureBox1.Location <- new Point(3,3)
        this.pictureBox1.Name <- "pictureBox1"
        this.pictureBox1.Size <- new Size(528,273)
        this.pictureBox1.TabIndex <- 0
        this.pictureBox1.TabStop <- false
        // 
        // checkBox1
        //
        this.checkBox1.Anchor <- (AnchorStyles.Top ||| AnchorStyles.Bottom ||| AnchorStyles.Left ||| AnchorStyles.Right)
        this.checkBox1.AutoSize <- true
        this.checkBox1.Location <- new Point(3,282)
        this.checkBox1.Name <- "checkBox1"
        this.checkBox1.Size <- new Size(74,26)
        this.checkBox1.TabIndex <- 1
        this.checkBox1.Text <- "拉伸"
        this.checkBox1.UseVisualStyleBackColor <- true
        this.checkBox1.CheckedChanged.AddHandler(new EventHandler(this.checkBox1_CheckedChanged))
        // 
        // flowLayoutPanel1
        //
        this.flowLayoutPanel1.Controls.Add(this.showButton)
        this.flowLayoutPanel1.Controls.Add(this.clearButton)
        this.flowLayoutPanel1.Controls.Add(this.backgroundButton)
        this.flowLayoutPanel1.Controls.Add(this.closeButton)
        this.flowLayoutPanel1.Dock <- DockStyle.Fill
        this.flowLayoutPanel1.Location <- new Point(83,282)
        this.flowLayoutPanel1.Name <- "flowLayoutPanel1"
        this.flowLayoutPanel1.RightToLeft <- RightToLeft.Yes
        this.flowLayoutPanel1.Size <- new Size(448,26)
        this.flowLayoutPanel1.TabIndex <- 2
        // 
        // showButton
        //
        this.showButton.Location <- new Point(330,3)
        this.showButton.Name <- "showButton"
        this.showButton.Size <- new Size(115,23)
        this.showButton.TabIndex <- 0
        this.showButton.Text <- "显示图片"
        this.showButton.UseVisualStyleBackColor <- true
        this.showButton.Click.AddHandler(new EventHandler(this.showButton_Click))
        // 
        // clearButton
        // 
        this.clearButton.Location <- new Point(209,3)
        this.clearButton.Name <- "clearButton"
        this.clearButton.Size <- new Size(115,23)
        this.clearButton.TabIndex <- 1
        this.clearButton.Text <- "清除图片"
        this.clearButton.UseVisualStyleBackColor <- true
        this.clearButton.Click.AddHandler(new EventHandler(this.clearButton_Click))
        // 
        // backgroundButton
        //
        this.backgroundButton.Location <- new Point(88,3)
        this.backgroundButton.Name <- "backgroundButton"
        this.backgroundButton.Size <- new Size(115,23)
        this.backgroundButton.TabIndex <- 2
        this.backgroundButton.Text <- "设置背景色"
        this.backgroundButton.UseVisualStyleBackColor <- true
        this.backgroundButton.Click.AddHandler(new EventHandler(this.backgroundButton_Click))
        //
        // closeButton
        //
        this.closeButton.Location <- new Point(7,3)
        this.closeButton.Name <- "closeButton"
        this.closeButton.Size <- Size(75,23)
        this.closeButton.TabIndex <- 3
        this.closeButton.Text <- "关闭"
        this.closeButton.UseVisualStyleBackColor <- true
        this.closeButton.Click.AddHandler(new EventHandler(this.closeButton_Click))
        // 
        // openFileDialog1
        //
        this.openFileDialog1.FileName <- "openFileDialog1"
        this.openFileDialog1.Filter <- "JPEG 文件 (*.jpg)|*.jpg|PNG 文件 (*.png)|*.png|BMP 文件 (*.bmp)|*.bmp|所有文件 (*.*)|*.*"
        this.openFileDialog1.Title <- "选择一个图片文件"
        // 
        // Form1
        // 
        this.AutoScaleDimensions <- new SizeF(6.0f,12.0f)
        this.AutoScaleMode <- AutoScaleMode.Font
        this.ClientSize <- Size(534,311)
        this.Controls.Add(this.tableLayoutPanel1)
        this.Name <- "Form1"
        this.Text <- "图片查看器"
        
        this.tableLayoutPanel1.ResumeLayout(false)
        this.tableLayoutPanel1.PerformLayout()
        (this.pictureBox1 :> System.ComponentModel.ISupportInitialize).EndInit()
        this.flowLayoutPanel1.ResumeLayout(false)
        this.ResumeLayout(false)
        ()
    override this.Dispose(disposing:bool) = 
       if(disposing && (not (isNull this.components))) then
           this.components.Dispose()
       base.Dispose(disposing)

    member private this.showButton_Click (_sender:Object) (_e:EventArgs) = 
        if this.openFileDialog1.ShowDialog() = DialogResult.OK then
            this.pictureBox1.Load(this.openFileDialog1.FileName)
    member private this.clearButton_Click (_sender:Object) (_e:EventArgs) = 
        this.pictureBox1.Image <- null
    member private this.backgroundButton_Click (_sender:Object) (_e:EventArgs) = 
        if this.colorDialog1.ShowDialog() = DialogResult.OK then
            this.pictureBox1.BackColor <- this.colorDialog1.Color
    member private this.closeButton_Click (_sender:Object) (_e:EventArgs) = 
        this.Close()
    member private this.checkBox1_CheckedChanged (_sender:Object) (_e:EventArgs) = 
            this.pictureBox1.SizeMode <-
                if this.checkBox1.Checked 
                then PictureBoxSizeMode.StretchImage 
                else PictureBoxSizeMode.Normal
