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
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton ButtonRefApply { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefDefinition { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefEnglishSentence { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefFurigana { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefImagePicker { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefJapaneseSentence { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefKanji { get; set; }

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
		AppKit.NSButton ButtonRefRadioInOrder { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefRadioRandom { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefRomaji { get; set; }

		[Outlet]
		AppKit.NSButton ButtonRefWordListPicker { get; set; }

		[Outlet]
		AppKit.NSPopUpButton DropdownRefDictionary { get; set; }

		[Outlet]
		AppKit.NSPopUpButton DropdownRefImageIntervalUnit { get; set; }

		[Outlet]
		AppKit.NSPopUpButton DropdownRefImageList { get; set; }

		[Outlet]
		AppKit.NSPopUpButton DropdownRefWordIntervalUnit { get; set; }

		[Outlet]
		AppKit.NSPopUpButton DropdownRefWordList { get; set; }

		[Outlet]
		AppKit.NSTextField FieldRefImageInterval { get; set; }

		[Outlet]
		AppKit.NSTextField FieldRefWordInterval { get; set; }

		[Outlet]
		AppKit.NSTextField LabelImagePath { get; set; }

		[Outlet]
		AppKit.NSTextField LabelWordListPath { get; set; }

		[Outlet]
		AppKit.NSStepper StepperRefImageInterval { get; set; }

		[Outlet]
		AppKit.NSStepper StepperRefWordInterval { get; set; }

		[Action ("ActionButtonApply:")]
		partial void ActionButtonApply (Foundation.NSObject sender);

		[Action ("ActionButtonNextImage:")]
		partial void ActionButtonNextImage (Foundation.NSObject sender);

		[Action ("ActionButtonNextWord:")]
		partial void ActionButtonNextWord (Foundation.NSObject sender);

		[Action ("ActionImageDialog:")]
		partial void ActionImageDialog (AppKit.NSButton sender);

		[Action ("ActionImageIntervalTextField:")]
		partial void ActionImageIntervalTextField (Foundation.NSObject sender);

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

		[Action ("ActionWordListDialog:")]
		partial void ActionWordListDialog (Foundation.NSObject sender);

		[Action ("ActionWordListDropdown:")]
		partial void ActionWordListDropdown (Foundation.NSObject sender);

		[Action ("CheckboxDefinition:")]
		partial void CheckboxDefinition (Foundation.NSObject sender);

		[Action ("CheckboxEnglishSentence:")]
		partial void CheckboxEnglishSentence (Foundation.NSObject sender);

		[Action ("CheckboxFurigana:")]
		partial void CheckboxFurigana (Foundation.NSObject sender);

		[Action ("CheckboxJapaneseSentence:")]
		partial void CheckboxJapaneseSentence (Foundation.NSObject sender);

		[Action ("CheckboxKanji:")]
		partial void CheckboxKanji (Foundation.NSObject sender);

		[Action ("CheckboxRomaji:")]
		partial void CheckboxRomaji (Foundation.NSObject sender);

		[Action ("DropdownDictionary:")]
		partial void DropdownDictionary (Foundation.NSObject sender);

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

			if (DropdownRefDictionary != null) {
				DropdownRefDictionary.Dispose ();
				DropdownRefDictionary = null;
			}

			if (DropdownRefWordIntervalUnit != null) {
				DropdownRefWordIntervalUnit.Dispose ();
				DropdownRefWordIntervalUnit = null;
			}

			if (DropdownRefImageIntervalUnit != null) {
				DropdownRefImageIntervalUnit.Dispose ();
				DropdownRefImageIntervalUnit = null;
			}

			if (ButtonRefDefinition != null) {
				ButtonRefDefinition.Dispose ();
				ButtonRefDefinition = null;
			}

			if (ButtonRefEnglishSentence != null) {
				ButtonRefEnglishSentence.Dispose ();
				ButtonRefEnglishSentence = null;
			}

			if (ButtonRefFurigana != null) {
				ButtonRefFurigana.Dispose ();
				ButtonRefFurigana = null;
			}

			if (ButtonRefImagePicker != null) {
				ButtonRefImagePicker.Dispose ();
				ButtonRefImagePicker = null;
			}

			if (ButtonRefJapaneseSentence != null) {
				ButtonRefJapaneseSentence.Dispose ();
				ButtonRefJapaneseSentence = null;
			}

			if (ButtonRefKanji != null) {
				ButtonRefKanji.Dispose ();
				ButtonRefKanji = null;
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

			if (ButtonRefRadioInOrder != null) {
				ButtonRefRadioInOrder.Dispose ();
				ButtonRefRadioInOrder = null;
			}

			if (ButtonRefRadioRandom != null) {
				ButtonRefRadioRandom.Dispose ();
				ButtonRefRadioRandom = null;
			}

			if (ButtonRefRomaji != null) {
				ButtonRefRomaji.Dispose ();
				ButtonRefRomaji = null;
			}

			if (ButtonRefWordListPicker != null) {
				ButtonRefWordListPicker.Dispose ();
				ButtonRefWordListPicker = null;
			}

			if (DropdownRefImageList != null) {
				DropdownRefImageList.Dispose ();
				DropdownRefImageList = null;
			}

			if (DropdownRefWordList != null) {
				DropdownRefWordList.Dispose ();
				DropdownRefWordList = null;
			}

			if (FieldRefImageInterval != null) {
				FieldRefImageInterval.Dispose ();
				FieldRefImageInterval = null;
			}

			if (FieldRefWordInterval != null) {
				FieldRefWordInterval.Dispose ();
				FieldRefWordInterval = null;
			}

			if (LabelImagePath != null) {
				LabelImagePath.Dispose ();
				LabelImagePath = null;
			}

			if (LabelWordListPath != null) {
				LabelWordListPath.Dispose ();
				LabelWordListPath = null;
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
