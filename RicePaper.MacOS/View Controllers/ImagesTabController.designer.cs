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
	[Register ("ImagesTabController")]
	partial class ImagesTabController
	{
		[Outlet]
		AppKit.NSButton ButtonRefApply { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefImagePicker { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosCB { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosCM { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosCT { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosLB { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosLM { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosLT { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosRB { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosRM { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefPosRT { get; set; }

		[Outlet]
		AppKit.NSPopUpButton DropdownRefImageList { get; set; }

		[Outlet]
		AppKit.NSTextField LabelImagePath { get; set; }

		[Outlet]
		AppKit.NSSlider SliderRefPrimaryText { get; set; }

		[Outlet]
		AppKit.NSSlider SliderRefSecondaryText { get; set; }

		[Action ("ActionButtonApply:")]
		partial void ActionButtonApply (Foundation.NSObject sender);

		[Action ("ActionImageDialog:")]
		partial void ActionImageDialog (AppKit.NSButton sender);

		[Action ("ActionImageListDropdown:")]
		partial void ActionImageListDropdown (Foundation.NSObject sender);

		[Action ("ActionPositionCB:")]
		partial void ActionPositionCB (Foundation.NSObject sender);

		[Action ("ActionPositionCM:")]
		partial void ActionPositionCM (Foundation.NSObject sender);

		[Action ("ActionPositionCT:")]
		partial void ActionPositionCT (Foundation.NSObject sender);

		[Action ("ActionPositionLB:")]
		partial void ActionPositionLB (Foundation.NSObject sender);

		[Action ("ActionPositionLM:")]
		partial void ActionPositionLM (Foundation.NSObject sender);

		[Action ("ActionPositionLT:")]
		partial void ActionPositionLT (Foundation.NSObject sender);

		[Action ("ActionPositionRB:")]
		partial void ActionPositionRB (Foundation.NSObject sender);

		[Action ("ActionPositionRM:")]
		partial void ActionPositionRM (Foundation.NSObject sender);

		[Action ("ActionPositionRT:")]
		partial void ActionPositionRT (Foundation.NSObject sender);

		[Action ("ActionSliderPrimary:")]
		partial void ActionSliderPrimary (Foundation.NSObject sender);

		[Action ("ActionSliderSecondary:")]
		partial void ActionSliderSecondary (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonRefApply != null) {
				ButtonRefApply.Dispose ();
				ButtonRefApply = null;
			}

			if (ButtonRefImagePicker != null) {
				ButtonRefImagePicker.Dispose ();
				ButtonRefImagePicker = null;
			}

			if (ButtonRefPosCB != null) {
				ButtonRefPosCB.Dispose ();
				ButtonRefPosCB = null;
			}

			if (ButtonRefPosCM != null) {
				ButtonRefPosCM.Dispose ();
				ButtonRefPosCM = null;
			}

			if (ButtonRefPosCT != null) {
				ButtonRefPosCT.Dispose ();
				ButtonRefPosCT = null;
			}

			if (ButtonRefPosLB != null) {
				ButtonRefPosLB.Dispose ();
				ButtonRefPosLB = null;
			}

			if (ButtonRefPosLM != null) {
				ButtonRefPosLM.Dispose ();
				ButtonRefPosLM = null;
			}

			if (ButtonRefPosLT != null) {
				ButtonRefPosLT.Dispose ();
				ButtonRefPosLT = null;
			}

			if (ButtonRefPosRB != null) {
				ButtonRefPosRB.Dispose ();
				ButtonRefPosRB = null;
			}

			if (ButtonRefPosRM != null) {
				ButtonRefPosRM.Dispose ();
				ButtonRefPosRM = null;
			}

			if (ButtonRefPosRT != null) {
				ButtonRefPosRT.Dispose ();
				ButtonRefPosRT = null;
			}

			if (DropdownRefImageList != null) {
				DropdownRefImageList.Dispose ();
				DropdownRefImageList = null;
			}

			if (LabelImagePath != null) {
				LabelImagePath.Dispose ();
				LabelImagePath = null;
			}

			if (SliderRefPrimaryText != null) {
				SliderRefPrimaryText.Dispose ();
				SliderRefPrimaryText = null;
			}

			if (SliderRefSecondaryText != null) {
				SliderRefSecondaryText.Dispose ();
				SliderRefSecondaryText = null;
			}
		}
	}
}
