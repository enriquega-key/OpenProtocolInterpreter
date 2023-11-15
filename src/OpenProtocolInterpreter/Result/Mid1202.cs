﻿using System.Collections.Generic;
using System.Linq;

namespace OpenProtocolInterpreter.Result
{
    /// <summary>
    /// Operation result object data
    /// <para>
    ///     This message contains the cycle data for one object, both data for the whole process and data related to 
    ///     the different steps in the process.The user defined values are preconfigured in the controller via the 
    ///     configuration tool. The message uses the Variable Parameter pattern for transmission of the values.
    /// </para>
    /// <para>
    ///     Note: Only values that exist in the result will be sent.So the actual data received may vary between 
    ///     the cycles if the settings differ between different programs.
    /// </para>
    /// <para>Message sent by: Controller</para>
    /// <para>
    ///     Answer: <see cref="Mid1203"/> Operation result data acknowledge or 
    ///             <see cref="Communication.Mid0005"/> with <see cref="Mid1202"/> in the data field.
    /// </para>
    ///         
    ///         If the sequence number acknowledge functionality is used there is no need for these acknowledges.
    /// </summary>
    public class Mid1202 : Mid, IResult, IController, IAcknowledgeable<Mid1203>, IAcceptableCommand
    {
        public const int MID = 1202;

        public int TotalNumberOfMessages
        {
            get => GetField(Header.StandardizedRevision, (int)DataFields.TOTAL_MESSAGES).GetValue(OpenProtocolConvert.ToInt32);
            set => GetField(Header.StandardizedRevision, (int)DataFields.TOTAL_MESSAGES).SetValue(OpenProtocolConvert.ToString, value);
        }
        public int MessageNumber
        {
            get => GetField(Header.StandardizedRevision, (int)DataFields.MESSAGE_NUMBER).GetValue(OpenProtocolConvert.ToInt32);
            set => GetField(Header.StandardizedRevision, (int)DataFields.MESSAGE_NUMBER).SetValue(OpenProtocolConvert.ToString, value);
        }
        public int ResultDataIdentifier
        {
            get => GetField(Header.StandardizedRevision, (int)DataFields.RESULT_DATA_ID).GetValue(OpenProtocolConvert.ToInt32);
            set => GetField(Header.StandardizedRevision, (int)DataFields.RESULT_DATA_ID).SetValue(OpenProtocolConvert.ToString, value);
        }
        public int ObjectId
        {
            get => GetField(Header.StandardizedRevision, (int)DataFields.OBJECT_ID).GetValue(OpenProtocolConvert.ToInt32);
            set => GetField(Header.StandardizedRevision, (int)DataFields.OBJECT_ID).SetValue(OpenProtocolConvert.ToString, value);
        }
        public string NodeGuid
        {
            get => GetField(Header.StandardizedRevision, (int)DataFields.NODE_GUID).Value;
            set => GetField(Header.StandardizedRevision, (int)DataFields.NODE_GUID).SetValue(value);
        }
        public int NumberOfDataFields
        {
            get => GetField(Header.StandardizedRevision, (int)DataFields.NUMBER_DATA_FIELDS).GetValue(OpenProtocolConvert.ToInt32);
            set => GetField(Header.StandardizedRevision, (int)DataFields.NUMBER_DATA_FIELDS).SetValue(OpenProtocolConvert.ToString, value);
        }

        public List<VariableDataField> VariableDataFields { get; set; }

        public Mid1202() : this(new Header()
        {
            Mid = MID,
            Revision = DEFAULT_REVISION
        })
        {
        }

        public Mid1202(Header header) : base(header)
        {
            VariableDataFields = [];
        }
        protected override string BuildHeader()
        {
            Header.Length = 20 + RevisionsByFields[Header.StandardizedRevision].Sum(x => x.TotalSize);
            return Header.ToString();
        }

        public override string Pack()
        {
            NumberOfDataFields = VariableDataFields.Count;
            var revision = Header.StandardizedRevision;
            GetField(revision, (int)DataFields.VARIABLE_DATA_FIELDS).SetValue(OpenProtocolConvert.ToString(VariableDataFields));
            int prefixIndex = 0;
            return string.Concat(BuildHeader(), base.Pack(revision, ref prefixIndex));
        }

        public override Mid Parse(string package)
        {
            Header = ProcessHeader(package);
            var revision = Header.StandardizedRevision;
            var variableDataField = GetField(revision, (int)DataFields.VARIABLE_DATA_FIELDS);
            variableDataField.Size = Header.Length - variableDataField.Index;
            ProcessDataFields(revision, package);

            VariableDataFields = VariableDataField.ParseAll(variableDataField.Value).ToList();
            return this;
        }

        protected override Dictionary<int, List<DataField>> RegisterDatafields()
        {
            return new Dictionary<int, List<DataField>>()
            {
                {
                    1, new List<DataField>()
                    {
                        new((int)DataFields.TOTAL_MESSAGES, 20, 3, '0', PaddingOrientation.LeftPadded, false),
                        new((int)DataFields.MESSAGE_NUMBER, 23, 3, '0', PaddingOrientation.LeftPadded, false),
                        new((int)DataFields.RESULT_DATA_ID, 26, 10, '0', PaddingOrientation.LeftPadded, false),
                        new((int)DataFields.OBJECT_ID, 36, 4, '0', PaddingOrientation.LeftPadded, false),
                        new((int)DataFields.NUMBER_DATA_FIELDS, 40, 3, '0', PaddingOrientation.LeftPadded, false),
                        new((int)DataFields.VARIABLE_DATA_FIELDS, 43, 0, false) //defined at runtime
                    }
                },
                {
                    2, new List<DataField>()
                    {
                        new((int)DataFields.TOTAL_MESSAGES, 20, 3, '0', PaddingOrientation.LeftPadded, false),
                        new((int)DataFields.MESSAGE_NUMBER, 23, 3, '0', PaddingOrientation.LeftPadded, false),
                        new((int)DataFields.RESULT_DATA_ID, 26, 10, '0', PaddingOrientation.LeftPadded, false),
                        new((int)DataFields.OBJECT_ID, 36, 4, '0', PaddingOrientation.LeftPadded, false),
                        new((int)DataFields.NODE_GUID, 40, 36, ' ', PaddingOrientation.LeftPadded, false),
                        new((int)DataFields.NUMBER_DATA_FIELDS, 76, 3, '0', PaddingOrientation.LeftPadded, false),
                        new((int)DataFields.VARIABLE_DATA_FIELDS, 79, 0, false) //defined at runtime
                    }
                }
            };
        }

        protected enum DataFields
        {
            TOTAL_MESSAGES,
            MESSAGE_NUMBER,
            RESULT_DATA_ID,
            OBJECT_ID,
            NODE_GUID,
            NUMBER_DATA_FIELDS,
            VARIABLE_DATA_FIELDS
        }
    }
}
