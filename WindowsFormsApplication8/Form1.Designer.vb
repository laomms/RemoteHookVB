Namespace WinFormEasyHook
	Partial Public Class Form1
		''' <summary>
		''' 必需的设计器变量。
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' 清理所有正在使用的资源。
		''' </summary>
		''' <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows 窗体设计器生成的代码"

		''' <summary>
		''' 设计器支持所需的方法 - 不要
		''' 使用代码编辑器修改此方法的内容。
		''' </summary>
		Private Sub InitializeComponent()
			Me.button1 = New System.Windows.Forms.Button()
			Me.textBox1 = New System.Windows.Forms.TextBox()
			Me.label1 = New System.Windows.Forms.Label()
			Me.label2 = New System.Windows.Forms.Label()
			Me.txtProcessName = New System.Windows.Forms.TextBox()
			Me.btnGet = New System.Windows.Forms.Button()
			Me.SuspendLayout()
			' 
			' button1
			' 
			Me.button1.Location = New System.Drawing.Point(223, 8)
			Me.button1.Name = "button1"
			Me.button1.Size = New System.Drawing.Size(68, 26)
			Me.button1.TabIndex = 0
			Me.button1.Text = "注入"
			Me.button1.UseVisualStyleBackColor = True
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.button1.Click += new System.EventHandler(this.button1_Click);
			' 
			' textBox1
			' 
			Me.textBox1.Location = New System.Drawing.Point(95, 12)
			Me.textBox1.Name = "textBox1"
			Me.textBox1.Size = New System.Drawing.Size(113, 21)
			Me.textBox1.TabIndex = 1
			' 
			' label1
			' 
			Me.label1.AutoSize = True
			Me.label1.Location = New System.Drawing.Point(18, 15)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(71, 12)
			Me.label1.TabIndex = 2
			Me.label1.Text = "Process ID:"
			' 
			' label2
			' 
			Me.label2.AutoSize = True
			Me.label2.Location = New System.Drawing.Point(6, 46)
			Me.label2.Name = "label2"
			Me.label2.Size = New System.Drawing.Size(83, 12)
			Me.label2.TabIndex = 3
			Me.label2.Text = "Process Name:"
			' 
			' txtProcessName
			' 
			Me.txtProcessName.Location = New System.Drawing.Point(95, 43)
			Me.txtProcessName.Name = "txtProcessName"
			Me.txtProcessName.Size = New System.Drawing.Size(113, 21)
			Me.txtProcessName.TabIndex = 4
			' 
			' btnGet
			' 
			Me.btnGet.Location = New System.Drawing.Point(223, 41)
			Me.btnGet.Name = "btnGet"
			Me.btnGet.Size = New System.Drawing.Size(68, 23)
			Me.btnGet.TabIndex = 5
			Me.btnGet.Text = "获取"
			Me.btnGet.UseVisualStyleBackColor = True
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 12F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(298, 84)
			Me.Controls.Add(Me.btnGet)
			Me.Controls.Add(Me.txtProcessName)
			Me.Controls.Add(Me.label2)
			Me.Controls.Add(Me.label1)
			Me.Controls.Add(Me.textBox1)
			Me.Controls.Add(Me.button1)
			Me.Name = "Form1"
			Me.Text = "Form1"
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private WithEvents button1 As System.Windows.Forms.Button
		Private textBox1 As System.Windows.Forms.TextBox
		Private label1 As System.Windows.Forms.Label
		Private label2 As System.Windows.Forms.Label
		Private txtProcessName As System.Windows.Forms.TextBox
		Private WithEvents btnGet As System.Windows.Forms.Button
	End Class
End Namespace

