﻿using StreamStore.Tests.Serialization;
using System.Globalization;


namespace StreamStore.Serialization.Tests.Protobuf
{
    public partial class ProtobufTestSuite
    {
        public override byte[] SerializedEvent => new byte[] { 10, 82, 83, 116, 114, 101, 97, 109, 83, 116, 111, 114, 101, 46, 84, 101, 115, 116, 115, 46, 83, 101, 114, 105, 97, 108, 105, 122, 97, 116, 105, 111, 110, 46, 80, 114, 111, 116, 111, 98, 117, 102, 82, 111, 111, 116, 69, 118, 101, 110, 116, 44, 32, 83, 116, 114, 101, 97, 109, 83, 116, 111, 114, 101, 46, 83, 101, 114, 105, 97, 108, 105, 122, 97, 116, 105, 111, 110, 46, 84, 101, 115, 116, 115, 18, 232, 7, 10, 198, 2, 8, 188, 1, 18, 40, 78, 97, 109, 101, 98, 100, 51, 102, 101, 101, 54, 53, 45, 53, 57, 48, 97, 45, 52, 99, 101, 50, 45, 56, 51, 100, 52, 45, 49, 99, 52, 51, 55, 54, 49, 51, 52, 97, 55, 50, 26, 92, 10, 40, 78, 97, 109, 101, 97, 54, 50, 50, 53, 51, 101, 99, 45, 55, 100, 51, 48, 45, 52, 53, 49, 97, 45, 57, 49, 100, 97, 45, 52, 54, 53, 57, 55, 50, 102, 100, 100, 101, 101, 98, 16, 221, 1, 26, 45, 78, 117, 108, 108, 86, 97, 108, 117, 101, 50, 55, 48, 51, 97, 99, 50, 53, 45, 98, 98, 100, 100, 45, 52, 52, 49, 56, 45, 98, 51, 101, 52, 45, 97, 57, 57, 55, 54, 56, 56, 56, 54, 99, 53, 100, 26, 92, 10, 40, 78, 97, 109, 101, 54, 99, 98, 102, 50, 57, 55, 102, 45, 57, 100, 97, 54, 45, 52, 101, 97, 99, 45, 97, 48, 50, 98, 45, 48, 99, 54, 55, 53, 102, 52, 50, 102, 102, 51, 51, 16, 130, 1, 26, 45, 78, 117, 108, 108, 86, 97, 108, 117, 101, 49, 100, 49, 100, 53, 55, 50, 100, 45, 102, 56, 57, 49, 45, 52, 48, 50, 50, 45, 97, 55, 50, 52, 45, 53, 101, 98, 97, 54, 101, 53, 100, 97, 50, 49, 48, 26, 91, 10, 40, 78, 97, 109, 101, 102, 99, 54, 53, 52, 97, 100, 52, 45, 54, 56, 54, 51, 45, 52, 54, 101, 48, 45, 57, 50, 55, 51, 45, 51, 99, 48, 48, 54, 100, 97, 50, 50, 50, 100, 56, 16, 101, 26, 45, 78, 117, 108, 108, 86, 97, 108, 117, 101, 56, 101, 98, 56, 57, 54, 101, 100, 45, 57, 99, 101, 48, 45, 52, 49, 98, 101, 45, 98, 52, 53, 57, 45, 101, 97, 101, 51, 98, 57, 52, 102, 100, 57, 52, 53, 10, 198, 2, 8, 91, 18, 40, 78, 97, 109, 101, 100, 98, 98, 52, 53, 99, 56, 54, 45, 54, 52, 52, 100, 45, 52, 56, 57, 56, 45, 57, 55, 101, 99, 45, 49, 54, 99, 98, 51, 102, 54, 53, 51, 102, 102, 50, 26, 92, 10, 40, 78, 97, 109, 101, 52, 50, 50, 56, 49, 49, 100, 51, 45, 48, 57, 101, 56, 45, 52, 49, 101, 102, 45, 97, 100, 50, 51, 45, 50, 51, 53, 48, 50, 57, 54, 52, 48, 100, 51, 50, 16, 144, 1, 26, 45, 78, 117, 108, 108, 86, 97, 108, 117, 101, 102, 51, 55, 52, 54, 51, 99, 100, 45, 48, 55, 98, 55, 45, 52, 98, 54, 98, 45, 98, 102, 56, 98, 45, 56, 56, 100, 101, 100, 57, 51, 54, 53, 98, 98, 48, 26, 92, 10, 40, 78, 97, 109, 101, 97, 49, 50, 99, 100, 98, 49, 56, 45, 98, 100, 101, 100, 45, 52, 98, 53, 102, 45, 97, 54, 54, 51, 45, 97, 50, 48, 56, 48, 100, 101, 48, 100, 57, 101, 56, 16, 246, 1, 26, 45, 78, 117, 108, 108, 86, 97, 108, 117, 101, 49, 102, 55, 55, 52, 49, 51, 52, 45, 48, 57, 101, 98, 45, 52, 101, 97, 56, 45, 98, 54, 52, 102, 45, 99, 48, 101, 99, 55, 52, 98, 48, 54, 49, 52, 55, 26, 92, 10, 40, 78, 97, 109, 101, 101, 52, 101, 55, 51, 99, 50, 56, 45, 102, 48, 53, 49, 45, 52, 102, 101, 101, 45, 97, 49, 97, 97, 45, 49, 52, 54, 98, 98, 53, 48, 57, 97, 99, 56, 49, 16, 168, 1, 26, 45, 78, 117, 108, 108, 86, 97, 108, 117, 101, 48, 52, 51, 102, 55, 56, 97, 51, 45, 102, 57, 100, 49, 45, 52, 56, 99, 53, 45, 56, 99, 51, 97, 45, 55, 97, 57, 55, 99, 50, 51, 100, 51, 49, 48, 53, 10, 199, 2, 8, 223, 1, 18, 40, 78, 97, 109, 101, 100, 101, 101, 52, 48, 99, 48, 98, 45, 101, 53, 48, 50, 45, 52, 48, 99, 54, 45, 56, 54, 51, 97, 45, 48, 55, 97, 100, 52, 52, 97, 55, 101, 97, 97, 53, 26, 92, 10, 40, 78, 97, 109, 101, 56, 48, 52, 53, 52, 56, 56, 54, 45, 55, 52, 48, 100, 45, 52, 50, 98, 97, 45, 98, 52, 50, 48, 45, 53, 54, 48, 48, 49, 53, 97, 52, 53, 52, 101, 48, 16, 186, 1, 26, 45, 78, 117, 108, 108, 86, 97, 108, 117, 101, 99, 102, 98, 54, 51, 56, 55, 98, 45, 53, 50, 52, 49, 45, 52, 98, 102, 99, 45, 98, 53, 51, 99, 45, 55, 50, 51, 56, 49, 50, 52, 48, 56, 100, 53, 53, 26, 92, 10, 40, 78, 97, 109, 101, 98, 50, 53, 53, 55, 57, 99, 99, 45, 99, 98, 100, 101, 45, 52, 52, 56, 98, 45, 57, 48, 55, 55, 45, 55, 98, 99, 57, 99, 55, 97, 102, 98, 52, 53, 100, 16, 248, 1, 26, 45, 78, 117, 108, 108, 86, 97, 108, 117, 101, 49, 57, 57, 50, 48, 51, 51, 102, 45, 48, 100, 100, 51, 45, 52, 53, 100, 50, 45, 56, 97, 50, 97, 45, 97, 54, 48, 55, 49, 54, 51, 52, 48, 101, 49, 54, 26, 92, 10, 40, 78, 97, 109, 101, 101, 56, 52, 99, 99, 53, 51, 54, 45, 102, 48, 99, 100, 45, 52, 55, 50, 49, 45, 97, 50, 99, 49, 45, 49, 52, 57, 56, 51, 100, 56, 52, 48, 99, 57, 102, 16, 219, 1, 26, 45, 78, 117, 108, 108, 86, 97, 108, 117, 101, 55, 101, 99, 49, 97, 97, 55, 48, 45, 52, 48, 49, 56, 45, 52, 50, 51, 50, 45, 97, 99, 101, 49, 45, 57, 54, 48, 53, 54, 52, 97, 50, 51, 51, 57, 50, 18, 8, 8, 128, 188, 174, 132, 13, 16, 3, 24, 127 };

