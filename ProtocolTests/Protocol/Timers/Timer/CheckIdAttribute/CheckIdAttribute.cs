namespace ProtocolTests.Protocol.Timers.Timer.CheckIdAttribute
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Skyline.DataMiner.CICD.Validators.Common.Interfaces;
    using Skyline.DataMiner.CICD.Validators.Common.Model;
    using Skyline.DataMiner.CICD.Validators.Protocol.Common;
    using Skyline.DataMiner.CICD.Validators.Protocol.Common.Extensions;
    using Skyline.DataMiner.CICD.Validators.Protocol.Interfaces;
    using Skyline.DataMiner.CICD.Validators.Protocol.Tests.Protocol.Timers.Timer.CheckIdAttribute;

    [TestClass]
    public class Validate
    {
        private readonly IValidate check = new CheckIdAttribute();

        #region Valid Checks

        [TestMethod]
        public void Timer_CheckIdAttribute_Valid()
        {
            Generic.ValidateData data = new Generic.ValidateData
            {
                TestType = Generic.TestType.Valid,
                FileName = "Valid",
                ExpectedResults = new List<IValidationResult>()
            };

            Generic.Validate(check, data);
        }

        #endregion

        #region Invalid Checks

        [TestMethod]
        public void Timer_CheckIdAttribute_DuplicatedId()
        {
            Generic.ValidateData data = new Generic.ValidateData
            {
                TestType = Generic.TestType.Invalid,
                FileName = "DuplicatedId",
                ExpectedResults = new List<IValidationResult>
                {
                    Error.DuplicatedId(null, null, null, "101", "Duplicate_101_1, Duplicate_101_2").WithSubResults(
                        Error.DuplicatedId(null, null, null, "101", "Duplicate_101_1"),
                        Error.DuplicatedId(null, null, null, "101", "Duplicate_101_2")),

                    Error.DuplicatedId(null, null, null, "102", "Duplicate_102_1, Duplicate_102_2, Duplicate_102_3").WithSubResults(
                        Error.DuplicatedId(null, null, null, "102", "Duplicate_102_1"),
                        Error.DuplicatedId(null, null, null, "102", "Duplicate_102_2"),
                        Error.DuplicatedId(null, null, null, "102", "Duplicate_102_3"))
                }
            };

            Generic.Validate(check, data);
        }

        [TestMethod]
        public void Timer_CheckIdAttribute_EmptyAttribute()
        {
            Generic.ValidateData data = new Generic.ValidateData
            {
                TestType = Generic.TestType.Invalid,
                FileName = "EmptyAttribute",
                ExpectedResults = new List<IValidationResult>
                {
                    Error.EmptyAttribute(null, null, null),
                    Error.EmptyAttribute(null, null, null),
                }
            };

            Generic.Validate(check, data);
        }

        [TestMethod]
        public void Timer_CheckIdAttribute_InvalidValue()
        {
            Generic.ValidateData data = new Generic.ValidateData
            {
                TestType = Generic.TestType.Invalid,
                FileName = "InvalidValue",
                ExpectedResults = new List<IValidationResult>
                {
                    Error.InvalidValue(null, null, null, "aaa", "String"),

                    Error.InvalidValue(null, null, null, "-2", "Number_Negative"),
                    Error.InvalidValue(null, null, null, "1.5", "Number_Double_1"),
                    Error.InvalidValue(null, null, null, "2,6", "Number_Double_2"),
                    Error.InvalidValue(null, null, null, "03", "Number_LeadingZero"),
                    Error.InvalidValue(null, null, null, "+4", "Number_LeadingPlusSign"),
                    Error.InvalidValue(null, null, null, "5x10^1", "Number_ScientificNotation"),
                }
            };

            Generic.Validate(check, data);
        }

        [TestMethod]
        public void Timer_CheckIdAttribute_MissingAttribute()
        {
            Generic.ValidateData data = new Generic.ValidateData
            {
                TestType = Generic.TestType.Invalid,
                FileName = "MissingAttribute",
                ExpectedResults = new List<IValidationResult>
                {
                    Error.MissingAttribute(null, null, null),
                }
            };

            Generic.Validate(check, data);
        }

        [TestMethod]
        public void Timer_CheckIdAttribute_UntrimmedAttribute()
        {
            Generic.ValidateData data = new Generic.ValidateData
            {
                TestType = Generic.TestType.Invalid,
                FileName = "UntrimmedAttribute",
                ExpectedResults = new List<IValidationResult>
                {
                    Error.UntrimmedAttribute(null, null, null, " 101"),
                    Error.UntrimmedAttribute(null, null, null, "102 "),
                    Error.UntrimmedAttribute(null, null, null, " 103 "),
                }
            };

            Generic.Validate(check, data);
        }

        #endregion
    }

    [TestClass]
    public class CodeFix
    {
        private readonly ICodeFix check = new CheckIdAttribute();

        [TestMethod]
        public void Timer_CheckIdAttribute_UntrimmedAttribute()
        {
            Generic.FixData data = new Generic.FixData
            {
                FileNameBase = "UntrimmedAttribute",
            };

            Generic.Fix(check, data);
        }
    }

    [TestClass]
    public class ErrorMessages
    {
        [TestMethod]
        public void Timer_CheckIdAttribute_DuplicatedId()
        {
            // Create ErrorMessage
            var message = Error.DuplicatedId(null, null, null, "2", "3");

            var expected = new ValidationResult()
            {
                ErrorId = 5,
                FullId = "7.6.5",
                Category = Category.Timer,
                Severity = Severity.Critical,
                Certainty = Certainty.Certain,
                Source = Source.Validator,
                FixImpact = FixImpact.Breaking,
                GroupDescription = "",
                Description = "More than one Timer with same ID '2'. Timer Names '3'.",
                HowToFix = "",
                ExampleCode = "",
                Details = "The id attribute is used internally as the identifier for each timer." + Environment.NewLine + "It is therefore mandatory and needs to follow a number of rules:" + Environment.NewLine + "- Each timer should have a unique id." + Environment.NewLine + "- Should be an unsigned integer." + Environment.NewLine + "- Only plain numbers are allowed (no leading signs, no leading zeros, no scientific notation, etc).",
                HasCodeFix = false,
            };

            // Assert
            message.Should().BeEquivalentTo(expected, Generic.ExcludePropertiesForErrorMessages);
        }

        [TestMethod]
        public void Timer_CheckIdAttribute_EmptyAttribute()
        {
            // Create ErrorMessage
            var message = Error.EmptyAttribute(null, null, null);

            var expected = new ValidationResult()
            {
                ErrorId = 2,
                FullId = "7.6.2",
                Category = Category.Timer,
                Severity = Severity.Critical,
                Certainty = Certainty.Certain,
                Source = Source.Validator,
                FixImpact = FixImpact.Breaking,
                GroupDescription = "",
                Description = "Empty attribute 'Timer@id'.",
                HowToFix = "",
                ExampleCode = "",
                Details = "The id attribute is used internally as the identifier for each timer." + Environment.NewLine + "It is therefore mandatory and needs to follow a number of rules:" + Environment.NewLine + "- Each timer should have a unique id." + Environment.NewLine + "- Should be an unsigned integer." + Environment.NewLine + "- Only plain numbers are allowed (no leading signs, no leading zeros, no scientific notation, etc).",
                HasCodeFix = false,
            };

            // Assert
            message.Should().BeEquivalentTo(expected, Generic.ExcludePropertiesForErrorMessages);
        }

        [TestMethod]
        public void Timer_CheckIdAttribute_InvalidValue()
        {
            // Create ErrorMessage
            var message = Error.InvalidValue(null, null, null, "AAA", "MyName");

            var expected = new ValidationResult()
            {
                ErrorId = 4,
                FullId = "7.6.4",
                Category = Category.Timer,
                Severity = Severity.Critical,
                Certainty = Certainty.Certain,
                Source = Source.Validator,
                FixImpact = FixImpact.Breaking,
                GroupDescription = "",
                Description = "Invalid value 'AAA' in attribute 'Timer@id'. Timer name 'MyName'.",
                HowToFix = "",
                ExampleCode = "",
                Details = "The id attribute is used internally as the identifier for each timer." + Environment.NewLine + "It is therefore mandatory and needs to follow a number of rules:" + Environment.NewLine + "- Each timer should have a unique id." + Environment.NewLine + "- Should be an unsigned integer." + Environment.NewLine + "- Only plain numbers are allowed (no leading signs, no leading zeros, no scientific notation, etc).",
                HasCodeFix = false,
            };

            // Assert
            message.Should().BeEquivalentTo(expected, Generic.ExcludePropertiesForErrorMessages);
        }

        [TestMethod]
        public void Timer_CheckIdAttribute_MissingAttribute()
        {
            // Create ErrorMessage
            var message = Error.MissingAttribute(null, null, null);

            var expected = new ValidationResult()
            {
                ErrorId = 1,
                FullId = "7.6.1",
                Category = Category.Timer,
                Severity = Severity.Critical,
                Certainty = Certainty.Certain,
                Source = Source.Validator,
                FixImpact = FixImpact.Breaking,
                GroupDescription = "",
                Description = "Missing attribute 'Timer@id'.",
                HowToFix = "",
                ExampleCode = "",
                Details = "The id attribute is used internally as the identifier for each timer." + Environment.NewLine + "It is therefore mandatory and needs to follow a number of rules:" + Environment.NewLine + "- Each timer should have a unique id." + Environment.NewLine + "- Should be an unsigned integer." + Environment.NewLine + "- Only plain numbers are allowed (no leading signs, no leading zeros, no scientific notation, etc).",
                HasCodeFix = false,
            };

            // Assert
            message.Should().BeEquivalentTo(expected, Generic.ExcludePropertiesForErrorMessages);
        }

        [TestMethod]
        public void Timer_CheckIdAttribute_UntrimmedAttribute()
        {
            // Create ErrorMessage
            var message = Error.UntrimmedAttribute(null, null, null, "2");

            var expected = new ValidationResult()
            {
                ErrorId = 3,
                FullId = "7.6.3",
                Category = Category.Timer,
                Severity = Severity.Warning,
                Certainty = Certainty.Certain,
                Source = Source.Validator,
                FixImpact = FixImpact.NonBreaking,
                GroupDescription = "",
                Description = "Untrimmed attribute 'Timer@id'. Current value '2'.",
                HowToFix = "",
                ExampleCode = "",
                Details = "The id attribute is used internally as the identifier for each timer." + Environment.NewLine + "It is therefore mandatory and needs to follow a number of rules:" + Environment.NewLine + "- Each timer should have a unique id." + Environment.NewLine + "- Should be an unsigned integer." + Environment.NewLine + "- Only plain numbers are allowed (no leading signs, no leading zeros, no scientific notation, etc).",
                HasCodeFix = true,
            };

            // Assert
            message.Should().BeEquivalentTo(expected, Generic.ExcludePropertiesForErrorMessages);
        }
    }

    [TestClass]
    public class Attribute
    {
        private readonly IRoot check = new CheckIdAttribute();

        [TestMethod]
        public void Timer_CheckIdAttribute_CheckCategory() => Generic.CheckCategory(check, Category.Timer);

        [TestMethod]
        public void Timer_CheckIdAttribute_CheckId() => Generic.CheckId(check, CheckId.CheckIdAttribute);
    }
}