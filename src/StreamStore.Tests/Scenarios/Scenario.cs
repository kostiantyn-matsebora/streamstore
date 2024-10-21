using System.Security.Cryptography;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;


namespace StreamStore.Tests.Scenarios
{
    public class Scenario: TestsBase<StreamStoreSuite>
    {
       public Scenario() : base(new StreamStoreSuite())
       {
       }

        protected IStreamStore store => Suite.Store;

        protected Id[] StreamIdentifiers => Suite.StreamIdentifiers;

        protected Id RandomStreamId => StreamIdentifiers[RandomNumberGenerator.GetInt32(0, StreamIdentifiers.Length - 1)];
    }
}
