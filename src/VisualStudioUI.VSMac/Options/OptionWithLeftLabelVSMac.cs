﻿using AppKit;
using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public abstract class OptionWithLeftLabelVSMac : OptionVSMac
    {
        private NSView? _optionView;
        private NSButton? _hintButton;
        private NSView _control;

        public OptionWithLeftLabelVSMac(Option option) : base(option)
        {
        }

        public override NSView View
        {
            get
            {
                if (_optionView == null)
                {
                    CreateView();
                }

                return _optionView!;
            }
        }

        protected abstract NSView ControlView { get; }

        private void CreateView()
        {
            // View:     optionView
            _optionView = new AppKit.NSView();
            _optionView.WidthAnchor.ConstraintEqualToConstant(600f - IndentValue()).Active = true;

            _control = ControlView;
            // TODO: Set a11y info properly
            _control.AccessibilityLabel = "Control";
            _control.AccessibilityHelp = "Provides a control";

            _optionView.AddSubview(_control);

            UpdateHintButton();

            var label = CreateLabelView();
            if (label != null)
            {
                _optionView.AddSubview(label);

                label.TrailingAnchor.ConstraintEqualToAnchor(_control.LeadingAnchor, -8f).Active = true;
                label.CenterYAnchor.ConstraintEqualToAnchor(_control.CenterYAnchor).Active = true;
            }

            NSTextField? descriptionView = CreateDescriptionView();
            if (descriptionView != null)
            {
                _optionView.AddSubview(descriptionView);

                descriptionView.LeadingAnchor.ConstraintEqualToAnchor(_control.LeadingAnchor, 0f).Active = true;
                descriptionView.TopAnchor.ConstraintEqualToAnchor(_control.BottomAnchor, 0f).Active = true;
                descriptionView.BottomAnchor.ConstraintEqualToAnchor(_optionView.BottomAnchor).Active = true;
            }
            else
            {
                _control.BottomAnchor.ConstraintEqualToAnchor(_optionView.BottomAnchor, -5f).Active = true;
            }

            float leftSpace = (Option.AllowSpaceForLabel||label != null) ? 222f : 20f;
            _control.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, leftSpace + IndentValue()).Active = true;

            _control.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 5f).Active = true;

        }

        public override void OnEnableChanged(bool enabled)
        {
        }

        protected override void UpdateHintButton()
        {
            if (_hintButton != null)
            {
                _hintButton.RemoveFromSuperview();
                _hintButton.Dispose(); // TODO: Is this needed?
                _hintButton = null;
            }

            _hintButton = CreateHintButton();

            if (_hintButton == null)
                return;

            _optionView!.AddSubview(_hintButton);

            _hintButton.LeadingAnchor.ConstraintEqualToAnchor(_control.TrailingAnchor, 11f).Active = true;
            _hintButton.CenterYAnchor.ConstraintEqualToAnchor(_control.CenterYAnchor).Active = true;
        }
    }
}
