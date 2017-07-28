<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmModPrep
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmModPrep))
        Me.txtEXEfile = New System.Windows.Forms.TextBox()
        Me.lblGAFile = New System.Windows.Forms.Label()
        Me.btnModify = New System.Windows.Forms.Button()
        Me.txtInfo = New System.Windows.Forms.RichTextBox()
        Me.btnExtractBNDs = New System.Windows.Forms.Button()
        Me.btnExtractDCX = New System.Windows.Forms.Button()
        Me.btnDeleteDCX = New System.Windows.Forms.Button()
        Me.btnExtractFRPG = New System.Windows.Forms.Button()
        Me.btnExtractBHDs = New System.Windows.Forms.Button()
        Me.progCurFile = New System.Windows.Forms.ProgressBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.progOperation = New System.Windows.Forms.ProgressBar()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblProgCurFile = New System.Windows.Forms.Label()
        Me.lblProgOperation = New System.Windows.Forms.Label()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.btnDeleteBNDs = New System.Windows.Forms.Button()
        Me.btnDeleteFRPG = New System.Windows.Forms.Button()
        Me.btnDeleteBHDs = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout
        Me.SuspendLayout
        '
        'txtEXEfile
        '
        Me.txtEXEfile.AllowDrop = true
        resources.ApplyResources(Me.txtEXEfile, "txtEXEfile")
        Me.txtEXEfile.Name = "txtEXEfile"
        Me.txtEXEfile.ReadOnly = true
        '
        'lblGAFile
        '
        resources.ApplyResources(Me.lblGAFile, "lblGAFile")
        Me.lblGAFile.Name = "lblGAFile"
        '
        'btnModify
        '
        resources.ApplyResources(Me.btnModify, "btnModify")
        Me.btnModify.Name = "btnModify"
        Me.btnModify.UseVisualStyleBackColor = true
        '
        'txtInfo
        '
        resources.ApplyResources(Me.txtInfo, "txtInfo")
        Me.txtInfo.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.ReadOnly = true
        Me.txtInfo.ShortcutsEnabled = false
        Me.txtInfo.ShowSelectionMargin = true
        Me.txtInfo.TabStop = false
        '
        'btnExtractBNDs
        '
        resources.ApplyResources(Me.btnExtractBNDs, "btnExtractBNDs")
        Me.btnExtractBNDs.Name = "btnExtractBNDs"
        Me.btnExtractBNDs.UseVisualStyleBackColor = true
        '
        'btnExtractDCX
        '
        resources.ApplyResources(Me.btnExtractDCX, "btnExtractDCX")
        Me.btnExtractDCX.Name = "btnExtractDCX"
        Me.btnExtractDCX.UseVisualStyleBackColor = true
        '
        'btnDeleteDCX
        '
        resources.ApplyResources(Me.btnDeleteDCX, "btnDeleteDCX")
        Me.btnDeleteDCX.Name = "btnDeleteDCX"
        Me.btnDeleteDCX.UseVisualStyleBackColor = true
        '
        'btnExtractFRPG
        '
        resources.ApplyResources(Me.btnExtractFRPG, "btnExtractFRPG")
        Me.btnExtractFRPG.Name = "btnExtractFRPG"
        Me.btnExtractFRPG.UseVisualStyleBackColor = true
        '
        'btnExtractBHDs
        '
        resources.ApplyResources(Me.btnExtractBHDs, "btnExtractBHDs")
        Me.btnExtractBHDs.Name = "btnExtractBHDs"
        Me.btnExtractBHDs.UseVisualStyleBackColor = true
        '
        'progCurFile
        '
        resources.ApplyResources(Me.progCurFile, "progCurFile")
        Me.progCurFile.MarqueeAnimationSpeed = 1
        Me.progCurFile.Maximum = 0
        Me.progCurFile.Name = "progCurFile"
        Me.progCurFile.Step = 1
        Me.progCurFile.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'progOperation
        '
        resources.ApplyResources(Me.progOperation, "progOperation")
        Me.progOperation.MarqueeAnimationSpeed = 0
        Me.progOperation.Maximum = 0
        Me.progOperation.Name = "progOperation"
        Me.progOperation.Step = 1
        Me.progOperation.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'lblProgCurFile
        '
        resources.ApplyResources(Me.lblProgCurFile, "lblProgCurFile")
        Me.lblProgCurFile.Name = "lblProgCurFile"
        '
        'lblProgOperation
        '
        resources.ApplyResources(Me.lblProgOperation, "lblProgOperation")
        Me.lblProgOperation.Name = "lblProgOperation"
        '
        'btnBrowse
        '
        resources.ApplyResources(Me.btnBrowse, "btnBrowse")
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.UseVisualStyleBackColor = true
        '
        'btnDeleteBNDs
        '
        resources.ApplyResources(Me.btnDeleteBNDs, "btnDeleteBNDs")
        Me.btnDeleteBNDs.Name = "btnDeleteBNDs"
        Me.btnDeleteBNDs.UseVisualStyleBackColor = true
        '
        'btnDeleteFRPG
        '
        resources.ApplyResources(Me.btnDeleteFRPG, "btnDeleteFRPG")
        Me.btnDeleteFRPG.Name = "btnDeleteFRPG"
        Me.btnDeleteFRPG.UseVisualStyleBackColor = true
        '
        'btnDeleteBHDs
        '
        resources.ApplyResources(Me.btnDeleteBHDs, "btnDeleteBHDs")
        Me.btnDeleteBHDs.Name = "btnDeleteBHDs"
        Me.btnDeleteBHDs.UseVisualStyleBackColor = true
        '
        'Panel1
        '
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.txtInfo)
        Me.Panel1.Name = "Panel1"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'frmModPrep
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnDeleteBHDs)
        Me.Controls.Add(Me.btnDeleteFRPG)
        Me.Controls.Add(Me.btnDeleteBNDs)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.lblProgOperation)
        Me.Controls.Add(Me.lblProgCurFile)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.progOperation)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.progCurFile)
        Me.Controls.Add(Me.btnExtractBHDs)
        Me.Controls.Add(Me.btnExtractFRPG)
        Me.Controls.Add(Me.btnDeleteDCX)
        Me.Controls.Add(Me.btnExtractDCX)
        Me.Controls.Add(Me.btnExtractBNDs)
        Me.Controls.Add(Me.btnModify)
        Me.Controls.Add(Me.txtEXEfile)
        Me.Controls.Add(Me.lblGAFile)
        Me.DoubleBuffered = true
        Me.Name = "frmModPrep"
        Me.Panel1.ResumeLayout(false)
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Friend WithEvents txtEXEfile As TextBox
    Friend WithEvents lblGAFile As Label
    Friend WithEvents btnModify As Button
    Friend WithEvents txtInfo As RichTextBox
    Friend WithEvents btnExtractBNDs As Button
    Friend WithEvents btnExtractDCX As Button
    Friend WithEvents btnDeleteDCX As Button
    Friend WithEvents btnExtractFRPG As Button
    Friend WithEvents btnExtractBHDs As Button
    Friend WithEvents progCurFile As ProgressBar
    Friend WithEvents Label1 As Label
    Friend WithEvents progOperation As ProgressBar
    Friend WithEvents Label2 As Label
    Friend WithEvents lblProgCurFile As Label
    Friend WithEvents lblProgOperation As Label
    Friend WithEvents btnBrowse As Button
    Friend WithEvents btnDeleteBNDs As Button
    Friend WithEvents btnDeleteFRPG As Button
    Friend WithEvents btnDeleteBHDs As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label3 As Label
End Class