        public override object DeserializedEvent => new ProtobufRootEvent
        {
            Value = 127,
            Timestamp = DateTime.Parse("6/8/2025 5:57:20 PM", CultureInfo.InvariantCulture),
            Branches = new ProtobufBranchEvent[]
            {
                new ProtobufBranchEvent {
                    Id = 188,
                    Name = "Namebd3fee65-590a-4ce2-83d4-1c4376134a72",
                    Leaves = new ProtobufLeafEvent[] {
                        new ProtobufLeafEvent {
                            Name = "Namea62253ec-7d30-451a-91da-465972fddeeb",
                            Value = 221,
                            NullValue = "NullValue2703ac25-bbdd-4418-b3e4-a99768886c5d",
                        },
                        new ProtobufLeafEvent {
                            Name = "Name6cbf297f-9da6-4eac-a02b-0c675f42ff33",
                            Value = 130,
                            NullValue = "NullValue1d1d572d-f891-4022-a724-5eba6e5da210",
                        },
                        new ProtobufLeafEvent {
                            Name = "Namefc654ad4-6863-46e0-9273-3c006da222d8",
                            Value = 101,
                            NullValue = "NullValue8eb896ed-9ce0-41be-b459-eae3b94fd945",
                        },
                    },
                },
                new ProtobufBranchEvent {
                    Id = 91,
                    Name = "Namedbb45c86-644d-4898-97ec-16cb3f653ff2",
                    Leaves = new ProtobufLeafEvent[] {
                        new ProtobufLeafEvent {
                            Name = "Name422811d3-09e8-41ef-ad23-235029640d32",
                            Value = 144,
                            NullValue = "NullValuef37463cd-07b7-4b6b-bf8b-88ded9365bb0",
                        },
                        new ProtobufLeafEvent {
                            Name = "Namea12cdb18-bded-4b5f-a663-a2080de0d9e8",
                            Value = 246,
                            NullValue = "NullValue1f774134-09eb-4ea8-b64f-c0ec74b06147",
                        },
                        new ProtobufLeafEvent {
                            Name = "Namee4e73c28-f051-4fee-a1aa-146bb509ac81",
                            Value = 168,
                            NullValue = "NullValue043f78a3-f9d1-48c5-8c3a-7a97c23d3105",
                        },
                    },
                },
                new ProtobufBranchEvent {
                    Id = 223,
                    Name = "Namedee40c0b-e502-40c6-863a-07ad44a7eaa5",
                    Leaves = new ProtobufLeafEvent[] {
                        new ProtobufLeafEvent {
                            Name = "Name80454886-740d-42ba-b420-560015a454e0",
                            Value = 186,
                            NullValue = "NullValuecfb6387b-5241-4bfc-b53c-723812408d55",
                        },
                        new ProtobufLeafEvent {
                            Name = "Nameb25579cc-cbde-448b-9077-7bc9c7afb45d",
                            Value = 248,
                            NullValue = "NullValue1992033f-0dd3-45d2-8a2a-a60716340e16",
                        },
                        new ProtobufLeafEvent {
                            Name = "Namee84cc536-f0cd-4721-a2c1-14983d840c9f",
                            Value = 219,
                            NullValue = "NullValue7ec1aa70-4018-4232-ace1-960564a23392",
                        },
                    },
                },
            },
        };
    }
}
