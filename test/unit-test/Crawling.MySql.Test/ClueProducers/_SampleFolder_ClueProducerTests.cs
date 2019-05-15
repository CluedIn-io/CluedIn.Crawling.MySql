using System;
using AutoFixture.Xunit2;
using CluedIn.Core.Data;
using CluedIn.Crawling;
using CluedIn.Crawling.MySql.ClueProducers;
using CluedIn.Crawling.MySql.Core.Models;
using Xunit;

namespace Crawling.MySql.Test.ClueProducers
{
    public class _SampleFolder_ClueProducerTests : BaseClueProducerTest<_SampleFolder_>
    {
        protected override BaseClueProducer<_SampleFolder_> Sut =>
            new _SampleFolder_ClueProducer(_clueFactory.Object);

        protected override EntityType ExpectedEntityType => EntityType.Files.Directory;

        [Theory]
        [InlineAutoData]
        public void ClueHasEdgeToProvider(_SampleFolder_ folder)
        {
            var clue = Sut.MakeClue(folder, Guid.NewGuid());
            _clueFactory.Verify(
                f => f.CreateEntityRootReference(clue, EntityEdgeType.PartOf)
                );
        }
    }
}
