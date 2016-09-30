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
        Me.txtEXEfile = New System.Windows.Forms.TextBox()
        Me.lblGAFile = New System.Windows.Forms.Label()
        Me.btnModify = New System.Windows.Forms.Button()
        Me.txtInfo = New System.Windows.Forms.TextBox()
        Me.btnExtractBNDs = New System.Windows.Forms.Button()
        Me.btnExtractDCX = New System.Windows.Forms.Button()
        Me.btnDeleteDCX = New System.Windows.Forms.Button()
        Me.btnExtractFRPG = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtEXEfile
        '
        Me.txtEXEfile.AllowDrop = True
        Me.txtEXEfile.Location = New System.Drawing.Point(68, 12)
        Me.txtEXEfile.Name = "txtEXEfile"
        Me.txtEXEfile.ReadOnly = True
        Me.txtEXEfile.Size = New System.Drawing.Size(671, 20)
        Me.txtEXEfile.TabIndex = 29
        Me.txtEXEfile.Text = "Drag 'n Drop DARKSOULS.exe here"
        '
        'lblGAFile
        '
        Me.lblGAFile.AutoSize = True
        Me.lblGAFile.Location = New System.Drawing.Point(12, 15)
        Me.lblGAFile.Name = "lblGAFile"
        Me.lblGAFile.Size = New System.Drawing.Size(50, 13)
        Me.lblGAFile.TabIndex = 30
        Me.lblGAFile.Text = "EXE File:"
        '
        'btnModify
        '
        Me.btnModify.Enabled = False
        Me.btnModify.Location = New System.Drawing.Point(68, 38)
        Me.btnModify.Name = "btnModify"
        Me.btnModify.Size = New System.Drawing.Size(80, 23)
        Me.btnModify.TabIndex = 31
        Me.btnModify.Text = "Modify EXE"
        Me.btnModify.UseVisualStyleBackColor = True
        '
        'txtInfo
        '
        Me.txtInfo.Location = New System.Drawing.Point(15, 202)
        Me.txtInfo.Multiline = True
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtInfo.Size = New System.Drawing.Size(805, 142)
        Me.txtInfo.TabIndex = 32
        '
        'btnExtractBNDs
        '
        Me.btnExtractBNDs.Enabled = False
        Me.btnExtractBNDs.Location = New System.Drawing.Point(154, 38)
        Me.btnExtractBNDs.Name = "btnExtractBNDs"
        Me.btnExtractBNDs.Size = New System.Drawing.Size(80, 23)
        Me.btnExtractBNDs.TabIndex = 33
        Me.btnExtractBNDs.Text = "Extract BNDs"
        Me.btnExtractBNDs.UseVisualStyleBackColor = True
        '
        'btnExtractDCX
        '
        Me.btnExtractDCX.Enabled = False
        Me.btnExtractDCX.Location = New System.Drawing.Point(240, 38)
        Me.btnExtractDCX.Name = "btnExtractDCX"
        Me.btnExtractDCX.Size = New System.Drawing.Size(80, 23)
        Me.btnExtractDCX.TabIndex = 34
        Me.btnExtractDCX.Text = "Extract DCXs"
        Me.btnExtractDCX.UseVisualStyleBackColor = True
        '
        'btnDeleteDCX
        '
        Me.btnDeleteDCX.Enabled = False
        Me.btnDeleteDCX.Location = New System.Drawing.Point(240, 67)
        Me.btnDeleteDCX.Name = "btnDeleteDCX"
        Me.btnDeleteDCX.Size = New System.Drawing.Size(80, 23)
        Me.btnDeleteDCX.TabIndex = 35
        Me.btnDeleteDCX.Text = "Delete DCXs"
        Me.btnDeleteDCX.UseVisualStyleBackColor = True
        '
        'btnExtractFRPG
        '
        Me.btnExtractFRPG.Enabled = False
        Me.btnExtractFRPG.Location = New System.Drawing.Point(326, 38)
        Me.btnExtractFRPG.Name = "btnExtractFRPG"
        Me.btnExtractFRPG.Size = New System.Drawing.Size(80, 23)
        Me.btnExtractFRPG.TabIndex = 36
        Me.btnExtractFRPG.Text = "Extract FRPG"
        Me.btnExtractFRPG.UseVisualStyleBackColor = True
        '
        'frmModPrep
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(832, 356)
        Me.Controls.Add(Me.btnExtractFRPG)
        Me.Controls.Add(Me.btnDeleteDCX)
        Me.Controls.Add(Me.btnExtractDCX)
        Me.Controls.Add(Me.btnExtractBNDs)
        Me.Controls.Add(Me.txtInfo)
        Me.Controls.Add(Me.btnModify)
        Me.Controls.Add(Me.txtEXEfile)
        Me.Controls.Add(Me.lblGAFile)
        Me.Name = "frmModPrep"
        Me.Text = "Wulf's Dark Souls Mod Prepper"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtEXEfile As TextBox
    Friend WithEvents lblGAFile As Label
    Friend WithEvents btnModify As Button
    Friend WithEvents txtInfo As TextBox
    Friend WithEvents btnExtractBNDs As Button
    Friend WithEvents btnExtractDCX As Button
    Friend WithEvents btnDeleteDCX As Button
    Friend WithEvents btnExtractFRPG As Button
End Class
