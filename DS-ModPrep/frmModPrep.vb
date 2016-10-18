Imports System.IO
Imports System.IO.Compression
Imports System.Reflection

Public Enum ExeType
    Unknown
    ReleaseVanilla
    ReleaseModded
    DebugVanilla
    DebugModded
    Beta
End Enum

Public Class frmModPrep
    Shared WithEvents maxProgSnapTimer As System.Timers.Timer
    Public Const RelDarkSoulsDir = "Dark Souls Prepare to Die Edition\DATA"
    Public Const SteamGames_Library = "SteamLibrary\steamapps\common"
    Public Const SteamGames_Main = "Program Files (x86)\Steam\steamapps\common"
    Public Shared ReadOnly ExeTypeNames = New Dictionary(Of ExeType, String) From
        {
            {ExeType.ReleaseVanilla, "Dark Souls (Release Ver.)"},
            {ExeType.DebugVanilla, "Dark Souls (Debug Ver.)"}
        }

    Public Shared ReadOnly MD5Hashes = New Dictionary(Of ExeType, String) From
        {
            {ExeType.ReleaseVanilla, "E62519C1DAA8D90AEF82128DA955B509"},
            {ExeType.DebugVanilla, "6A4A9B3EFF57368B72708A283AFEFC50"}
        }

    Shared bigEndian = False
    Shared dataPath As String = ""
    Shared progressSnapInterval = 1000
    Shared realCurrentProgress As Integer = 0
    Shared realCurrentProgressMax As Integer = 0
    Shared selectedExeType As ExeType = ExeType.Unknown
    Public Sub New()
        InitializeComponent()

        lblProgCurFile.Text = ""
        lblProgCurFile.Visible = False
        lblProgOperation.Text = ""
        lblProgOperation.Visible = False

        maxProgSnapTimer = New System.Timers.Timer
        maxProgSnapTimer.Enabled = False
        maxProgSnapTimer.Interval = progressSnapInterval
        maxProgSnapTimer.AutoReset = False

        'Automatically writes in the build date!
        txtInfo.Text = "DS-ModPrep Build " & Assembly.GetCallingAssembly().GetLinkerTime().ToUniversalTime() & " (UTC)" & Environment.NewLine & "GitHub Repository: https://github.com/Wulf2k/DS-ModPrep"
    End Sub

    Shared Async Sub maxProgSnapTimer_Elapsed(source As Object, e As Timers.ElapsedEventArgs) Handles maxProgSnapTimer.Elapsed
        Await Task.Delay(progressSnapInterval)
    End Sub
    Public Async Function DecompressAsync(ByVal cmpBytes() As Byte) As Task(Of Byte())
        Return Await Task.Run(
            Async Function() As Task(Of Byte())
                Dim sourceFile As MemoryStream = New MemoryStream(cmpBytes)
                Dim destFile As MemoryStream = New MemoryStream()

                Dim compStream As New DeflateStream(sourceFile, CompressionMode.Decompress)

                Await compStream.CopyToAsync(destFile)

                Dim arr = destFile.ToArray

                destFile.Dispose()
                sourceFile.Dispose()

                Return arr
            End Function)
    End Function

    Public Async Function ScanForDarkSoulsDataDirAsync() As Task

        Dim steamInstallDirs = Await GetAllSteamInstallLocationsAsync()

        Await Task.Run(
            Async Function()
                For Each d In steamInstallDirs
                    Dim possibleDarkSoulsPath = Path.Combine(d, RelDarkSoulsDir)

                    If Directory.Exists(possibleDarkSoulsPath) Then
                        Dim allExes = Directory.GetFiles(possibleDarkSoulsPath, "*.exe", SearchOption.TopDirectoryOnly)
                        For Each e In allExes
                            If e.ToUpper.Contains("DARK") And e.ToUpper.Contains("SOULS") Then 'I think this file might be Dark Souls, I'm not 100% sure though ( ͡° ͜ʖ ͡°)
                                If Await CheckAndSetFileTxtAsync(e, False) Then
                                    Return
                                End If
                            End If

                        Next
                    End If
                Next
            End Function)

    End Function

    Async Function SetEnabledAsync(ctrl As Control, enabled As Boolean) As Task
        Await Task.Run(
            Sub()
                If ctrl.Enabled = enabled Then
                    Return
                End If

                If ctrl.InvokeRequired Then
                    ctrl.BeginInvoke(Sub() ctrl.Enabled = enabled)
                Else
                    ctrl.Enabled = enabled
                End If
            End Sub)
    End Function

    Async Function SetMaxAsync(prog As ProgressBar, newMax As Integer) As Task
        Await Task.Run(
            Sub()
                If prog.Maximum = newMax Then
                    Return
                End If

                If prog.InvokeRequired Then
                    prog.BeginInvoke(Sub() prog.Maximum = newMax)
                Else
                    prog.Maximum = newMax
                End If
            End Sub)
    End Function

    Async Function SetTextAsync(textBox As TextBox, newText As String) As Task
        Await Task.Run(
            Sub()
                If textBox.Text = newText Then
                    Return
                End If
                If textBox.InvokeRequired Then
                    textBox.BeginInvoke(Sub() textBox.Text = newText)
                Else
                    textBox.Text = newText
                End If
            End Sub)
    End Function

    Async Function SetTextAsync(lbl As Label, newText As String) As Task
        Await Task.Run(
            Sub()
                If lbl.Text = newText Then
                    Return
                End If
                If lbl.InvokeRequired Then
                    lbl.BeginInvoke(Sub() lbl.Text = newText)
                Else
                    lbl.Text = newText
                End If
            End Sub)
    End Function

    Async Function SetUseWaitCursorAsync(ctrl As Control, useWaitCursor As Boolean) As Task
        Await Task.Run(
            Sub()
                If ctrl.UseWaitCursor = useWaitCursor Then
                    Return
                End If

                If ctrl.InvokeRequired Then
                    ctrl.BeginInvoke(Sub() ctrl.UseWaitCursor = useWaitCursor)
                Else
                    ctrl.UseWaitCursor = useWaitCursor
                End If
            End Sub)
    End Function

    Async Function SetValueAsync(prog As ProgressBar, newValue As Integer) As Task
        Await Task.Run(
            Sub()
                If prog.Value = newValue Then
                    Return
                End If

                If newValue > prog.Maximum Then
                    newValue = prog.Maximum
                End If

                If prog.InvokeRequired Then
                    prog.BeginInvoke(Sub() prog.Value = newValue)
                Else
                    prog.Value = newValue
                End If
            End Sub)
    End Function

    Async Function SetVisibleAsync(ctrl As Control, visible As Boolean) As Task
        Await Task.Run(
            Sub()
                If ctrl.Visible = visible Then
                    Return
                End If

                If ctrl.InvokeRequired Then
                    ctrl.BeginInvoke(Sub() ctrl.Visible = visible)
                Else
                    ctrl.Visible = visible
                End If
            End Sub)
    End Function

    Private Async Function AddTextLineAsync(line As String, Optional replacePrevLine As Boolean = False) As Task
        Await Task.Run(
            Sub()
                If txtInfo.InvokeRequired Then
                    txtInfo.Invoke(
                    Sub()
                        If replacePrevLine Then
                            txtInfo.SelectionStart = txtInfo.GetFirstCharIndexOfCurrentLine()
                            txtInfo.SelectionLength = txtInfo.TextLength - txtInfo.SelectionStart
                            txtInfo.SelectedText = line
                        Else
                            txtInfo.SelectionStart = txtInfo.TextLength
                            txtInfo.SelectedText = Environment.NewLine & line
                        End If
                    End Sub)
                Else
                    If replacePrevLine Then
                        txtInfo.SelectionStart = txtInfo.GetFirstCharIndexOfCurrentLine()
                        txtInfo.SelectionLength = txtInfo.TextLength - txtInfo.SelectionStart
                        txtInfo.SelectedText = line
                    Else
                        txtInfo.SelectionStart = txtInfo.TextLength
                        txtInfo.SelectedText = Environment.NewLine & line
                    End If
                End If
            End Sub)
    End Function

    Private Async Function AppendCurrentProgressMaxAsync(max As Integer) As Task
        realCurrentProgressMax += max

        Await UpdateVisibleProgressAsync()
    End Function

    Private Async Function AppendOperationProgressMaxAsync(max As Integer) As Task
        Await SetMaxAsync(progOperation, progOperation.Maximum + max)

        Await UpdateVisibleProgressAsync()
    End Function

    Private Async Function AppendProgressAsync(prog As Integer) As Task
        Await Task.Run(
        Async Function()
            Dim increaseOperationProgMax = 0

            'Don't want bad int truncation when dividing to make the bar stop.
            If (prog < 1) Then
                prog = 1
            End If

            If (realCurrentProgress + prog) > realCurrentProgressMax Then
                Dim oldMax = realCurrentProgressMax
                realCurrentProgressMax += prog
                realCurrentProgress += prog
                increaseOperationProgMax = realCurrentProgressMax - oldMax
            Else
                realCurrentProgress += prog
            End If

            Await SetMaxAsync(progOperation, progOperation.Maximum + increaseOperationProgMax)
            Await SetValueAsync(progOperation, progOperation.Value + prog)

            Await UpdateVisibleProgressAsync()
        End Function)

    End Function

    Private Async Function ASCIIStrFromStreamAsync(fs As Stream, ByVal loc As UInteger) As Task(Of String)
        Dim byt = New Byte() {0}

        Dim Str As String = ""

        fs.Position = loc

        Await Task.Run(
            Async Function()
                While fs.Position < fs.Length
                    Await fs.ReadAsync(byt, 0, 1)
                    If byt(0) > 0 Then
                        Str = Str + Convert.ToChar(byt(0))
                    Else
                        Exit While
                    End If
                End While
            End Function)

        Return str
    End Function

    Private Async Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Await ShowFileSelectDlgAsync()
    End Sub

    Private Async Sub btnDeleteBHDs_Click(sender As Object, e As EventArgs) Handles btnDeleteBHDs.Click
        'TODO: Implement this and enable button
        Await Task.Delay(1) 'Gets VS to stop warning us about this method not using Await
    End Sub

    Private Async Sub btnDeleteBNDs_Click(sender As Object, e As EventArgs) Handles btnDeleteBNDs.Click
        'TODO: Implement this and enable button
        Await Task.Delay(1) 'Gets VS to stop warning us about this method not using Await
    End Sub

    Private Async Sub btnDeleteDCX_Click(sender As Object, e As EventArgs) Handles btnDeleteDCX.Click
        Dim dcxlist() As String = Directory.GetFiles(dataPath, "*.dcx", SearchOption.AllDirectories)
        Await SetLoadingAsync(True)
        Await Task.Run(
            Async Function()
                Await AddTextLineAsync("Scanning DCX files...")

                Await AppendOperationProgressMaxAsync(dcxlist.Sum(Function(x) New FileInfo(x).Length / 10000))

                Await AddTextLineAsync("Deleting all DCX files...")
                Dim curFile = 0
                'No way to tell current progress on File.Delete
                For Each dcx In dcxlist
                    Await AddTextLineAsync("Deleting DCX [File " & curFile & "/" & dcxlist.Count & "]...", True)
                    Await ResetProgCurAsync()
                    Dim fileLength = (New FileInfo(dcx).Length)
                    Await AppendCurrentProgressMaxAsync(fileLength / 10000)
                    File.Delete(dcx)
                    Await SetProgressAsync(fileLength / 10000)
                    curFile += 1
                Next
            End Function)
        Await SetLoadingAsync(False)
        Await AddTextLineAsync("DCXs deleted.")
    End Sub

    Private Async Sub btnDeleteFRPG_Click(sender As Object, e As EventArgs) Handles btnDeleteFRPG.Click
        'TODO: Implement this and enable button
        Await Task.Delay(1) 'Gets VS to stop warning us about this method not using Await
    End Sub

    Private Async Sub btnExtractBHDs_Click(sender As Object, e As EventArgs) Handles btnExtractBHDs.Click
        'TODO: Finish implementing this and enable button
        'finish tpfbhd
        'don't forget chrbnds contain tpf headers




        Await ResetProgressBarsAsync()
        Await SetLoadingAsync(True)
        Await Task.Run(
            Async Function()
                Dim list() as String
                list = {"*.tpfbhd", "*.chrtpfbhd", "*.hkxbhd"}
                Dim totalFileList = New List(Of String)

                For Each bndtype In list
                    totalFileList.AddRange(Directory.GetFiles(dataPath, bndtype, SearchOption.AllDirectories))
                Next

                Await AppendOperationProgressMaxAsync(totalFileList.Sum(Function(x) New FileInfo(x).Length))


                For Each bhd In totalFileList
                    Await ExtractBHF3Async(bhd)
                Next
            End Function)
        Await SetLoadingAsync(False)

    End Sub

    Private Async Sub btnExtractBNDs_Click(sender As Object, e As EventArgs) Handles btnExtractBNDs.Click

