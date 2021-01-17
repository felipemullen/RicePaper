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
	[Register ("AboutController")]
	partial class AboutController
	{
		[Outlet]
		AppKit.NSTextField FieldRefVersion { get; set; }

		[Action ("OnActionGithubLink:")]
		partial void OnActionGithubLink (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (FieldRefVersion != null) {
				FieldRefVersion.Dispose ();
				FieldRefVersion = null;
			}
		}
	}
}
