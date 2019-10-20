Imports System
Imports System.Text
Imports System.Runtime.InteropServices
Imports EasyHook
Imports System.Threading
Imports System.Windows.Forms

Namespace ClassLibrary1
	<Serializable>
	Public Class HookParameter
		Public Property Msg() As String
		Public Property HostProcessId() As Integer
	End Class

	Public Class Main
		Implements IEntryPoint

		Public MessageBoxWHook As LocalHook = Nothing
		Public MessageBoxAHook As LocalHook = Nothing

		Public Sub New(ByVal context As RemoteHooking.IContext, ByVal channelName As String, ByVal parameter As HookParameter)
			MessageBox.Show(parameter.Msg, "Hooked")
		End Sub

		Public Sub Run(ByVal context As RemoteHooking.IContext, ByVal channelName As String, ByVal parameter As HookParameter)
			Try
				MessageBoxWHook = LocalHook.Create(LocalHook.GetProcAddress("user32.dll", "MessageBoxW"), New DMessageBoxW(AddressOf MessageBoxW_Hooked), Me)
				MessageBoxWHook.ThreadACL.SetExclusiveACL(New Integer(0){})

				MessageBoxAHook = LocalHook.Create(LocalHook.GetProcAddress("user32.dll", "MessageBoxA"), New DMessageBoxW(AddressOf MessageBoxA_Hooked), Me)
				MessageBoxAHook.ThreadACL.SetExclusiveACL(New Integer(0){})
			Catch ex As Exception
				MessageBox.Show(ex.Message)
				Return
			End Try

			Try
				Do
					Thread.Sleep(10)
				Loop
			Catch

			End Try
		End Sub

		#Region "MessageBoxW"

		<DllImport("user32.dll", EntryPoint := "MessageBoxW", CharSet := CharSet.Unicode)>
		Public Shared Function MessageBoxW(ByVal hWnd As Integer, ByVal text As String, ByVal caption As String, ByVal type As UInteger) As IntPtr
		End Function

		<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet := CharSet.Unicode)>
		Private Delegate Function DMessageBoxW(ByVal hWnd As Integer, ByVal text As String, ByVal caption As String, ByVal type As UInteger) As IntPtr

		Private Shared Function MessageBoxW_Hooked(ByVal hWnd As Integer, ByVal text As String, ByVal caption As String, ByVal type As UInteger) As IntPtr
			Return MessageBoxW(hWnd, "已注入-" & text, "已注入-" & caption, type)
		End Function

		#End Region

		#Region "MessageBoxA"

		<DllImport("user32.dll", EntryPoint := "MessageBoxA", CharSet := CharSet.Ansi)>
		Public Shared Function MessageBoxA(ByVal hWnd As Integer, ByVal text As String, ByVal caption As String, ByVal type As UInteger) As IntPtr
		End Function

		<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet := CharSet.Ansi)>
		Private Delegate Function DMessageBoxA(ByVal hWnd As Integer, ByVal text As String, ByVal caption As String, ByVal type As UInteger) As IntPtr

		Private Shared Function MessageBoxA_Hooked(ByVal hWnd As Integer, ByVal text As String, ByVal caption As String, ByVal type As UInteger) As IntPtr
			Return MessageBoxA(hWnd, "已注入-" & text, "已注入-" & caption, type)
		End Function

		#End Region
	End Class
End Namespace