#If IS_64_BIT Then
        Dim loadBdtIntoMemory = CheckLargeFileSupport()
#Else
        Dim loadBdtIntoMemory = False
#End If
        If loadBdtIntoMemory Then
            Dim dlgRes = MessageBox.Show("Enough free RAM is available to store each large DVDBND. Would you like to load them directly into memory in order to slightly boost extraction speed? (Be sure to close any high memory usage applications if you have <= 8GB of RAM.)" + Environment.NewLine + Environment.NewLine + "Note: This feature is only available on the 64-bit version of DS-ModPrep.", "Load BNDs into RAM?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information)

            If dlgRes = DialogResult.No Then
                loadBdtIntoMemory = False
                Await AddTextLineAsync("Extracting DVDBNDs from drive...")
            ElseIf dlgRes = DialogResult.Cancel Then
                Return
            End If

            Await AddTextLineAsync("Extracting DVDBNDs from RAM...")
        Else
            Await AddTextLineAsync("Extracting DVDBNDs from drive...")
        End If

        Await SetLoadingAsync(True)

        For i As Integer = 0 To 3
            'Divided by 1000 to prevent integer overflow on dvdbnd0 and/or dvdbnd1
            'We have to make sure we divide the progress by 100 as well
            Await AppendOperationProgressMaxAsync(New FileInfo(dataPath & "dvdbnd" & i & ".bdt").Length / 1000)
        Next

        Try
            Await ExtractBHD5Async("dvdbnd0", loadBdtIntoMemory)
            Await AddTextLineAsync("DVDBND0 extracted.")

            Await ExtractBHD5Async("dvdbnd1", loadBdtIntoMemory)
            Await AddTextLineAsync("DVDBND1 extracted.")

            'These two are so small they can always be loaded into memory:
            Await ExtractBHD5Async("dvdbnd2", True)
            Await AddTextLineAsync("DVDBND2 extracted.")

            Await ExtractBHD5Async("dvdbnd3", True)
            Await AddTextLineAsync("DVDBND3 extracted.")
        Catch ex As FileNotFoundException
            MessageBox.Show("Could not find file to extract: """ & ex.FileName & """")
            GC.Collect()
            Return
        Finally
            lblProgCurFile.Visible = False
            lblProgOperation.Visible = False
            progOperation.Value = 0
            progOperation.Maximum = 0
            realCurrentProgress = 0
            realCurrentProgressMax = 0
        End Try

        Await SetLoadingAsync(False)

        Await AddTextLineAsync("All DVDBNDs extracted.")

    End Sub

    Private Async Sub btnExtractDCX_Click(sender As Object, e As EventArgs) Handles btnExtractDCX.Click
        Await AddTextLineAsync("Scanning data directory for DCXs...")
        Await SetLoadingAsync(True)
        Dim dcxlist() As String = Directory.GetFiles(dataPath, "*.dcx", SearchOption.AllDirectories)
        Await AppendOperationProgressMaxAsync(dcxlist.Sum(Function(x) New FileInfo(x).Length) / 1000)

        Await AddTextLineAsync("Extracting all DCXs...")
        'No way to tell current progress on decompression
        Dim curIndex = 1
        For Each dcx In dcxlist
            Await AddTextLineAsync("Extracting DCX " & curIndex & "/" & dcxlist.Length & ": """ & dcx & """...", True)
            Await ExtractDFLTAsync(dcx)
            Await SetProgressAsync(New FileInfo(dcx).Length / 1000)
            curIndex += 1
        Next

        Await SetLoadingAsync(False)

        Await AddTextLineAsync("All DCXs extracted.")
    End Sub

    Private Async Sub btnExtractFRPG_Click(sender As Object, e As EventArgs) Handles btnExtractFRPG.Click

        'excluded for now:
        'remobnd
        'chrtpfbdt
        'tpf
        'tpfBHD
        'hkxbhd
        'shaderbnd
        Await AddTextLineAsync("Extracting all archives to ""C:\FRPG""...")
        Await SetLoadingAsync(True)
        Await Task.Run(
            Async Function()
                Dim list() As String
                list = {"*.anibnd", "*.chrbnd", "*.chresdbnd", "*.fgbnd", "*.nvmbnd", "partsbnd", "*.luabnd", "*.talkesdbnd",
                    "*.msgbnd", "*,mtdbnd", "*.objbnd", "*.rumblebnd", "*.parambnd", "*.paramdefbnd", "*.ffxbnd"}

                Dim totalFileList = New List(Of String)

                For Each bndtype In list
                    totalFileList.AddRange(Directory.GetFiles(dataPath, bndtype, SearchOption.AllDirectories))
                Next
                Await AppendOperationProgressMaxAsync(totalFileList.Sum(Function(x) New FileInfo(x).Length))
                Dim fileNum = 0
                For Each bndFileToExtract In totalFileList
                    Await AddTextLineAsync("Extracting the contents of [File " & fileNum.ToString("000") & "/" & totalFileList.Count.ToString("000") & "] """ & bndFileToExtract & """ to ""C:\FRPG\""...", True)
                    Await ExtractBND3Async(bndFileToExtract)
                    fileNum += 1
                Next
            End Function)
        Await SetLoadingAsync(False)
        Await AddTextLineAsync("C:\FRPG populated.")
    End Sub

    Private Async Sub btnModify_Click(sender As Object, e As EventArgs) Handles btnModify.Click
        Await CreateFrpgFoldersAsync()

        Dim EXEstream = New IO.FileStream(txtEXEfile.Text, IO.FileMode.Open)
        Select Case selectedExeType
            Case ExeType.ReleaseVanilla
                Await SetLoadingAsync(True)
                Await AppendCurrentProgressMaxAsync(31)
                Await AppendOperationProgressMaxAsync(30)
                Await modReleaseEXEAsync(EXEstream)
            Case ExeType.DebugVanilla, ExeType.DebugModded, ExeType.ReleaseModded
                'TODO:  Fix EXE ID
                Await SetLoadingAsync(True)
                Await AppendCurrentProgressMaxAsync(30)
                Await AppendOperationProgressMaxAsync(30)
                Await modDebugEXEAsync(EXEstream)
            Case "never again" 'ExeType.ReleaseModded
                MessageBox.Show("The selected Dark Souls executable has already been modded", "Already Modded", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            Case "never" 'ExeType.DebugModded
                MessageBox.Show("The selected Dark Souls executable has already been modded", "Already Modded", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            Case ExeType.Beta
                MessageBox.Show("The selected Dark Souls executable is not supported in this version of DS-ModPrep", "Not Supported", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
        End Select
        EXEstream.Dispose()
        Await SetLoadingAsync(False)
        Await AddTextLineAsync(txtEXEfile.Text & " has successfully been modified.")
    End Sub

    Private Async Function CheckAndSetFileTxtAsync(file As String, showErrDlg As Boolean) As Task(Of Boolean)
        If CheckDarkSoulsExeVer(file, showErrDlg) Then
            Await SetTextAsync(txtEXEfile, file)
            dataPath = Microsoft.VisualBasic.Left(file, InStrRev(file, "\"))
            Await SetControlsEnabledAsync(True)
            Return True
        Else
            Await SetControlsEnabledAsync(False)
            Return False
        End If
    End Function

    Private Function CheckDarkSoulsExeVer(exe As String, showErrDlg As Boolean)

        Dim validExe = True

        Dim fs As FileStream
        fs = File.Open(exe, FileMode.Open)

        Dim byt as Byte
        
        fs.Position = &H80
        byt = fs.ReadByte
        fs.Close


        Select Case byt
            Case &H54
                selectedExeType = ExeType.ReleaseVanilla
            Case &HB4
                selectedExeType = ExeType.DebugVanilla
        End Select


        Return validExe

    End Function

    Private Function CheckLargeFileSupport() As Boolean

        Dim info = New Devices.ComputerInfo()
        Return info.AvailablePhysicalMemory > 6442450944 '6GB

    End Function

    Private Function CompareMd5Strings(a As String, b As String) As Boolean
        Return a.ToLower().Contains(b.ToLower())
    End Function

    Private Async Function CreateFrpgFoldersAsync() As Task
        Await Task.Run(
            Async Function()
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
                    Await AddTextLineAsync("Created c:\FRPG and game:/testdata folders")
                End If
            End Function)
    End Function

    Private Async Function ExtractBHD5Async(filename As String, loadBdtIntoMemory As Boolean) As Task
        Dim BHDstream = Await OpenFileIntoMemoryAsync(dataPath & filename & ".bhd5")
        Dim BDTstream

        If loadBdtIntoMemory Then
            Await AddTextLineAsync("Loading " & filename & " into memory...")
            BDTstream = Await OpenFileIntoMemoryAsync(dataPath & filename & ".bdt")
        Else
            BDTstream = New IO.FileStream(dataPath & filename & ".bdt", IO.FileMode.Open)
        End If

        Await AddTextLineAsync("Extracting " & filename & "...")

        Await ResetProgCurAsync()
        Await AppendCurrentProgressMaxAsync(BDTstream.Length / 1000)

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
            hashidx(i) = Await HashFileNameAsync(fileidx(i))
        Next

        bigEndian = False

        numFiles = Await Int32FromStreamAsync(BHDstream, &H10)
        Await Task.Run(
            Async Function()
                For i As Integer = 0 To numFiles - 1
                    count = Await Int32FromStreamAsync(BHDstream, &H18 + i * &H8)
                    BHDoffset = Await Int32FromStreamAsync(BHDstream, &H1C + i * 8)

                    For j = 0 To count - 1
                        currFileSize = Await Int32FromStreamAsync(BHDstream, BHDoffset + &H4)
                        currFileOffset = Await Int32FromStreamAsync(BHDstream, BHDoffset + &H8)

                        ReDim currFileBytes(currFileSize - 1)

                        BDTstream.Position = currFileOffset
                        Await BDTstream.ReadAsync(currFileBytes, 0, currFileSize)

                        currFileName = ""

                        If hashidx.Contains(Await UInt32FromStreamAsync(BHDstream, BHDoffset)) Then
                            idx = Array.IndexOf(hashidx, Await UInt32FromStreamAsync(BHDstream, BHDoffset))
                            currFileName = fileidx(idx)
                            currFileName = currFileName.Replace("/", "\")
                        Else
                            currFileName = "\NOMATCH\" & Hex(BHDoffset)
                        End If

                        Dim currFileRelName = currFileName

                        currFileName = Microsoft.VisualBasic.Left(dataPath, dataPath.Length - 1) & currFileName
                        currFilePath = Microsoft.VisualBasic.Left(currFileName, InStrRev(currFileName, "\"))

                        If (Not System.IO.Directory.Exists(currFilePath)) Then
                            System.IO.Directory.CreateDirectory(currFilePath)
                        End If

                        Await AddTextLineAsync("Extracting [Chunk " & (i + 1).ToString("000") & "/" & numFiles.ToString("000") & "][File " & (j + 1).ToString("000") & "/" & count.ToString("000") & "] from """ & filename & ".bdt"": """ & currFileRelName & """...", True)

                        Await WriteBytesToFileAsync(currFileName, currFileBytes)

                        BHDoffset += &H10

                        'Progress'''''''''''''''''''''''''''''''''''''''''
                        Await AppendProgressAsync(currFileSize / 1000)
                        ''''''''''''''''''''''''''''''''''''''''''''''''''
                    Next
                Next

                'Progress'''''''''''''''''''''''''''''''''''''''''
                Await SetProgressAsync(BDTstream.Length / 1000)
                ''''''''''''''''''''''''''''''''''''''''''''''''''

                BDTstream.Dispose()
                BHDstream.Dispose()

                'File.Move(dataPath & filename & ".bhd5", dataPath & filename & ".bhd5.bak")
                'File.Move(dataPath & filename & ".bdt", dataPath & filename & ".bdt.bak")
            End Function)

    End Function

    Private Async Function ExtractBHF3Async(filename As String) As Task
        Await AppendCurrentProgressMaxAsync(1)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(1)
        ''''''''''''''''''''''''''''''''''''''''''''''''''
        
        Dim BHDstream = Await OpenFileIntoMemoryAsync(filename)
        Dim BDTstream

        Dim outpath As String = datapath

        BDTstream = Await OpenFileIntoMemoryAsync(Microsoft.VisualBasic.Left(filename, filename.Length - 3) & "bdt")

        Await AddTextLineAsync("Extracting " & filename & "...")

        Await ResetProgCurAsync()
        Await AppendCurrentProgressMaxAsync(BDTstream.Length / 1000)


        Select Case Microsoft.VisualBasic.right(filename, filename.Length - InStrRev(FileName, ".")).ToLower
            Case "tpfbhd", "hkxbhd"
                outpath = outpath & "map\"
            Case "chrtpfbhd"
                outpath = outpath & "chr\"
        End Select


        Dim BHDoffset As Integer = 0

        If Await UInt32FromStreamAsync(BHDstream, &H10) = 0 Then 
            bigEndian = True
        Else
            bigEndian = False
        End If

        Dim currFileName As String = ""
        Dim currFileSize As UInteger = 0
        Dim currFileOffset As UInteger = 0
        Dim currFileID As UInteger = 0
        Dim currFileNameOffset As UInteger = 0
        Dim currFileBytes() As Byte = {}

        Dim count As UInteger = 0
        Dim flags As UInteger
        Dim numfiles As UInteger


        flags = Await UInt32FromStreamAsync(BHDstream, &HC)
        numFiles = Await UInt32FromStreamAsync(BHDstream, &H10)

        BHDoffset = &H20

        For i As UInteger = 0 To numFiles - 1

            currFileSize = Await UInt32FromStreamAsync(BHDstream, bhdOffSet + &H4)
            currFileOffset = Await UInt32FromStreamAsync(BHDstream, bhdOffSet + &H8)
            currFileID = Await UInt32FromStreamAsync(BHDstream, bhdOffSet + &HC)

            ReDim currFileBytes(currFileSize - 1)

            BDTStream.Position = currFileOffset

            For k = 0 To currFileSize - 1
                currFileBytes(k) = BDTStream.ReadByte
            Next

            currFileName = await ASCIIStrFromStreamAsync(BHDstream, Await UInt32FromStreamAsync(BHDstream, bhdOffSet + &H10))
            currFileName = outpath & currFileName


            Await AddTextLineAsync("Extracting " & Microsoft.VisualBasic.right(currFileName, currfilename.Length - InStrRev(currFileName, "\")), true)

            If (Not System.IO.Directory.Exists(Microsoft.VisualBasic.Left(currFileName, InStrRev(currFileName, "\")))) Then
                System.IO.Directory.CreateDirectory(Microsoft.VisualBasic.Left(currFileName, InStrRev(currFileName, "\")))
            End If

            Await WriteBytesToFileAsync(currFileName, currFileBytes)
            BHDoffset += &H18

            'Progress'''''''''''''''''''''''''''''''''''''''''
            Await AppendProgressAsync(currFileSize / 1000)
            ''''''''''''''''''''''''''''''''''''''''''''''''''

        Next

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(BDTstream.Length / 1000)
        ''''''''''''''''''''''''''''''''''''''''''''''''''
        BHDstream.Close
        BDTstream.Close
 
        'YEAH WE DID IT

    End Function

    Private Async Function ExtractBND3Async(filename As String) As Task
        Dim BNDstream = Await OpenFileIntoMemoryAsync(filename)
               
        'Dim BNDstream As FileStream = File.Open(filename, FileMode.Open) 

        
        Await ResetProgCurAsync()
        Await AppendCurrentProgressMaxAsync(BNDstream.Length)

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

        flags = Await UInt32FromStreamAsync(BNDstream, &HC)

        numfiles = Await UInt32FromStreamAsync(BNDstream, &H10)
        namesEndLoc = Await UInt32FromStreamAsync(BNDstream, &H14)

        Await Task.Run(
            Async Function() As Task
                If numfiles > 0 Then

                    Dim entryLength As UInteger = 0
                    Select Case flags
                        Case &H70
                            entryLength = &H14
                        Case &H74, &H54
                            entryLength = &H18
                    End Select

                    For i As UInteger = 0 To numfiles - 1
                        currFileSize = Await UInt32FromStreamAsync(BNDstream, &H24 + i * entryLength)
                        currFileOffset = Await UInt32FromStreamAsync(BNDstream, &H28 + i * entryLength)
                        currFileID = Await UInt32FromStreamAsync(BNDstream, &H2C + i * entryLength)
                        currFileNameOffset = Await UInt32FromStreamAsync(BNDstream, &H30 + i * entryLength)
                        currFileName = Await ASCIIStrFromStreamAsync(BNDstream, currFileNameOffset)

                        currFileName.Replace("N:\", "")
                        currFileName = "C:\" & currFileName
                        currFilePath = Microsoft.VisualBasic.Left(currFileName, InStrRev(currFileName, "\"))

                        If (Not System.IO.Directory.Exists(currFilePath)) Then
                            System.IO.Directory.CreateDirectory(currFilePath)
                        End If

                        ReDim currFileBytes(currFileSize - 1)

                        BNDstream.Position = currFileOffset

                        Await BNDstream.ReadAsync(currFileBytes, 0, currFileSize)

                        Await WriteBytesToFileAsync(currFileName, currFileBytes)

                        'supply chrtpfbhds to chr:/
                        If currFileName.Split(".").Count > 0 Then
                            If currFileName.Split(".")(1).ToLower  = "chrtpfbhd" Then
                                Dim newFileName As String
                                newfilename = dataPath & "chr\" & Microsoft.VisualBasic.right(currFileName, currfilename.Length - InStrRev(currFileName, "\"))
                                If File.Exists(newFileName) Then
                                    File.Delete(newFileName)
                                End If
                                File.Copy(currFileName, newfilename)
                            End If
                        End If

                        'Progress'''''''''''''''''''''''''''''''''''''''''
                        Await AppendProgressAsync(currFileSize)
                        ''''''''''''''''''''''''''''''''''''''''''''''''''
                    Next
                End If

                'Progress'''''''''''''''''''''''''''''''''''''''''
                Await SetProgressAsync(BNDstream.Length)
                ''''''''''''''''''''''''''''''''''''''''''''''''''
            End Function)

    End Function

    Private Async Function ExtractDFLTAsync(filename As String) As Task
        Dim DCXstream = Await OpenFileIntoMemoryAsync(filename)
        Dim currFileName As String = ""
        bigEndian = True

        Await ResetProgCurAsync()
        Await AppendCurrentProgressMaxAsync(DCXstream.Length / 1000)

        Dim startOffset As UInteger = (Await Int32FromStreamAsync(DCXstream, &H14)) + &H22

        Dim newbytes(Await Int32FromStreamAsync(DCXstream, &H20) - 1) As Byte
        Dim decbytes(Await Int32FromStreamAsync(DCXstream, &H1C)) As Byte

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await AppendProgressAsync((DCXstream.Length / 1000) / 4)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        DCXstream.Position = startOffset
        Await DCXstream.ReadAsync(newbytes, 0, newbytes.Length - 1) 'TODO: Make sure reading all but 1 byte is intentional

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await AppendProgressAsync((DCXstream.Length / 1000) / 4)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        decbytes = Await DecompressAsync(newbytes)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await AppendProgressAsync((DCXstream.Length / 1000) / 4)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        currFileName = Microsoft.VisualBasic.Left(filename, filename.Length - &H4)

        Await WriteBytesToFileAsync(currFileName, decbytes)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await AppendProgressAsync((DCXstream.Length / 1000) / 4)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        DCXstream.Dispose()
    End Function

    Private Async Sub frmModPrep_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await ScanForDarkSoulsDataDirAsync()
    End Sub

    Private Async Function GetAllSteamInstallLocationsAsync() As Task(Of String())
        Dim installDirList = New List(Of String)

        Await Task.Run(
            Sub()
                Dim allDrives() As DriveInfo = DriveInfo.GetDrives()
                Dim foundMainInstallDir = False
                For Each d In allDrives
                    'If we haven't already found a main Steam install, check for that.
                    'Otherwise, only check for SteamLibrary on this drive.
                    If Not foundMainInstallDir Then
                        Dim potentialMainSteamInstall = Path.Combine(d.Name, SteamGames_Main)
                        If Directory.Exists(potentialMainSteamInstall) Then
                            'This is likely the main steam installation folder.
                            foundMainInstallDir = True
                            installDirList.Add(potentialMainSteamInstall)
                        End If
                    End If

                    Dim possibleSteamLibDir = Path.Combine(d.Name, SteamGames_Library)
                    If Directory.Exists(possibleSteamLibDir) Then
                        installDirList.Add(possibleSteamLibDir)
                    End If
                Next
            End Sub)

        Return installDirList.ToArray
    End Function
    Private Async Function GetMd5ChecksumAsync(fileName As String) As Task(Of String)
        Return Await Task.Run(
            Async Function() As Task(Of String)
                Dim md5 = Security.Cryptography.MD5.Create()
                Dim file = Await OpenFileIntoMemoryAsync(fileName)
                Dim str = BitConverter.ToString(md5.ComputeHash(file))
                file.Dispose()
                Return str
            End Function)
    End Function

    Private Async Function HashFileNameAsync(filename As String) As Task(Of UInteger)

        ' This code copied from https://github.com/Burton-Radons/Alexandria
        ' and altered to run asynchronously.

        If filename Is Nothing Then
            Return 0
        End If

        Dim hash As UInteger = 0

        Await Task.Run(
            Sub()
                For Each ch As Char In filename
                    hash = hash * &H25 + Asc(Char.ToLowerInvariant(ch))
                Next
            End Sub)
        Return hash
    End Function

    Private Async Function Int32FromStreamAsync(fs As Stream, ByVal loc As Integer) As Task(Of Integer)
        Dim tmpInt As Integer = 0
        Dim byt = New Byte() {0, 0, 0, 0}

        fs.Position = loc

        Await fs.ReadAsync(byt, 0, 4)

        Await Task.Run(
            Sub()
                If bigEndian Then
                    For i = 0 To 3
                        tmpInt += Convert.ToInt32(byt(i)) * &H100 ^ (3 - i)
                    Next
                Else
                    For i = 0 To 3
                        tmpInt += Convert.ToInt32(byt(i)) * &H100 ^ i
                    Next
                End If
            End Sub)

        Return tmpInt
    End Function

    Private Async Function modDebugEXEAsync(EXEstream As FileStream) As Task
        Dim byt() As Byte

        Await Task.Run(
            Async Function()
                If Not File.Exists(txtEXEfile.Text & ".debug.bak") Then
                    File.Copy(txtEXEfile.Text, txtEXEfile.Text & ".debug.bak")
                    Await AddTextLineAsync("Created " & txtEXEfile.Text & ".debug.bak")
                Else
                    Await AddTextLineAsync(txtEXEfile.Text & ".debug.bak already exists.")
                End If
            End Function)

        byt = System.Text.Encoding.Unicode.GetBytes("dvdroot:")

        'DVDBND0:
        Await WriteBytesAsync(EXEstream, &HD6816C, byt)
        Await WriteBytesAsync(EXEstream, &HD683C0, byt)
        Await WriteBytesAsync(EXEstream, &HD68448, byt)
        Await WriteBytesAsync(EXEstream, &HD68544, byt)
        Await WriteBytesAsync(EXEstream, &HD68590, byt)
        Await WriteBytesAsync(EXEstream, &HD685E0, byt)
        Await WriteBytesAsync(EXEstream, &HD68F58, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(7)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        'DVDBND1:
        Await WriteBytesAsync(EXEstream, &HD5C2D4, byt)
        Await WriteBytesAsync(EXEstream, &HD68074, byt)
        Await WriteBytesAsync(EXEstream, &HD680BC, byt)
        Await WriteBytesAsync(EXEstream, &HD682C4, byt)
        Await WriteBytesAsync(EXEstream, &HD68404, byt)
        Await WriteBytesAsync(EXEstream, &HD68634, byt)
        Await WriteBytesAsync(EXEstream, &HD6874C, byt)
        Await WriteBytesAsync(EXEstream, &HD688B8, byt)
        Await WriteBytesAsync(EXEstream, &HD689AC, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(16)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        'DVDBND2:
        Await WriteBytesAsync(EXEstream, &HD689FC, byt)
        Await WriteBytesAsync(EXEstream, &HD68A8C, byt)
        Await WriteBytesAsync(EXEstream, &HD69174, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(19)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        'DVDBND3:
        Await WriteBytesAsync(EXEstream, &HD680FC, byt)
        Await WriteBytesAsync(EXEstream, &HD68678, byt)
        Await WriteBytesAsync(EXEstream, &HD686FC, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(22)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        byt = System.Text.Encoding.Unicode.GetBytes("C:")

        'N:
        Await WriteBytesAsync(EXEstream, &HD67E7C, byt)
        Await WriteBytesAsync(EXEstream, &HD67F04, byt)
        Await WriteBytesAsync(EXEstream, &HD71ED0, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(25)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        'HKXBND:
        byt = System.Text.Encoding.Unicode.GetBytes("maphkx:")
        Await WriteBytesAsync(EXEstream, &HD941F0, byt)
        Await WriteBytesAsync(EXEstream, &HD94218, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(27)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        'TPFBND:
        byt = System.Text.Encoding.Unicode.GetBytes("maptpf:")
        Await WriteBytesAsync(EXEstream, &HD6489C, byt)
        Await WriteBytesAsync(EXEstream, &HD94400, byt)


        '%stpf
        byt = System.Text.Encoding.Unicode.GetBytes("chr")
        ReDim Preserve byt(8)
        Await WriteBytesAsync(EXEstream, &HDB1CD8, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(30)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        'Disable DCX loading
        Await WriteBytesAsync(EXEstream, &H8FB726, {&HEB, &H12})

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(31)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

    End Function

    Private Async Function modReleaseEXEAsync(EXEstream As FileStream) As Task
        Dim byt() As Byte

        Await Task.Run(
            Async Function()
                If Not File.Exists(txtEXEfile.Text & ".release.bak") Then
                    File.Copy(txtEXEfile.Text, txtEXEfile.Text & ".release.bak")
                    Await AddTextLineAsync("Created " & txtEXEfile.Text & ".release.bak")
                Else
                    Await AddTextLineAsync(txtEXEfile.Text & ".release.bak already exists.")
                End If
            End Function)

        byt = System.Text.Encoding.Unicode.GetBytes("dvdroot:")

        'DVDBND0:
        Await WriteBytesAsync(EXEstream, &HD65EA4, byt)
        Await WriteBytesAsync(EXEstream, &HD660F8, byt)
        Await WriteBytesAsync(EXEstream, &HD66180, byt)
        Await WriteBytesAsync(EXEstream, &HD6627C, byt)
        Await WriteBytesAsync(EXEstream, &HD662C8, byt)
        Await WriteBytesAsync(EXEstream, &HD66318, byt)
        Await WriteBytesAsync(EXEstream, &HD66C90, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(7)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        'DVDBND1:
        Await WriteBytesAsync(EXEstream, &HD57F14, byt)
        Await WriteBytesAsync(EXEstream, &HD65DAC, byt)
        Await WriteBytesAsync(EXEstream, &HD65DF4, byt)
        Await WriteBytesAsync(EXEstream, &HD65FFC, byt)
        Await WriteBytesAsync(EXEstream, &HD6613C, byt)
        Await WriteBytesAsync(EXEstream, &HD6636C, byt)
        Await WriteBytesAsync(EXEstream, &HD66484, byt)
        Await WriteBytesAsync(EXEstream, &HD665F0, byt)
        Await WriteBytesAsync(EXEstream, &HD666E4, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(16)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        'DVDBND2:
        Await WriteBytesAsync(EXEstream, &HD66734, byt)
        Await WriteBytesAsync(EXEstream, &HD667C4, byt)
        Await WriteBytesAsync(EXEstream, &HD66EAC, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(19)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        'DVDBND3:
        Await WriteBytesAsync(EXEstream, &HD65E34, byt)
        Await WriteBytesAsync(EXEstream, &HD663B0, byt)
        Await WriteBytesAsync(EXEstream, &HD66434, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(22)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        byt = System.Text.Encoding.Unicode.GetBytes("C:")

        'N:
        Await WriteBytesAsync(EXEstream, &HD65BB4, byt)
        Await WriteBytesAsync(EXEstream, &HD65C3C, byt)
        Await WriteBytesAsync(EXEstream, &HD6FBA0, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(25)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        'HKXBND:
        byt = System.Text.Encoding.Unicode.GetBytes("maphkx:")

        Await WriteBytesAsync(EXEstream, &HD91740, byt)
        Await WriteBytesAsync(EXEstream, &HD91768, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(27)
        ''''''''''''''''''''''''''''''''''''''''''''''''''


        'TPFBND:
        byt = System.Text.Encoding.Unicode.GetBytes("maptpf:")
        Await WriteBytesAsync(EXEstream, &HD625FC, byt)
        Await WriteBytesAsync(EXEstream, &HD91950, byt)


        '%stpf
        byt = System.Text.Encoding.Unicode.GetBytes("chr")
        ReDim Preserve byt(8)
        Await WriteBytesAsync(EXEstream, &HDAEBF8, byt)

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(30)
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        'Disable DCX loading
        Await WriteBytesAsync(EXEstream, &H8FB816, {&HEB, &H12})

        'Progress'''''''''''''''''''''''''''''''''''''''''
        Await SetProgressAsync(31)
        ''''''''''''''''''''''''''''''''''''''''''''''''''









    End Function

    Private Async Function OpenFileIntoMemoryAsync(fileName As String) As Task(Of MemoryStream)
        Return Await Task.Run(
            Async Function() As Task(Of MemoryStream)
                Dim fs = New FileStream(fileName, FileMode.Open, FileAccess.Read)
                Dim ms = New MemoryStream(fs.Length)
                Await fs.CopyToAsync(ms)
                ms.Position = 0
                fs.Dispose()
                Return ms
            End Function)
    End Function

    Private Async Function ResetProgCurAsync() As Task
        realCurrentProgressMax = 0
        realCurrentProgress = 0
        Await UpdateVisibleProgressAsync()
    End Function

    Private Async Function ResetProgOperationAsync() As Task
        Await SetValueAsync(progOperation, 0)
        Await SetMaxAsync(progOperation, 0)

        Await UpdateVisibleProgressAsync()
    End Function

    Private Async Function ResetProgressBarsAsync() As Task
        Await ResetProgCurAsync()
        Await ResetProgOperationAsync()
    End Function

    Private Async Function SetControlsEnabledAsync(enabled As Boolean) As Task

        Await SetEnabledAsync(btnModify, enabled)
        Await SetEnabledAsync(btnExtractBNDs, enabled)
        Await SetEnabledAsync(btnExtractDCX, enabled)
        Await SetEnabledAsync(btnDeleteDCX, enabled)
        Await SetEnabledAsync(btnExtractFRPG, enabled)
        Await SetEnabledAsync(btnExtractBHDs, enabled)
        Await SetEnabledAsync(btnBrowse, enabled)
        Await SetEnabledAsync(txtEXEfile, enabled)
        Await SetEnabledAsync(txtInfo, enabled)

    End Function

    Private Async Function SetLoadingAsync(isLoading As Boolean) As Task
        Await SetControlsEnabledAsync(Not isLoading)
        Await SetUseWaitCursorAsync(Me, isLoading)
        Await SetVisibleAsync(lblProgCurFile, isLoading)
        Await SetVisibleAsync(lblProgCurFile, isLoading)
        If Not isLoading Then
            realCurrentProgress = 0
            Await SetValueAsync(progOperation, 0)
            realCurrentProgressMax = 0
            Await SetMaxAsync(progOperation, 0)
        End If
    End Function

    Private Async Function SetProgressAsync(prog As Integer) As Task

        Await AppendProgressAsync(prog - realCurrentProgress)

    End Function

    Private Async Function ShowFileSelectDlgAsync() As Task
        Await Task.Run(
            Async Function()
                Dim dlgResult = DialogResult.Cancel
                Dim fileName = ""
                Invoke(
                Sub()
                    Dim dlg = New OpenFileDialog()
                    dlg.CheckFileExists = True
                    dlg.CheckPathExists = True
                    dlg.Multiselect = False
                    dlg.InitialDirectory = Environment.CurrentDirectory
                    dlg.Filter = "Executable files (*.exe)|*.exe"
                    dlg.SupportMultiDottedExtensions = True
                    dlg.Title = "Select Dark Souls EXE"
                    dlg.ValidateNames = True
                    dlg.AddExtension = True
                    dlg.AutoUpgradeEnabled = True
                    dlg.FileName = "DARKSOULS.exe"

                    dlgResult = dlg.ShowDialog()

                    fileName = dlg.FileName
                End Sub)
                If dlgResult = DialogResult.OK Then
                    Await CheckAndSetFileTxtAsync(fileName, True)
                End If
            End Function)
    End Function

    Private Sub txt_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtEXEfile.DragEnter
        Dim file() As String = e.Data.GetData(DataFormats.FileDrop)
        If Not (New FileInfo(file(0)).Extension.ToUpper().Equals(".EXE")) Then
            e.Effect = DragDropEffects.None
            Return
        End If
        e.Effect = DragDropEffects.Copy
    End Sub

    Private Async Sub txt_Drop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles txtEXEfile.DragDrop
        Dim file() As String = e.Data.GetData(DataFormats.FileDrop)
        If Not New FileInfo(file(0)).Extension.ToUpper().Equals(".EXE") Then
            Return
        End If
        Await CheckAndSetFileTxtAsync(file(0), True)
    End Sub

    Private Async Sub txtInfo_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles txtInfo.LinkClicked
        Await Task.Run(
            Sub()
                Process.Start(e.LinkText)
            End Sub)
    End Sub

    Private Async Function UInt32FromStreamAsync(fs As Stream, ByVal loc As Integer) As Task(Of UInteger)
        Dim tmpUInt As UInteger = 0
        Dim byt = New Byte() {0, 0, 0, 0}

        fs.Position = loc

        Await fs.ReadAsync(byt, 0, 4)
        Await Task.Run(
            Sub()
                If bigEndian Then
                    For i = 0 To 3
                        tmpUInt += Convert.ToUInt32(byt(i)) * &H100 ^ (3 - i)
                    Next
                Else
                    For i = 0 To 3
                        tmpUInt += Convert.ToUInt32(byt(i)) * &H100 ^ i
                    Next
                End If
            End Sub)

        Return tmpUInt
    End Function

    Private Async Function UpdateVisibleProgressAsync() As Task

        Await Task.Run(
            Async Function()
                If realCurrentProgress > realCurrentProgressMax Then
                    realCurrentProgress = realCurrentProgressMax
                End If

                If Not maxProgSnapTimer.Enabled Then
                    Await SetMaxAsync(progCurFile, realCurrentProgressMax)
                    Await SetValueAsync(progCurFile, realCurrentProgress)
                End If

                If realCurrentProgress >= realCurrentProgressMax Then
                    Await SetMaxAsync(progCurFile, realCurrentProgressMax)
                    Await SetValueAsync(progCurFile, realCurrentProgress)
                    'Starting the timer, which will prevent these from changing (but only visibly)
                    If maxProgSnapTimer.Enabled Then
                        maxProgSnapTimer.Stop()
                    End If
                    maxProgSnapTimer.Start()
                End If

                'Don't wanna divide by zero.
                If progCurFile.Maximum = 0 Then
                    Await SetTextAsync(lblProgCurFile, "0%")
                Else
                    Await SetTextAsync(lblProgCurFile, (progCurFile.Value / progCurFile.Maximum).ToString("0%"))
                End If

                If progOperation.Maximum = 0 Then
                    Await SetTextAsync(lblProgOperation, "0%")
                Else
                    Await SetTextAsync(lblProgOperation, (progOperation.Value / progOperation.Maximum).ToString("0%"))
                End If
            End Function)

    End Function

    Private Async Function WriteBytesAsync(fs As FileStream, ByVal loc As Integer, ByVal byt() As Byte) As Task
        fs.Position = loc
        Await fs.WriteAsync(byt, 0, byt.Length)
    End Function

    Private Async Function WriteBytesToFileAsync(fileName As String, bytes() As Byte) As Task
        Dim file = New FileStream(fileName, FileMode.OpenOrCreate)
        Await file.WriteAsync(bytes, 0, bytes.Length)
        file.Dispose()
    End Function
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