using System.Windows.Media;
using Meerkatalyst.Lonestar.EditorExtension.LineResultMarkers;
using Xunit;

namespace lonestar.tests.EditorExtensions.LineResultMarkers
{
    public class LineResultMarkerTests
    {
        [Fact]
        public void Setups_up_fill_and_outline_in_ctor()
        {
            LineResultMarker marker = new StubTestLineResultMarker(Colors.Black);
            Assert.NotNull(marker.Fill);
            Assert.NotNull(marker.Outline);
        }

        [Fact]
        public void Fill_and_outline_should_be_frozen()
        {
            LineResultMarker marker = new StubTestLineResultMarker(Colors.Black);
            Assert.True(marker.Fill.IsFrozen);
            Assert.True(marker.Outline.IsFrozen);
        }
    }

    public class StubTestLineResultMarker : LineResultMarker
    {
        public StubTestLineResultMarker(Color color) : base(color)
        {
            
        }
    }
}
