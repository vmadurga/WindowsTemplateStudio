﻿Imports Windows.Storage

Imports Param_RootNamespace.Activation
Imports Param_RootNamespace.Helpers

Namespace Services

    ' The SuspendAndResumeService allows you to save the App data before the App is being suspended (or enters in background state).
    ' In case the App is terminated during suspension, the data is restored during App launch by this ActivationHandler.
    ' In case the App is resumed without being terminated no data should be lost, a resume event is fired that allows you to refresh App data that might
    ' be outdated (e.g data from online feeds)
    ' Documentation:
    '     * How to implement and test: https://github.com/Microsoft/WindowsTemplateStudio/blob/dev/docs/features/suspend-and-resume.md
    '     * Application Lifecycle: https://docs.microsoft.com/windows/uwp/launch-resume/app-lifecycle
    Friend Class SuspendAndResumeService
        Inherits ActivationHandler(Of LaunchActivatedEventArgs)

        Private Const StateFilename As String = "SuspendAndResumeState"

        ' This method saves the application state before entering background state. It fires the event OnBackgroundEntering to collect
        ' state data from the current subscriber and saves it to the local storage.
        Public Async Function SaveStateAsync() As Task
            Dim suspensionState = New SuspensionState() With {
                .SuspensionDate = DateTime.Now
            }

            Dim target As Type = Nothing
            
            If OnBackgroundEnteringEvent IsNot Nothing Then
                target = OnBackgroundEnteringEvent.Target.GetType
            End If

            Dim onBackgroundEnteringArgs = New OnBackgroundEnteringEventArgs(suspensionState, target)

            RaiseEvent OnBackgroundEntering(Me, onBackgroundEnteringArgs)

            Await ApplicationData.Current.LocalFolder.SaveAsync(StateFilename, onBackgroundEnteringArgs)
        End Function

        ' This method allows subscribers to refresh data that might be outdated when the App is resuming from suspension.
        ' If the App was terminated during suspension this event will not fire, data restore is handled by the method HandleInternalAsync.
        Public Sub ResumeApp()
            RaiseEvent OnResuming(Me, EventArgs.Empty)         
        End Sub

        ' This method restores application state when the App is launched after termination, it navigates to the stored Page passing the recovered state data.
        Protected Overrides Async Function HandleInternalAsync(args As LaunchActivatedEventArgs) As Task
            Dim saveState = Await ApplicationData.Current.LocalFolder.ReadAsync(Of OnBackgroundEnteringEventArgs)(StateFilename)
        End Function

        Protected Overrides Function CanHandleInternal(args As LaunchActivatedEventArgs) As Boolean
            ' Application State must only be restored if the App was terminated during suspension.
            Return args.PreviousExecutionState = ApplicationExecutionState.Terminated
        End Function

    End Class
End Namespace
