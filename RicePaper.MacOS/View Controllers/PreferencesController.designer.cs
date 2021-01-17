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
	[Register ("PreferencesController")]
	partial class PreferencesController
	{
		[Outlet]
		AppKit.NSButton ButtonRefShowInDock { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefStartOnBoot { get; set; }

		[Action ("ActionOnBtnShowInDock:")]
		partial void ActionOnBtnShowInDock (AppKit.NSButton sender);

		[Action ("ActionOnBtnStartOnBoot:")]
		partial void ActionOnBtnStartOnBoot (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonRefShowInDock != null) {
				ButtonRefShowInDock.Dispose ();
				ButtonRefShowInDock = null;
			}

			if (ButtonRefStartOnBoot != null) {
				ButtonRefStartOnBoot.Dispose ();
				ButtonRefStartOnBoot = null;
			}
		}
	}
}
