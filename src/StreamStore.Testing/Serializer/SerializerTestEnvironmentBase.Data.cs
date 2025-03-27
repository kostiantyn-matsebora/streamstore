using System;
using System.Globalization;

namespace StreamStore.Testing.Serializer
{
    public abstract partial class SerializerTestEnvironmentBase
    {
        public virtual object DeserializedEvent => new RootEvent
        {
            Value = 127,
            Timestamp = DateTime.Parse("6/8/2025 5:57:20 PM", CultureInfo.InvariantCulture),
            Branches =
            [
                new BranchEvent {
                    Id = 188,
                    Name = "Namebd3fee65-590a-4ce2-83d4-1c4376134a72",
                    Leaves = [
                        new LeafEvent {
                            Name = "Namea62253ec-7d30-451a-91da-465972fddeeb",
                            Value = 221,
                            NullValue = "NullValue2703ac25-bbdd-4418-b3e4-a99768886c5d",
                        },
                        new LeafEvent {
                            Name = "Name6cbf297f-9da6-4eac-a02b-0c675f42ff33",
                            Value = 130,
                            NullValue = "NullValue1d1d572d-f891-4022-a724-5eba6e5da210",
                        },
                        new LeafEvent {
                            Name = "Namefc654ad4-6863-46e0-9273-3c006da222d8",
                            Value = 101,
                            NullValue = "NullValue8eb896ed-9ce0-41be-b459-eae3b94fd945",
                        },
                    ],
                },
                new BranchEvent {
                    Id = 91,
                    Name = "Namedbb45c86-644d-4898-97ec-16cb3f653ff2",
                    Leaves = [
                        new LeafEvent {
                            Name = "Name422811d3-09e8-41ef-ad23-235029640d32",
                            Value = 144,
                            NullValue = "NullValuef37463cd-07b7-4b6b-bf8b-88ded9365bb0",
                        },
                        new LeafEvent {
                            Name = "Namea12cdb18-bded-4b5f-a663-a2080de0d9e8",
                            Value = 246,
                            NullValue = "NullValue1f774134-09eb-4ea8-b64f-c0ec74b06147",
                        },
                        new LeafEvent {
                            Name = "Namee4e73c28-f051-4fee-a1aa-146bb509ac81",
                            Value = 168,
                            NullValue = "NullValue043f78a3-f9d1-48c5-8c3a-7a97c23d3105",
                        },
                    ],
                },
                new BranchEvent {
                    Id = 223,
                    Name = "Namedee40c0b-e502-40c6-863a-07ad44a7eaa5",
                    Leaves = [
                        new LeafEvent {
                            Name = "Name80454886-740d-42ba-b420-560015a454e0",
                            Value = 186,
                            NullValue = "NullValuecfb6387b-5241-4bfc-b53c-723812408d55",
                        },
                        new LeafEvent {
                            Name = "Nameb25579cc-cbde-448b-9077-7bc9c7afb45d",
                            Value = 248,
                            NullValue = "NullValue1992033f-0dd3-45d2-8a2a-a60716340e16",
                        },
                        new LeafEvent {
                            Name = "Namee84cc536-f0cd-4721-a2c1-14983d840c9f",
                            Value = 219,
                            NullValue = "NullValue7ec1aa70-4018-4232-ace1-960564a23392",
                        },
                    ],
                },
            ],
        };
    }
}
