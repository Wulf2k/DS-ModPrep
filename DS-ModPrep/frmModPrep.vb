Imports System.IO
Imports System.IO.Compression

Public Class frmModPrep
    Public EXEver As String = "unknown"
    Public dataPath As String = ""
    Public bigEndian = False

    Private Sub WriteBytes(ByRef fs As FileStream, ByVal loc As Integer, ByVal byt() As Byte)
        fs.Position = loc
        For i = 0 To byt.Length - 1
            fs.WriteByte(byt(i))
        Next
    End Sub

    Private Sub CreateFolders()
        If Not Directory.Exists("C:\FRPG") Then
            Directory.CreateDirectory("C:\FRPG")
            Directory.CreateDirectory("C:\FRPG\data")
            Directory.CreateDirectory("C:\FRPG\data\CAPTURE")
            Directory.CreateDirectory("C:\FRPG\data\CAPTURE\breakobj")
            Directory.CreateDirectory("C:\FRPG\data\CAPTURE\dbgreport")
            Directory.CreateDirectory("C:\FRPG\data\CAPTURE\EnvLightMap")
            Directory.CreateDirectory("C:\FRPG\data\CAPTURE\event")
            Directory.CreateDirectory("C:\FRPG\data\CAPTURE\mapstudio")
            Directory.CreateDirectory("C:\FRPG\data\CAPTURE\param")
            Directory.CreateDirectory("C:\FRPG\data\CAPTURE\Replay")
            Directory.CreateDirectory("C:\FRPG\data\CAPTURE\screenshot")
            Directory.CreateDirectory("C:\FRPG\data\CAPTURE\win32savedata")
            Directory.CreateDirectory("C:\FRPG\data\INTERROOT_win32")
            Directory.CreateDirectory("C:\FRPG\data\INTERROOT_win32\script")
            Directory.CreateDirectory("C:\FRPG\data\INTERROOT_win32\script\ai")
            Directory.CreateDirectory("C:\FRPG\data\INTERROOT_win32\script\ai\out")
            Directory.CreateDirectory("C:\FRPG\data\INTERROOT_win32\script\talk")
            Directory.CreateDirectory("C:\FRPG\data\INTERROOT_win32\sfx")
            Directory.CreateDirectory("C:\FRPG\data\INTERROOT_win32\sfx\effects")
            Directory.CreateDirectory("C:\FRPG\data\INTERROOT_win32\sfx\lua")
            Directory.CreateDirectory("C:\FRPG\data\INTERROOT_win32\sfx\model")
            Directory.CreateDirectory("C:\FRPG\data\INTERROOT_win32\sfx\tex")
            Directory.CreateDirectory("C:\FRPG\data\INTERROOT_win32\sfx\debug")
            Directory.CreateDirectory(dataPath & "\testdata")
            txtInfo.Text = txtInfo.Text & "Created c:\FRPG and game:/testdata folders" & Environment.NewLine
        End If
    End Sub
    Private Sub modReleaseEXE(ByRef EXEstream As FileStream)
        Dim byt() As Byte

        If Not File.Exists(txtEXEfile.Text & ".release.bak") Then
            File.Copy(txtEXEfile.Text, txtEXEfile.Text & ".release.bak")
            txtInfo.Text = txtInfo.Text & "Created " & txtEXEfile.Text & ".release.bak" & Environment.NewLine
        Else
            txtInfo.Text = txtInfo.Text & txtEXEfile.Text & ".release.bak already exists" & Environment.NewLine
        End If


        byt = System.Text.Encoding.Unicode.GetBytes("dvdroot:")

        'DVDBND0:
        WriteBytes(EXEstream, &HD65EA4, byt)
        WriteBytes(EXEstream, &HD660F8, byt)
        WriteBytes(EXEstream, &HD66180, byt)
        WriteBytes(EXEstream, &HD6627C, byt)
        WriteBytes(EXEstream, &HD662C8, byt)
        WriteBytes(EXEstream, &HD66318, byt)
        WriteBytes(EXEstream, &HD66C90, byt)

        'DVDBND1:
        WriteBytes(EXEstream, &HD57F14, byt)
        WriteBytes(EXEstream, &HD65DAC, byt)
        WriteBytes(EXEstream, &HD65DF0, byt)
        WriteBytes(EXEstream, &HD65FFC, byt)
        WriteBytes(EXEstream, &HD6613C, byt)
        WriteBytes(EXEstream, &HD6636C, byt)
        WriteBytes(EXEstream, &HD66484, byt)
        WriteBytes(EXEstream, &HD665F0, byt)
        WriteBytes(EXEstream, &HD666E4, byt)

        'DVDBND2:
        WriteBytes(EXEstream, &HD66734, byt)
        WriteBytes(EXEstream, &HD667C4, byt)
        WriteBytes(EXEstream, &HD66EAC, byt)

        'DVDBND3:
        WriteBytes(EXEstream, &HD65E34, byt)
        WriteBytes(EXEstream, &HD663B0, byt)
        WriteBytes(EXEstream, &HD66434, byt)


        byt = System.Text.Encoding.Unicode.GetBytes("C:")

        'N:
        WriteBytes(EXEstream, &HD65BB4, byt)
        WriteBytes(EXEstream, &HD65C3C, byt)
        WriteBytes(EXEstream, &HD6FBA0, byt)
    End Sub
    Private Sub modDebugEXE(ByRef EXEstream As FileStream)
        Dim byt() As Byte
        If Not File.Exists(txtEXEfile.Text & ".debug.bak") Then
            File.Copy(txtEXEfile.Text, txtEXEfile.Text & ".debug.bak")
            txtInfo.Text = txtInfo.Text & "Created " & txtEXEfile.Text & ".debug.bak" & Environment.NewLine
        Else
            txtInfo.Text = txtInfo.Text & txtEXEfile.Text & ".debug.bak already exists" & Environment.NewLine
        End If



        byt = System.Text.Encoding.Unicode.GetBytes("dvdroot:")

        'DVDBND0:
        WriteBytes(EXEstream, &HD6816C, byt)
        WriteBytes(EXEstream, &HD683C0, byt)
        WriteBytes(EXEstream, &HD68448, byt)
        WriteBytes(EXEstream, &HD68544, byt)
        WriteBytes(EXEstream, &HD68590, byt)
        WriteBytes(EXEstream, &HD685E0, byt)
        WriteBytes(EXEstream, &HD68F58, byt)

        'DVDBND1:
        WriteBytes(EXEstream, &HD5C2D4, byt)
        WriteBytes(EXEstream, &HD68074, byt)
        WriteBytes(EXEstream, &HD680BC, byt)
        WriteBytes(EXEstream, &HD682C4, byt)
        WriteBytes(EXEstream, &HD68404, byt)
        WriteBytes(EXEstream, &HD68634, byt)
        WriteBytes(EXEstream, &HD6874C, byt)
        WriteBytes(EXEstream, &HD688B8, byt)
        WriteBytes(EXEstream, &HD689AC, byt)

        'DVDBND2:
        WriteBytes(EXEstream, &HD689FC, byt)
        WriteBytes(EXEstream, &HD68A8C, byt)
        WriteBytes(EXEstream, &HD69174, byt)

        'DVDBND3:
        WriteBytes(EXEstream, &HD680FC, byt)
        WriteBytes(EXEstream, &HD68678, byt)
        WriteBytes(EXEstream, &HD686FC, byt)


        byt = System.Text.Encoding.Unicode.GetBytes("C:")

        'N:
        WriteBytes(EXEstream, &HD67E7C, byt)
        WriteBytes(EXEstream, &HD67F04, byt)
        WriteBytes(EXEstream, &HD71ED0, byt)

    End Sub
    Private Sub btnModify_Click(sender As Object, e As EventArgs) Handles btnModify.Click
        CreateFolders()

        Dim EXEstream = New IO.FileStream(txtEXEfile.Text, IO.FileMode.Open)
        Select Case EXEver
            Case "release"
                modReleaseEXE(EXEstream)
            Case "debug"
                modDebugEXE(EXEstream)
        End Select
        EXEstream.Dispose()


    End Sub




    Private Sub txt_Drop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtEXEfile.DragDrop
        Dim file() As String = e.Data.GetData(DataFormats.FileDrop)
        sender.Text = file(0)

        Dim EXEstream = New IO.FileStream(file(0), IO.FileMode.Open)
        EXEstream.Position = &H80

        Dim verCheck As Byte
        verCheck = EXEstream.ReadByte
        btnModify.Enabled = True
        btnExtractBNDs.Enabled = True
        btnExtractDCX.Enabled = True
        btnDeleteDCX.Enabled = True
        btnExtractFRPG.Enabled = True

        dataPath = Microsoft.VisualBasic.Left(txtEXEfile.Text, InStrRev(txtEXEfile.Text, "\"))

        Select Case verCheck
            Case &H54
                EXEver = "release"
            Case &HB4
                EXEver = "debug"
            Case &HE2
                EXEver = "beta"
                btnModify.Enabled = False
                btnExtractBNDs.Enabled = False
                btnExtractDCX.Enabled = False
                btnDeleteDCX.Enabled = False
                btnExtractFRPG.Enabled = False
            Case Else
                EXEver = "unknown"
                btnModify.Enabled = False
                btnExtractBNDs.Enabled = False
                btnExtractDCX.Enabled = False
                btnDeleteDCX.Enabled = False
                btnExtractFRPG.Enabled = False
        End Select

        txtInfo.Text = txtInfo.Text & "EXE type: " & EXEver & Environment.NewLine
        EXEstream.Dispose()
    End Sub
    Private Sub txt_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtEXEfile.DragEnter
        e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub ExtractBND3(ByRef filename As String)
        Dim BNDstream As New IO.FileStream(filename, IO.FileMode.Open)

        Dim BNDoffset As Integer = 0

        Dim currFileSize As UInteger = 0
        Dim currFileOffset As UInteger = 0
        Dim currFileID As UInteger = 0
        Dim currFileNameOffset As UInteger = 0
        Dim currFileBytes() As Byte = {}
        Dim currFileName As String = ""
        Dim currFilePath As String = ""

        bigEndian = False

        Dim flags As UInteger = 0
        Dim numfiles As UInteger = 0
        Dim namesEndLoc As UInteger = 0



        flags = UInt32FromStream(BNDstream, &HC)

        numfiles = UInt32FromStream(BNDstream, &H10)
        namesEndLoc = UInt32FromStream(BNDstream, &H14)

        If numfiles > 0 Then
            Select Case flags
                Case &H70
                    For i As UInteger = 0 To numfiles - 1
                        currFileSize = UInt32FromStream(BNDstream, &H24 + i * &H14)
                        currFileOffset = UInt32FromStream(BNDstream, &H28 + i * &H14)
                        currFileID = UInt32FromStream(BNDstream, &H2C + i * &H14)
                        currFileNameOffset = UInt32FromStream(BNDstream, &H30 + i * &H14)
                        currFileName = ASCIIStrFromStream(BNDstream, currFileNameOffset)

                        currFileName = "C:\" & Microsoft.VisualBasic.Right(currFileName, currFileName.Length - &H3)
                        currFilePath = Microsoft.VisualBasic.Left(currFileName, InStrRev(currFileName, "\"))

                        If (Not System.IO.Directory.Exists(currFilePath)) Then
                            System.IO.Directory.CreateDirectory(currFilePath)
                        End If

                        ReDim currFileBytes(currFileSize - 1)

                        BNDstream.Position = currFileOffset
                        For k = 0 To currFileSize - 1
                            currFileBytes(k) = BNDstream.ReadByte
                        Next

                        File.WriteAllBytes(currFileName, currFileBytes)
                    Next

                Case &H74, &H54
                    For i As UInteger = 0 To numfiles - 1
                        currFileSize = UInt32FromStream(BNDstream, &H24 + i * &H18)
                        currFileOffset = UInt32FromStream(BNDstream, &H28 + i * &H18)
                        currFileID = UInt32FromStream(BNDstream, &H2C + i * &H18)
                        currFileNameOffset = UInt32FromStream(BNDstream, &H30 + i * &H18)
                        currFileName = ASCIIStrFromStream(BNDstream, currFileNameOffset)

                        currFileName = "C:\" & Microsoft.VisualBasic.Right(currFileName, currFileName.Length - &H3)
                        currFilePath = Microsoft.VisualBasic.Left(currFileName, InStrRev(currFileName, "\"))

                        If (Not System.IO.Directory.Exists(currFilePath)) Then
                            System.IO.Directory.CreateDirectory(currFilePath)
                        End If

                        ReDim currFileBytes(currFileSize - 1)

                        BNDstream.Position = currFileOffset
                        For k = 0 To currFileSize - 1
                            currFileBytes(k) = BNDstream.ReadByte
                        Next

                        File.WriteAllBytes(currFileName, currFileBytes)
                    Next
            End Select
        End If

    End Sub
    Private Sub ExtractBHD5(ByRef filename As String)
        Dim BHDstream As New IO.FileStream(dataPath & filename & ".bhd5", IO.FileMode.Open)
        Dim BDTstream As New IO.FileStream(dataPath & filename & ".bdt", IO.FileMode.Open)

        Dim BHDoffset As Integer = 0

        Dim numFiles As Integer = 0

        Dim currFileSize As Integer = 0
        Dim currFileOffset As Integer = 0
        Dim currFileID As Integer = 0
        Dim currFileNameOffset As Integer = 0
        Dim currFileBytes() As Byte = {}
        Dim currFileName As String = ""
        Dim currFilePath As String = ""

        Dim count As Integer = 0

        Dim idx As Integer
        Dim fileidx() As String = My.Resources.fileidx.Replace(Chr(&HD), "").Split(Chr(&HA))
        Dim hashidx(fileidx.Length - 1) As UInteger

        For i = 0 To fileidx.Length - 1
            hashidx(i) = HashFileName(fileidx(i))
        Next


        bigEndian = False

        numFiles = Int32FromStream(BHDstream, &H10)

        For i As Integer = 0 To numFiles - 1
            count = Int32FromStream(BHDstream, &H18 + i * &H8)
            BHDoffset = Int32FromStream(BHDstream, &H1C + i * 8)

            For j = 0 To count - 1
                currFileSize = Int32FromStream(BHDstream, BHDoffset + &H4)
                currFileOffset = Int32FromStream(BHDstream, BHDoffset + &H8)

                ReDim currFileBytes(currFileSize - 1)

                BDTstream.Position = currFileOffset
                For k = 0 To currFileSize - 1
                    currFileBytes(k) = BDTstream.ReadByte
                Next

                currFileName = ""

                If hashidx.Contains(UInt32FromStream(BHDstream, BHDoffset)) Then
                    idx = Array.IndexOf(hashidx, UInt32FromStream(BHDstream, BHDoffset))
                    currFileName = fileidx(idx)
                    currFileName = currFileName.Replace("/", "\")
                Else
                    currFileName = "\NOMATCH\" & Hex(BHDoffset)
                End If

                currFileName = Microsoft.VisualBasic.Left(dataPath, dataPath.Length - 1) & currFileName
                currFilePath = Microsoft.VisualBasic.Left(currFileName, InStrRev(currFileName, "\"))

                If (Not System.IO.Directory.Exists(currFilePath)) Then
                    System.IO.Directory.CreateDirectory(currFilePath)
                End If

                File.WriteAllBytes(currFileName, currFileBytes)

                BHDoffset += &H10
            Next
        Next
        BDTstream.Dispose()
        BHDstream.Dispose()
    End Sub
    Private Sub ExtractDFLT(ByRef filename As String)
        Dim DCXstream As New IO.FileStream(filename, IO.FileMode.Open)
        Dim currFileName As String = ""
        bigEndian = True

        Dim startOffset As UInteger = Int32FromStream(DCXstream, &H14) + &H22

        Dim newbytes(Int32FromStream(DCXstream, &H20) - 1) As Byte
        Dim decbytes(Int32FromStream(DCXstream, &H1C)) As Byte

        DCXstream.Position = startOffset
        For i = 0 To newbytes.Length - 2
            newbytes(i) = DCXstream.ReadByte

        Next

        decbytes = Decompress(newbytes)

        currFileName = Microsoft.VisualBasic.Left(filename, filename.Length - &H4)

        File.WriteAllBytes(currFileName, decbytes)

        DCXstream.Dispose()
    End Sub

    Private Sub btnExtractBNDs_Click(sender As Object, e As EventArgs) Handles btnExtractBNDs.Click
        ExtractBHD5("dvdbnd0")
        txtInfo.Text = txtInfo.Text & "DVDBND0 extracted." & Environment.NewLine

        ExtractBHD5("dvdbnd1")
        txtInfo.Text = txtInfo.Text & "DVDBND1 extracted." & Environment.NewLine

        ExtractBHD5("dvdbnd2")
        txtInfo.Text = txtInfo.Text & "DVDBND2 extracted." & Environment.NewLine

        ExtractBHD5("dvdbnd3")
        txtInfo.Text = txtInfo.Text & "DVDBND3 extracted." & Environment.NewLine

    End Sub
    Private Function HashFileName(filename As String) As UInteger

        REM This code copied from https://github.com/Burton-Radons/Alexandria

        If filename Is Nothing Then
            Return 0
        End If

        Dim hash As UInteger = 0

        For Each ch As Char In filename
            hash = hash * &H25 + Asc(Char.ToLowerInvariant(ch))
        Next

        Return hash
    End Function
    Private Function ASCIIStrFromStream(ByRef fs As FileStream, ByVal loc As UInteger) As String
        Dim Str As String = ""
        Dim cont As Boolean = True
        Dim byt As Byte

        fs.Position = loc

        While cont
            byt = fs.ReadByte

            If byt > 0 Then
                Str = Str + Convert.ToChar(byt)
            Else
                cont = False
            End If
        End While

        Return Str
    End Function
    Private Function Int32FromStream(ByRef fs As FileStream, ByVal loc As Integer) As Integer
        Dim tmpInt As Integer = 0
        Dim byt As Byte

        fs.Position = loc

        If bigEndian Then
            For i = 0 To 3
                byt = fs.ReadByte
                tmpInt += Convert.ToInt32(byt) * &H100 ^ (3 - i)
            Next
        Else
            For i = 0 To 3
                byt = fs.ReadByte
                tmpInt += Convert.ToInt32(byt) * &H100 ^ i
            Next
        End If


        Return tmpInt
    End Function
    Private Function UInt32FromStream(ByRef fs As FileStream, ByVal loc As Integer) As UInteger
        Dim tmpUInt As UInteger = 0
        Dim byt As Byte

        fs.Position = loc
        If bigEndian Then
            For i = 0 To 3
                byt = fs.ReadByte
                tmpUInt += Convert.ToUInt32(byt) * &H100 ^ (3 - i)
            Next
        Else
            For i = 0 To 3
                byt = fs.ReadByte
                tmpUInt += Convert.ToUInt32(byt) * &H100 ^ i
            Next
        End If


        Return tmpUInt
    End Function
    Public Function Decompress(ByVal cmpBytes() As Byte) As Byte()
        Dim sourceFile As MemoryStream = New MemoryStream(cmpBytes)
        Dim destFile As MemoryStream = New MemoryStream()
        Dim compStream As New DeflateStream(sourceFile, CompressionMode.Decompress)
        Dim myByte As Integer = compStream.ReadByte()

        While myByte <> -1
            destFile.WriteByte(CType(myByte, Byte))
            myByte = compStream.ReadByte()
        End While

        destFile.Close()
        sourceFile.Close()

        Return destFile.ToArray
    End Function
    Private Sub btnExtractDCX_Click(sender As Object, e As EventArgs) Handles btnExtractDCX.Click
        Dim dcxlist() As String = Directory.GetFiles(dataPath, "*.dcx", SearchOption.AllDirectories)

        For Each dcx In dcxlist
            ExtractDFLT(dcx)
        Next

        txtInfo.Text = txtInfo.Text & "DCXs extracted" & Environment.NewLine
    End Sub
    Private Sub btnDeleteDCX_Click(sender As Object, e As EventArgs) Handles btnDeleteDCX.Click
        Dim dcxlist() As String = Directory.GetFiles(dataPath, "*.dcx", SearchOption.AllDirectories)

        For Each dcx In dcxlist
            File.Delete(dcx)
        Next

        txtInfo.Text = txtInfo.Text & "DCXs deleted" & Environment.NewLine
    End Sub

    Private Sub btnExtractFRPG_Click(sender As Object, e As EventArgs) Handles btnExtractFRPG.Click

        'excluded for now:
        'remobnd
        'chrtpfbdt
        'tpf
        'tpfBHD
        'hkxbhd
        'shaderbnd

        Dim list() As String
        list = {"*.anibnd", "*.chrbnd", "*.chresdbnd", "*.fgbnd", "*.nvmbnd", "partsbnd", "*.luabnd", "*.talkesdbnd",
            "*.msgbnd", "*,mtdbnd", "*.objbnd", "*.rumblebnd", "*.parambnd", "*.paramdefbnd", "*.ffxbnd"}

        For Each bndtype In list
            Dim bndlist() As String = Directory.GetFiles(dataPath, bndtype, SearchOption.AllDirectories)

            For Each bnd In bndlist
                ExtractBND3(bnd)
            Next
        Next


        txtInfo.Text = txtInfo.Text & "FRPG populated" & Environment.NewLine
    End Sub
End Class




'List of Folder Aliases
'game = system:/
'debug = game:/
'capture = N:/FRPG/data/CAPTURE
'dvdroot
'interroot = N:/FRPG/data/INTERROOT_win32
'model = dvdroot:/testdata
'animation
'texture
'material
'loadlist
'shader = dvdbnd1:/shader
'font = dvdbnd1:/font
'msg = dvdbnd3:/msg
'msgpatch
'chr = dvdbnd0:/chr
'chrtpf
'chrflver
'chranibnd
'chrhkx
'chresdpatch
'obj = dvdbnd1:/obj
'objflver
'objtpf
'objhkx
'objanibnd
'item = dvdbnd0:/item
'parts = dvdbnd1:/parts
'map = dvdbnd0:/map
'mappatch
'maptpf
'mapflver
'maphkx
'remo = dvdbnd0:/remo
'breakobj = 'dvdbnd0:/map/breakobj
'msb = dvdbnd0:/map/mapstudio
'mtd = dvdbnd1:/mtd
'param = dvdbnd3:/param
'parampatch
'paramdef = dvdbnd3:/paramdef
'facegen = dvdbnd1:/facegen
'movjp = dvdroot:/movjp
'movna = dvdroot:/movna
'moveu = dvdroot:/moveu
'movww = dvdroot:/movww
'sound = dvdbnd1:/sound
'sndmap
'sndchr
'sndremo
'fmod
'other = dvdbnd1:/other
'luascript = dvdbnd2:/script
'luascriptpatch
'talkscript = dvdbnd2:/script/talk
'talkscriptpatch
'navimesh
'dbgscript = interroot:/script
'dbgai = interroot:/script/ai
'cap_param = capture:/param
'cap_breakobj = capture:/breakobj
'cap_screenshot = capture:/screenshot
'cap_dbgrep = capture:/dbgreport
'cap_mapstudio = capture:/mapstudio
'cap_EnvLightMap = capture:/EnvLightMap
'replay = capture:/Replay
'config = dvdroot:/
'ffx = interroot:/sfx/effects
'ffxlua = interroot:/sfx/lua
'ffxbnd = dvdbnd0:/sfx
'ffxbndpatch
'ffxmodel = interroot:/sfx/model
'ffxtex = interroot:/sfx/tex
'ffxdebug = interroot:/sfx/debug
'menutexture
'menutexpatch
'menuesd
'event = dvdbnd2:/event
'eventpatch
'cap_event = capture:/event
'fmod_dlc
'sound_dlc
'sndmap_dlc
'sndchr_dlc
'sndremo_dlc
'maptpf_dlc
'map_dlc
'mtd_dlc
'remobnd_dlc
'menu
'menutex_dlc
'menuesd_dlc
'other_dlc
'chranibnd_dlc
'breakobj_dlc