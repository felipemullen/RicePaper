// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace RicePaper.MacOS
{
	[Register ("ScheduleTabController")]
	partial class ScheduleTabController
	{
		[Outlet]
		AppKit.NSButton ButtonRefApply { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefRadioInOrder { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefRadioRandom { get; set; }

		[Outlet]
		AppKit.NSPopUpButton DropdownRefImageIntervalUnit { get; set; }

		[Outlet]
		AppKit.NSPopUpButton DropdownRefWordIntervalUnit { get; set; }

		[Outlet]
		AppKit.NSTextField FieldRefImageInterval { get; set; }

		[Outlet]
		AppKit.NSTextField FieldRefWordInterval { get; set; }

		[Outlet]
		AppKit.NSStepper StepperRefImageInterval { get; set; }

		[Outlet]
		AppKit.NSStepper StepperRefWordInterval { get; set; }

		[Action ("ActionButtonApply:")]
		partial void ActionButtonApply (Foundation.NSObject sender);

		[Action ("ActionImageIntervalTextField:")]
		partial void ActionImageIntervalTextField (Foundation.NSObject sender);

		[Action ("ActionRadioIterateInOrder:")]
		partial void ActionRadioIterateInOrder (Foundation.NSObject sender);

		[Action ("ActionRadioIterateRandom:")]
		partial void ActionRadioIterateRandom (Foundation.NSObject sender);

		[Action ("ActionStepperImageInterval:")]
		partial void ActionStepperImageInterval (Foundation.NSObject sender);

		[Action ("ActionStepperWordInterval:")]
		partial void ActionStepperWordInterval (Foundation.NSObject sender);

		[Action ("ActionWordIntervalTextField:")]
		partial void ActionWordIntervalTextField (Foundation.NSObject sender);

		[Action ("DropdownImageIntervalUnit:")]
		partial void DropdownImageIntervalUnit (Foundation.NSObject sender);

		[Action ("DropdownWordIntervalUnit:")]
		partial void DropdownWordIntervalUnit (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonRefApply != null) {
				ButtonRefApply.Dispose ();
				ButtonRefApply = null;
			}

			if (ButtonRefRadioInOrder != null) {
				ButtonRefRadioInOrder.Dispose ();
				ButtonRefRadioInOrder = null;
			}

			if (ButtonRefRadioRandom != null) {
				ButtonRefRadioRandom.Dispose ();
				ButtonRefRadioRandom = null;
			}

			if (DropdownRefImageIntervalUnit != null) {
				DropdownRefImageIntervalUnit.Dispose ();
				DropdownRefImageIntervalUnit = null;
			}

			if (DropdownRefWordIntervalUnit != null) {
				DropdownRefWordIntervalUnit.Dispose ();
				DropdownRefWordIntervalUnit = null;
			}

			if (FieldRefImageInterval != null) {
				FieldRefImageInterval.Dispose ();
				FieldRefImageInterval = null;
			}

			if (FieldRefWordInterval != null) {
				FieldRefWordInterval.Dispose ();
				FieldRefWordInterval = null;
			}

			if (StepperRefImageInterval != null) {
				StepperRefImageInterval.Dispose ();
				StepperRefImageInterval = null;
			}

			if (StepperRefWordInterval != null) {
				StepperRefWordInterval.Dispose ();
				StepperRefWordInterval = null;
			}
		}
	}
}
