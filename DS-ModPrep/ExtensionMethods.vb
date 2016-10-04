Imports System.IO
Imports System.Reflection
Imports System.Runtime.CompilerServices

Module ExtensionMethods

    <Extension()>
    Public Function GetLinkerTime(ByRef assembly As Assembly, Optional target As TimeZoneInfo = Nothing) As Date
        Dim filePath = assembly.Location
        Const c_PeHeaderOffset As Integer = 60
        Const c_LinkerTimestampOffset As Integer = 8

        Dim Buffer(2048) As Byte

        Using stream As New FileStream(filePath, FileMode.Open, FileAccess.Read)
            stream.Read(Buffer, 0, 2048)
        End Using

        Dim offset = BitConverter.ToInt32(Buffer, c_PeHeaderOffset)
        Dim secondsSince1970 = BitConverter.ToInt32(Buffer, offset + c_LinkerTimestampOffset)
        Dim epoch = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)

        Dim linkTimeUtc = epoch.AddSeconds(secondsSince1970)

        Dim tz = TimeZoneInfo.Local
        If Not target Is Nothing Then
            tz = target
        End If
        Dim localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz)

        Return localTime
    End Function

End Module
