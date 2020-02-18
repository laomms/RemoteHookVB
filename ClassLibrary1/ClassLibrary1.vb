Imports System
Imports System.Text
Imports System.Runtime.InteropServices
Imports EasyHook
Imports System.Threading
Imports System.Windows.Forms


Public Class ClassLibrary1
	Implements IEntryPoint

	Public fnServerInterface As ServerInterface = Nothing
	Public messageQueue As New Queue(Of String)()
	Public Sub New(ByVal context As EasyHook.RemoteHooking.IContext, ByVal channelName As String)
		fnServerInterface = EasyHook.RemoteHooking.IpcConnectClient(Of ServerInterface)(channelName)
	End Sub

	Public Sub Run(ByVal context As EasyHook.RemoteHooking.IContext, ByVal channelName As String)
		fnServerInterface.IsInstalled(EasyHook.RemoteHooking.GetCurrentProcessId())
		Dim MessageBoxWHook = LocalHook.Create(LocalHook.GetProcAddress("user32.dll", "MessageBoxW"), New DMessageBoxW(AddressOf MessageBoxW_Hooked), Me)
		Dim MessageBoxAHook = LocalHook.Create(LocalHook.GetProcAddress("user32.dll", "MessageBoxA"), New DMessageBoxW(AddressOf MessageBoxA_Hooked), Me)
		MessageBoxWHook.ThreadACL.SetExclusiveACL(New Int32() {0})
		MessageBoxAHook.ThreadACL.SetExclusiveACL(New Int32() {0})

		'fnServerInterface.ReportMessage("MessageBoxA, MessageBoxW hooks installed")
		MsgBox("hooks installed", vbMsgBoxSetForeground + vbSystemModal)
		EasyHook.RemoteHooking.WakeUpProcess()

		Try
			Do
				System.Threading.Thread.Sleep(500)
				Dim queued() As String = Nothing
				SyncLock messageQueue
					queued = messageQueue.ToArray()
					messageQueue.Clear()
				End SyncLock
				If queued IsNot Nothing AndAlso queued.Length > 0 Then
					fnServerInterface.ReportMessages(queued)
				End If
			Loop
		Catch
		End Try
		MessageBoxWHook.Dispose()
		MessageBoxAHook.Dispose()
		EasyHook.LocalHook.Release()
	End Sub


	<DllImport("Kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Private Shared Function GetFinalPathNameByHandle(ByVal hFile As IntPtr, <MarshalAs(UnmanagedType.LPTStr)> ByVal lpszFilePath As StringBuilder, ByVal cchFilePath As UInteger, ByVal dwFlags As UInteger) As UInteger
	End Function


#Region "MessageBoxW"

	<DllImport("user32.dll", EntryPoint:="MessageBoxW", CharSet:=CharSet.Unicode)>
	Public Shared Function MessageBoxW(ByVal hWnd As Integer, ByVal text As String, ByVal caption As String, ByVal type As UInteger) As IntPtr
	End Function

	<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
	Private Delegate Function DMessageBoxW(ByVal hWnd As Integer, ByVal text As String, ByVal caption As String, ByVal type As UInteger) As IntPtr

	Private Function MessageBoxW_Hooked(ByVal hWnd As Integer, ByVal text As String, ByVal caption As String, ByVal type As UInteger) As IntPtr
		SyncLock messageQueue
			If messageQueue.Count < 1000 Then
				Dim mode As String = String.Empty
				mode = "MessageBoxW_Hooked"
				messageQueue.Enqueue(String.Format("[{0}:{1}]: HOOKED ({2}) ""{3}""", EasyHook.RemoteHooking.GetCurrentProcessId(), EasyHook.RemoteHooking.GetCurrentThreadId(), mode, "text"))
			End If
		End SyncLock
		Return MessageBoxW(hWnd, "已注入-" & text, "已注入-" & caption, type)
	End Function

#End Region

#Region "MessageBoxA"

	<DllImport("user32.dll", EntryPoint:="MessageBoxA", CharSet:=CharSet.Ansi)>
	Public Shared Function MessageBoxA(ByVal hWnd As Integer, ByVal text As String, ByVal caption As String, ByVal type As UInteger) As IntPtr
	End Function

	<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Ansi)>
	Private Delegate Function DMessageBoxA(ByVal hWnd As Integer, ByVal text As String, ByVal caption As String, ByVal type As UInteger) As IntPtr

	Private Function MessageBoxA_Hooked(ByVal hWnd As Integer, ByVal text As String, ByVal caption As String, ByVal type As UInteger) As IntPtr
		SyncLock messageQueue
			If messageQueue.Count < 1000 Then
				Dim mode As String = String.Empty
				mode = "MessageBoxA_Hooked"
				messageQueue.Enqueue(String.Format("[{0}:{1}]: HOOKED ({2}) ""{3}""", EasyHook.RemoteHooking.GetCurrentProcessId(), EasyHook.RemoteHooking.GetCurrentThreadId(), mode, text))
			End If
		End SyncLock
		Return MessageBoxA(hWnd, "已注入-" & text, "已注入-" & caption, type)
	End Function

#End Region
End Class

