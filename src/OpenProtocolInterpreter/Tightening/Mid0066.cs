﻿using System.Collections.Generic;
using System.Text;

namespace OpenProtocolInterpreter.Tightening
{
    /// <summary>
    /// Number of offline results
    /// <para>Number of results when offline</para>
    /// <para>Message sent by: Controller</para>
    /// <para>Answer: None</para>
    /// </summary>
    public class Mid0066 : Mid, ITightening, IController
    {
        public const int MID = 66;

        public int NumberOfOfflineResults
        {
            get => GetField(1, (int)DataFields.NumberOfOfflineResults).GetValue(OpenProtocolConvert.ToInt32);
            set => GetField(1, (int)DataFields.NumberOfOfflineResults).SetValue(OpenProtocolConvert.ToString, value);
        }

        public int NumberOfOfflineCurves
        {
            get => GetField(2, (int)DataFields.NumberOfOfflineCurves).GetValue(OpenProtocolConvert.ToInt32);
            set => GetField(2, (int)DataFields.NumberOfOfflineCurves).SetValue(OpenProtocolConvert.ToString, value);
        }

        public Mid0066() : this(new Header()
        {
            Mid = MID, 
            Revision = DEFAULT_REVISION
        })
        {
        }

        public Mid0066(Header header) : base(header)
        {
        }

        public override Mid Parse(string package)
        {
            Header = ProcessHeader(package);
            HandleRevisionSizes();
            ProcessDataFields(package);
            return this;
        }

        public override string Pack()
        {
            HandleRevisionSizes();
            return base.Pack();
        }

        private void HandleRevisionSizes()
        {
            if(Header.Revision > 1)
            {
                GetField(1, (int)DataFields.NumberOfOfflineResults).Size = 3;
            }
        }

        protected override Dictionary<int, List<DataField>> RegisterDatafields()
        {
            return new Dictionary<int, List<DataField>>()
            {
                {
                    1, new List<DataField>()
                            {
                                new((int)DataFields.NumberOfOfflineResults, 20, 2, '0', PaddingOrientation.LeftPadded, true)
                            }
                },
                {
                    2, new List<DataField>()
                            {
                                new((int)DataFields.NumberOfOfflineCurves, 25, 3, '0', PaddingOrientation.LeftPadded, true),
                            }
                }
            };
        }

        protected enum DataFields
        {
            NumberOfOfflineResults,
            NumberOfOfflineCurves
        }
    }
}
